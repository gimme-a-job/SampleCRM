using SampleCRM.Models.Base;
using SampleCRM.Models.TableModels;

namespace SampleCRM.Models.ViewModels
{
    /// <summary>
    /// UserController の Edit 用
    /// </summary>
    public class UserEditViewModel : UserViewModelBase
    {
        // 現状パスワードの変更は別画面から行うため、こちらでは[Required]を付けていない
        public override string Password { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <remarks>
        /// これを消すとController側でのBind時にエラーを起こしたため残している
        /// </remarks>
        public UserEditViewModel() { }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roleName"></param>
        /// <param name="roles"></param>
        /// <remarks>
        /// TableModelをViewModelに変換する
        /// </remarks>
        public UserEditViewModel( IdentityUser user, string roleName, IEnumerable<IdentityRole> roles )
            : base( user, roleName, roles ) { }
    }
}
