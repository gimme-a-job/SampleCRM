using SampleCRM.Models.Base;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleCRM.Models.TableModels
{
    /// <summary>
    /// 会社状態マスタ
    /// </summary>
    [Table( "M_CompanyStatuses" )]
    public class CompanyStatus : TableBase
    {
        [Key]
        [DatabaseGenerated( DatabaseGeneratedOption.Identity )]
        [Display( Name = "会社状態コード" )]
        [Comment( "会社状態コード" )]
        public int CompanyStatusCode { get; set; }

        [Required]
        [StringLength( 16 )]
        [Display( Name = "会社状態名" )]
        [Comment( "会社状態名" )]
        public string CompanyStatusName { get; set; }
    }
}
