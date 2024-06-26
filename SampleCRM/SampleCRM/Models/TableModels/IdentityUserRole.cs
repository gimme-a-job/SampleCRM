using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleCRM.Models.TableModels
{
    [Comment( "ユーザーロール" )]
    public class IdentityUserRole : IdentityUserRole<int>
    {
        [Display( Name = "ユーザーコード" )]
        [Comment( "ユーザーコード" )]
        [Column( "UserCode" )]
        public override int UserId { get; set; }

        [Display( Name = "ロールコード" )]
        [Comment( "ロールコード" )]
        [Column( "RoleCode" )]
        public override int RoleId { get; set; }
    }
}
