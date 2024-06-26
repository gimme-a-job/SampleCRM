using SampleCRM.Models.Base;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleCRM.Models.TableModels;

/// <summary>
/// GUIDバージョンマスタ
/// </summary>
[Table( "M_GUIDVersions" )]
public class GUIDVersion : TableBase
{
    [Key]
    [DatabaseGenerated( DatabaseGeneratedOption.Identity )]
    [Display( Name = "バージョンコード" )]
    [Comment( "バージョンコード" )]
    public int VersionCode { get; set; }

    [Required]
    [StringLength( 32 )]
    [Display( Name = "バージョン名" )]
    [Comment( "バージョン名" )]
    public string VersionName { get; set; }

    [Required]
    [StringLength( 38 )]
    [Display( Name = "バージョン" )]
    [Comment( "バージョン" )]
    public string Version { get; set; }

    [StringLength( 2048 )]
    [Display( Name = "備考" )]
    [Comment( "備考" )]
    public string Note { get; set; }
}
