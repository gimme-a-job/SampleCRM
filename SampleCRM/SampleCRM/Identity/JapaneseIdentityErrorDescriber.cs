using Microsoft.AspNetCore.Identity;

namespace SampleCRM.Identity
{
#nullable enable
    /// <summary>
    /// Identity周りでエラーメッセージが英語になっている部分を日本語化する
    /// </summary>
    /// <remarks>
    /// リソースファイルだけ作りメッセージを差し替えることは出来ない様なので、継承クラスを作成
    /// </remarks>
    public class JapaneseIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DefaultError() => new IdentityError
        {
            Code = nameof( DefaultError ),
            Description = "不明なエラーが発生しました。"
        };

        public override IdentityError ConcurrencyFailure() => new IdentityError
        {
            Code = nameof( ConcurrencyFailure ),
            Description = "他のユーザーが変更を加えた可能性があります。"
        };

        public override IdentityError PasswordMismatch() => new IdentityError
        {
            Code = nameof( PasswordMismatch ),
            Description = "パスワードが間違っています。"
        };

        public override IdentityError InvalidToken() => new IdentityError
        {
            Code = nameof( InvalidToken ),
            Description = "不正なトークンです。"
        };

        public override IdentityError RecoveryCodeRedemptionFailed() => new IdentityError
        {
            Code = nameof( RecoveryCodeRedemptionFailed ),
            Description = "リカバリーコードが確認されていません。"
        };

        public override IdentityError LoginAlreadyAssociated() => new IdentityError
        {
            Code = nameof( LoginAlreadyAssociated ),
            Description = "既にそのログイン方法はユーザーと関連付けがされています。"
        };

        public override IdentityError InvalidUserName( string? userName ) => new IdentityError
        {
            Code = nameof( InvalidUserName ),
            Description = $"ユーザー名：{userName} は正しくありません。"
        };

        public override IdentityError InvalidEmail( string? email ) => new IdentityError
        {
            Code = nameof( InvalidEmail ),
            Description = $"メールアドレス：{email} は正しくありません。"
        };

        public override IdentityError DuplicateUserName( string userName ) => new IdentityError
        {
            Code = nameof( DuplicateUserName ),
            Description = $"ユーザー名：{userName} は既に使用されています。"
        };

        public override IdentityError DuplicateEmail( string email ) => new IdentityError
        {
            Code = nameof( DuplicateEmail ),
            Description = $"メールアドレス：{email} は既に使用されています。"
        };

        public override IdentityError InvalidRoleName( string? role ) => new IdentityError
        {
            Code = nameof( InvalidRoleName ),
            Description = $"ロール名：{role} は正しくありません。"
        };

        public override IdentityError DuplicateRoleName( string role ) => new IdentityError
        {
            Code = nameof( DuplicateRoleName ),
            Description = $"ロール名：{role} は既に存在します。"
        };

        public override IdentityError UserAlreadyHasPassword() => new IdentityError
        {
            Code = nameof( UserAlreadyHasPassword ),
            Description = "ユーザーにはパスワードが既に設定されています。"
        };

        public override IdentityError UserLockoutNotEnabled() => new IdentityError
        {
            Code = nameof( UserLockoutNotEnabled ),
            Description = "このユーザーにはロックアウトが有効ではありません。"
        };

        public override IdentityError UserAlreadyInRole( string role ) => new IdentityError
        {
            Code = nameof( UserAlreadyInRole ),
            Description = $"ユーザーには ロール{role} が既に割り当てられています。"
        };

        public override IdentityError UserNotInRole( string role ) => new IdentityError
        {
            Code = nameof( UserNotInRole ),
            Description = $"ユーザーに ロール{role} が割り当てられていません。"
        };

        public override IdentityError PasswordTooShort( int length ) => new IdentityError
        {
            Code = nameof( PasswordTooShort ),
            Description = $"パスワードは{length}文字以上である必要があります。"
        };

        public override IdentityError PasswordRequiresUniqueChars( int uniqueChars ) => new IdentityError
        {
            Code = nameof( PasswordRequiresUniqueChars ),
            Description = $"パスワードには{uniqueChars}種類以上の文字を含める必要があります。"
        };

        public override IdentityError PasswordRequiresNonAlphanumeric() => new IdentityError
        {
            Code = nameof( PasswordRequiresNonAlphanumeric ),
            Description = "パスワードには英数字以外の文字を含める必要があります。"
        };

        public override IdentityError PasswordRequiresDigit() => new IdentityError
        {
            Code = nameof( PasswordRequiresDigit ),
            Description = "パスワードには数字を含める必要があります。"
        };

        public override IdentityError PasswordRequiresLower() => new IdentityError
        {
            Code = nameof( PasswordRequiresLower ),
            Description = "パスワードには英字(小文字)を含める必要があります。"
        };

        public override IdentityError PasswordRequiresUpper() => new IdentityError
        {
            Code = nameof( PasswordRequiresUpper ),
            Description = "パスワードには英字(大文字)を含める必要があります。"
        };
    }
}
