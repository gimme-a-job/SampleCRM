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
    public class SalesEmployeeController : Controller
    {
        private readonly SampleCRMContext _context;

        public SalesEmployeeController( SampleCRMContext context )
        {
            _context = context;
        }

        // TODO: Indexは使わないが検索の作りによっては利用する可能性があるのでそれまで残しておく(Viewも忘れずに)
        // GET: SalesEmployee
        //public async Task<IActionResult> Index()
        //{
        //      return View(await _context.SalesEmployee.ToListAsync());
        //}

        // IDはEmployeeCode
        // GET: SalesEmployee/Details/5
        public async Task<IActionResult> Details( int? id )
        {
            if ( id == null )
            {
                return NotFound();
            }

            // RDBからデータ取得
            var employee = await _context.Employees.FindAsync( id );
            // URLパラメータ改ざんして存在しないIDを指定した場合はnullになる
            if ( employee == null )
            {
                return NotFound();
            }

            var department = await _context.Departments.FindAsync( employee.DepartmentCode );
            var company = await _context.Companies.FindAsync( department.CompanyCode );

            // ViewModelにデータ登録
            var viewModel = new SalesEmployee
            {
                CompanyName = company.CompanyName,
                DepartmentCode = department.DepartmentCode,
                DepartmentName = department.DepartmentName,
                EmployeeCode = employee.EmployeeCode,
                EmployeeName = employee.EmployeeName,
                PhoneticName = employee.PhoneticName,
                Position = employee.Position,
                CellPhoneNumber = employee.CellPhoneNumber,
                Email = employee.Email,
                SNS = employee.SNS,
            };

            return View( viewModel );
        }

        // IDはDepartmentCode
        // GET: SalesEmployee/Create
        [Authorize( Policy = nameof( AuthorizationPolicies.GlobalAdministratorOnlyPolicy ) )]
        public async Task<IActionResult> Create( int? id )
        {
            if ( id == null )
            {
                return NotFound();
            }

            var department = await _context.Departments.FindAsync( id );

            // URLパラメータ改ざんして存在しないIDを指定した場合は0要素のLISTになる
            if ( department == null )
            {
                return NotFound();
            }

            var company = await _context.Companies.FindAsync( department.CompanyCode );

            // ViewModeにデータ登録
            var viewModel = new SalesEmployee
            {
                CompanyCode = company.CompanyCode,
                CompanyName = company.CompanyName,
                CompanyPhoneticName = company.PhoneticName,
                DepartmentCode = department.DepartmentCode,
                DepartmentName = department.DepartmentName,
                DepartmentPhoneticName = department.PhoneticName,
            };

            // 営業会社の情報取得
            return View( viewModel );
        }

        // POST: SalesEmployee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize( Policy = nameof( AuthorizationPolicies.GlobalAdministratorOnlyPolicy ) )]
        public async Task<IActionResult> Create(
            [Bind( "DepartmentCode,IsMainEmployee,EmployeeName,PhoneticName,Position,CellPhoneNumber,Email,SNS" )] SalesEmployee viewModel )
        {
            if ( ModelState.IsValid )
            {
                // TableModelに登録
                var employee = new Employee
                {
                    DepartmentCode = viewModel.DepartmentCode,
                    // TODO: 代表担当者かどうかを選べるようにする。
                    // TODO: 在籍しているかどうかを選べるようにする。
                    EmployeeName = viewModel.EmployeeName,
                    PhoneticName = viewModel.PhoneticName,
                    Position = viewModel.Position,
                    CellPhoneNumber = viewModel.CellPhoneNumber,
                    Email = viewModel.Email,
                    SNS = viewModel.SNS,
                };

                // 表示順の設定
                var codeList = await _context.Employees.Select( x => x.DisplayOrder ).ToListAsync();
                employee.DisplayOrder = codeList.Any() ? codeList.Max() + 1 : 1;

                _context.Add( employee );
                await _context.SaveChangesAsync();

                // 戻り先は営業部署の詳細画面(営業担当一覧)
                return RedirectToAction( "Details", "SalesDepartment", new { @id = viewModel.DepartmentCode } );
            }
            return View( viewModel );
        }

        // GET: SalesEmployee/Edit/5
        [Authorize( Policy = nameof( AuthorizationPolicies.GlobalAdministratorOnlyPolicy ) )]
        public async Task<IActionResult> Edit( int? id )
        {
            if ( id == null || _context.Employees == null || _context.Departments == null || _context.Companies == null )
            {
                return NotFound();
            }

            // RDBから現状のデータ取得
            var employee = await _context.Employees.FindAsync( id );
            // URLパラメータ改ざんして存在しないIDを指定した場合はnullになる
            if ( employee == null )
            {
                return NotFound();
            }

            var department = await _context.Departments.FindAsync( employee.DepartmentCode );
            var company = await _context.Companies.FindAsync( department.DepartmentCode );

            // ViewModelにデータ登録
            var viewModel = new SalesEmployee
            {
                CompanyName = company.CompanyName,
                DepartmentCode = department.DepartmentCode,
                DepartmentName = department.DepartmentName,
                EmployeeCode = employee.EmployeeCode,
                EmployeeName = employee.EmployeeName,
                PhoneticName = employee.PhoneticName,
                Position = employee.Position,
                CellPhoneNumber = employee.CellPhoneNumber,
                Email = employee.Email,
                SNS = employee.SNS,
            };

            return View( viewModel );
        }

        // POST: SalesEmployee/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize( Policy = nameof( AuthorizationPolicies.GlobalAdministratorOnlyPolicy ) )]
        public async Task<IActionResult> Edit( int id,
            [Bind("CompanyName,DepartmentCode,DepartmentName," +
                  "EmployeeCode,IsMainEmployee,EmployeeName,PhoneticName,Position,CellPhoneNumber,Email,SNS")] SalesEmployee viewModel )
        {
            if ( id != viewModel.EmployeeCode )
            {
                return NotFound();
            }

            if ( ModelState.IsValid )
            {
                // RDBからデータを取得して上書けるデータを更新する
                var employee = await _context.Employees.FindAsync( viewModel.EmployeeCode );
                // TODO: 代表担当者かどうかを選べるようにする。
                // TODO: 在籍しているかどうかを選べるようにする。
                employee.EmployeeName = viewModel.EmployeeName;
                employee.PhoneticName = viewModel.PhoneticName;
                employee.Position = viewModel.Position;
                employee.CellPhoneNumber = viewModel.CellPhoneNumber;
                employee.Email = viewModel.Email;
                employee.SNS = viewModel.SNS;

                try
                {
                    _context.Update( employee );
                    await _context.SaveChangesAsync();
                }
                catch ( DbUpdateConcurrencyException )
                {
                    if ( !SalesEmployeeExists( employee.EmployeeCode ) )
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                // 戻り先は営業部署の詳細画面(営業担当一覧)
                return RedirectToAction( "Details", "SalesDepartment", new { @id = viewModel.DepartmentCode } );
            }
            return View( viewModel );
        }

        // GET: SalesEmployee/Delete/5
        [Authorize( Policy = nameof( AuthorizationPolicies.GlobalAdministratorOnlyPolicy ) )]
        public async Task<IActionResult> Delete( int? id )
        {
            if ( id == null || _context.Employees == null || _context.Departments == null || _context.Companies == null )
            {
                return NotFound();
            }

            // RDBから現状のデータ取得
            var employee = await _context.Employees.FindAsync( id );
            // URLパラメータ改ざんして存在しないIDを指定した場合はnullになる
            if ( employee == null )
            {
                return NotFound();
            }

            var department = await _context.Departments.FindAsync( employee.DepartmentCode );
            var company = await _context.Companies.FindAsync( department.DepartmentCode );

            // ViewModelにデータ登録
            var viewModel = new SalesEmployee
            {
                CompanyName = company.CompanyName,
                DepartmentCode = department.DepartmentCode,
                DepartmentName = department.DepartmentName,
                EmployeeCode = employee.EmployeeCode,
                EmployeeName = employee.EmployeeName,
                PhoneticName = employee.PhoneticName,
                Position = employee.Position,
                CellPhoneNumber = employee.CellPhoneNumber,
                Email = employee.Email,
                SNS = employee.SNS
            };

            return View( viewModel );
        }

        // POST: SalesEmployee/Delete/5
        [HttpPost, ActionName( "Delete" )]
        [ValidateAntiForgeryToken]
        [Authorize( Policy = nameof( AuthorizationPolicies.GlobalAdministratorOnlyPolicy ) )]
        public async Task<IActionResult> DeleteConfirmed( int id )
        {
            if ( _context.Employees == null )
            {
                return Problem( "Entity set 'SampleCRMContext.SalesEmployee'  is null." );
            }
            var employee = await _context.Employees.FindAsync( id );
            if ( employee != null )
            {
                _context.Employees.Remove( employee );
            }

            await _context.SaveChangesAsync();

            // 戻り先は営業部署の詳細画面(営業担当一覧)
            return RedirectToAction( "Details", "SalesDepartment", new { @id = employee.DepartmentCode } );
        }

        private bool SalesEmployeeExists( int id )
        {
            return _context.Employees.Any( e => e.EmployeeCode == id );
        }
    }
}
