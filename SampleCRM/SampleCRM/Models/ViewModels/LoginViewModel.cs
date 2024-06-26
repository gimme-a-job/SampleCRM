using System.ComponentModel.DataAnnotations;

namespace SampleCRM.Models.ViewModels
{
    /// <summary>
    /// AccountController の Login 用
    /// </summary>
    public class LoginViewModel
    {
        [Required( ErrorMessage = "{0}は必須です。" )]
        [Display( Name = "ユーザー名" )]
        [StringLength( 100, ErrorMessage =
            "{0}は{2}から{1}文字の範囲で設定してください。",
            MinimumLength = 4 )]
        public string UserName { get; set; }

        [Required( ErrorMessage = "{0}は必須です。" )]
        [StringLength( 100, ErrorMessage =
            "{0}は{2}から{1}文字の範囲で設定してください。",
            MinimumLength = 6 )]
        [DataType( DataType.Password )]
        [Display( Name = "パスワード" )]
        public string Password { get; set; }

        [Display( Name = "このアカウントを記憶する" )]
        public bool RememberMe { get; set; }
    }
}
