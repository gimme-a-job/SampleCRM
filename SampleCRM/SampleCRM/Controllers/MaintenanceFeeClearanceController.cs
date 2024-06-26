using SampleCRM.Contexts;
using SampleCRM.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace SampleCRM.Controllers
{
    public class MaintenanceFeeClearanceController : Controller
    {
        private readonly SampleCRMContext _context;

        public MaintenanceFeeClearanceController( SampleCRMContext context )
        {
            _context = context;
        }

        // GET: MaintenanceFeeClearanceController
        public ActionResult Index()
        {
            return View();
        }

        // GET: MaintenanceFeeClearanceController/Details/5
        public async Task<ActionResult> Details( int? id )
        {
            if ( id == null || await _context.MaintenanceFeeClearances.FindAsync( id ) is not { } maintenanceFeeClearance )
            {
                return NotFound();
            }

            return View( new MaintenanceFeeClearanceListItemViewModel( maintenanceFeeClearance ) );
        }

        // GET: MaintenanceFeeClearanceController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MaintenanceFeeClearanceController/Create
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

        // GET: MaintenanceFeeClearanceController/Edit/5
        public ActionResult Edit( int id )
        {
            return View();
        }

        // POST: MaintenanceFeeClearanceController/Edit/5
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

        // GET: MaintenanceFeeClearanceController/Delete/5
        public ActionResult Delete( int id )
        {
            return View();
        }

        // POST: MaintenanceFeeClearanceController/Delete/5
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
