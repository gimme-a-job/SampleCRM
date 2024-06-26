using SampleCRM.Identity;
using SampleCRM.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdentityRole = SampleCRM.Models.TableModels.IdentityRole;


namespace SampleCRM.Controllers
{
    /// <summary>
    /// ロール一覧ページのController
    /// </summary>
    [Authorize( Policy = nameof( AuthorizationPolicies.UserAdministratorPolicy ) )]
    public class RoleController : Controller
    {
        private RoleManager<IdentityRole> RoleManager { get; }

        public RoleController( RoleManager<IdentityRole> roleManager ) => RoleManager = roleManager;

        /// <summary>
        /// ロール一覧画面
        /// </summary>
        /// <returns></returns>
        /// <remarks>Get: Role/Index</remarks>
        public async Task<IActionResult> Index()
            => View( await RoleManager.Roles.OrderBy( x => x.Id ).Select( x => new RoleViewModel( x ) ).ToListAsync() );
    }
}
