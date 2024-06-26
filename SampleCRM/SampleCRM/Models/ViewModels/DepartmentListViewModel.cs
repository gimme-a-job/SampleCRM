using SampleCRM.Models.TableModels;
using System.ComponentModel.DataAnnotations;

namespace SampleCRM.Models.ViewModels
{
    public class DepartmentListViewModel
    {
        // TODO: 要らない場合は削除
        public int CompanyCode { get; }

        public IEnumerable<DepartmentListItemViewModel> Departments { get; }

        public DepartmentListViewModel( int conpanyCode, IEnumerable<DepartmentListItemViewModel> departments )
        {
            CompanyCode = conpanyCode;
            Departments = departments;
        }
    }

    /// <summary>
    /// ViewModelの会社に属する部署情報
    /// 契約会社の部署と担当者は1対1
    /// </summary>
    public class DepartmentListItemViewModel
    {
        [Display( Name = "部署コード" )]
        public int DepartmentCode { get; set; }

        [Display( Name = "代表部署かどうか" )]
        public bool IsMain { get; set; }

        [Display( Name = "部署名" )]
        public string DepartmentName { get; set; }

        public string DepartmentNameKana { get; set; }

        [Display( Name = "担当者名" )]
        public string EmployeeName { get; set; }

        public string EmployeeNameKana { get; set; }

        [Display( Name = "郵便番号" )]
        public string DepartmentPostalCode { get; set; }

        [Display( Name = "住所" )]
        public string DepartmentAddress { get; set; }

        [Display( Name = "電話番号" )]
        public string DepartmentPhoneNumber { get; set; }

        [Display( Name = "メールアドレス" )]
        public string DepartmentEmail { get; set; }

        [Display( Name = "表示順" )]
        public int DisplayOrder { get; set; }

        public DepartmentListItemViewModel() { }

        public DepartmentListItemViewModel( Department department ) : this()
        {
            DepartmentCode = department.DepartmentCode;
            IsMain = department.IsMain;
            DepartmentName = department.DepartmentName;
            DepartmentNameKana = department.PhoneticName;
            DepartmentPostalCode = department.PostalCode;
            DepartmentAddress = $"{department.Prefecture}{department.City}{department.Block} {department.Building}";
            DepartmentPhoneNumber = department.TelNumber;
            DepartmentEmail = department.Email;
            DisplayOrder = department.DisplayOrder;
        }

        public DepartmentListItemViewModel( Department department, Employee employee ) : this( department )
        {
            EmployeeName = employee.EmployeeName;
            EmployeeNameKana = employee.PhoneticName;
        }
    }
}
