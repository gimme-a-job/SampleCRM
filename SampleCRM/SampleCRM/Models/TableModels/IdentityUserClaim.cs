using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleCRM.Models.TableModels
{
    // Microsoft.AspNetCore.Identity.EntityFrameworkCoreの動作上必要なTableModel
    [Comment( "ユーザークレーム" )]
    public class IdentityUserClaim : IdentityUserClaim<int>
    {
        [Display( Name = "要求コード" )]
        [Comment( "要求コード" )]
        [Column( "ClaimCode" )]
        public override int Id { get; set; } = default!;

        [Display( Name = "ユーザーコード" )]
        [Comment( "ユーザーコード" )]
        [Column( "UserCode" )]
        public override int UserId { get; set; } = default!;

        [Display( Name = "要求タイプ" )]
        [Comment( "要求タイプ" )]
        public override string ClaimType { get; set; }

        [Display( Name = "要求値" )]
        [Comment( "要求値" )]
        public override string ClaimValue { get; set; }
    }
}
