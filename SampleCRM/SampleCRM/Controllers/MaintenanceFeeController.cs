using SampleCRM.Contexts;
using SampleCRM.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace SampleCRM.Controllers
{
    public class MaintenanceFeeController : Controller
    {
        private readonly SampleCRMContext _context;

        public MaintenanceFeeController( SampleCRMContext context )
        {
            _context = context;
        }

        // GET: MaintenanceFeeController
        public ActionResult Index()
        {
            return View();
        }

        // GET: MaintenanceFeeController/Details/5
        public async Task<ActionResult> Details( int? id )
        {
            if ( id == null || await _context.MaintenanceFees.FindAsync( id ) is not { } maintenanceFee )
            {
                return NotFound();
            }

            return View( new MaintenanceFeeListItemViewModel( maintenanceFee ) );
        }

        // GET: MaintenanceFeeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MaintenanceFeeController/Create
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

        // GET: MaintenanceFeeController/Edit/5
        public ActionResult Edit( int id )
        {
            return View();
        }

        // POST: MaintenanceFeeController/Edit/5
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

        // GET: MaintenanceFeeController/Delete/5
        public ActionResult Delete( int id )
        {
            return View();
        }

        // POST: MaintenanceFeeController/Delete/5
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
