using SampleCRM.Models.Base;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleCRM.Models.TableModels
{
    /// <summary>
    /// CD発送履歴
    /// </summary>
    [Table( "T_CDShippingLogs" )]
    public class CDShippingLog : TransactionBase
    {
        [Key]
        [DatabaseGenerated( DatabaseGeneratedOption.Identity )]
        [Display( Name = "CD発送履歴コード" )]
        [Comment( "CD発送履歴コード" )]
        public int CDShippingLogCode { get; set; }

        [Required]
        [Display( Name = "契約コード" )]
        [Comment( "契約コード" )]
        public int ContractCode { get; set; }

        [Display( Name = "CD発送日" )]
        [Comment( "CD発送日" )]
        [Precision( 0 )]
        [DataType( DataType.Date )]
        public DateTime? CDShippingDate { get; set; }

        [DataType( DataType.MultilineText )]
        [StringLength( 2048 )]
        [Display( Name = "CD発送内容" )]
        [Comment( "CD発送内容" )]
        public string CDShippingContents { get; set; }
    }
}
