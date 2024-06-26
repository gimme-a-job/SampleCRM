using SampleCRM.Common;
using SampleCRM.Identity;
using SampleCRM.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdentityRole = SampleCRM.Models.TableModels.IdentityRole;
using IdentityUser = SampleCRM.Models.TableModels.IdentityUser;

namespace SampleCRM.Controllers
{
    [Authorize( Policy = nameof( AuthorizationPolicies.UserAdministratorPolicy ) )]
    public class UserController : Controller
    {
        private UserManager<IdentityUser> UserManager { get; }
        private RoleManager<IdentityRole> RoleManager { get; }

        public UserController( UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager )
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        /// <summary>
        /// ユーザー一覧画面
        /// </summary>
        /// <returns></returns>
        /// <remarks>GET: User/Index</remarks>
        public async Task<IActionResult> Index()
        {
            var viewModels = new List<UserRoleViewModel>();

            if ( await UserManager.Users.OrderBy( user => user.Id ).ToListAsync() is not { } users )
            {
                return NotFound();
            }

            foreach ( var user in users )
            {
                // ViewModelのユーザー一人作成し登録
                viewModels.Add( new UserRoleViewModel( user, await GetAssignedRoleNameAsync( user ) ) );
            }

            return View( viewModels );
        }

        /// <summary>
        /// ユーザー新規作成
        /// </summary>
        /// <returns></returns>
        /// <remarks>GET: User/Create</remarks>
        public IActionResult Create()
        {
            var viewModel = new UserRegisterViewModel( RoleNames.NotAssigned, RoleManager.Roles );

            return View( viewModel );
        }

        /// <summary>
        /// 作成ボタン押下時
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        /// <remarks>POST: User/Create</remarks>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( [Bind( @$"
            {nameof( viewModel.UserName )},
            {nameof( viewModel.Email )},
            {nameof( viewModel.Password )},
            {nameof( viewModel.ConfirmPassword )},
            {nameof( viewModel.SelectedRole )}" )]
            UserRegisterViewModel viewModel )
        {
            if ( ModelState.IsValid )
            {
                var user = viewModel.GetTableModel();

                var result = await UserManager.CreateAsync( user, viewModel.Password );

                if ( result.Succeeded )
                {
                    SampleLog.logger.Info( $"新規ユーザーが作成されました。 {nameof( viewModel.UserName )}：{viewModel.UserName}" );

                    // ユーザー登録に成功したら今度はロールへの登録を実施する

                    // 「ロール無し」が選択されている場合はロールへの登録を行わない
                    if ( viewModel.SelectedRole != RoleNames.NotAssigned )
                    {
                        // 新規登録の時点ではロールがアサイン済みかどうかの確認は不要
                        result = await UserManager.AddToRoleAsync( user, viewModel.SelectedRole );
                        if ( !result.Succeeded )
                        {
                            AddErrorsToModelState( result );

                            return View( viewModel );
                        }
                    }

                    // 登録に成功したら User/Index にリダイレクト
                    return RedirectToAction( nameof( Index ), "User" );
                }

                // result.Succeeded が false の場合 ModelSate にエラー情報を追加しないとエラーメッセージが出ない。
                AddErrorsToModelState( result );
            }

            // ユーザー登録に失敗した場合、登録画面を再描画
            return View( viewModel );
        }

        /// <summary>
        /// パスワードの変更のみ(初期化ではない)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>GET: User/ChangePassword/Id</remarks>
        public IActionResult ChangePassword( string id )
        {
            if ( id is null )
            {
                return NotFound();
            }

            return View( new PasswordViewModel() );
        }

        /// <summary>
        /// 変更ボタン押下時
        /// </summary>
        /// <param name="id"></param>
        /// <param name="viewModel"></param>
        /// <returns>POST: User/ChangePassword/Id</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword( string id, [Bind( @$"
            {nameof( viewModel.Password )},
            {nameof( viewModel.ConfirmPassword )}" )]
            PasswordViewModel viewModel )
        {
            // モデル側でPasswordとConfirmPasswordの比較は実施済み
            if ( id is null )
            {
                return NotFound();
            }

            if ( ModelState.IsValid )
            {
                var user = await UserManager.FindByIdAsync( id );

                if ( user is null )
                {
                    return NotFound();
                }

                var resultPassword = await UserManager.PasswordValidators[ 0 ].ValidateAsync( UserManager, user, viewModel.Password );

                if ( resultPassword.Succeeded )
                {
                    // 検証 OK の場合、入力パスワードのハッシュ値を取得
                    user.PasswordHash = UserManager.PasswordHasher.HashPassword( user, viewModel.Password );
                }
                else
                {
                    // 検証 NG の場合 ModelSate にエラー情報を
                    // 追加して編集画面を再描画
                    AddErrorsToModelState( resultPassword );

                    return View( viewModel );
                }

                // Update実行
                var resultUpdate = await UserManager.UpdateAsync( user );

                if ( resultUpdate.Succeeded )
                {
                    // 更新に成功したら User/Index にリダイレクト
                    return RedirectToAction( nameof( Index ), "User" );
                }
                // Register.cshtml.cs のものをコピー
                AddErrorsToModelState( resultUpdate );
            }

            // 更新に失敗した場合、編集画面を再描画
            return View( viewModel );
        }

        /// <summary>
        /// ユーザー編集画面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>GET: User/Edit/Id</remarks>
        public async Task<IActionResult> Edit( string id )
        {
            if ( id is null || await UserManager.FindByIdAsync( id ) is not { } user )
            {
                return NotFound();
            }

            return View( new UserEditViewModel( user, await GetAssignedRoleNameAsync( user ), RoleManager.Roles ) );
        }

        /// <summary>
        /// 保存ボタン押下時
        /// </summary>
        /// <param name="id"></param>
        /// <param name="viewModel"></param>
        /// <returns>POST: User/Edit/Id</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( string id, [Bind( @$"
            {nameof( viewModel.UserName )},
            {nameof( viewModel.Email )},
            {nameof( viewModel.Password )},
            {nameof( viewModel.ConfirmPassword )},
            {nameof( viewModel.SelectedRole )}" )]
            UserEditViewModel viewModel )
        {
            if ( id is null )
            {
                return NotFound();
            }

            if ( ModelState.IsValid )
            {
                if ( await UserManager.FindByIdAsync( id ) is not { } user
                    || await RoleManager.Roles.OrderBy( role => role.Id ).ToListAsync() is not { } roles )
                {
                    return NotFound();
                }

                // View側での入力内容をTableModel側に適用していく
                user.UserName = viewModel.UserName;
                user.Email = viewModel.Email;

                // DBから取得したロールを回しているため、ここには「ロール無し」が含まれていない
                // 「ロール無し」のアサインや解除の場合はDBへの追加も削除も必要ない
                foreach ( var role in roles )
                {
                    // 対象のロールについて
                    var resultRole = await UserManager.IsInRoleAsync( user, role.Name ) switch
                    {
                        // アサインされた場合は登録
                        false when IsSelected() => await UserManager.AddToRoleAsync( user, role.Name ),
                        // アサインが解除された場合は削除
                        true when !IsSelected() => await UserManager.RemoveFromRoleAsync( user, role.Name ),
                        // 変更なしの場合
                        _ => null
                    };

                    // 変更があったが何らかの理由で処理が失敗した場合
                    if ( resultRole is not null && !resultRole.Succeeded )
                    {
                        AddErrorsToModelState( resultRole );

                        return View( viewModel );
                    }

                    bool IsSelected() => role.Name == viewModel.SelectedRole;
                }

                // Update実行
                var resultUpdate = await UserManager.UpdateAsync( user );

                if ( resultUpdate.Succeeded )
                {
                    // 更新に成功したら User/Index にリダイレクト
                    return RedirectToAction( nameof( Index ), "User" );
                }

                AddErrorsToModelState( resultUpdate );
            }

            // 更新に失敗した場合、編集画面を再描画
            return View( viewModel );
        }

        /// <summary>
        /// ユーザー削除画面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>GET: User/Delete/Id</remarks>
        public async Task<IActionResult> Delete( string id )
        {
            if ( id is null || await UserManager.FindByIdAsync( id ) is not { } user )
            {
                return NotFound();
            }

            return View( user );
        }

        /// <summary>
        /// 削除ボタン押下時
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// 上の Delete(string id) と同シグネチャのメソッドは定義できないため、メソッド名を変えてActionName("Delete") を設定している
        /// </remarks>
        [HttpPost, ActionName( nameof( Delete ) )]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed( string id )
        {
            if ( id is null || await UserManager.FindByIdAsync( id ) is not { } user )
            {
                return NotFound();
            }

            var result = await UserManager.DeleteAsync( user );

            if ( result.Succeeded )
            {
                // 削除に成功したら User/Index にリダイレクト
                return RedirectToAction( nameof( Index ), "User" );
            }

            AddErrorsToModelState( result );

            return View( user );
        }

        /// <summary>
        /// ユーザーにアサインされているロールの名前を取得する
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        /// <remarks>
        /// ロールのアサインが無い場合は文字列"ロール無し"を返す
        /// もし複数のロールがアサインされていた場合は例外となる。
        /// </remarks>
        private async Task<string> GetAssignedRoleNameAsync( IdentityUser user )
        {
            // ユーザーにアサインされているロールを取得する
            var role = await UserManager.GetRolesAsync( user );

            return role.Count switch
            {
                // ロールのアサインが無い場合
                0 => RoleNames.NotAssigned,

                // ロールがアサインされている場合
                1 => role.Single(),

                // 複数のロールがアサインされていた場合
                _ => throw new NotSupportedException(),
            };
        }

        /// <summary>
        /// CUD操作に失敗した際に、エラー内容をView側に表示する
        /// </summary>
        /// <param name="result"></param>
        private void AddErrorsToModelState( IdentityResult result )
        {
            foreach ( var error in result.Errors )
            {
                ModelState.AddModelError( string.Empty, error.Description );
            }
        }
    }
}