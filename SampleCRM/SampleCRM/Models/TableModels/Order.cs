using SampleCRM.Models.Base;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleCRM.Models.TableModels
{
    /// <summary>
    /// 発注
    /// </summary>
    [Table( "T_Orders" )]
    public class Order : TransactionBase
    {
        [Key]
        [DatabaseGenerated( DatabaseGeneratedOption.Identity )]
        [Display( Name = "発注コード" )]
        [Comment( "発注コード" )]
        public int OrderCode { get; set; }

        [Required]
        [Display( Name = "発注日" )]
        [Comment( "発注日" )]
        [Precision( 0 )]
        [DataType( DataType.Date )]
        public DateTime? OrderDate { get; set; }

        [Display( Name = "納品予定日" )]
        [Comment( "納品予定日" )]
        [Precision( 0 )]
        [DataType( DataType.Date )]
        public DateTime? DeliveryDate { get; set; }

        [Required]
        [Display( Name = "納品状態コード" )]
        [Comment( "納品状態コード" )]
        public int DeliveryStatusCode { get; set; }

        [Required]
        [Display( Name = "新規発注の会社かどうか" )]
        [Comment( "新規発注の会社かどうか" )]
        public bool IsNewOrderCompany { get; set; }

        [Required]
        [Display( Name = "発注書でまとめて支払うかどうか" )]
        [Comment( "発注書でまとめて支払うかどうか" )]
        public bool IsPayTogether { get; set; }

        // WARNING: ForeignKey属性を追加しない。setアクセサを追加しない。
        public virtual DeliveryStatus DeliveryStatus { get; }
    }
}
