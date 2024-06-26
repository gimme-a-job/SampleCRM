using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleCRM.Models.TableModels
{
    // Microsoft.AspNetCore.Identity.EntityFrameworkCoreの動作上必要なTableModel
    [Comment( "ユーザートークン" )]
    public class IdentityUserToken : IdentityUserToken<int>
    {
        [Display( Name = "ユーザーコード" )]
        [Comment( "ユーザーコード" )]
        [Column( "UserCode" )]
        public override int UserId { get; set; } = default!;

        [Display( Name = "ログインプロバイダー" )]
        [Comment( "ログインプロバイダー" )]
        public override string LoginProvider { get; set; } = default!;

        [Display( Name = "トークン名" )]
        [Comment( "トークン名" )]
        public override string Name { get; set; } = default!;

        [Display( Name = "トークン値" )]
        [Comment( "トークン値" )]
        public override string Value { get; set; }
    }
}
