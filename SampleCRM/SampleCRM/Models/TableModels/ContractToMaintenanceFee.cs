using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleCRM.Models.TableModels
{
    /// <summary>
    /// 契約-保守料
    /// </summary>
    [Table( "C_ContractToMaintenanceFees" )]
    public class ContractToMaintenanceFee
    {
        [Required]
        [Display( Name = "契約コード" )]
        [Comment( "契約コード" )]
        public int ContractCode { get; set; }

        [Required]
        [Display( Name = "保守料コード" )]
        [Comment( "保守料コード" )]
        public int MaintenanceFeeCode { get; set; }
    }
}
