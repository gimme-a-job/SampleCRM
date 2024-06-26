using SampleCRM.Common;
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
    public class ClientDepartmentController : Controller
    {
        private readonly SampleCRMContext _context;

        public ClientDepartmentController( SampleCRMContext context )
        {
            _context = context;
        }

        // TODO: Indexは使わないが検索の作りによっては利用する可能性があるのでそれまで残しておく(Viewも忘れずに)
        // GET: ClientDepartment
        public async Task<IActionResult> Index()
        {
            return View( await _context.Departments.ToListAsync() );
        }

        // GET: ClientDepartment/Details/5
        // idは部署コード
        public async Task<IActionResult> Details( int? id )
        {
            if ( id == null || _context.Companies == null || _context.Departments == null || _context.Employees == null )
            {
                return NotFound();
            }

            // 会社情報、部署情報、担当者情報の取得
            var department = await _context.Departments.FindAsync( id );
            if ( department == null )
            {
                return NotFound();
            }

            var employee = await _context.Employees.SingleOrDefaultAsync( x => x.DepartmentCode == id && x.IsMain );
            if ( employee == null )
            {
                return NotFound();
            }

            var company = await _context.Companies.FindAsync( department.CompanyCode );
            if ( company == null )
            {
                return NotFound();
            }

            var model = GetViewModelFromTableEntities( company, department, employee, true );

            return View( model );
        }

        // GET: ClientDepartment/Create
        // idは会社コード
        [Authorize( Policy = nameof( AuthorizationPolicies.EditorPolicy ) )]
        public async Task<IActionResult> Create( int? id )
        {
            if ( id == null )
            {
                return NotFound();
            }

            // 会社名取得
            var company = await _context.Companies.FindAsync( id );
            // URLパラメータ改ざんして存在しないIDを指定した場合はnullになる
            if ( company == null )
            {
                return NotFound();
            }
            var model = new ClientDepartmentViewModel();
            model.CompanyCode = id.Value;
            model.CompanyName = company.CompanyName;
            model.CompanyPhoneticName = company.PhoneticName;

            // 代表部署が既に登録されているか確認する
            var mainDepartment = await _context.Departments.Where( x => x.CompanyCode == model.CompanyCode && x.IsMain ).FirstOrDefaultAsync();
            model.EditableMainDepartment = ( mainDepartment == null ) ? true : false; // 代表部署登録がなければ編集可能

            return View( model );
        }

        // POST: ClientDepartment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize( Policy = nameof( AuthorizationPolicies.EditorPolicy ) )]
        public async Task<IActionResult> Create(
            [Bind( "CompanyCode,EditableMainDepartment," +
                   "IsMainDepartment,DepartmentName,DepartmentPhoneticName,DepartmentPostalCode,DepartmentAddress,DepartmentTelNumber,DepartmentFaxNumber,DepartmentEmail,"+
                   "EmployeeName,EmployeePhoneticName,EmployeePosition,EmployeeTelNumber,EmployeeEmail,EmployeeSNS")] ClientDepartmentViewModel model )
        {
            if ( ModelState.IsValid )
            {
                // 部署の登録
                var department = new Department
                {
                    CompanyCode = model.CompanyCode,
                    IsMain = model.IsMainDepartment,
                    DepartmentName = model.DepartmentName,
                    PhoneticName = model.DepartmentPhoneticName,
                    PostalCode = model.DepartmentPostalCode,
                    //TODO: 住所関連修正 department.Address = model.DepartmentAddress;
                    TelNumber = model.DepartmentTelNumber,
                    FaxNumber = model.DepartmentFaxNumber,
                    Email = model.DepartmentEmail
                };
                var order = await _context.Departments.Select( x => x.DisplayOrder ).ToListAsync();
                department.DisplayOrder = order.Any() ? order.Max() + 1 : 1;
                _context.Departments.Add( department );

                // 自動採番のDepartmentCodeを確定させる。
                await _context.SaveChangesAsync();

                // 担当者の登録
                var employee = new Employee
                {
                    DepartmentCode = department.DepartmentCode, // 確定したDepartmentCodeを担当者に付与。
                    IsMain = true, // 契約会社の担当者は一人のため必ず代表担当者
                    EmployeeName = model.EmployeeName,
                    PhoneticName = model.EmployeePhoneticName,
                    Position = model.EmployeePosition,
                    CellPhoneNumber = model.EmployeeTelNumber,
                    Email = model.EmployeeEmail,
                    SNS = model.EmployeeSNS
                };
                order = await _context.Employees.Select( x => x.DisplayOrder ).ToListAsync();
                employee.DisplayOrder = order.Any() ? order.Max() + 1 : 1;
                _context.Employees.Add( employee );

                await _context.SaveChangesAsync();

                // 戻り先は契約会社の詳細画面(部署一覧)
                return RedirectToAction( "Details", "ClientCompany", new { @id = model.CompanyCode } );
            }
            return View( model );
        }

        // GET: ClientDepartment/Edit/5
        [Authorize( Policy = nameof( AuthorizationPolicies.EditorPolicy ) )]
        public async Task<IActionResult> Edit( int? id )
        {
            if ( id == null || _context.Companies == null || _context.Departments == null || _context.Employees == null )
            {
                return NotFound();
            }

            // 会社情報、部署情報、担当者情報の取得
            var department = await _context.Departments.FindAsync( id );
            if ( department == null )
            {
                return NotFound();
            }

            var employee = await _context.Employees.SingleOrDefaultAsync( x => x.DepartmentCode == id && x.IsMain );
            if ( employee == null )
            {
                return NotFound();
            }

            var company = await _context.Companies.FindAsync( department.CompanyCode );
            if ( company == null )
            {
                return NotFound();
            }

            var model = GetViewModelFromTableEntities( company, department, employee );

            // 代表部署のチェックボックスの操作可否
            // 自分が代表部署か代表部署が存在しない場合は編集可能
            // 自分が非代表部署で代表部署が存在する場合は編集不可
            if ( department.IsMain )  // 自分が代表部署の場合は編集可能
            {
                model.EditableMainDepartment = true;
            }
            else    // 自分が非代表部署
            {
                var mainDepartment = await _context.Departments.Where( x => x.CompanyCode == model.CompanyCode && x.IsMain ).FirstOrDefaultAsync();
                model.EditableMainDepartment = ( mainDepartment == null ) ? true : false;    // 代表部署登録がなければ編集可能
            }

            return View( model );
        }

        // POST: ClientDepartment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize( Policy = nameof( AuthorizationPolicies.EditorPolicy ) )]
        public async Task<IActionResult> Edit( int id,
            [Bind( "CompanyCode,EditableMainDepartment,DepartmentCode,EmployeeCode,DepartmentDisplayOrder,EmployeeDisplayOrder," +
                   "IsMainDepartment,DepartmentName,DepartmentPhoneticName,DepartmentPostalCode,DepartmentAddress,DepartmentTelNumber,DepartmentFaxNumber,DepartmentEmail,"+
                   "EmployeeName,EmployeePhoneticName,EmployeePosition,EmployeeTelNumber,EmployeeEmail,EmployeeSNS")] ClientDepartmentViewModel model )
        {
            if ( id != model.DepartmentCode )
            {
                return NotFound();
            }

            if ( ModelState.IsValid )
            {
                // 部署用更新Entityの作成
                var department = new Department();
                department.DepartmentCode = model.DepartmentCode;
                department.CompanyCode = model.CompanyCode;
                department.IsMain = model.IsMainDepartment;
                department.DepartmentName = model.DepartmentName;
                department.PhoneticName = model.DepartmentPhoneticName;
                department.PostalCode = model.DepartmentPostalCode;
                //TODO: 住所関連修正 
                department.City = model.DepartmentAddress;
                department.TelNumber = model.DepartmentTelNumber;
                department.FaxNumber = model.DepartmentFaxNumber;
                department.Email = model.DepartmentEmail;
                department.DisplayOrder = model.DepartmentDisplayOrder;

                // 担当者更新用Entityの作成
                var employee = new Employee();
                employee.EmployeeCode = model.EmployeeCode;
                employee.DepartmentCode = model.DepartmentCode;
                employee.IsMain = true; // 契約会社の場合は必ずメイン担当者
                employee.EmployeeName = model.EmployeeName;
                employee.PhoneticName = model.EmployeePhoneticName;
                employee.Position = model.EmployeePosition;
                employee.CellPhoneNumber = model.EmployeeTelNumber;
                employee.Email = model.EmployeeEmail;
                employee.SNS = model.EmployeeSNS;
                employee.DisplayOrder = model.EmployeeDisplayOrder;

                try
                {
                    _context.Update( department );
                    _context.Update( employee );
                    await _context.SaveChangesAsync();
                }
                catch ( DbUpdateConcurrencyException )
                {
                    if ( !DepartmentsExists( model.DepartmentCode ) )
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction( "Details", "ClientCompany", new { @id = model.CompanyCode } );
            }
            return View( model );
        }

        // GET: ClientDepartment/Delete/5
        [Authorize( Policy = nameof( AuthorizationPolicies.EditorPolicy ) )]
        public async Task<IActionResult> Delete( int? id )
        {
            if ( id == null || _context.Companies == null || _context.Departments == null || _context.Employees == null )
            {
                return NotFound();
            }

            // 会社情報、部署情報、担当者情報の取得
            var department = await _context.Departments.FindAsync( id );
            if ( department == null )
            {
                return NotFound();
            }

            var employee = await _context.Employees.SingleOrDefaultAsync( x => x.DepartmentCode == id && x.IsMain );
            if ( employee == null )
            {
                return NotFound();
            }
            var company = await _context.Companies.FindAsync( department.CompanyCode );
            if ( company == null )
            {
                return NotFound();
            }

            var model = GetViewModelFromTableEntities( company, department, employee );

            return View( model );
        }

        // POST: ClientDepartment/Delete/5
        [HttpPost, ActionName( "Delete" )]
        [ValidateAntiForgeryToken]
        [Authorize( Policy = nameof( AuthorizationPolicies.EditorPolicy ) )]
        public async Task<IActionResult> DeleteConfirmed( int id )
        {
            if ( _context.Departments == null || _context.Employees == null )
            {
                return Problem( "Entity set 'SampleCRMContext.Departments Employees'  is null." );
            }

            // 部署の削除
            var department = await _context.Departments.FindAsync( id );
            int companyCode = department.CompanyCode;
            if ( department != null )
            {
                _context.Departments.Remove( department );
            }

            // 担当者の削除
            var employee = await _context.Employees.Where( x => x.DepartmentCode == id ).FirstOrDefaultAsync();
            if ( employee != null )
            {
                _context.Employees.Remove( employee );
            }

            await _context.SaveChangesAsync();
            return RedirectToAction( "Details", "ClientCompany", new { @id = companyCode } );
        }

        private bool DepartmentsExists( int id )
        {
            return _context.Departments.Any( e => e.DepartmentCode == id );
        }

        /// <summary>
        /// 会社、部署、担当者情報をViewModelに格納して返す
        /// </summary>
        /// <param name="company">会社情報テーブルEntity</param>
        /// <param name="department">部署情報テーブルEntity</param>
        /// <param name="employee">担当者テーブルEntity</param>
        /// <returns></returns>
        private ClientDepartmentViewModel GetViewModelFromTableEntities( Company company, Department department, Employee employee, bool isReplaceNullString = false )
        {
            var model = new ClientDepartmentViewModel();

            // ViewModelへ登録
            model.CompanyCode = company.CompanyCode;
            model.CompanyName = company.CompanyName;
            model.CompanyPhoneticName = company.PhoneticName;
            model.DepartmentCode = department.DepartmentCode;
            model.DepartmentName = department.DepartmentName;
            model.DepartmentPhoneticName = department.PhoneticName;
            model.IsMainDepartment = department.IsMain;
            model.DepartmentPostalCode = department.PostalCode;
            //TODO: 住所関連修正 
            model.DepartmentAddress = ReplaceNullString( department.City );
            model.DepartmentTelNumber = ReplaceNullString( department.TelNumber );
            model.DepartmentFaxNumber = ReplaceNullString( department.FaxNumber );
            model.DepartmentEmail = department.Email; // 固定文字にも"EmailAddressAttribute"が働いてしまうため、置き換えはしない。
            model.DepartmentDisplayOrder = department.DisplayOrder;
            model.EmployeeCode = employee.EmployeeCode;
            model.EmployeeName = ReplaceNullString( employee.EmployeeName );
            model.EmployeePhoneticName = employee.PhoneticName;
            model.EmployeePosition = ReplaceNullString( employee.Position );
            model.EmployeeTelNumber = ReplaceNullString( employee.CellPhoneNumber );
            model.EmployeeEmail = employee.Email; // 固定文字にも"EmailAddressAttribute"が働いてしまうため、置き換えはしない。
            model.EmployeeSNS = ReplaceNullString( employee.SNS );
            model.EmployeeDisplayOrder = employee.DisplayOrder;

            return model;

            string ReplaceNullString( string target )
                => isReplaceNullString ? target.ToInvalidStrIsNullOrEmpty() : target;
        }
    }
}
