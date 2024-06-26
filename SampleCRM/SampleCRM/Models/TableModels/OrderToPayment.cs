using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleCRM.Models.TableModels
{
    /// <summary>
    /// 発注-支払い
    /// </summary>
    [Table( "C_OrderToPayments" )]
    public class OrderToPayment
    {
        [Required]
        [Display( Name = "発注コード" )]
        [Comment( "発注コード" )]
        public int OrderCode { get; set; }

        [Required]
        [Display( Name = "支払いコード" )]
        [Comment( "支払いコード" )]
        public int PaymentCode { get; set; }

        // WARNING: ForeignKey属性を追加しない。setアクセサを追加しない。
        public virtual Order Order { get; }

        // WARNING: ForeignKey属性を追加しない。setアクセサを追加しない。
        public virtual Payment Payment { get; }
    }
}
