using System.ComponentModel.DataAnnotations;

namespace SampleCRM.Models.ViewModels
{
    public class SalesCompany
    {
        // Companiesから取得できるデータ
        [Key]
        [Display( Name = "会社コード" )]
        public int CompanyCode { get; set; } = 0;

        [Required( ErrorMessage = "{0}は必須です。" )]
        [StringLength( 128 )]
        [Display( Name = "会社名" )]
        public string CompanyName { get; set; }

        [Display( Name = "会社名カナ" )]
        [RegularExpression( @"\A[ァ-ヶ ー]+\z", ErrorMessage = "全角カタカナのみ入力できます。" )]
        public string PhoneticName { get; set; }

        [Display( Name = "備考" )]
        [StringLength( 2048 )]
        [DataType( DataType.MultilineText )]
        public string Note { get; set; }

        // Departmentsから取得できるデータ
        [Display( Name = "郵便番号" )]
        public string PostalCode { get; set; }

        [Display( Name = "住所" )]
        public string Address { get; set; }

        [Display( Name = "電話番号" )]
        public string TelNumber { get; set; }

        [Display( Name = "営業部署一覧" )]
        public IList<SalesDepartment> Departments { get; set; }
    }
}
