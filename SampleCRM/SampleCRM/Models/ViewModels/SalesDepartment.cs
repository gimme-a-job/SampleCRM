using System.ComponentModel.DataAnnotations;

namespace SampleCRM.Models.ViewModels
{
    public class SalesDepartment
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

        // 部署コードはAutoincrement属性のため部署テーブルには登録不要だが
        // DisplayOrderと紐づける事と担当者テーブルに登録するために取得の必要はある
        public int DepartmentCode { get; set; }

        // 代表部署かどうかを編集できるか
        public bool EditableMainDepartment { get; set; }

        [Required]
        [Display( Name = "代表部署" )]
        public bool IsMainDepartment { get; set; }

        [Required( ErrorMessage = "部署名は必須です。" )]
        [StringLength( 32, ErrorMessage = "32文字まで" )]
        [Display( Name = "部署名" )]
        public string DepartmentName { get; set; }

        [StringLength( 32, ErrorMessage = "32文字まで" )]
        [RegularExpression( @"\A[ァ-ヶ ー]+\z", ErrorMessage = "全角カタカナのみ入力できます。" )]
        [Display( Name = "部署名カナ" )]
        public string PhoneticName { get; set; }

        [StringLength( 8, ErrorMessage = "8文字まで" )]
        [DataType( DataType.PostalCode )]
        [RegularExpression( @"\d{3}-\d{4}", ErrorMessage = "○○○-○○○○で記述してください。" )]
        [Display( Name = "部署の郵便番号" )]
        public string PostalCode { get; set; }

        [StringLength( 64, ErrorMessage = "64文字まで" )]
        [Display( Name = "部署の住所" )]
        public string Address { get; set; }

        //[DataType(DataType.PhoneNumber)]
        [Phone( ErrorMessage = "正しい電話番号ではありません。" )]
        [StringLength( 16, ErrorMessage = "16文字まで" )]
        [Display( Name = "部署の電話番号" )]
        public string TelNumber { get; set; }

        //[DataType(DataType.PhoneNumber)]
        [Phone( ErrorMessage = "正しいFAX番号ではありません。" )]
        [StringLength( 16, ErrorMessage = "16文字まで" )]
        [Display( Name = "部署のFAX番号" )]
        public string FaxNumber { get; set; }

        //[DataType(DataType.EmailAddress)]
        [EmailAddress( ErrorMessage = "正しいメールアドレスではありません。" )]
        [StringLength( 255, ErrorMessage = "255文字まで" )]
        [Display( Name = "部署のメールアドレス" )]
        public string Email { get; set; }

        // 営業担当一覧
        [Display( Name = "営業担当一覧" )]
        public IList<SalesEmployee> Employees { get; set; }
    }
}
