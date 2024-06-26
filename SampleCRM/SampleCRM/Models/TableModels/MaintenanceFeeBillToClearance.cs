using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleCRM.Models.TableModels
{
    /// <summary>
    /// 保守料請求-消込
    /// </summary>
    [Table( "C_MaintenanceFeeBillToClearances" )]
    public class MaintenanceFeeBillToClearance
    {
        [Required]
        [Display( Name = "請求書コード" )]
        [Comment( "請求書コード" )]
        public int BillCode { get; set; }

        [Required]
        [Display( Name = "消込コード" )]
        [Comment( "消込コード" )]
        public int ClearanceCode { get; set; }
    }
}
