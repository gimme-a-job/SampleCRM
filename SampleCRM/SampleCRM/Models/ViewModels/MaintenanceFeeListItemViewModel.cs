using SampleCRM.Models.TableModels;
using System.ComponentModel.DataAnnotations;

namespace SampleCRM.Models.ViewModels
{
    public class MaintenanceFeeListItemViewModel
    {
        private const string DateFormat = "yyyy/MM/dd";

        [Display( Name = "保守料コード" )]
        public int MaintenanceFeeCode { get; }

        [Display( Name = "保守料" )]
        public int? MaintenanceFeePrice { get; }

        [Display( Name = "保守開始日" )]
        public string MaintenanceStartDate { get; }

        [Display( Name = "保守終了日" )]
        public string MaintenanceEndDate { get; }

        [Display( Name = "備考" )]
        public string Note { get; }

        public MaintenanceFeeListItemViewModel( MaintenanceFee maintenanceFee )
        {
            this.MaintenanceFeeCode = maintenanceFee.MaintenanceFeeCode;
            this.MaintenanceFeePrice = maintenanceFee.MaintenanceFeePrice;
            this.MaintenanceStartDate = maintenanceFee.MaintenanceStartDate?.ToString( DateFormat );
            this.MaintenanceEndDate = maintenanceFee.MaintenanceEndDate?.ToString( DateFormat );
            this.Note = maintenanceFee.Note;
        }
    }
}
