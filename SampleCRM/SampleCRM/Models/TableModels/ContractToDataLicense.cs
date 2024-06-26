using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleCRM.Models.TableModels
{
    /// <summary>
    /// 契約-データライセンス
    /// </summary>
    [Table( "C_ContractToDataLicenses" )]
    public class ContractToDataLicense
    {
        [Required]
        [Display( Name = "契約コード" )]
        [Comment( "契約コード" )]
        public int ContractCode { get; set; }

        [Required]
        [Display( Name = "データライセンスコード" )]
        [Comment( "データライセンスコード" )]
        public int DataLicenseCode { get; set; }
    }
}
