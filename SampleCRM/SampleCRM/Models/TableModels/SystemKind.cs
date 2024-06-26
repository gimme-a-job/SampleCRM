using SampleCRM.Models.Base;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleCRM.Models.TableModels
{
    /// <summary>
    /// システム種別マスタ
    /// </summary>
    [Table( "M_SystemKinds" )]
    public class SystemKind : TableBase
    {
        [Key]
        [DatabaseGenerated( DatabaseGeneratedOption.Identity )]
        [Display( Name = "システム種別コード" )]
        [Comment( "システム種別コード" )]
        public int SystemKindCode { get; set; }

        [Required]
        [StringLength( 32 )]
        [Display( Name = "システム種別名" )]
        [Comment( "システム種別名" )]
        public string SystemKindName { get; set; }

        // TODO: エクスポート機能 #3814 のclose時にこのカラムを消せるようにしておく。
        [StringLength( 16 )]
        [Display( Name = "システム種別名略称" )]
        [Comment( "システム種別名略称" )]
        public string SystemKindShortName { get; set; }
    }
}
