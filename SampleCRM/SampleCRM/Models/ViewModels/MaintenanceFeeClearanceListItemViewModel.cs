using SampleCRM.Models.TableModels;
using System.ComponentModel.DataAnnotations;

namespace SampleCRM.Models.ViewModels
{
    public class MaintenanceFeeClearanceListItemViewModel
    {
        private const string DateFormat = "yyyy/MM/dd";

        [Display( Name = "消込コード" )]
        public int ClearanceCode { get; set; }

        [Display( Name = "消込入金日" )]
        public string ClearanceDepositDate { get; set; }

        [Display( Name = "消込額" )]
        public int ClearanceAmount { get; set; }

        [Display( Name = "消込入力日" )]
        public string ClearanceInputDate { get; set; }

        [Display( Name = "振込番号" )]
        public int? TransferNumber { get; set; }

        public MaintenanceFeeClearanceListItemViewModel( MaintenanceFeeClearance maintenanceFeeClearance )
        {
            this.ClearanceCode = maintenanceFeeClearance.ClearanceCode;
            this.ClearanceDepositDate = maintenanceFeeClearance.ClearanceDepositDate?.ToString( DateFormat );
            this.ClearanceAmount = maintenanceFeeClearance.ClearanceAmount;
            this.ClearanceInputDate = maintenanceFeeClearance.ClearanceInputDate?.ToString( DateFormat );
            this.TransferNumber = maintenanceFeeClearance.TransferNumber;
        }
    }
}
