using SampleCRM.Models.Base;
using SampleCRM.Models.TableModels;
using System.ComponentModel.DataAnnotations;

namespace SampleCRM.Models.ViewModels
{
    /// <summary>
    /// UserController の Create 用
    /// </summary>
    public class UserRegisterViewModel : UserViewModelBase
    {
        [Required( ErrorMessage = "{0}は必須です。" )]
        public override string Password { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <remarks>
        /// これを消すとController側でのBind時にエラーを起こしたため残している
        /// </remarks>
        public UserRegisterViewModel() { }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="roles"></param>
        /// <remarks>
        /// ロール一覧とアサインされているロールをVMに設定する
        /// </remarks>
        public UserRegisterViewModel( string roleName, IEnumerable<IdentityRole> roles )
            : base( roleName, roles ) { }
    }
}
