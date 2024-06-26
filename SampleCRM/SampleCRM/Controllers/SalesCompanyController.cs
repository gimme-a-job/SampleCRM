using SampleCRM.Contexts;
using SampleCRM.Identity;
using SampleCRM.Models.TableModels;
using SampleCRM.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SampleCRM.Controllers
{
    [Authorize( Policy = nameof( AuthorizationPolicies.AnyRolePolicy ) )]
    public class SalesCompanyController : Controller
    {
        private readonly SampleCRMContext _context;

        /// <summary>
        /// 営業会社コード開始番号(10億)
        /// </summary>
        private const int ORIGIN_SALES_COMPANY_CODE = 1000000000;

        public SalesCompanyController( SampleCRMContext context )
        {
            _context = context;
        }

        // GET: SalesCompany
        public async Task<IActionResult> Index()
        {
            // 営業会社全て取得
            var companies = await _context.Companies.Where( x => !x.IsClientCompany ).OrderBy( x => x.CompanyCode ).ToListAsync();
            var model = new List<SalesCompany>();
            if ( companies.Any() )
            {
                foreach ( var company in companies )
                {
                    var temp = new SalesCompany();
                    // 会社情報から分かるデータ登録
                    temp.CompanyCode = company.CompanyCode;
                    temp.CompanyName = company.CompanyName;
                    temp.PhoneticName = company.PhoneticName;
                    temp.Note = company.Note;

                    // 代表部署情報の取得
                    var department = await _context.Departments.Where( x => x.CompanyCode == company.CompanyCode && x.IsMain ).FirstOrDefaultAsync();
                    if ( department != null )
                    {
                        // 代表部署の情報登録
                        temp.PostalCode = department.PostalCode;
                        //TODO: 住所関連修正 
                        temp.Address = department.City;
                        temp.TelNumber = department.TelNumber;
                    }
                    model.Add( temp );
                }
            }

            return View( model );
        }

        // GET: SalesCompany/Details/5
        public async Task<IActionResult> Details( int? id )
        {
            if ( id == null || _context.Companies == null || _context.Departments == null )
            {
                return NotFound();
            }

            // 会社情報の取得と登録
            var company = await _context.Companies.FirstOrDefaultAsync( m => m.CompanyCode == id );
            if ( company == null )
            {
                return NotFound();
            }
            var model = new SalesCompany();
            model.CompanyCode = company.CompanyCode;
            model.CompanyName = company.CompanyName;
            model.PhoneticName = company.PhoneticName;
            model.Note = company.Note;

            // 会社コードを持った部署情報を全て取得し登録
            var departments = await _context.Departments.Where( x => x.CompanyCode == company.CompanyCode ).OrderBy( x => x.DisplayOrder ).ToListAsync();
            model.Departments = [];
            foreach ( var department in departments )
            {
                var temp = new SalesDepartment();
                temp.DepartmentCode = department.DepartmentCode;
                temp.IsMainDepartment = department.IsMain;
                temp.DepartmentName = department.DepartmentName;
                temp.PhoneticName = department.PhoneticName;
                temp.PostalCode = department.PostalCode;
                //TODO: 住所関連修正 
                temp.Address = department.City;
                temp.TelNumber = department.TelNumber;
                temp.FaxNumber = department.FaxNumber;
                temp.Email = department.Email;
                model.Departments.Add( temp );
            }

            return View( model );
        }

        // GET: SalesCompany/Create
        [Authorize( Policy = nameof( AuthorizationPolicies.GlobalAdministratorOnlyPolicy ) )]
        public IActionResult Create()
        {
            return View();
        }

        // POST: SalesCompany/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize( Policy = nameof( AuthorizationPolicies.GlobalAdministratorOnlyPolicy ) )]
        public async Task<IActionResult> Create( [Bind( "CompanyName,PhoneticName,Note" )] SalesCompany model )
        {
            if ( ModelState.IsValid )
            {
                var tableModel = new Company();

                // フォームから登録
                tableModel.CompanyName = model.CompanyName;
                tableModel.PhoneticName = model.PhoneticName;
                tableModel.Note = model.Note;

                //tableModel.UpdateUser = 1;
                //tableModel.UpdateDate = DateTime.Now;

                // 固定値
                tableModel.IsClientCompany = false;
                tableModel.CompanyStatusCode = _context.CompanyStatuses.Single( x => x.CompanyStatusName == "営業会社" ).CompanyStatusCode;  // TODO: 一致無し時・複数一致時はエラーページに飛ばす。

                // 計算で出す
                int tempNum = ORIGIN_SALES_COMPANY_CODE;
                var order = await _context.Companies.Where( x => !x.IsClientCompany ).Select( x => x.CompanyCode ).ToListAsync();
                if ( order.Any() )
                {
                    tempNum = order.Max() + 1;
                }
                tableModel.CompanyCode = tempNum;

                // 追加
                _context.Add( tableModel );
                await _context.SaveChangesAsync();
                return RedirectToAction( nameof( Index ) );
            }
            return View( model );
        }

        // GET: SalesCompany/Edit/5
        [Authorize( Policy = nameof( AuthorizationPolicies.GlobalAdministratorOnlyPolicy ) )]
        public async Task<IActionResult> Edit( int? id )
        {
            if ( id == null || _context.Companies == null )
            {
                return NotFound();
            }

            var company = await _context.Companies.FindAsync( id );
            if ( company == null )
            {
                return NotFound();
            }

            var model = new SalesCompany();
            model.CompanyCode = (int)id;
            model.CompanyName = company.CompanyName;
            model.PhoneticName = company.PhoneticName;
            model.Note = company.Note;

            return View( model );
        }

        // POST: SalesCompany/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize( Policy = nameof( AuthorizationPolicies.GlobalAdministratorOnlyPolicy ) )]
        public async Task<IActionResult> Edit( int id, [Bind( "CompanyCode,CompanyName,PhoneticName,Note" )] SalesCompany model )
        {
            if ( id != model.CompanyCode )
            {
                return NotFound();
            }

            if ( ModelState.IsValid )
            {
                var tableModel = await _context.Companies.FindAsync( id );
                tableModel.CompanyName = model.CompanyName;
                tableModel.PhoneticName = model.PhoneticName;
                tableModel.Note = model.Note;
                try
                {
                    _context.Update( tableModel );
                    await _context.SaveChangesAsync();
                }
                catch ( DbUpdateConcurrencyException )
                {
                    if ( !SalesCompanyExists( model.CompanyCode ) )
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction( nameof( Index ) );
            }
            return View( model );
        }

        // GET: SalesCompany/Delete/5
        [Authorize( Policy = nameof( AuthorizationPolicies.GlobalAdministratorOnlyPolicy ) )]
        public async Task<IActionResult> Delete( int? id )
        {
            if ( id == null || _context.Companies == null )
            {
                return NotFound();
            }

            var company = await _context.Companies.FirstOrDefaultAsync( m => m.CompanyCode == id );
            if ( company == null )
            {
                return NotFound();
            }

            var model = new SalesCompany();
            model.CompanyCode = id.Value;
            model.CompanyName = company.CompanyName;
            model.PhoneticName = company.PhoneticName;

            return View( model );
        }

        // POST: SalesCompany/Delete/5
        [HttpPost, ActionName( "Delete" )]
        [ValidateAntiForgeryToken]
        [Authorize( Policy = nameof( AuthorizationPolicies.GlobalAdministratorOnlyPolicy ) )]
        public async Task<IActionResult> DeleteConfirmed( int id )
        {
            if ( _context.Companies == null )
            {
                return Problem( "Entity set 'SampleCRMContext.SalesCompany'  is null." );
            }
            var model = await _context.Companies.FindAsync( id );
            if ( model != null )
            {
                _context.Companies.Remove( model );
            }

            await _context.SaveChangesAsync();
            return RedirectToAction( nameof( Index ) );
        }

        private bool SalesCompanyExists( int id )
        {
            return _context.Companies.Any( e => e.CompanyCode == id );
        }
    }
}
