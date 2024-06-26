using SampleCRM.Models.Base;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleCRM.Models.TableModels
{
    /// <summary>
    /// 契約
    /// </summary>
    [Table( "T_Contracts" )]
    public class Contract : TransactionBase
    {
        [Key]
        [DatabaseGenerated( DatabaseGeneratedOption.Identity )]
        [Display( Name = "契約コード" )]
        [Comment( "契約コード" )]
        public int ContractCode { get; set; }

        [Required]
        [Display( Name = "発注コード" )]
        [Comment( "発注コード" )]
        public int OrderCode { get; set; }

        // WARNING: ForeignKey属性を持つプロパティを追加しない。

        [Required]
        [Display( Name = "契約状態コード" )]
        [Comment( "契約状態コード" )]
        public int ContractStatusCode { get; set; }

        [Required]
        [Display( Name = "更新方法コード" )]
        [Comment( "更新方法コード" )]
        public int UpdateMethodCode { get; set; }

        [Required]
        [Display( Name = "システム種別コード" )]
        [Comment( "システム種別コード" )]
        public int SystemKindCode { get; set; }

        [Display( Name = "CD発送先部署コード" )]
        [Comment( "CD発送先部署コード" )]
        public int? CDShippingDepartmentCode { get; set; }

        [StringLength( 2048 )]
        [DataType( DataType.MultilineText )]
        [Display( Name = "発送備考" )]
        [Comment( "発送備考" )]
        public string ShippingNote { get; set; }
    }
}
