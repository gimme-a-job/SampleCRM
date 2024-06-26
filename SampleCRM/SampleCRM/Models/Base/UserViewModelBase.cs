using SampleCRM.Models.TableModels;
using System.ComponentModel.DataAnnotations;

namespace SampleCRM.Models.Base
{
    /// <summary>
    /// ユーザーアカウント作成・編集時VMの基底クラス
    /// </summary>
    public abstract class UserViewModelBase
    {
        [Required( ErrorMessage = "{0}は必須です。" )]
        [StringLength( 100, ErrorMessage = "{0}は{2}から{1}文字の範囲で設定してください。", MinimumLength = 4 )]
        [Display( Name = "ユーザー名" )]
        public string UserName { get; set; }

        [StringLength( 100, ErrorMessage = "{0}は{2}から{1}文字の範囲で設定してください。", MinimumLength = 6 )]
        [DataType( DataType.Password )]
        [Display( Name = "パスワード" )]
        public abstract string Password { get; set; }

        [DataType( DataType.Password )]
        [Display( Name = "パスワード確認" )]
        [Compare( "Password", ErrorMessage = "確認パスワードが一致しません。" )]
        public string ConfirmPassword { get; set; }

        [Required( ErrorMessage = "{0}は必須です。" )]
        [EmailAddress( ErrorMessage = "メールアドレスの形式で入力して下さい。" )]
        [Display( Name = "メールアドレス" )]
        public string Email { get; set; }

        /// <summary>
        /// 選択されているロール
        /// </summary>
        public string SelectedRole { get; set; }

        /// <summary>
        /// ロール一覧
        /// </summary>
        /// <remarks>
        /// プルダウン表示用
        /// </remarks>
        public IEnumerable<IdentityRole> Roles { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UserViewModelBase() { }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="selectedRole"></param>
        /// <param name="roles"></param>
        /// <remarks>
        /// ロール一覧とアサインされているロールをVMに設定する
        /// </remarks>
        public UserViewModelBase( string selectedRole, IEnumerable<IdentityRole> roles )
            : this()
        {
            this.SelectedRole = selectedRole;
            this.Roles = roles;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="user"></param>
        /// <param name="selectedRole"></param>
        /// <param name="roles"></param>
        /// <remarks>
        /// TableModelをViewModelに変換する
        /// </remarks>
        public UserViewModelBase( IdentityUser user, string selectedRole, IEnumerable<IdentityRole> roles )
            : this( selectedRole, roles )
        {
            this.UserName = user.UserName;
            this.Email = user.Email;
        }

        /// <summary>
        /// ViewModelをTableModelに変換する
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// TableModel側にコンストラクタを追加したくないため、こちらで行っている
        /// </remarks>
        public IdentityUser GetTableModel() => new IdentityUser
        {
            UserName = this.UserName,
            Email = this.Email,
        };
    }
}
