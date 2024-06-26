using System.ComponentModel.DataAnnotations;

namespace SampleCRM.Models.ViewModels
{
    public class PasswordViewModel
    {
        [Required( ErrorMessage = "{0}は必須です。" )]
        [StringLength( 100, ErrorMessage = "{0}は{2}から{1}文字の範囲で設定してください。", MinimumLength = 6 )]
        [DataType( DataType.Password )]
        [Display( Name = "新パスワード" )]
        public string Password { get; set; }

        [DataType( DataType.Password )]
        [Display( Name = "新パスワード確認" )]
        [Compare( "Password", ErrorMessage = "確認パスワードが一致しません。" )]
        public string ConfirmPassword { get; set; }
    }
}
