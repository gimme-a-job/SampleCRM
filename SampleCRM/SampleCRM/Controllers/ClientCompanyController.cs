using DocumentFormat.OpenXml.InkML;
using SampleCRM.Common;
using SampleCRM.Contexts;
using SampleCRM.Identity;
using SampleCRM.Models.Enums;
using SampleCRM.Models.TableModels;
using SampleCRM.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using X.PagedList.EF;
using Company = SampleCRM.Models.TableModels.Company;

namespace SampleCRM.Controllers
{
    [Authorize( Policy = nameof( AuthorizationPolicies.AnyRolePolicy ) )]
    public class ClientCompanyController : Controller
    {
        private SampleCRMContext SampleCRMContext { get; }

        // 会社状態の選択リスト
        private SelectList CompanyStatusSelectList;
        // CompanyStatusをキーとしたDescriptionの連想配列
        private Dictionary<int, string> CompanyStatuses;

        public ClientCompanyController( SampleCRMContext sampleCRMContext )
        {
            SampleCRMContext = sampleCRMContext;

            // 会社状態のSelectListを作成
            string description;
            CompanyStatuses = new Dictionary<int, string>();

            foreach ( var status in SampleCRMContext.CompanyStatuses )
            {
                if ( status.CompanyStatusName == "営業会社" )
                {
                    continue; // 「営業会社」なので選択リストに追加する必要がない
                }

                description = status.CompanyStatusName;
                CompanyStatuses.Add( status.CompanyStatusCode, description );
            }

            CompanyStatusSelectList = new SelectList( CompanyStatuses.Values.ToList() );
        }

        // GET: ClientCompany
        public async Task<IActionResult> Index( int? page = 1, string searchWord = "" )
        {
            try
            {
                var stopwatch = new Stopwatch();

                // 計測開始
                stopwatch.Start();

                // 'page'が１未満の場合は404を返す。
                if ( page < 1 )
                {
                    return NotFound();
                }

                // 顧客会社一覧を取得開始
                IQueryable<Company>? filteredRecords;

                // 検索キーワードを元に検索を行う
                if ( searchWord.IsNullOrEmpty() )
                {
                    // 「検索キーワード」欄が空欄の場合
                    filteredRecords = SampleCRMContext.Companies;
                }
                else
                {
                    filteredRecords = ( CookieUtility.GetEnumValueFromCookie<SearchMethod>( Request ) switch
                    {
                        // 「会社名カナ」
                        SearchMethod.CompanyNameKana
                            => SampleCRMContext.Companies
                                .Where( x => x.PhoneticName.Contains( searchWord ) ),

                        // 「部署名」
                        SearchMethod.DepartmentName
                            => SampleCRMContext.GetJoinnedCompaniesWithDepartments()
                                .Where( x => x.Department.DepartmentName.Contains( searchWord )
                                            || x.Department.DetailedDepartmentName.Contains( searchWord ) )
                                .Select( x => x.Company ),

                        // 「部署名カナ」
                        SearchMethod.DepartmentNameKana
                            => SampleCRMContext.GetJoinnedCompaniesWithDepartments()
                                .Where( x => x.Department.PhoneticName.Contains( searchWord ) )
                                .Select( x => x.Company ),

                        // 「電話番号」
                        SearchMethod.PhoneNumber
                            => SampleCRMContext.GetJoinnedCompaniesWithEmployees()
                                .Where( x => x.Department.TelNumber.Contains( searchWord )
                                            || x.Department.FaxNumber.Contains( searchWord )
                                            || x.Employee.CellPhoneNumber.Contains( searchWord ) )
                                .Select( x => x.Company ),

                        // 「会社名」(何らかの理由でCookieがNullの場合もこちら)
                        SearchMethod.CompanyName or _
                            => SampleCRMContext.Companies
                                .Where( x => x.CompanyName.Contains( searchWord ) ),

                    } ).Distinct(); // 検索の過程で発生した重複を削除(「部署」や「担当者」テーブルがJoinされた場合に発生)
                }

                // 顧客会社かつ論理削除されていないもののみにする。
                filteredRecords = filteredRecords.Where( x => x.IsClientCompany && x.DeleteDate == DateTime.MaxValue );

                // 会社状態
                int selectedCompanyStatusCode;
                if ( CookieUtility.GetValueFromCookie( Request, $"Selected{nameof( CompanyStatus )}" ) is { } selectedCompanyStatus
                    && !string.IsNullOrEmpty( selectedCompanyStatus ) // 「全て」が選択されているかどうかをこちらで確認している
                    && int.TryParse( selectedCompanyStatus, out selectedCompanyStatusCode ) )
                {
                    filteredRecords = filteredRecords.Where( x => x.CompanyStatusCode == selectedCompanyStatusCode );
                }

                // 会社コードの昇順・降順
                filteredRecords = CookieUtility.GetEnumValueFromCookie<CompanyCodeOrder>( Request ) switch
                {
                    CompanyCodeOrder.DESC => filteredRecords.OrderByDescending( x => x.CompanyCode ),

                    // CookieがNullなら(シークレットモードの場合とか？)取り敢えず昇順で表示する。適当
                    CompanyCodeOrder.ASC or _ => filteredRecords.OrderBy( x => x.CompanyCode ),
                };

                // 以上の検索条件・表示条件を元にDBアクセスを行う
                var companies = await filteredRecords
                    .ToPagedListAsync(
                    page!.Value,
                    CookieUtility.GetEnumValueFromCookie<DisplayCountPerPage>( Request ).GetValueOrDefault( DisplayCountPerPage.UpTo20 ).GetHashCode() );

                // 指定したページが全ページ数を超えている場合は404を返す。(special case first page if no items exist)
                if ( companies.PageNumber != 1 && page > companies.PageCount )
                {
                    return NotFound();
                }

                ViewBag.Companies = companies;

                var viewModels = new List<ClientCompanyCommonViewModel>();
                foreach ( var company in companies )
                {
                    var viewModel = new ClientCompanyCommonViewModel(
                        company,
                        await SampleCRMContext.Departments
                                .Where( x => x.CompanyCode == company.CompanyCode && x.IsMain ) // 部署情報は代表部署だけを取得する
                                .SingleOrDefaultAsync() );

                    viewModel.CompanyStatus = await SampleCRMContext.GetCompanyStatusNameAsync( company.CompanyStatusCode );

                    viewModel.SystemKinds = string.Join(
                        ",",
                        ( await SampleCRMContext.GetActiveContractListByCompanyCodeAsync( company.CompanyCode ) )
                            .OrderBy( x => x.SystemKindName )
                            .Select( x => x.SystemKindName )
                            .Distinct() );

                    viewModels.Add( viewModel );
                }

                // 計測終了
                stopwatch.Stop();

                // 結果表示
                Console.WriteLine( $"処理時間: {stopwatch.ElapsedMilliseconds} ms" );

                return View( viewModels );
            }
            catch ( Exception ex )
            {
                throw ex;
            }
        }

        [HttpPost]
        public IActionResult Search( string searchword, SearchMethod searchMethod, int? page = 1 )
        {
            // 検索方法をCookieに保存
            CookieUtility.SaveEnumValueToCookie( Response, searchMethod );

            // Indexアクションメソッドと同じ処理を実行
            return RedirectToAction( nameof( Index ), new { page, searchword } );
        }

        [HttpPost]
        public IActionResult SwitchCompanyCodeOrder( string searchword, CompanyCodeOrder companyCodeOrder, int? page = 1 )
        {
            // 検索方法をCookieに保存
            CookieUtility.SaveEnumValueToCookie( Response, companyCodeOrder );

            // Indexアクションメソッドと同じ処理を実行
            return RedirectToAction( nameof( Index ), new { page, searchword } );
        }

        [HttpPost]
        public IActionResult FilterCompanyStatus( string searchword, int? selectedCompanyStatusCode, int? page = 1 )
        {
            // 検索方法をCookieに保存
            CookieUtility.SaveValueToCookie(
                Response,
                $"Selected{nameof( CompanyStatus )}",
                ( selectedCompanyStatusCode is null ) switch
                {
                    // 「全て」が選択されている場合を想定
                    true => string.Empty,

                    false => selectedCompanyStatusCode.ToString(),
                } );

            // Indexアクションメソッドと同じ処理を実行
            return RedirectToAction( nameof( Index ), new { page, searchword } );
        }

        [HttpPost]
        public IActionResult ChangeDisplayCount( string searchword, DisplayCountPerPage displayCount, int? page = 1 )
        {
            // 検索方法をCookieに保存
            CookieUtility.SaveEnumValueToCookie( Response, displayCount );

            // Indexアクションメソッドと同じ処理を実行
            return RedirectToAction( nameof( Index ), new { page, searchword } );
        }

        // 詳細ページだが部署一覧も兼ねている
        // GET: ClientCompany/Details/5
        public async Task<IActionResult> Details( int? id )
        {
            // 会社、部署、担当者の取得
            if ( id is null || await SampleCRMContext.Companies.FindAsync( id ) is not { } company )
            {
                return NotFound();
            }

            var viewModel = new ClientCompanyCommonViewModel( company );

            viewModel.CompanyStatus = await SampleCRMContext.GetCompanyStatusNameAsync( company.CompanyStatusCode );

            // 部署情報取得
            viewModel.DepartmentList = new DepartmentListViewModel( company.CompanyCode, await SampleCRMContext.GetDepartmentListWithEmployeesByCompanyCodeAsync( company.CompanyCode ) );

            // 発注情報取得
            viewModel.OrderList = new OrderListViewModel( company.CompanyCode, await SampleCRMContext.GetOrderListByCompanyCodeAsync( company.CompanyCode ) );

            // 契約情報取得
            viewModel.ContractList = new ContractListViewModel( await SampleCRMContext.GetContractListWithLicensesByCompanyCodeAsync( company.CompanyCode ) )
            {
                // TODO: 微妙
                CompanyCode = company.CompanyCode,
            };

            // 保守情報取得
            viewModel.MaintenanceFeeBillList = await SampleCRMContext.GetMaintenanceFeeBillListByCompanyCodeAsync( company.CompanyCode );

            return View( viewModel );
        }

        // GET: ClientCompany/Create
        [Authorize( Policy = nameof( AuthorizationPolicies.EditorPolicy ) )]
        public async Task<IActionResult> Create()
        {
            var viewModel = new ClientCompanyCommonViewModel()
            {
                IsNeedVisiting = true
            };

            return View( viewModel );
        }

        // POST: ClientCompany/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize( Policy = nameof( AuthorizationPolicies.EditorPolicy ) )]
        public async Task<IActionResult> Create( [Bind( "CompanyCode,CompanyName,PhoneticName,IsNeedVisiting,BusinessEvaluationPermitNumber,Note" )] ClientCompanyCommonViewModel model )
        {
            if ( ModelState.IsValid )
            {
                // 会社登録
                var company = new Company();

                // テーブルEntityに各種データコピー
                company.IsClientCompany = true; // 顧客会社登録フォームなので必ず顧客会社
                                                // 新規登録時は必ず続行中
                company.CompanyStatusCode = CompanyStatuses.Single( x => x.Value == "続行中" ).Key; // TODO: 一致無し時・複数一致時はエラーページに飛ばす。
                company.CompanyCode = model.CompanyCode;
                company.CompanyName = model.CompanyName;
                company.PhoneticName = model.PhoneticName;
                company.IsNeedVisiting = model.IsNeedVisiting;
                company.BusinessEvaluationPermitNumber = model.BusinessEvaluationPermitNumber;
                company.Note = model.Note;

                // 表示順はAutoIncrement
                SampleCRMContext.Add( company );
                await SampleCRMContext.SaveChangesAsync();

                return RedirectToAction( nameof( Index ) );
            }
            return View( model );
        }

        // GET: ClientCompany/Edit/5
        [Authorize( Policy = nameof( AuthorizationPolicies.EditorPolicy ) )]
        public async Task<IActionResult> Edit( int? id )
        {
            if ( id == null )
            {
                return NotFound();
            }

            var company = await SampleCRMContext.Companies.FindAsync( id );
            if ( company == null )
            {
                return NotFound();
            }

            var viewModel = new ClientCompanyCommonViewModel( company );

            // ViewModelにデータをコピー
            viewModel.CompanyStatus = CompanyStatuses.Single( x => x.Key == company.CompanyStatusCode ).Value; // TODO: 一致無し時・複数一致時はエラーページに飛ばす。

            // 会社状態のリストをViewへ
            viewModel.CompanyStatusSelectList = this.CompanyStatusSelectList;

            return View( viewModel );
        }

        // POST: ClientCompany/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize( Policy = nameof( AuthorizationPolicies.EditorPolicy ) )]
        public async Task<IActionResult> Edit( int id,
            [Bind( "CompanyCode,CompanyName,PhoneticName,CompanyStatus,IsNeedVisiting,BusinessEvaluationPermitNumber,Note" )] ClientCompanyCommonViewModel model )
        {
            if ( ModelState.IsValid )
            {
                // ViewModelからTableModelにデータを移して更新を行う
                var tableModel = new Company();

                tableModel.IsClientCompany = true; // 固定
                tableModel.CompanyCode = model.CompanyCode;
                tableModel.CompanyName = model.CompanyName;
                tableModel.PhoneticName = model.PhoneticName;
                tableModel.CompanyStatusCode = CompanyStatuses.Single( x => x.Value == model.CompanyStatus ).Key; // TODO: 一致無し時・複数一致時はエラーページに飛ばす。
                tableModel.IsNeedVisiting = model.IsNeedVisiting;
                tableModel.BusinessEvaluationPermitNumber = model.BusinessEvaluationPermitNumber;
                tableModel.Note = model.Note;

                // 会社情報の更新
                SampleCRMContext.Update( tableModel );

                try
                {
                    await SampleCRMContext.SaveChangesAsync();
                }
                catch ( Exception )
                {
                    throw;
                }
                return RedirectToAction( nameof( Index ) );
            }

            // 入力内容不備のため再度描画
            // 会社状態のリストをViewへ
            model.CompanyStatusSelectList = this.CompanyStatusSelectList;
            return View( model );
        }

        // GET: ClientCompany/Delete/5
        [Authorize( Policy = nameof( AuthorizationPolicies.EditorPolicy ) )]
        public async Task<IActionResult> Delete( int? id )
        {
            if ( id == null )
            {
                return NotFound();
            }

            // 会社、部署、担当者の取得
            var company = await SampleCRMContext.Companies.FindAsync( id );
            if ( company == null )
            {
                return NotFound();
            }

            var viewModel = new ClientCompanyCommonViewModel( company );

            // 会社情報をViewに登録
            viewModel.CompanyStatus = CompanyStatuses.Single( x => x.Key == company.CompanyStatusCode ).Value; // TODO: 一致無し時・複数一致時はエラーページに飛ばす。

            return View( viewModel );
        }

        // POST: ClientCompany/Delete/5
        [HttpPost, ActionName( nameof( Delete ) )]
        [ValidateAntiForgeryToken]
        [Authorize( Policy = nameof( AuthorizationPolicies.EditorPolicy ) )]
        public async Task<IActionResult> DeleteConfirmed( int id )
        {
            if ( SampleCRMContext.Companies == null )
            {
                return base.Problem( "Entity set 'SampleCRMContext.ClientCompany'  is null." );
            }

            // Companiesから対象の会社を削除
            var company = await SampleCRMContext.Companies.FindAsync( id );
            if ( company != null )
            {
                // TODO: 論理削除されていないものを消そうとしたらまず論理削除、論理削除済のものを消そうとしたら物理削除、という暫定仕様のため、画面構築時に正式な仕様を決める。
                if ( !company.IsSoftDeleted() )
                {
                    company.SoftDelete(); // 論理削除
                }
                else
                {
                    SampleCRMContext.Companies.Remove( company ); // 物理削除（C_CompanyToConstructionKindsテーブルも連鎖削除）
                }
            }

            await SampleCRMContext.SaveChangesAsync();
            return RedirectToAction( nameof( Index ) );
        }

        private bool CompanyExists( int id ) => SampleCRMContext.Companies.Any( e => e.CompanyCode == id );
    }
}
