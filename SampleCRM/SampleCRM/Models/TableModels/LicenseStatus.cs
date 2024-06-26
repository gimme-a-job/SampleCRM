using SampleCRM.Models.Base;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleCRM.Models.TableModels
{
    /// <summary>
    /// ライセンス状態マスタ
    /// </summary>
    [Table( "M_LicenseStatuses" )]
    public class LicenseStatus : TableBase
    {
        [Key]
        [DatabaseGenerated( DatabaseGeneratedOption.Identity )]
        [Display( Name = "ライセンス状態コード" )]
        [Comment( "ライセンス状態コード" )]
        public int LicenseStatusCode { get; set; }

        [Required]
        [StringLength( 16 )]
        [Display( Name = "ライセンス状態名" )]
        [Comment( "ライセンス状態名" )]
        public string LicenseStatusName { get; set; }
    }
}
