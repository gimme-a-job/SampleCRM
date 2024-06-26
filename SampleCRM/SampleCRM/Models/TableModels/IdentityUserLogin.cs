using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleCRM.Models.TableModels
{
    // Microsoft.AspNetCore.Identity.EntityFrameworkCoreの動作上必要なTableModel
    [Comment( "ユーザーログイン" )]
    public class IdentityUserLogin : IdentityUserLogin<int>
    {
        [Display( Name = "ログインプロバイダー", Description = " facebook、googleなど" )]
        [Comment( "ログインプロバイダー" )]
        public override string LoginProvider { get; set; } = default!;

        [Display( Name = "プロバイダーキー" )]
        [Comment( "プロバイダーキー" )]
        public override string ProviderKey { get; set; } = default!;

        [Display( Name = "プロバイダー表示名" )]
        [Comment( "プロバイダー表示名" )]
        public override string ProviderDisplayName { get; set; }

        [Display( Name = "ユーザーコード" )]
        [Comment( "ユーザーコード" )]
        [Column( "UserCode" )]
        public override int UserId { get; set; } = default!;
    }
}
