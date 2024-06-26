using SampleCRM.Models.Base;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleCRM.Models.TableModels
{
    /// <summary>
    /// 更新方法マスタ
    /// </summary>

    [Table( "M_UpdateMethods" )]
    public class UpdateMethod : TableBase
    {
        [Key]
        [DatabaseGenerated( DatabaseGeneratedOption.Identity )]
        [Display( Name = "更新方法コード" )]
        [Comment( "更新方法コード" )]
        public int UpdateMethodCode { get; set; }

        [Required]
        [StringLength( 64 )]
        [Display( Name = "更新方法名" )]
        [Comment( "更新方法名" )]
        public string UpdateMethodName { get; set; }
    }
}
