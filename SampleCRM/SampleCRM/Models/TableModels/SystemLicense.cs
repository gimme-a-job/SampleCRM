using SampleCRM.Models.Base;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleCRM.Models.TableModels
{
    /// <summary>
    /// システムライセンス
    /// </summary>
    [Table( "T_SystemLicenses" )]
    public class SystemLicense : TransactionBase
    {
        [Key]
        [DatabaseGenerated( DatabaseGeneratedOption.Identity )]
        [Display( Name = "システムライセンスコード" )]
        [Comment( "システムライセンスコード" )]
        public int SystemLicenseCode { get; set; }

        [Required]
        [StringLength( 25 )]
        [Display( Name = "システムライセンスキー" )]
        [Comment( "システムライセンスキー" )]
        public string SystemLicenseKey { get; set; }

        [Required]
        [Display( Name = "無期限ライセンスかどうか" )]
        [Comment( "無期限ライセンスかどうか" )]
        public bool IsIndefinitePeriodLicense { get; set; }

        [Required]
        [Display( Name = "開始日" )]
        [Comment( "開始日" )]
        [Precision( 0 )]
        [DataType( DataType.Date )]
        public DateTime StartDate { get; set; }

        [Display( Name = "終了日" )]
        [Comment( "終了日" )]
        [Precision( 0 )]
        [DataType( DataType.Date )]
        public DateTime? EndDate { get; set; }

        [Required]
        [Display( Name = "ライセンス状態コード" )]
        [Comment( "ライセンス状態コード" )]
        public int LicenseStatusCode { get; set; }

        [Required]
        [Display( Name = "ログイン回数" )]
        [Comment( "ログイン回数" )]
        public int LoginCount { get; set; }

        [Display( Name = "最終ログイン日" )]
        [Comment( "最終ログイン日" )]
        [Precision( 0 )]
        [DataType( DataType.Date )]
        public DateTime? LastLoginDate { get; set; }

        [StringLength( 16 )]
        [Display( Name = "ライセンスキー利用最終利用IPアドレス" )]
        [Comment( "ライセンスキー利用最終利用IPアドレス" )]
        public string LastIPAddress { get; set; }

        [StringLength( 44 )]
        [Display( Name = "ハッシュ値" )]
        [Comment( "ハッシュ値" )]
        public string HashedValue { get; set; }

        [StringLength( 44 )]
        [Display( Name = "ソルト値" )]
        [Comment( "ソルト値" )]
        public string SaltValue { get; set; }

        [DataType( DataType.MultilineText )]
        [StringLength( 2048 )]
        [Display( Name = "備考" )]
        [Comment( "備考" )]
        public string Note { get; set; }

        // WARNING: ForeignKey属性を追加しない。setアクセサを追加しない。
        public virtual LicenseStatus LicenseStatus { get; }
    }
}
