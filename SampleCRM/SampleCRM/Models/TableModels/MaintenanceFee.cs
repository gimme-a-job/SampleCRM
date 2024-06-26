using SampleCRM.Models.Base;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleCRM.Models.TableModels
{
    /// <summary>
    /// 保守料
    /// </summary>
    [Table( "T_MaintenanceFees" )]
    public class MaintenanceFee : TransactionBase
    {
        [Key]
        [DatabaseGenerated( DatabaseGeneratedOption.Identity )]
        [Display( Name = "保守料コード" )]
        [Comment( "保守料コード" )]
        public int MaintenanceFeeCode { get; set; }

        [Required]
        [Display( Name = "保守料" )]
        [Comment( "保守料" )]
        public int? MaintenanceFeePrice { get; set; }

        [Display( Name = "保守開始日" )]
        [Comment( "保守開始日" )]
        [Precision( 0 )]
        [DataType( DataType.Date )]
        public DateTime? MaintenanceStartDate { get; set; }

        [Display( Name = "保守終了日" )]
        [Comment( "保守終了日" )]
        [Precision( 0 )]
        [DataType( DataType.Date )]
        public DateTime? MaintenanceEndDate { get; set; }

        [DataType( DataType.MultilineText )]
        [StringLength( 2048 )]
        [Display( Name = "備考" )]
        [Comment( "備考" )]
        public string Note { get; set; }
    }
}
