using System.ComponentModel.DataAnnotations;

namespace SampleCRM.Models.ViewModels
{
    /// <summary>
    /// 顧客会社用部署情報
    /// 担当者とは1対1の関係
    /// </summary>
    public class ClientDepartmentViewModel
    {
        // 契約会社関連情報
        [Display( Name = "会社コード" )]
        public int CompanyCode { get; set; }

        [Display( Name = "会社名" )]
        public string CompanyName { get; set; }

        [Display( Name = "会社名カナ" )]
        public string CompanyPhoneticName { get; set; }

        // 代表部署かどうかを編集できるか
        public bool EditableMainDepartment { get; set; }

        // 部署関連情報

        // 部署コードはAutoincrement属性のため部署テーブルには登録不要だが
        // DisplayOrderと紐づける事と担当者テーブルに登録するために取得の必要はある
        public int DepartmentCode { get; set; }

        [Required]
        [Display( Name = "代表部署かどうか" )]
        public bool IsMainDepartment { get; set; }

        [Required( ErrorMessage = "部署名は必須です。" )]
        [StringLength( 32, ErrorMessage = "32文字まで" )]
        [Display( Name = "部署名" )]
        public string DepartmentName { get; set; }

        [StringLength( 32, ErrorMessage = "32文字まで" )]
        [RegularExpression( @"\A[ァ-ヶ ー]+\z", ErrorMessage = "全角カタカナのみ入力できます。" )]
        [Display( Name = "部署名カナ" )]
        public string DepartmentPhoneticName { get; set; }

        [StringLength( 8, ErrorMessage = "8文字まで" )]
        [DataType( DataType.PostalCode )]
        [RegularExpression( @"\d{3}-\d{4}", ErrorMessage = "○○○-○○○○で記述してください。" )]
        [Display( Name = "部署の郵便番号" )]
        public string DepartmentPostalCode { get; set; }

        [StringLength( 64, ErrorMessage = "64文字まで" )]
        [Display( Name = "部署の住所" )]
        public string DepartmentAddress { get; set; }

        //[DataType(DataType.PhoneNumber)]
        [Phone( ErrorMessage = "正しい電話番号ではありません。" )]
        [StringLength( 16, ErrorMessage = "16文字まで" )]
        [Display( Name = "部署の電話番号" )]
        public string DepartmentTelNumber { get; set; }

        //[DataType(DataType.PhoneNumber)]
        [Phone( ErrorMessage = "正しいFAX番号ではありません。" )]
        [StringLength( 16, ErrorMessage = "16文字まで" )]
        [Display( Name = "部署のFAX番号" )]
        public string DepartmentFaxNumber { get; set; }

        //[DataType(DataType.EmailAddress)]
        [EmailAddress( ErrorMessage = "正しいメールアドレスではありません。" )]
        [StringLength( 255, ErrorMessage = "255文字まで" )]
        [Display( Name = "部署のメールアドレス" )]
        public string DepartmentEmail { get; set; }

        [Display( Name = "備考" )]
        [StringLength( 2048 )]
        [DataType( DataType.MultilineText )]
        public string Note { get; set; }

        // 部署コードと同じ値に設定する
        // SHOW TABLE STATUS LIKE 'Departments' を実行し、Auto_increment列の値が次の値となるのでそれを挿入する
        [Required]
        [Display( Name = "表示順" )]
        public int DepartmentDisplayOrder { get; set; }


        // 担当者関連情報

        // 外部キーである部署コードは登録の際に部署のインスタンスを登録すればOK
        // 担当者コードについても部署コードと同じ理屈で取得の必要がある

        public int EmployeeCode { get; set; }

        [Required]
        [Display( Name = "代表担当者かどうか" )]
        public bool IsMainEmployee { get; set; }

        [Required( ErrorMessage = "担当者名は必須です。" )]
        [StringLength( 32, ErrorMessage = "32文字まで" )]
        [Display( Name = "担当者名" )]
        public string EmployeeName { get; set; }

        [StringLength( 32, ErrorMessage = "32文字まで" )]
        [RegularExpression( @"^[ァ-ヶ|\-|ー]+$", ErrorMessage = "全角カタカナのみ入力できます。" )]
        [Display( Name = "担当者名カナ" )]
        public string EmployeePhoneticName { get; set; }

        [StringLength( 32, ErrorMessage = "32文字まで" )]
        [Display( Name = "担当者役職" )]
        public string EmployeePosition { get; set; }

        //[DataType(DataType.PhoneNumber)]
        [Phone( ErrorMessage = "正しい電話番号ではありません。" )]
        [StringLength( 16, ErrorMessage = "16文字まで" )]
        [Display( Name = "担当者電話番号" )]
        public string EmployeeTelNumber { get; set; }

        //[DataType(DataType.EmailAddress)]
        [EmailAddress( ErrorMessage = "正しいメールアドレスではありません。" )]
        [StringLength( 255, ErrorMessage = "255文字まで" )]
        [Display( Name = "担当者メールアドレス" )]
        public string EmployeeEmail { get; set; }

        [StringLength( 255, ErrorMessage = "255文字まで" )]
        [Display( Name = "担当者SNSアカウント" )]
        public string EmployeeSNS { get; set; }

        // 担当者コードと同じ値に設定する
        // SHOW TABLE STATUS LIKE 'Departments' を実行し、Auto_increment列の値が次の値となるのでそれを挿入する
        [Required]
        [Display( Name = "表示順" )]
        public int EmployeeDisplayOrder { get; set; }
    }
}
