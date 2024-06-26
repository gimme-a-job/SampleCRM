using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleCRM.Models.TableModels
{
    /// <summary>
    /// 保守料-請求
    /// </summary>
    [Table( "C_MaintenanceFeeToBills" )]
    public class MaintenanceFeeToBill
    {
        [Required]
        [Display( Name = "保守料コード" )]
        [Comment( "保守料コード" )]
        public int MaintenanceFeeCode { get; set; }

        [Required]
        [Display( Name = "請求書コード" )]
        [Comment( "請求書コード" )]
        public int BillCode { get; set; }
    }
}
