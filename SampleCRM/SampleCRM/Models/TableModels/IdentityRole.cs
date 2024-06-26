using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleCRM.Models.TableModels
{
    [Comment( "ロールマスタ" )]
    public class IdentityRole : IdentityRole<int>
    {
        [Display( Name = "ロールコード" )]
        [Comment( "ロールコード" )]
        [Column( "RoleCode" )]
        public override int Id { get; set; }

        [Display( Name = "ロール名" )]
        [Comment( "ロール名" )]
        public override string Name { get; set; }

        [Display( Name = "正規化ロール名", Description = "検索の効率化のために使用される" )]
        [Comment( "正規化ロール名" )]
        public override string NormalizedName { get; set; }

        [Display( Name = "コンカレンシースタンプ", Description = "更新の競合を防止するために使用される値" )]
        [Comment( "コンカレンシースタンプ" )]
        public override string ConcurrencyStamp { get; set; }
    }
}
