using SampleCRM.Models.Base;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleCRM.Models.TableModels
{
    /// <summary>
    /// 契約状態マスタ
    /// </summary>
    [Table( "M_ContractStatuses" )]
    public class ContractStatus : TableBase
    {
        [Key]
        [DatabaseGenerated( DatabaseGeneratedOption.Identity )]
        [Display( Name = "契約状態コード" )]
        [Comment( "契約状態コード" )]
        public int ContractStatusCode { get; set; }

        [Required]
        [StringLength( 32 )]
        [Display( Name = "契約状態名" )]
        [Comment( "契約状態名" )]
        public string ContractStatusName { get; set; }
    }
}
