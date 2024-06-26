using SampleCRM.Contexts;
using SampleCRM.Models.TableModels;
using SampleCRM.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace SampleCRM.Controllers
{
    public class MaintenanceFeeBillController : Controller
    {
        private readonly SampleCRMContext _context;

        public MaintenanceFeeBillController( SampleCRMContext context )
        {
            _context = context;
        }

        // GET: MaintenanceFeeBillController
        public ActionResult Index()
        {
            return View();
        }

        // GET: MaintenanceFeeBillController/Details/5
        public async Task<ActionResult> Details( int? id )
        {
            // 指定された請求書コードで請求が存在しない可能性もあるため、存在チェックは必要
            if ( id == null
                || await _context.MaintenanceFeeBills.FindAsync( id ) is not { } maintenanceFeeBill )
            {
                return NotFound();
            }

            // 請求情報詳細
            var viewModel = new MaintenanceFeeBillListItemViewModel(
                    maintenanceFeeBill,
                    await _context.Departments.FindAsync( maintenanceFeeBill.BillingDepartmentCode ) );

            // (請求コードに紐付く) 保守料情報一覧
            viewModel.MaintenanceFees = from MaintenanceFee in _context.MaintenanceFees
                                        join MaintenanceFeeToBill in _context.MaintenanceFeeToBills
                                        on MaintenanceFee.MaintenanceFeeCode equals MaintenanceFeeToBill.MaintenanceFeeCode
                                        where MaintenanceFeeToBill.BillCode == maintenanceFeeBill.BillCode
                                        select new MaintenanceFeeListItemViewModel( MaintenanceFee );

            // (請求コードに紐付く) 消込情報一覧
            viewModel.MaintenanceFeeClearances = from MaintenanceFeeClearance in _context.MaintenanceFeeClearances
                                                 join MaintenanceFeeBillToClearance in _context.MaintenanceFeeBillToClearances
                                                 on MaintenanceFeeClearance.ClearanceCode equals MaintenanceFeeBillToClearance.ClearanceCode
                                                 where MaintenanceFeeBillToClearance.BillCode == maintenanceFeeBill.BillCode
                                                 select new MaintenanceFeeClearanceListItemViewModel( MaintenanceFeeClearance );

            return View( viewModel );
        }

        // GET: MaintenanceFeeBillController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MaintenanceFeeBillController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( IFormCollection collection )
        {
            try
            {
                return RedirectToAction( nameof( Index ) );
            }
            catch
            {
                return View();
            }
        }

        // GET: MaintenanceFeeBillController/Edit/5
        public ActionResult Edit( int id )
        {
            return View();
        }

        // POST: MaintenanceFeeBillController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( int id, IFormCollection collection )
        {
            try
            {
                return RedirectToAction( nameof( Index ) );
            }
            catch
            {
                return View();
            }
        }

        // GET: MaintenanceFeeBillController/Delete/5
        public ActionResult Delete( int id )
        {
            return View();
        }

        // POST: MaintenanceFeeBillController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete( int id, IFormCollection collection )
        {
            try
            {
                return RedirectToAction( nameof( Index ) );
            }
            catch
            {
                return View();
            }
        }
    }
}
