using SampleCRM.Models.Base;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleCRM.Models.TableModels
{
    /// <summary>
    /// 保守料請求
    /// </summary>
    [Table( "T_MaintenanceFeeBills" )]
    public class MaintenanceFeeBill : TransactionBase
    {
        [Key]
        [DatabaseGenerated( DatabaseGeneratedOption.Identity )]
        [Display( Name = "請求書コード" )]
        [Comment( "請求書コード" )]
        public int BillCode { get; set; }

        [Required]
        [Display( Name = "代表部署に請求するかどうか" )]
        [Comment( "代表部署に請求するかどうか" )]
        public bool IsBillingMainDepartment { get; set; }

        [Required]
        [Display( Name = "請求部署コード" )]
        [Comment( "請求部署コード" )]
        public int BillingDepartmentCode { get; set; }

        [Display( Name = "請求日" )]
        [Comment( "請求日" )]
        [Precision( 0 )]
        [DataType( DataType.Date )]
        public DateTime? BillingDate { get; set; }

        [Display( Name = "請求額" )]
        [Comment( "請求額" )]
        public int BillingAmount { get; set; }

        [Display( Name = "合計入金額" )]
        [Comment( "合計入金額" )]
        public int TotalDepositAmount { get; set; }

        [StringLength( 30 )]
        [Display( Name = "送金口座名義" )]
        [Comment( "送金口座名義" )]
        public string BankAccountName { get; set; }

        [StringLength( 2048 )]
        [DataType( DataType.MultilineText )]
        [Display( Name = "備考" )]
        [Comment( "備考" )]
        public string Note { get; set; }

        [StringLength( 2048 )]
        [DataType( DataType.MultilineText )]
        [Display( Name = "特記" )]
        [Comment( "特記" )]
        public string SpecialNote { get; set; }
    }
}
