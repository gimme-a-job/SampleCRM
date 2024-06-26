using SampleCRM.Models.Base;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleCRM.Models.TableModels
{
    /// <summary>
    /// 変更対象マスタ
    /// </summary>
    [Table( "M_ChangeTargets" )]
    public class ChangeTarget : TableBase
    {
        [Key]
        [DatabaseGenerated( DatabaseGeneratedOption.Identity )]
        [Display( Name = "変更対象コード" )]
        [Comment( "変更対象コード" )]
        public int ChangeTargetCode { get; set; }

        [Required]
        [StringLength( 16 )]
        [Display( Name = "変更対象名" )]
        [Comment( "変更対象名" )]
        public string ChangeTargetName { get; set; }
    }
}
