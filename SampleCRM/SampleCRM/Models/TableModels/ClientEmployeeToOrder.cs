using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleCRM.Models.TableModels
{
    /// <summary>
    /// 顧客担当者-発注
    /// </summary>
    [Table( "C_ClientEmployeeToOrders" )]
    public class ClientEmployeeToOrder
    {
        [Required]
        [Display( Name = "顧客担当者コード" )]
        [Comment( "顧客担当者コード" )]
        public int EmployeeCode { get; set; }

        [Required]
        [Display( Name = "発注コード" )]
        [Comment( "発注コード" )]
        public int OrderCode { get; set; }

        // WARNING: ForeignKey属性を追加しない。setアクセサを追加しない。
        public virtual Employee Employee { get; }

        // WARNING: ForeignKey属性を追加しない。setアクセサを追加しない。
        public virtual Order Order { get; }
    }
}
