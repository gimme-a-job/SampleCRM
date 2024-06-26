using System.ComponentModel.DataAnnotations;

namespace SampleCRM.Models.Enums
{
    public enum SearchMethod
    {
        [Display( Name = "会社名" )]
        CompanyName,

        [Display( Name = "会社名カナ" )]
        CompanyNameKana,

        [Display( Name = "部署名" )]
        DepartmentName,

        [Display( Name = "部署名カナ" )]
        DepartmentNameKana,

        [Display( Name = "電話番号" )]
        PhoneNumber,
    }

    public enum CompanyCodeOrder
    {
        [Display( Name = "昇順" )]
        ASC,

        [Display( Name = "降順" )]
        DESC,
    }

    public enum DisplayCountPerPage
    {
        [Display( Name = "20件" )]
        UpTo20 = 20,

        [Display( Name = "50件" )]
        UpTo50 = 50,

        [Display( Name = "100件" )]
        UpTo100 = 100,

        [Display( Name = "全件" )]
        UpTo50000 = 50000,
    }
}
