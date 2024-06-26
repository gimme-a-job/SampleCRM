using SampleCRM.Contexts;
using SampleCRM.Models.TableModels;
using SampleCRM.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Company = SampleCRM.Models.TableModels.Company;

namespace SampleCRM.Common
{
    public static class SampleContextExtensions
    {
        #region Join時に使用するクラス

        // TODO: 使わないようだったら消す
        //private class DenormalizedCompany
        //{
        //    Company Company { get; set; }
        //    CompanyStatus CompanyStatus { get; set; }
        //}

        public class JoinnedCompanyWithDepartment
        {
            public Company Company { get; set; }
            public Department Department { get; set; }
        }

        private class JoinnedDepartmentWithEmployee
        {
            public Department Department { get; set; }
            public Employee Employee { get; set; }
        }

        /// <summary>
        /// 「会社」と「担当者」の結合結果
        /// </summary>
        public class JoinnedCompanyWithEmployee
        {
            public Company Company { get; set; }

            public Department Department { get; set; }

            public Employee Employee { get; set; }
        }

        /// <summary>
        /// 「顧客部署」と「発注」の結合結果
        /// </summary>
        private class JoinnedClientDepartmentWithOrder
        {
            public Department ClientDepartment { get; set; }
            public Employee ClientEmployee { get; set; }
            public Order Order { get; set; }
        }

        /// <summary>
        /// 「顧客担当者」と「発注」の結合結果
        /// </summary>
        private class JoinnedClientEmployeeWithOrder
        {
            public Employee ClientEmployee { get; set; }
            public Order Order { get; set; }
        }

        /// <summary>
        /// 「営業担当者」と「発注」の結合結果
        /// </summary>
        private class JoinnedSalesEmployeeWithOrder
        {
            public Employee SalesEmployee { get; set; }
            public Order Order { get; set; }
        }

        private class JoinnedClientDepartmentWithContract
        {
            public Department ClientDepartment { get; set; }
            public Contract Contract { get; set; }
        }

        private class DenormalizedContract
        {
            public Contract Contract { get; set; }
            public ContractStatus ContractStatus { get; set; }
            public UpdateMethod UpdateMethod { get; set; }
            public SystemKind SystemKind { get; set; }
            public Department CDShippingDepartment { get; set; }
        }

        /// <summary>
        /// 「発注」と「契約」の結合結果
        /// </summary>
        private class JoinnedOrderWithContract
        {
            public Order Order { get; set; }
            public Contract Contract { get; set; }
        }

        #endregion

        #region テーブルを結合してIQueryableで返す
        /// <summary>
        /// 「会社」を「部署」を結合した結果を取得する
        /// </summary>
        /// <param name="companies"></param>
        /// <param name="departments"></param>
        /// <returns></returns>
        public static IQueryable<JoinnedCompanyWithDepartment> GetJoinnedCompaniesWithDepartments( this SampleCRMContext context )
            => context.Companies
                  .Join( context.Departments,
                            company => company.CompanyCode,
                            department => department.CompanyCode,
                       ( company, department )
                => new JoinnedCompanyWithDepartment
                {
                    Company = company,
                    Department = department
                } );

        /// <summary>
        /// 「部署」と「担当者」を結合した結果を取得する
        /// </summary>
        /// <param name="departments"></param>
        /// <param name="employees"></param>
        /// <returns></returns>
        private static IQueryable<JoinnedDepartmentWithEmployee> GetJoinnedDepartmentsWithEmployees( this SampleCRMContext context )
            => context.Departments
                  .Join( context.Employees,
                            department => department.DepartmentCode,
                            employee => employee.DepartmentCode,
                       ( department, employee )
                => new JoinnedDepartmentWithEmployee
                {
                    Department = department,
                    Employee = employee
                } );

        /// <summary>
        /// 「会社」から「担当者」までを結合した結果を取得する
        /// </summary>
        /// <param name="companies"></param>
        /// <param name="employees"></param>
        /// <returns></returns>
        public static IQueryable<JoinnedCompanyWithEmployee> GetJoinnedCompaniesWithEmployees( this SampleCRMContext context )
            => context.GetJoinnedCompaniesWithDepartments()
                  .Join( context.GetJoinnedDepartmentsWithEmployees(),
                            companyDepartment => companyDepartment.Department.DepartmentCode,
                            departmentEmployee => departmentEmployee.Department.DepartmentCode,
                       ( companyDepartment, departmentEmployee )
                => new JoinnedCompanyWithEmployee
                {
                    Company = companyDepartment.Company,
                    Department = companyDepartment.Department,
                    Employee = departmentEmployee.Employee
                } );

        /// <summary>
        /// 「顧客部署」から「発注」までを結合した結果を取得する
        /// </summary>
        /// <param name="clientDepartments"></param>
        /// <param name="orders"></param>
        /// <returns></returns>
        private static IQueryable<JoinnedClientDepartmentWithOrder> GetJoinnedClientDepartmentWithOrders( this SampleCRMContext context )
            => context.GetJoinnedDepartmentsWithEmployees()
                  .Join( context.GetJoinnedClientEmployeesWithOrders(),
                            clientDepartmentEmployee => clientDepartmentEmployee.Employee.EmployeeCode,
                            clientEmployeeOrder => clientEmployeeOrder.ClientEmployee.EmployeeCode,
                       ( clientDepartmentEmployee, clientEmployeeOrder )
                => new JoinnedClientDepartmentWithOrder
                {
                    ClientDepartment = clientDepartmentEmployee.Department,
                    ClientEmployee = clientEmployeeOrder.ClientEmployee,
                    Order = clientEmployeeOrder.Order,
                } );


        /// <summary>
        /// 「顧客担当者」と「発注」を結合した結果を取得する
        /// </summary>
        /// <param name="clientEmployees"></param>
        /// <param name="orders"></param>
        /// <returns></returns>
        private static IQueryable<JoinnedClientEmployeeWithOrder> GetJoinnedClientEmployeesWithOrders( this SampleCRMContext context )
            => context.Employees
                  .Join( context.ClientEmployeeToOrders,
                            clientEmployee => clientEmployee.EmployeeCode,
                            clientEmployeeToOrder => clientEmployeeToOrder.EmployeeCode,
                       ( clientEmployee, clientEmployeeToOrder )
                => new { clientEmployee, clientEmployeeToOrder } )
                  .Join( context.Orders,
                            joinned => joinned.clientEmployeeToOrder.OrderCode,
                            order => order.OrderCode,
                       ( joinned, order )
                => new JoinnedClientEmployeeWithOrder
                {
                    ClientEmployee = joinned.clientEmployee,
                    Order = order
                } );

        /// <summary>
        /// 「営業担当者」と「発注」を結合した結果を取得する
        /// </summary>
        /// <param name="salesEmployees"></param>
        /// <param name="orders"></param>
        /// <returns></returns>
        private static IQueryable<JoinnedSalesEmployeeWithOrder> GetJoinnedSalesEmployeesWithOrders( this SampleCRMContext context )
            => context.Employees
                  .Join( context.SalesEmployeeToOrders,
                            salesEmployee => salesEmployee.EmployeeCode,
                            salesEmployeeToOrder => salesEmployeeToOrder.EmployeeCode,
                       ( salesEmployee, salesEmployeeToOrder )
                => new { salesEmployee, salesEmployeeToOrder } )
                  .Join( context.Orders,
                            joinned => joinned.salesEmployeeToOrder.OrderCode,
                            order => order.OrderCode,
                       ( joinned, order )
                => new JoinnedSalesEmployeeWithOrder
                {
                    SalesEmployee = joinned.salesEmployee,
                    Order = order
                } );

        /// <summary>
        /// 「発注」と「契約」を結合した結果を取得する
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static IQueryable<JoinnedOrderWithContract> GetJoinnedOrdersWithContracts( this SampleCRMContext context )
            => context.Orders
                  .Join( context.Contracts,
                            order => order.OrderCode,
                            contract => contract.OrderCode,
                       ( order, contract )
                => new JoinnedOrderWithContract
                {
                    Order = order,
                    Contract = contract
                } );

        /// <summary>
        /// 「顧客部署」から「契約」までを結合した結果を取得する
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static IQueryable<JoinnedClientDepartmentWithContract> GetJoinnedClientDepartmentsWithContracts( this SampleCRMContext context )
            => context.GetJoinnedClientDepartmentWithOrders()
                  .Join( context.GetJoinnedOrdersWithContracts(),
                            clientDepartmentOrder => clientDepartmentOrder.Order.OrderCode,
                            orderContract => orderContract.Order.OrderCode,
                       ( clientDepartmentOrder, orderContract )
                => new JoinnedClientDepartmentWithContract
                {
                    ClientDepartment = clientDepartmentOrder.ClientDepartment,
                    Contract = orderContract.Contract,
                } );

        private static IQueryable<DenormalizedContract> GetDenormalizedContracts( this SampleCRMContext context )
            => context.Contracts
                  .Join( context.ContractStatuses,
                            contract => contract.ContractStatusCode,
                            contractStatus => contractStatus.ContractStatusCode,
                       ( contract, contractStatus )
                => new { contract, contractStatus } )
                  .Join( context.UpdateMethods,
                            joinned => joinned.contract.UpdateMethodCode,
                            updateMethod => updateMethod.UpdateMethodCode,
                       ( joinned, updateMethod )
                => new { joinned.contract, joinned.contractStatus, updateMethod } )
                  .Join( context.SystemKinds,
                            joinned => joinned.contract.SystemKindCode,
                            systemKind => systemKind.SystemKindCode,
                       ( joinned, systemKind )
                => new { joinned.contract, joinned.contractStatus, joinned.updateMethod, systemKind } )
                  .GroupJoin( context.Departments,
                                joinned => joinned.contract.CDShippingDepartmentCode,
                                cdShippingDepartment => cdShippingDepartment.DepartmentCode,
                            ( joinned, groupJoinedCDShippingDepartments )
                => new { joinned.contract, joinned.contractStatus, joinned.updateMethod, joinned.systemKind, groupJoinedCDShippingDepartments } )
                  .SelectMany( joinned => joinned.groupJoinedCDShippingDepartments.DefaultIfEmpty(),
                            ( joinned, cdShippingDepartment )
                => new DenormalizedContract
                {
                    Contract = joinned.contract,
                    ContractStatus = joinned.contractStatus,
                    UpdateMethod = joinned.updateMethod,
                    SystemKind = joinned.systemKind,
                    CDShippingDepartment = cdShippingDepartment
                } );

        #endregion

        #region こっちを使って貰う

        /// <summary>
        /// 会社状態名を取得する
        /// </summary>
        /// <param name="context"></param>
        /// <param name="companyStatusCode"></param>
        /// <returns></returns>
        public static async Task<string> GetCompanyStatusNameAsync( this SampleCRMContext context, int companyStatusCode )
            => ( await context.CompanyStatuses.FindAsync( companyStatusCode ) )!.CompanyStatusName; // TODO: 一致無し時・複数一致時はエラーページに飛ばす。

        public static async Task<IEnumerable<DepartmentListItemViewModel>> GetDepartmentListWithEmployeesByCompanyCodeAsync(
            this SampleCRMContext context,
            int companyCode )
            => await context.GetJoinnedDepartmentsWithEmployees()
                .Where( x => x.Department.CompanyCode == companyCode )
                .Select( x => new DepartmentListItemViewModel( x.Department, x.Employee ) )
                .ToListAsync();

        /// <summary>
        /// 発注情報取得
        /// </summary>
        /// <param name="companyCode"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<OrderListItemViewModel>> GetOrderListByCompanyCodeAsync( this SampleCRMContext context, int companyCode )
            => await context.GetJoinnedClientDepartmentWithOrders()
                .Join( context.DeliveryStatuses,
                        clientDepartmentOrder => clientDepartmentOrder.Order.DeliveryStatusCode,
                        deliveryStatus => deliveryStatus.DeliveryStatusCode,
                     ( clientDepartmentOrder, deliveryStatus )
                => new { clientDepartmentOrder, deliveryStatus } )
                .Where( x => x.clientDepartmentOrder.ClientDepartment.CompanyCode == companyCode )
                .Join( context.GetJoinnedSalesEmployeesWithOrders(),
                        joinned => joinned.clientDepartmentOrder.Order.OrderCode,
                        salesEmployeeOrder => salesEmployeeOrder.Order.OrderCode,
                      ( joinned, salesEmployeeOrder )
                => new OrderListItemViewModel(
                        joinned.clientDepartmentOrder.Order,
                        joinned.deliveryStatus,
                        joinned.clientDepartmentOrder.ClientEmployee,
                        salesEmployeeOrder.SalesEmployee ) )
                .ToListAsync();

        /// <summary>
        /// 契約情報取得(ライセンス情報も含まない、また契約状態が「続行中」のもののみ取得)
        /// </summary>
        /// <param name="companyCode"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<ContractListItemViewModel>> GetActiveContractListByCompanyCodeAsync( this SampleCRMContext context, int companyCode )
        => await context.GetDenormalizedContracts()
                    .Join( context.GetJoinnedClientDepartmentsWithContracts(),
                            denormalizedContract => denormalizedContract.Contract.ContractCode,
                            clientDepartmentWithContract => clientDepartmentWithContract.Contract.ContractCode,
                         ( denormalizedContract, clientDepartmentWithContract )
            => new { denormalizedContract, clientDepartmentWithContract } )
                    .Where( joinned => joinned.clientDepartmentWithContract.ClientDepartment.DepartmentCode == companyCode
                                        && joinned.clientDepartmentWithContract.Contract.ContractStatusCode == 3 ) // 続行中のもののみ
                    .Select( joinned
            => new ContractListItemViewModel(
                        joinned.clientDepartmentWithContract.Contract,
                        joinned.denormalizedContract.ContractStatus,
                        joinned.denormalizedContract.UpdateMethod,
                        joinned.denormalizedContract.SystemKind,
                        joinned.denormalizedContract.CDShippingDepartment
                    ) )
                    .ToListAsync();

        /// <summary>
        /// 契約情報取得(ライセンス情報も含む)
        /// </summary>
        /// <param name="companyCode">会社コード</param>
        /// <returns></returns>
        public static async Task<IEnumerable<ContractListItemViewModel>> GetContractListWithLicensesByCompanyCodeAsync( this SampleCRMContext context, int companyCode )
            => await ( from DenormalizedContract in context.GetDenormalizedContracts()
                       join ClientDepartmentWithContract in context.GetJoinnedClientDepartmentsWithContracts()
                           on DenormalizedContract.Contract.ContractCode equals ClientDepartmentWithContract.Contract.ContractCode

                       join ContractToSystemLicense in context.ContractToSystemLicenses
                           on ClientDepartmentWithContract.Contract.ContractCode equals ContractToSystemLicense.ContractCode into groupJoinnedContractToSystemLicense
                       from ContractToSystemLicense in groupJoinnedContractToSystemLicense.DefaultIfEmpty()
                       join SystemLicense in context.SystemLicenses
                           on ContractToSystemLicense.SystemLicenseCode equals SystemLicense.SystemLicenseCode into groupJoinnedSystemLicenses
                       from SystemLicense in groupJoinnedSystemLicenses.DefaultIfEmpty()

                       join ContractToDataLicense in context.ContractToDataLicenses
                           on ClientDepartmentWithContract.Contract.ContractCode equals ContractToDataLicense.ContractCode into groupJoinnedContractToDataLicenses
                       from ContractToDataLicense in groupJoinnedContractToDataLicenses.DefaultIfEmpty()
                       join DataLicense in context.DataLicenses
                           on ContractToDataLicense.DataLicenseCode equals DataLicense.DataLicenseCode into groupJoinnedDataLicense
                       from DataLicense in groupJoinnedDataLicense.DefaultIfEmpty()

                       where ClientDepartmentWithContract.ClientDepartment.DepartmentCode == companyCode

                       select new ContractListItemViewModel(
                           ClientDepartmentWithContract.Contract,
                           DenormalizedContract.ContractStatus,
                           DenormalizedContract.UpdateMethod,
                           DenormalizedContract.SystemKind,
                           DenormalizedContract.CDShippingDepartment,
                           SystemLicense,
                           DataLicense
                       ) ).ToListAsync();

        /// <summary>
        /// 保守料請求情報取得
        /// </summary>
        /// <param name="companyCode"></param>
        /// <returns></returns>
        /// <remarks>
        /// どの請求にも紐付かない保守料があった場合は別途表示させる
        /// </remarks>
        public static async Task<IEnumerable<MaintenanceFeeBillListItemViewModel>> GetMaintenanceFeeBillListByCompanyCodeAsync( this SampleCRMContext context, int companyCode )
            => await ( from MaintenanceFee in context.MaintenanceFees
                       join maintenanceFeeToBill in context.MaintenanceFeeToBills
                           on MaintenanceFee.MaintenanceFeeCode equals maintenanceFeeToBill.MaintenanceFeeCode into groupJoinnedMaintenanceFeeToBills
                       from MaintenanceFeeToBill in groupJoinnedMaintenanceFeeToBills.DefaultIfEmpty()
                       join MaintenanceFeeBill in context.MaintenanceFeeBills
                           on MaintenanceFeeToBill.BillCode equals MaintenanceFeeBill.BillCode into groupJoinnedMaintenanceFeeBills
                       from MaintenanceFeeBill in groupJoinnedMaintenanceFeeBills.DefaultIfEmpty()
                       join BillingDepartment in context.Departments
                           on MaintenanceFeeBill.BillingDepartmentCode equals BillingDepartment.DepartmentCode into groupJoinnedBillingDepartments
                       from BillingDepartment in groupJoinnedBillingDepartments.DefaultIfEmpty()
                       join maintenanceFeeBillToClearance in context.MaintenanceFeeBillToClearances
                           on MaintenanceFeeBill.BillCode equals maintenanceFeeBillToClearance.BillCode into groupJoinnedMaintenanceFeeBillToClearances
                       from MaintenanceFeeBillToClearance in groupJoinnedMaintenanceFeeBillToClearances.DefaultIfEmpty()
                       join MaintenanceFeeClearance in context.MaintenanceFeeClearances
                           on MaintenanceFeeBillToClearance.ClearanceCode equals MaintenanceFeeClearance.ClearanceCode into groupJoinnedMaintenanceFeeClearances
                       from MaintenanceFeeClearance in groupJoinnedMaintenanceFeeClearances.DefaultIfEmpty()
                       join ContractToMaintenanceFee in context.ContractToMaintenanceFees
                           on MaintenanceFee.MaintenanceFeeCode equals ContractToMaintenanceFee.MaintenanceFeeCode

                       join ClientDepartmentsContracts in context.GetJoinnedClientDepartmentsWithContracts()
                           on ContractToMaintenanceFee.ContractCode equals ClientDepartmentsContracts.Contract.ContractCode
                       where ClientDepartmentsContracts.ClientDepartment.CompanyCode == companyCode

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
    }
    #endregion
}