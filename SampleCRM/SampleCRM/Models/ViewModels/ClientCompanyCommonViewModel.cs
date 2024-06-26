using SampleCRM.Models.TableModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Department = SampleCRM.Models.TableModels.Department;

namespace SampleCRM.Models.ViewModels
{
    /// <summary>
    /// 契約会社一覧用ViewModel
    /// CompaniesテーブルのIsClientCompanyカラムがtrueのレコードが対象
    /// </summary>
    public class ClientCompanyCommonViewModel
    {
        // TODO: 整数型なので桁数と正負判断だけで良い。エラーの日本語化対応後に修正。
        [Required( ErrorMessage = "{0}は必須です。" )]
        [RegularExpression( @"^[0-9]{1,9}$", ErrorMessage = "{0}は1桁以上9桁以下の整数のみ入力出来ます。" )]
        [Display( Name = "会社コード" )]
        public int CompanyCode { get; set; } = 0;

        [Required( ErrorMessage = "{0}は必須です。" )]
        [StringLength( 128 )]
        [Display( Name = "会社名" )]
        public string CompanyName { get; set; }

        [Display( Name = "会社名カナ" )]
        [RegularExpression( @"\A[ァ-ヶ ー]+\z", ErrorMessage = "全角カタカナのみ入力できます。" )]
        public string PhoneticName { get; set; }

        [Display( Name = "会社状態" )]
        public string CompanyStatus { get; set; }

        [Display( Name = "会社状態選択" )]
        public SelectList CompanyStatusSelectList { get; set; }

        [Display( Name = "訪問が必要かどうか" )]
        public bool IsNeedVisiting { get; set; }

        [Display( Name = "代表部署住所" )]
        public string MainDepartmentAddress { get; set; }

        [Display( Name = "代表部署電話番号" )]
        public string MainDepartmentPhoneNumber { get; set; }

        [Display( Name = "システム種別（契約中）" )]
        public string SystemKinds { get; set; }

        [Display( Name = "経審許可番号" )]
        [RegularExpression( @"^[0-9]{2}-[0-9]{6}$", ErrorMessage = "[○○-○○○○○○]の形式で記載して下さい。" )]
        [StringLength( 10 )]
        public string BusinessEvaluationPermitNumber { get; set; }

        [Display( Name = "備考" )]
        [StringLength( 2048 )]
        [DataType( DataType.MultilineText )]
        public string Note { get; set; }

        // 部署
        public DepartmentListViewModel DepartmentList { get; set; }

        public OrderListViewModel OrderList { get; set; }

        public ContractListViewModel ContractList { get; set; }

        public IEnumerable<MaintenanceFeeBillListItemViewModel> MaintenanceFeeBillList { get; set; }

        public ClientCompanyCommonViewModel() { }

        public ClientCompanyCommonViewModel( Company company ) : this()
        {
            this.CompanyCode = company.CompanyCode;
            this.CompanyName = company.CompanyName;
            this.PhoneticName = company.PhoneticName;
            this.IsNeedVisiting = company.IsNeedVisiting;
            //this.BusinessEvaluationPermitNumber = company.BusinessEvaluationPermitNumber.ToInvalidStrIsNullOrEmpty();
            this.BusinessEvaluationPermitNumber = company.BusinessEvaluationPermitNumber;
            this.Note = company.Note;
        }

        public ClientCompanyCommonViewModel( Company company, Department? mainDepartment ) : this( company )
        {
            if ( mainDepartment is not null )
            {
                this.MainDepartmentAddress = $"{mainDepartment.Prefecture}{mainDepartment.City}{mainDepartment.Block} {mainDepartment.Building}";

                this.MainDepartmentPhoneNumber = mainDepartment.TelNumber;
            }
            // 代表部署が未登録の場合
            else
            {
                this.MainDepartmentAddress = this.MainDepartmentPhoneNumber = "―";
            }

        }
    }
}
