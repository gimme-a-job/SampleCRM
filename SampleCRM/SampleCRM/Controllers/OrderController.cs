using DocumentFormat.OpenXml.Drawing.Charts;
using SampleCRM.Contexts;
using SampleCRM.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SampleCRM.Controllers
{
    public class OrderController : Controller
    {
        private readonly SampleCRMContext _context;

        public OrderController( SampleCRMContext context )
        {
            _context = context;
        }

        // GET: OrderController
        public ActionResult Index()
        {
            return View();
        }

        // GET: OrderController/Details/5
        public async Task<ActionResult> Details( int? id )
        {
            if ( id is null )
            {
                return NotFound();
            }

            var viewModel = await ( from Order in _context.Orders
                                    join DeliveryStatus in _context.DeliveryStatuses on Order.DeliveryStatusCode equals DeliveryStatus.DeliveryStatusCode
                                    join ClientEmployeeToOrder in _context.ClientEmployeeToOrders on Order.OrderCode equals ClientEmployeeToOrder.OrderCode
                                    join ClientEmployee in _context.Employees on ClientEmployeeToOrder.EmployeeCode equals ClientEmployee.EmployeeCode
                                    join SalesEmployeeToOrder in _context.SalesEmployeeToOrders on Order.OrderCode equals SalesEmployeeToOrder.OrderCode
                                    join SalesEmployee in _context.Employees on SalesEmployeeToOrder.EmployeeCode equals SalesEmployee.EmployeeCode
                                    join orderToPayment in _context.OrderToPayments on Order.OrderCode equals orderToPayment.OrderCode into groupJoinnedOrderToPayments
                                    from OrderToPayment in groupJoinnedOrderToPayments.DefaultIfEmpty()
                                    join Payment in _context.Payments on OrderToPayment.PaymentCode equals Payment.PaymentCode into groupJoinnedPayments
                                    from Payment in groupJoinnedPayments.DefaultIfEmpty()
                                    join PaymentMethod in _context.PaymentMethods on Payment.PaymentMethodCode equals PaymentMethod.PaymentMethodCode into groupJoinnedPaymentMethods
                                    from PaymentMethod in groupJoinnedPaymentMethods.DefaultIfEmpty()
                                    where Order.OrderCode == id
                                    select new OrderDetailsViewModel( Order, DeliveryStatus, ClientEmployee, SalesEmployee, Payment, PaymentMethod ) )
                                    .SingleOrDefaultAsync();

            if ( viewModel is null )
            {
                return NotFound();
            }

            viewModel.ContractList = new ContractListViewModel(
                await ( from Contract in _context.Contracts
                        join ContractStatus in _context.ContractStatuses on Contract.ContractStatusCode equals ContractStatus.ContractStatusCode
                        join UpdateMethod in _context.UpdateMethods on Contract.UpdateMethodCode equals UpdateMethod.UpdateMethodCode
                        join SystemKind in _context.SystemKinds on Contract.SystemKindCode equals SystemKind.SystemKindCode
                        join CDShippingDepartment in _context.Departments on Contract.CDShippingDepartmentCode equals CDShippingDepartment.DepartmentCode into groupJoinnedCDShippingDepartments
                        from CDShippingDepartment in groupJoinnedCDShippingDepartments.DefaultIfEmpty()
                        join ContractToSystemLicense in _context.ContractToSystemLicenses on Contract.ContractCode equals ContractToSystemLicense.ContractCode into groupJoinnedContractToSystemLicense
                        from ContractToSystemLicense in groupJoinnedContractToSystemLicense.DefaultIfEmpty()
                        join SystemLicense in _context.SystemLicenses on ContractToSystemLicense.SystemLicenseCode equals SystemLicense.SystemLicenseCode into groupJoinnedSystemLicenses
                        from SystemLicense in groupJoinnedSystemLicenses.DefaultIfEmpty()
                        join ContractToDataLicense in _context.ContractToDataLicenses on Contract.ContractCode equals ContractToDataLicense.ContractCode into groupJoinnedContractToDataLicenses
                        from ContractToDataLicense in groupJoinnedContractToDataLicenses.DefaultIfEmpty()
                        join DataLicense in _context.DataLicenses on ContractToDataLicense.DataLicenseCode equals DataLicense.DataLicenseCode into groupJoinnedDataLicense
                        from DataLicense in groupJoinnedDataLicense.DefaultIfEmpty()
                        where Contract.OrderCode == id
                        select new ContractListItemViewModel(
                            Contract,
                            ContractStatus,
                            UpdateMethod,
                            SystemKind,
                            CDShippingDepartment,
                            SystemLicense,
                            DataLicense
                        ) ).ToListAsync() )
            {
                OrderCode = viewModel.OrderCode
            };

            viewModel.MaintenanceFeeBillList = await ( from MaintenanceFee in _context.MaintenanceFees
                                                       join maintenanceFeeToBill in _context.MaintenanceFeeToBills on MaintenanceFee.MaintenanceFeeCode equals maintenanceFeeToBill.MaintenanceFeeCode into groupJoinnedMaintenanceFeeToBills
                                                       from MaintenanceFeeToBill in groupJoinnedMaintenanceFeeToBills.DefaultIfEmpty()
                                                       join MaintenanceFeeBill in _context.MaintenanceFeeBills on MaintenanceFeeToBill.BillCode equals MaintenanceFeeBill.BillCode into groupJoinnedMaintenanceFeeBills
                                                       from MaintenanceFeeBill in groupJoinnedMaintenanceFeeBills.DefaultIfEmpty()
                                                       join BillingDepartment in _context.Departments on MaintenanceFeeBill.BillingDepartmentCode equals BillingDepartment.DepartmentCode into groupJoinnedBillingDepartments
                                                       from BillingDepartment in groupJoinnedBillingDepartments.DefaultIfEmpty()
                                                       join maintenanceFeeBillToClearance in _context.MaintenanceFeeBillToClearances on MaintenanceFeeBill.BillCode equals maintenanceFeeBillToClearance.BillCode into groupJoinnedMaintenanceFeeBillToClearances
                                                       from MaintenanceFeeBillToClearance in groupJoinnedMaintenanceFeeBillToClearances.DefaultIfEmpty()
                                                       join MaintenanceFeeClearance in _context.MaintenanceFeeClearances on MaintenanceFeeBillToClearance.ClearanceCode equals MaintenanceFeeClearance.ClearanceCode into groupJoinnedMaintenanceFeeClearances
                                                       from MaintenanceFeeClearance in groupJoinnedMaintenanceFeeClearances.DefaultIfEmpty()
                                                       join ContractToMaintenanceFee in _context.ContractToMaintenanceFees on MaintenanceFee.MaintenanceFeeCode equals ContractToMaintenanceFee.MaintenanceFeeCode
                                                       join Contract in _context.Contracts on ContractToMaintenanceFee.ContractCode equals Contract.ContractCode
                                                       join Order in _context.Orders on Contract.OrderCode equals Order.OrderCode
                                                       where Order.OrderCode == id
                                                       orderby MaintenanceFeeBill.BillingDate descending
                                                       group new
                                                       {
                                                           MaintenanceFee,
                                                           MaintenanceFeeBill,
                                                           BillingDepartment,
                                                           MaintenanceFeeClearance
                                                       } by MaintenanceFeeBill.BillCode into grouped
                                                       select new MaintenanceFeeBillListItemViewModel(
                                                           grouped.Select( x => x.MaintenanceFee ).OrderBy( x => x.MaintenanceStartDate ).ToList(),
                                                           grouped.Single().MaintenanceFeeBill,
                                                           grouped.Single().BillingDepartment,
                                                           grouped.Select( x => x.MaintenanceFeeClearance ).OrderBy( x => x.ClearanceDepositDate ).ToList() )
                                          ).ToListAsync();


            return View( viewModel );
        }

        // GET: OrderController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OrderController/Create
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

        // GET: OrderController/Edit/5
        public ActionResult Edit( int id )
        {
            return View();
        }

        // POST: OrderController/Edit/5
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

        // GET: OrderController/Delete/5
        public ActionResult Delete( int id )
        {
            return View();
        }

        // POST: OrderController/Delete/5
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
