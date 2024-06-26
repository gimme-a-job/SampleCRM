using SampleCRM.Common;
using SampleCRM.Contexts;
using SampleCRM.Identity;
using SampleCRM.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Net.Mime;
using System.Runtime.Versioning;
using System.Text;

namespace SampleCRM.Controllers
{
    [Authorize( Policy = nameof( AuthorizationPolicies.AnyRolePolicy ) )]
    // for GetSampleFormatString
    [SupportedOSPlatform( "windows" )]
    public class DataManagementController : Controller
    {
        private readonly SampleCRMContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public DataManagementController( SampleCRMContext context, IWebHostEnvironment hostingEnvironment )
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: DataManagementController
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 画面上からInsertMastersを実行
        /// </summary>
        [Authorize( Policy = nameof( AuthorizationPolicies.GlobalAdministratorOnlyPolicy ) )]
        public ActionResult InsertMasters()
        {
            SeedData.SeedData.InsertMasters( _context );

            return RedirectToAction( nameof( Index ) );
        }

        /// <summary>
        /// 画面上からスキーマの削除および再作成
        /// </summary>
        /// <remarks>
        /// 全テーブルのAUTO_INCREMENTを一斉にリセットする方法が不明なため、スキーマ自体を削除したのち再作成して疑似的にリセットを実現している。
        /// </remarks>
        // TODO: デプロイ時にはこのメソッドを消す。（全データが消えるので運用開始後に間違えて実行するとまずい。）
        [Authorize( Policy = nameof( AuthorizationPolicies.GlobalAdministratorOnlyPolicy ) )]
        public ActionResult ResetSchema()
        {
            var rdc = (RelationalDatabaseCreator)_context.GetService<IDatabaseCreator>();

            rdc.EnsureDeleted(); // スキーマ削除
            _context.Database.EnsureCreated(); // スキーマ作成
            SeedData.SeedData.InsertIdentities( _context );

            return RedirectToAction( nameof( Index ) );
        }

        // データが多いとForm間での受け渡しが現実的でなかったので
        private async Task<SelectList> GetCompanySelectList()
        {
            // 営業会社一覧取得(コードと名前だけ)
            var companies = await _context.Companies
                .Where( x => !x.IsClientCompany )
                .OrderBy( x => x.CompanyCode )
                .Select( x => new { x.CompanyCode, x.CompanyName } )
                .ToListAsync();
            return new SelectList( companies, "CompanyCode", "CompanyName" );
        }        

        /// <summary>
        /// ファイルダウンロード
        /// </summary>
        public ActionResult DownloadFile( string filePath )
        {
            return File(
                System.IO.File.ReadAllBytes( filePath ),
                MediaTypeNames.Application.Octet,
                DateTime.Now.ToString( "yyyy年MM月dd日HH時mm分ss秒" ) + ".xlsx" );
        }
    }
}
