using SampleCRM.Models.Base;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleCRM.Models.TableModels
{
    /// <summary>
    /// 変更種別マスタ
    /// </summary>
    [Table( "M_ChangeTypes" )]
    public class ChangeType : TableBase
    {
        [Key]
        [DatabaseGenerated( DatabaseGeneratedOption.Identity )]
        [Display( Name = "変更種別コード" )]
        [Comment( "変更種別コード" )]
        public int ChangeTypeCode { get; set; }

        [Required]
        [StringLength( 16 )]
        [Display( Name = "変更種別名" )]
        [Comment( "変更種別名" )]
        public string ChangeTypeName { get; set; }
    }
}
