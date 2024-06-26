using SampleCRM.Models.Base;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleCRM.Models.TableModels
{
    /// <summary>
    /// オフラインライセンス発行履歴
    /// </summary>
    [Table( "T_OffLineLicenseIssuanceHistories" )]
    public class OffLineLicenseIssuanceHistory : TransactionBase
    {
        [Key]
        [DatabaseGenerated( DatabaseGeneratedOption.Identity )]
        [Display( Name = "オフラインライセンス発行履歴コード" )]
        [Comment( "オフラインライセンス発行履歴コード" )]
        public int OffLineLicenseIssuanceHistoryCode { get; set; }

        [Required]
        [Display( Name = "システムライセンスコード" )]
        [Comment( "システムライセンスコード" )]
        public int SystemLicenseCode { get; set; }

        // WARNING: ForeignKey属性を持つプロパティを追加しない。

        [Required]
        [Display( Name = "開始日" )]
        [Comment( "開始日" )]
        [Precision( 0 )]
        [DataType( DataType.Date )]
        public DateTime StartDate { get; set; }

        [Required]
        [Display( Name = "終了日" )]
        [Comment( "終了日" )]
        [Precision( 0 )]
        [DataType( DataType.Date )]
        public DateTime EndDate { get; set; }

        [Required]
        [Display( Name = "ライセンス状態コード" )]
        [Comment( "ライセンス状態コード" )]
        public int LicenseStatusCode { get; set; }

        [DataType( DataType.MultilineText )]
        [StringLength( 2048 )]
        [Display( Name = "備考" )]
        [Comment( "備考" )]
        public string Note { get; set; }

        // WARNING: ForeignKey属性を追加しない。setアクセサを追加しない。
        public virtual LicenseStatus LicenseStatus { get; }
    }
}
