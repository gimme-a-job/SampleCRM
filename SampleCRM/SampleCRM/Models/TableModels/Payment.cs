using SampleCRM.Models.Base;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleCRM.Models.TableModels
{
    /// <summary>
    /// 支払い
    /// </summary>
    [Table( "T_Payments" )]
    public class Payment : TransactionBase
    {
        [Key]
        [Display( Name = "支払いコード" )]
        [Comment( "支払いコード" )]
        public int PaymentCode { get; set; }

        [Required]
        [Display( Name = "支払い方法コード" )]
        [Comment( "支払い方法コード" )]
        public int PaymentMethodCode { get; set; }

        [Display( Name = "リース年数" )]
        [Comment( "リース年数" )]
        public int? LeaseTerm { get; set; }

        [Display( Name = "分割回数" )]
        [Comment( "分割回数" )]
        public int? NumberOfPayments { get; set; }

        [Display( Name = "リース・分割金額" )]
        [Comment( "リース・分割金額" )]
        public int? LeaseOrSplitPrice { get; set; }

        [Display( Name = "一括・頭金金額" )]
        [Comment( "一括・頭金金額" )]
        public int? LumpSumPrice { get; set; }

        [StringLength( 2048 )]
        [DataType( DataType.MultilineText )]
        [Display( Name = "支払い備考" )]
        [Comment( "支払い備考" )]
        public string PaymentNote { get; set; }

        // WARNING: ForeignKey属性を追加しない。setアクセサを追加しない。
        public virtual PaymentMethod PaymentMethod { get; }
    }
}
