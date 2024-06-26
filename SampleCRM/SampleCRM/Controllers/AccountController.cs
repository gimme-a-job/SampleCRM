using SampleCRM.Common;
using SampleCRM.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IdentityUser = SampleCRM.Models.TableModels.IdentityUser;

namespace SampleCRM.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private SignInManager<IdentityUser> SignInManager { get; }
        private UserManager<IdentityUser> UserManager { get; }

        public AccountController( SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager )
        {
            SignInManager = signInManager;
            UserManager = userManager;
        }

        public IActionResult Login( string returnUrl = null )
        {
            returnUrl ??= Url.Content( "~/" );

            ViewBag.ReturnUrl = returnUrl;

            // 既にログイン済ならトップページに飛ばす
            if ( SignInManager.IsSignedIn( User ) )
            {
                HttpContext.Response.Redirect( returnUrl );
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login( [Bind( @$"
            {nameof( viewModel.UserName )},
            {nameof( viewModel.Password )},
            {nameof( viewModel.RememberMe )}" )]
            LoginViewModel viewModel, string returnUrl = null )
        {
            returnUrl ??= Url.Content( "~/" );

            if ( ModelState.IsValid )
            {
                var result = await SignInManager.PasswordSignInAsync( viewModel.UserName, viewModel.Password, viewModel.RememberMe, lockoutOnFailure: true );

                if ( result.Succeeded )
                {
                    SampleLog.logger.Info( $"ユーザーがログインしました。 {nameof( viewModel.UserName )}：{viewModel.UserName}" );
                    return LocalRedirect( returnUrl );
                }

                if ( result.IsLockedOut )
                {
                    SampleLog.logger.Warn(
                        $"ロックアウトされたユーザーがログインを試みました。 {nameof( viewModel.UserName )}：{viewModel.UserName}" );
                    AddErrorToModelState( "規定の回数ログインに失敗したため、アカウントがロックされています。" );
                }
                else
                {
                    AddErrorToModelState( "無効なログイン" );
                }
            }

            return View( viewModel );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout( string returnUrl = null )
        {
            await SignInManager.SignOutAsync();

            // _LayoutPartial.cshtml の form 要素の
            // asp-route-returnUrl="@Url.Action("Index", "Home", new { area = "" })
            // により returnUrl は "/" になる。
            if ( returnUrl is not null )
            {
                return LocalRedirect( returnUrl );
            }
            else
            {
                // とりあえずログイン画面に飛ばしている
                return RedirectToAction( nameof( Login ), "Account" );
            }
        }

        /// <summary>
        /// アクセス拒否画面
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public IActionResult AccessDenied() => View();

        /// <summary>
        /// エラー内容をView側に表示させる
        /// </summary>
        /// <param name="errorMessage"></param>
        private void AddErrorToModelState( string errorMessage ) => ModelState.AddModelError( string.Empty, errorMessage );
    }
}
