using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleCRM.Models.TableModels
{
    /// <summary>
    /// 契約-システムライセンス
    /// </summary>
    [Table( "C_ContractToSystemLicenses" )]
    public class ContractToSystemLicense
    {
        [Required]
        [Display( Name = "契約コード" )]
        [Comment( "契約コード" )]
        public int ContractCode { get; set; }

        [Required]
        [Display( Name = "システムライセンスコード" )]
        [Comment( "システムライセンスコード" )]
        public int SystemLicenseCode { get; set; }
    }
}
