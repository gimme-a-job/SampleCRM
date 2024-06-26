using SampleCRM.Models.Base;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleCRM.Models.TableModels
{
    /// <summary>
    /// 会社
    /// </summary>
    [Table( "T_Companies" )]
    public class Company : TransactionBase
    {
        [Key]
        [DatabaseGenerated( DatabaseGeneratedOption.Identity )]
        [Display( Name = "会社コード" )]
        [Comment( "会社コード" )]
        public int CompanyCode { get; set; }

        [Display( Name = "Customersの契約コード" )]
        [Comment( "Customersの契約コード" )]
        public int? CustomersContractCode { get; set; }

        [Required]
        [Display( Name = "顧客会社かどうか" )]
        [Comment( "顧客会社かどうか" )]
        public bool IsClientCompany { get; set; }

        [Required]
        [StringLength( 128 )]
        [Display( Name = "会社名" )]
        [Comment( "会社名" )]
        public string CompanyName { get; set; }

        [StringLength( 128 )]
        [RegularExpression( @"\A[ァ-ヶ ー]+\z", ErrorMessage = "全角カタカナのみ入力できます。" )]
        [Display( Name = "会社名フリガナ" )]
        [Comment( "会社名フリガナ" )]
        public string PhoneticName { get; set; }

        [Required]
        [Display( Name = "会社状態コード" )]
        [Comment( "会社状態コード" )]
        public int CompanyStatusCode { get; set; }

        [Required]
        [Display( Name = "訪問が必要かどうか" )]
        [Comment( "訪問が必要かどうか" )]
        public bool IsNeedVisiting { get; set; }

        [RegularExpression( @"^[\d]{2}-[\d]{6}$", ErrorMessage = "[○○-○○○○○○]の形式で記載して下さい。" )]
        [StringLength( 10 )]
        [Display( Name = "経審許可番号" )]
        [Comment( "経審許可番号" )]
        public string BusinessEvaluationPermitNumber { get; set; }

        [StringLength( 2048 )]
        [DataType( DataType.MultilineText )]
        [Display( Name = "備考" )]
        [Comment( "備考" )]
        public string Note { get; set; }
    }
}
