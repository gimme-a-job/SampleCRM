using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleCRM.Models.TableModels
{
    /// <summary>
    /// 変更履歴
    /// </summary>
    /// <remarks>
    /// 証拠隠滅を防ぐためTransactionBaseの継承はしない。（論理削除すら許容しない。）
    /// 変更者と変更日時のカラムを自力で持っているので、TableBaseも継承しない。
    /// </remarks>
    [Table( "T_ChangeLogs" )]
    public class ChangeLog
    {
        [Key]
        [DatabaseGenerated( DatabaseGeneratedOption.Identity )]
        [Display( Name = "変更履歴コード" )]
        [Comment( "変更履歴コード" )]
        public int ChangeLogCode { get; set; }

        [Required]
        [Display( Name = "変更者コード" )]
        [Comment( "変更者コード" )]
        public int ChangeUserCode { get; set; }

        [Display( Name = "変更日時" )]
        [Comment( "変更日時" )]
        [Precision( 0 )]
        [DataType( DataType.Date )]
        public DateTime? ChangeDate { get; set; }

        [Required]
        [Display( Name = "変更種別コード" )]
        [Comment( "変更種別コード" )]
        public int ChangeTypeCode { get; set; }

        [Required]
        [Display( Name = "変更会社コード" )]
        [Comment( "変更会社コード" )]
        public int ChangeCompanyCode { get; set; }

        // WARNING: ForeignKey属性を持つプロパティを追加しない。

        [Required]
        [Display( Name = "変更対象コード" )]
        [Comment( "変更対象コード" )]
        public int ChangeTargetCode { get; set; }

        [StringLength( 16 )]
        [Display( Name = "変更箇所" )]
        [Comment( "変更箇所" )]
        public string ChangeItem { get; set; }

        [StringLength( 2048 )]
        [Display( Name = "変更前情報" )]
        [Comment( "変更前情報" )]
        public string BeforeInformation { get; set; }

        [StringLength( 2048 )]
        [Display( Name = "変更後情報" )]
        [Comment( "変更後情報" )]
        public string AfterInformation { get; set; }

        // WARNING: ForeignKey属性を追加しない。setアクセサを追加しない。
        public virtual IdentityUser ChangeUser { get; }

        // WARNING: ForeignKey属性を追加しない。setアクセサを追加しない。
        public virtual ChangeType ChangeType { get; }

        // WARNING: ForeignKey属性を追加しない。setアクセサを追加しない。
        public virtual ChangeTarget ChangeTarget { get; }
    }
}
