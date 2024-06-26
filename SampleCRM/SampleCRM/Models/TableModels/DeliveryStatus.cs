using SampleCRM.Models.Base;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleCRM.Models.TableModels
{
    /// <summary>
    /// 納品状態マスタ
    /// </summary>
    [Table( "M_DeliveryStatuses" )]
    public class DeliveryStatus : TableBase
    {
        [Key]
        [DatabaseGenerated( DatabaseGeneratedOption.Identity )]
        [Display( Name = "納品状態コード" )]
        [Comment( "納品状態コード" )]
        public int DeliveryStatusCode { get; set; }

        [Required]
        [StringLength( 32 )]
        [Display( Name = "納品状態名" )]
        [Comment( "納品状態名" )]
        public string DeliveryStatusName { get; set; }
    }
}
