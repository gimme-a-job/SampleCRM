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
    public class SalesDepartmentController : Controller
    {
        private readonly SampleCRMContext _context;

        public SalesDepartmentController( SampleCRMContext context )
        {
            _context = context;
        }

        //TODO: Indexは使わないが検索の作りによっては利用する可能性があるのでそれまで残しておく(Viewも忘れずに)
        // GET: SalesDepartment
        //public async Task<IActionResult> Index()
        //{
        //      return View(await _context.SalesDepartment.ToListAsync());
        //}

        // GET: SalesDepartment/Details/5
        public async Task<IActionResult> Details( int? id )
        {
            if ( id == null || _context.Companies == null || _context.Departments == null || _context.Employees == null )
            {
                return NotFound();
            }

            // 部署情報を取得
            var department = await _context.Departments.FindAsync( id );
            // URLパラメータ改ざんして存在しないIDを指定した場合はnullになる
            if ( department == null )
            {
                return NotFound();
            }

            var company = await _context.Companies.FindAsync( department.CompanyCode );

            // 部署情報の取得(部署は0以上)
            var employees = await _context.Employees.Where( x => x.DepartmentCode == id ).ToListAsync();

            // ViewModelにデータ登録
            var model = new SalesDepartment();
            model.CompanyCode = company.CompanyCode;
            model.CompanyName = company.CompanyName;
            model.CompanyPhoneticName = company.PhoneticName;
            model.DepartmentCode = department.DepartmentCode;
            model.DepartmentName = department.DepartmentName;
            model.PhoneticName = department.PhoneticName;
            model.PostalCode = department.PostalCode;
            //TODO: 住所関連修正 model.Address = department.Address;
            model.TelNumber = department.TelNumber;
            model.FaxNumber = department.FaxNumber;
            model.Email = department.Email;
            model.Employees = [];
            foreach ( var employee in employees )
            {
                var temp = new SalesEmployee();
                temp.EmployeeCode = employee.EmployeeCode;
                temp.EmployeeName = employee.EmployeeName;
                temp.PhoneticName = employee.PhoneticName;
                temp.Position = employee.Position;
                temp.CellPhoneNumber = employee.CellPhoneNumber;
                temp.Email = employee.Email;
                temp.SNS = employee.SNS;
                model.Employees.Add( temp );
            }

            return View( model );
        }

        // GET: SalesDepartment/Create
        [Authorize( Policy = nameof( AuthorizationPolicies.GlobalAdministratorOnlyPolicy ) )]
        public async Task<IActionResult> Create( int? id )
        {
            if ( id == null ) return NotFound();

            // 営業会社の情報取得
            var company = await _context.Companies.FindAsync( id );
            if ( company == null ) return NotFound();

            // modelに必要情報の登録
            var model = new SalesDepartment();
            model.CompanyCode = company.CompanyCode;
            model.CompanyName = company.CompanyName;
            model.CompanyPhoneticName = company.PhoneticName;

            // 代表部署が既に登録されているか確認する
            var mainDepartment = await _context.Departments.Where( x => x.CompanyCode == id && x.IsMain ).FirstOrDefaultAsync();
            model.EditableMainDepartment = ( mainDepartment == null ) ? true : false;   // 代表部署登録がなければ編集可能

            return View( model );
        }

        // POST: SalesDepartment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize( Policy = nameof( AuthorizationPolicies.GlobalAdministratorOnlyPolicy ) )]
        public async Task<IActionResult> Create(
            [Bind("CompanyCode,CompanyName,CompanyPhoneticName," +
                  "EditableMainDepartment,IsMainDepartment,DepartmentName,PhoneticName,PostalCode,Address,TelNumber,FaxNumber,Email")] SalesDepartment model )
        {
            if ( ModelState.IsValid )
            {
                // テーブルモデルを作成しデータ登録
                var department = new Department();
                department.CompanyCode = model.CompanyCode;
                department.IsMain = model.IsMainDepartment;
                department.DepartmentName = model.DepartmentName;
                department.PhoneticName = model.PhoneticName;
                department.PostalCode = model.PostalCode;
                //TODO: 住所関連修正 
                department.City = model.Address;
                department.TelNumber = model.TelNumber;
                department.FaxNumber = model.FaxNumber;
                department.Email = model.Email;

                // 表示順の設定
                var orders = await _context.Departments.Select( x => x.DisplayOrder ).ToListAsync();
                department.DisplayOrder = orders.Any() ? orders.Max() + 1 : 1;

                _context.Add( department );
                await _context.SaveChangesAsync();

                // 戻り先は販売会社の詳細画面(部署一覧)
                return RedirectToAction( "Details", "SalesCompany", new { @id = model.CompanyCode } );
            }
            return View( model );
        }

        // GET: SalesDepartment/Edit/5
        [Authorize( Policy = nameof( AuthorizationPolicies.GlobalAdministratorOnlyPolicy ) )]
        public async Task<IActionResult> Edit( int? id )
        {
            if ( id == null || _context.Companies == null || _context.Departments == null )
            {
                return NotFound();
            }

            var query = await _context.Departments.Where( x => x.DepartmentCode == id )
                                            .Join( _context.Companies, d => d.CompanyCode, c => c.CompanyCode,
                                                ( department, company ) => new { department, company } ).ToListAsync();
            // URLパラメータ改ざんして存在しないIDを指定した場合は0要素のLISTになる
            if ( !query.Any() ) return NotFound();
            var department = query.First().department;
            var company = query.First().company;

            // ViewModelへデータの登録
            var model = new SalesDepartment();
            model.CompanyCode = company.CompanyCode;
            model.CompanyName = company.CompanyName;
            model.CompanyPhoneticName = company.PhoneticName;
            model.DepartmentCode = department.DepartmentCode;
            model.IsMainDepartment = department.IsMain;
            model.DepartmentName = department.DepartmentName;
            model.PhoneticName = department.PhoneticName;
            model.PostalCode = department.PostalCode;
            //TODO: 住所関連修正 
            model.Address = department.City;
            model.TelNumber = department.TelNumber;
            model.FaxNumber = department.FaxNumber;
            model.Email = department.Email;

            // 代表部署のチェックボックス編集可否の確認
            model.EditableMainDepartment = true;
            if ( !department.IsMain )
            {
                // 自身が代表部署ではなく既に代表部署が存在するなら編集不可
                var mainDepartment = await _context.Departments.Where( x => x.CompanyCode == company.CompanyCode && x.IsMain ).FirstOrDefaultAsync();
                model.EditableMainDepartment = ( mainDepartment == null );
            }

            return View( model );
        }

        // POST: SalesDepartment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize( Policy = nameof( AuthorizationPolicies.GlobalAdministratorOnlyPolicy ) )]
        public async Task<IActionResult> Edit( int id,
            [Bind("CompanyCode,CompanyName,CompanyPhoneticName,EditableMainDepartment," +
                  "DepartmentCode,IsMainDepartment,DepartmentName,PhoneticName,PostalCode,Address,TelNumber,FaxNumber,Email")] SalesDepartment model )
        {
            if ( id != model.DepartmentCode )
            {
                return NotFound();
            }

            if ( ModelState.IsValid )
            {
                // テーブル用Entityにデータ登録
                var department = await _context.Departments.FindAsync( id );
                department.IsMain = model.IsMainDepartment;
                department.DepartmentName = model.DepartmentName;
                department.PhoneticName = model.PhoneticName;
                department.PostalCode = model.PostalCode;
                //TODO: 住所関連修正 
                department.City = model.Address;
                department.TelNumber = model.TelNumber;
                department.FaxNumber = model.FaxNumber;
                department.Email = model.Email;

                try
                {
                    _context.Update( department );
                    await _context.SaveChangesAsync();
                }
                catch ( DbUpdateConcurrencyException )
                {
                    if ( !SalesDepartmentExists( department.DepartmentCode ) )
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                // 戻り先は販売会社の詳細画面(部署一覧)
                return RedirectToAction( "Details", "SalesCompany", new { @id = model.CompanyCode } );
            }
            return View( model );
        }

        // GET: SalesDepartment/Delete/5
        [Authorize( Policy = nameof( AuthorizationPolicies.GlobalAdministratorOnlyPolicy ) )]
        public async Task<IActionResult> Delete( int? id )
        {
            if ( id == null || _context.Companies == null || _context.Departments == null )
            {
                return NotFound();
            }

            // 会社と部署情報の取得
            var query = await _context.Departments.Where( x => x.DepartmentCode == id )
                                .Join( _context.Companies, d => d.CompanyCode, c => c.CompanyCode,
                                    ( department, company ) => new { department, company } ).ToListAsync();
            // URLパラメータ改ざんして存在しないIDを指定した場合は0要素のLISTになる
            if ( !query.Any() ) return NotFound();
            var company = query.First().company;
            var department = query.First().department;

            // ViewModelにデータ登録
            var model = new SalesDepartment();
            model.CompanyCode = company.CompanyCode;
            model.CompanyName = company.CompanyName;
            model.CompanyPhoneticName = company.PhoneticName;
            model.DepartmentCode = department.DepartmentCode;
            model.IsMainDepartment = department.IsMain;
            model.DepartmentName = department.DepartmentName;
            model.PhoneticName = department.PhoneticName;
            model.PostalCode = department.PostalCode;
            //TODO: 住所関連修正 
            model.Address = department.City;
            model.TelNumber = department.TelNumber;
            model.FaxNumber = department.FaxNumber;
            model.Email = department.Email;

            return View( model );
        }

        // POST: SalesDepartment/Delete/5
        [HttpPost, ActionName( "Delete" )]
        [ValidateAntiForgeryToken]
        [Authorize( Policy = nameof( AuthorizationPolicies.GlobalAdministratorOnlyPolicy ) )]
        public async Task<IActionResult> DeleteConfirmed( int id )
        {
            if ( _context.Departments == null )
            {
                return Problem( "Entity set 'SampleCRMContext.SalesDepartment'  is null." );
            }
            var department = await _context.Departments.FindAsync( id );
            if ( department != null )
            {
                _context.Departments.Remove( department );
            }

            await _context.SaveChangesAsync();
            return RedirectToAction( "Details", "ClientCompany", new { @id = department.CompanyCode } );
        }

        private bool SalesDepartmentExists( int id )
        {
            return _context.Departments.Any( e => e.DepartmentCode == id );
        }
    }
}
