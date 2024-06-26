using SampleCRM.Models.Base;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleCRM.Models.TableModels
{
    /// <summary>
    /// 担当者
    /// </summary>
    [Table( "T_Employees" )]
    public class Employee : TransactionBase
    {
        [Key]
        [DatabaseGenerated( DatabaseGeneratedOption.Identity )]
        [Display( Name = "担当者コード" )]
        [Comment( "担当者コード" )]
        public int EmployeeCode { get; set; }

        [Required]
        [Display( Name = "部署コード" )]
        [Comment( "部署コード" )]
        public int DepartmentCode { get; set; }

        [Required]
        [Display( Name = "代表担当者かどうか" )]
        [Comment( "代表担当者かどうか" )]
        public bool IsMain { get; set; }

        [Required]
        [Display( Name = "在籍しているかどうか" )]
        [Comment( "在籍しているかどうか" )]
        public bool IsEnrollment { get; set; }

        [Required( ErrorMessage = "担当者名は必須です。" )]
        [StringLength( 32 )]
        [Display( Name = "担当者名" )]
        [Comment( "担当者名" )]
        public string EmployeeName { get; set; }

        [StringLength( 32 )]
        [RegularExpression( @"\A[ァ-ヶ ー]+\z", ErrorMessage = "全角カタカナのみ入力できます。" )]
        [Display( Name = "担当者名フリガナ" )]
        [Comment( "担当者名フリガナ" )]
        public string PhoneticName { get; set; }

        [StringLength( 32 )]
        [Display( Name = "役職" )]
        [Comment( "役職" )]
        public string Position { get; set; }

        //[DataType(DataType.PhoneNumber)]
        [Phone( ErrorMessage = "正しい電話番号ではありません。" )]
        [StringLength( 16 )]
        [Display( Name = "携帯電話番号" )]
        [Comment( "携帯電話番号" )]
        public string CellPhoneNumber { get; set; }

        //[DataType(DataType.EmailAddress)]
        [EmailAddress( ErrorMessage = "正しいメールアドレスではありません。" )]
        [StringLength( 255 )]
        [Display( Name = "メールアドレス" )]
        [Comment( "メールアドレス" )]
        public string Email { get; set; }

        [StringLength( 255 )]
        [Display( Name = "SNSアカウント" )]
        [Comment( "SNSアカウント" )]
        public string SNS { get; set; }

        // 担当者コードと同じ値に設定する
        // SHOW TABLE STATUS LIKE 'Departments' を実行し、Auto_increment列の値が次の値となるのでそれを挿入する
        [Required]
        [Display( Name = "表示順" )]
        [Comment( "表示順" )]
        public int DisplayOrder { get; set; }

    }
}
