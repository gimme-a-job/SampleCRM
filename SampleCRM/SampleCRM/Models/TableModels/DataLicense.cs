using SampleCRM.Models.Base;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleCRM.Models.TableModels
{
    /// <summary>
    /// データライセンス
    /// </summary>
    [Table( "T_DataLicenses" )]
    public class DataLicense : TransactionBase
    {
        [Key]
        [DatabaseGenerated( DatabaseGeneratedOption.Identity )]
        [Display( Name = "データライセンスコード" )]
        [Comment( "データライセンスコード" )]
        public int DataLicenseCode { get; set; }

        [Required]
        [StringLength( 25 )]
        [Display( Name = "データライセンスキー" )]
        [Comment( "データライセンスキー" )]
        public string DataLicenseKey { get; set; }

        [Required]
        [StringLength( 25 )]
        [Display( Name = "システムライセンスキー" )]
        [Comment( "システムライセンスキー" )]
        public string SystemLicenseKey { get; set; }

        [Required]
        [Display( Name = "事業区分コード" )]
        [Comment( "事業区分コード" )]
        public int ProjectKindCode { get; set; }

        [Required]
        [Display( Name = "ライセンス地域コード" )]
        [Comment( "ライセンス地域コード" )]
        public int LicenseDistrictCode { get; set; }

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

        [DataType( DataType.MultilineText )]
        [StringLength( 2048 )]
        [Display( Name = "備考" )]
        [Comment( "備考" )]
        public string Note { get; set; }
    }
}
