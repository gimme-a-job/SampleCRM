using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleCRM.Models.TableModels
{
    // TODO: 退会時には全ロールを剥奪することの明文化。（1度アカウントを作ったら消さない運用を想定。）
    [Comment( "ユーザーマスタ" )]
    public class IdentityUser : IdentityUser<int>
    {
        [Display( Name = "ユーザーコード" )]
        [Comment( "ユーザーコード" )]
        [Column( "UserCode" )]

        public override int Id { get; set; }

        [Display( Name = "ユーザー名" )]
        [Comment( "ユーザー名" )]
        public override string UserName { get; set; }

        [Display( Name = "正規化ユーザー名", Description = "検索の効率化のために使用される" )]
        [Comment( "正規化ユーザー名" )]
        public override string NormalizedUserName { get; set; }

        [EmailAddress( ErrorMessage = "メールアドレスの形式で入力して下さい" )]
        [Display( Name = "メールアドレス" )]
        [Comment( "メールアドレス" )]
        public override string Email { get; set; }

        [Display( Name = "正規化メールアドレス", Description = "検索の効率化のために使用される" )]
        [Comment( "正規化メールアドレス" )]
        public override string NormalizedEmail { get; set; }

        [Display( Name = "メールアドレスを確認したかどうか" )]
        [Comment( "メールアドレスを確認したかどうか" )]
        public override bool EmailConfirmed { get; set; }

        [DataType( DataType.Password )]
        [Display( Name = "パスワード" )]
        [Comment( "パスワード" )]
        public override string PasswordHash { get; set; }

        [Display( Name = "セキュリティスタンプ", Description = "ユーザーの資格情報が変更されるたびに変更する必要があるランダムな値 (パスワードの変更、ログインの削除)" )]
        [Comment( "セキュリティスタンプ" )]
        public override string SecurityStamp { get; set; }

        [Display( Name = "コンカレンシースタンプ", Description = "更新の競合を防止するために使用される値" )]
        [Comment( "コンカレンシースタンプ" )]
        public override string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

        [Display( Name = "電話番号" )]
        [Comment( "電話番号" )]
        public override string PhoneNumber { get; set; }

        [Display( Name = "電話番号を確認したかどうか" )]
        [Comment( "電話番号を確認したかどうか" )]
        public override bool PhoneNumberConfirmed { get; set; }

        [Display( Name = "2要素認証が有効かどうか" )]
        [Comment( "2要素認証が有効かどうか" )]
        public override bool TwoFactorEnabled { get; set; }

        [Display( Name = "ロックアウト終了日時 (UTC) " )]
        [Comment( "ロックアウト終了日時 (UTC)" )]
        [Precision( 0 )]
        public override DateTimeOffset? LockoutEnd { get; set; }

        [Display( Name = "ロックアウトが有効かどうか" )]
        [Comment( "ロックアウトが有効かどうか" )]
        public override bool LockoutEnabled { get; set; }

        [Display( Name = "ログイン失敗回数" )]
        [Comment( "ログイン失敗回数" )]
        public override int AccessFailedCount { get; set; }
    }
}