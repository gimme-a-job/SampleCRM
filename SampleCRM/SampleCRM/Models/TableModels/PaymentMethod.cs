using SampleCRM.Models.Base;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleCRM.Models.TableModels
{
    /// <summary>
    /// 支払い方法マスタ
    /// </summary>
    [Table( "M_PaymentMethods" )]
    public class PaymentMethod : TableBase
    {
        [Key]
        [DatabaseGenerated( DatabaseGeneratedOption.Identity )]
        [Display( Name = "支払い方法コード" )]
        [Comment( "支払い方法コード" )]
        public int PaymentMethodCode { get; set; }

        [Required]
        [StringLength( 16 )]
        [Display( Name = "支払い方法名" )]
        [Comment( "支払い方法名" )]
        public string PaymentMethodName { get; set; }
    }
}
