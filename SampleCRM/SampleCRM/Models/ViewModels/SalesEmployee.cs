using System.ComponentModel.DataAnnotations;

namespace SampleCRM.Models.ViewModels
{
    public class SalesEmployee
    {
        // 契約会社関連情報
        [Key]
        [Display( Name = "会社コード" )]
        public int CompanyCode { get; set; }

        [Display( Name = "会社名" )]
        public string CompanyName { get; set; }

        [Display( Name = "会社名カナ" )]
        public string CompanyPhoneticName { get; set; }

        // 部署関連情報

        [Display( Name = "部署コード" )]
        public int DepartmentCode { get; set; }

        [Display( Name = "部署名" )]
        public string DepartmentName { get; set; }

        [Display( Name = "部署名カナ" )]
        public string DepartmentPhoneticName { get; set; }

        // 担当者情報

        public int EmployeeCode { get; set; }

        [Required]
        [Display( Name = "代表担当者かどうか" )]
        public bool IsMainEmployee { get; set; }

        [Required( ErrorMessage = "担当者名は必須です。" )]
        [StringLength( 32, ErrorMessage = "32文字まで" )]
        [Display( Name = "担当者名" )]
        public string EmployeeName { get; set; }

        [StringLength( 32, ErrorMessage = "32文字まで" )]
        [RegularExpression( @"\A[ァ-ヶ ー]+\z", ErrorMessage = "全角カタカナのみ入力できます。" )]
        [Display( Name = "担当者名カナ" )]
        public string PhoneticName { get; set; }

        [StringLength( 32, ErrorMessage = "32文字まで" )]
        [Display( Name = "担当者役職" )]
        public string Position { get; set; }

        //[DataType(DataType.PhoneNumber)]
        [Phone( ErrorMessage = "正しい電話番号ではありません。" )]
        [StringLength( 16, ErrorMessage = "16文字まで" )]
        [Display( Name = "担当者携帯電話番号" )]
        public string CellPhoneNumber { get; set; }

        //[DataType(DataType.EmailAddress)]
        [EmailAddress( ErrorMessage = "正しいメールアドレスではありません。" )]
        [StringLength( 255, ErrorMessage = "255文字まで" )]
        [Display( Name = "担当者メールアドレス" )]
        public string Email { get; set; }

        [StringLength( 255, ErrorMessage = "255文字まで" )]
        [Display( Name = "担当者SNSアカウント" )]
        public string SNS { get; set; }
    }
}
