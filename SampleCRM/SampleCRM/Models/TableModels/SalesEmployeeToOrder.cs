using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleCRM.Models.TableModels
{
    /// <summary>
    /// 営業担当者-発注
    /// </summary>
    [Table( "C_SalesEmployeeToOrders" )]
    public class SalesEmployeeToOrder
    {
        [Required]
        [Display( Name = "営業担当者コード" )]
        [Comment( "営業担当者コード" )]
        public int EmployeeCode { get; set; }

        [Required]
        [Display( Name = "発注コード" )]
        [Comment( "発注コード" )]
        public int OrderCode { get; set; }
    }
}
