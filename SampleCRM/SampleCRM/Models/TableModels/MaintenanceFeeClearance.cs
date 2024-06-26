using SampleCRM.Models.Base;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleCRM.Models.TableModels
{
    /// <summary>
    /// 保守料消込
    /// </summary>
    [Table( "T_MaintenanceFeeClearances" )]
    public class MaintenanceFeeClearance : TransactionBase
    {
        [Key]
        [DatabaseGenerated( DatabaseGeneratedOption.Identity )]
        [Display( Name = "消込コード" )]
        [Comment( "消込コード" )]
        public int ClearanceCode { get; set; }

        [Display( Name = "消込入金日" )]
        [Comment( "消込入金日" )]
        [Precision( 0 )]
        [DataType( DataType.Date )]
        public DateTime? ClearanceDepositDate { get; set; }

        [Required]
        [Display( Name = "消込額" )]
        [Comment( "消込額" )]
        public int ClearanceAmount { get; set; }

        [Display( Name = "消込入力日" )]
        [Comment( "消込入力日" )]
        [Precision( 0 )]
        [DataType( DataType.Date )]
        public DateTime? ClearanceInputDate { get; set; }

        [Display( Name = "振込番号" )]
        [Comment( "振込番号" )]
        public int? TransferNumber { get; set; }
    }
}
