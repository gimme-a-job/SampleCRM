using SampleCRM.Models.TableModels;
using System.ComponentModel.DataAnnotations;

namespace SampleCRM.Models.ViewModels
{
    /// <summary>
    /// ユーザーの一覧表示用のVM
    /// </summary>
    public class UserRoleViewModel
    {
        public string UserCode { get; }

        [Display( Name = "ユーザー名" )]
        public string UserName { get; }

        [Display( Name = "メールアドレス" )]
        public string Email { get; }

        [Display( Name = "ロール" )]
        public string Role { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roleName"></param>
        public UserRoleViewModel( IdentityUser user, string roleName )
        {
            this.UserCode = user.Id.ToString();
            this.UserName = user.UserName;
            this.Email = user.Email;
            this.Role = roleName;
        }
    }
}
