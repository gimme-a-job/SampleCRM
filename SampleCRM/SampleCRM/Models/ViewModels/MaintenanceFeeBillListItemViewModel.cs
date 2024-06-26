using SampleCRM.Models.TableModels;
using System.ComponentModel.DataAnnotations;

namespace SampleCRM.Models.ViewModels
{
    public class MaintenanceFeeBillListItemViewModel
    {
        private const string DateFormat = "yyyy/MM/dd";

        // T_MaintenanceFees
        public IEnumerable<MaintenanceFeeListItemViewModel> MaintenanceFees { get; set; }


        // T_MaintenanceFeeBills
        [Display( Name = "請求書コード" )]
        public int BillCode { get; set; }

        [Display( Name = "代表部署に請求するかどうか" )]
        public bool IsBillingMainDepartment { get; set; }

        [Display( Name = "請求部署名" )]
        public string BillingDepartmentName { get; set; }

        [Display( Name = "請求日" )]
        public string BillingDate { get; set; }

        [Display( Name = "請求額" )]
        public int BillingAmount { get; set; }

        [Display( Name = "合計入金額" )]
        public int TotalDepositAmount { get; set; }

        [Display( Name = "請求差額" )]
        public int BillingDifferenceAmount
        {
            get => TotalDepositAmount - BillingAmount;
        }

        [Display( Name = "送金口座名義" )]
        public string BankAccountName { get; set; }

        [Display( Name = "備考(請求)" )]
        public string BillingNote { get; set; }

        [Display( Name = "特記" )]
        public string BillingSpecialNote { get; set; }


        // T_MaintenanceFeeClearances
        public IEnumerable<MaintenanceFeeClearanceListItemViewModel> MaintenanceFeeClearances { get; set; }

        public MaintenanceFeeBillListItemViewModel( MaintenanceFeeBill maintenanceFeeBill, Department billingDepartment )
        {
            this.BillCode = maintenanceFeeBill.BillCode;
            this.IsBillingMainDepartment = maintenanceFeeBill.IsBillingMainDepartment;
            this.BillingDepartmentName = billingDepartment.DepartmentName;
            this.BillingDate = maintenanceFeeBill.BillingDate?.ToString( DateFormat );
            this.BillingAmount = maintenanceFeeBill.BillingAmount;
            this.TotalDepositAmount = maintenanceFeeBill.TotalDepositAmount;
            this.BankAccountName = maintenanceFeeBill.BankAccountName;
            this.BillingNote = maintenanceFeeBill.Note;
            this.BillingSpecialNote = maintenanceFeeBill.SpecialNote;
        }

        public MaintenanceFeeBillListItemViewModel( IEnumerable<MaintenanceFee> maintenanceFees, MaintenanceFeeBill maintenanceFeeBill, Department billingDepartment, IEnumerable<MaintenanceFeeClearance> maintenanceFeeClearances )
            : this( maintenanceFeeBill, billingDepartment )
        {
            this.MaintenanceFees = maintenanceFees.Select( x => new MaintenanceFeeListItemViewModel( x ) );

            // TODO : 「保守情報」毎に重複が出る理由が不明なので、実装時によく確認する
            this.MaintenanceFeeClearances = maintenanceFeeClearances.DistinctBy( x => x.ClearanceCode ).Select( x => new MaintenanceFeeClearanceListItemViewModel( x ) );
        }
    }
}
