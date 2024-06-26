using SampleCRM.Common;
using SampleCRM.Contexts;
using SampleCRM.Models.TableModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using static SampleCRM.Identity.RoleNames;

namespace SampleCRM.SeedData
{
    /// <summary>
    /// 初回起動時にSeedデータを投入する
    /// </summary>
    public static class SeedData
    {
        public static void Initialize( IServiceProvider serviceProvider )
        {
            using var context = new SampleCRMContext(
                serviceProvider.GetRequiredService<DbContextOptions<SampleCRMContext>>(),
                serviceProvider.GetRequiredService<UserResolverService>() );
            var rdc = (RelationalDatabaseCreator)context.GetService<IDatabaseCreator>();

            // スキーマがなければ作る。
            if ( !rdc.Exists() )
            {
                context.Database.EnsureCreated();
            }

            // テーブルがなければ作る。
            if ( !rdc.HasTables() )
            {
                rdc.CreateTables();
            }

            InsertIdentities( context );
        }

        /// <summary>
        /// IdentityUsers・IdentityUserRoles・IdentityRolesへのデータ投入
        /// </summary>
        public static void InsertIdentities( SampleCRMContext context )
        {
            // ロールが登録されていない状況は全てのユーザーデータが存在しない事になる
            if ( !context.Roles.Any() )
            {
                // ロールの登録
                context.Roles.AddRange(
                    new IdentityRole
                    {
                        Id = 1,
                        Name = Reader,
                        NormalizedName = Reader.ToUpper()
                    },
                    new IdentityRole
                    {
                        Id = 2,
                        Name = Editor,
                        NormalizedName = Editor.ToUpper()
                    },
                    new IdentityRole
                    {
                        Id = 3,
                        Name = UserAdministrator,
                        NormalizedName = UserAdministrator.ToUpper()
                    },
                    new IdentityRole
                    {
                        Id = 4,
                        Name = GlobalAdministrator,
                        NormalizedName = GlobalAdministrator.ToUpper()
                    }
                );

                // 必須管理者の登録
                context.Users.Add(
                    new IdentityUser
                    {
                        Id = 1,
                        UserName = "admin",
                        NormalizedUserName = "admin".ToUpper(),
                        PasswordHash = "AQAAAAEAACcQAAAAEAwh16XxLFTE2mNcyyUG0Dv0pDNb+ZX7r4jeK7EUYCuvJuC7fGHIFmdAvAkVNS4aaA==",
                        Email = "info@sample.co.jp",
                        NormalizedEmail = "info@sample.co.jp".ToUpper(),
                        SecurityStamp = "5YT2XGA346ZYQAG6OADHGE527CTYXO6T",
                    }
                );

                // ユーザー毎のロール情報の登録
                context.UserRoles.AddRange(
                    new IdentityUserRole
                    {
                        UserId = 1,
                        RoleId = 4,
                    }
                );

                context.SaveChangesWithoutMetaFields();
            }
        }

        /// <summary>
        /// マスタ系テーブルへの初期データ投入（旧InitialSQLの処理内容）
        /// </summary>
        public static void InsertMasters( SampleCRMContext context )
        {
            // TODO: 一部のマスタだけを見て全体にデータが入っているか簡易的に判定しているため、できればテーブル毎に判定するようにする。
            if ( context.CompanyStatuses.Any() )
            {
                return; // 初期データが既に入っている場合はスルー。
            }

            var companyStatuses = new List<CompanyStatus>
            {
                new CompanyStatus { CompanyStatusCode = 1, CompanyStatusName = "営業会社" },
                new CompanyStatus { CompanyStatusCode = 2, CompanyStatusName = "続行中" },
                new CompanyStatus { CompanyStatusCode = 3, CompanyStatusName = "キャンセル" },
                new CompanyStatus { CompanyStatusCode = 4, CompanyStatusName = "保留・一時停止" },
                new CompanyStatus { CompanyStatusCode = 5, CompanyStatusName = "コンサル" },
                new CompanyStatus { CompanyStatusCode = 6, CompanyStatusName = "解約" },
                new CompanyStatus { CompanyStatusCode = 7, CompanyStatusName = "倒産" },
            };
            foreach ( var item in companyStatuses )
            {
                item.UpdateUserCode = 1;
                item.UpdateDate = DateTime.Now;
            }
            context.CompanyStatuses.AddRange( companyStatuses );

            context.SaveChanges();

            InsertCompanyDepartmentEmployee( context );

            var deliveryStatuses = new List<DeliveryStatus>
            {
                new DeliveryStatus { DeliveryStatusCode = 1, DeliveryStatusName = "キャンセル" },
                new DeliveryStatus { DeliveryStatusCode = 2, DeliveryStatusName = "発注～納品" },
                new DeliveryStatus { DeliveryStatusCode = 3, DeliveryStatusName = "納品" },
            };
            foreach ( var item in deliveryStatuses )
            {
                item.UpdateUserCode = 1;
                item.UpdateDate = DateTime.Now;
            }
            context.DeliveryStatuses.AddRange( deliveryStatuses );

            var paymentMethods = new List<PaymentMethod>
            {
                new PaymentMethod { PaymentMethodCode = 1, PaymentMethodName = "リース" },
                new PaymentMethod { PaymentMethodCode = 2, PaymentMethodName = "リース＋現金一括" },
                new PaymentMethod { PaymentMethodCode = 3, PaymentMethodName = "リース＋現金分割" },
                new PaymentMethod { PaymentMethodCode = 4, PaymentMethodName = "現金一括" },
                new PaymentMethod { PaymentMethodCode = 5, PaymentMethodName = "現金分割" },
                new PaymentMethod { PaymentMethodCode = 6, PaymentMethodName = "クレジットカード" },
            };
            foreach ( var item in paymentMethods )
            {
                item.UpdateUserCode = 1;
                item.UpdateDate = DateTime.Now;
            }
            context.PaymentMethods.AddRange( paymentMethods );

            var contractStatuses = new List<ContractStatus>
            {
                new ContractStatus { ContractStatusCode = 1, ContractStatusName = "発注～納品" },
                new ContractStatus { ContractStatusCode = 2, ContractStatusName = "キャンセル" },
                new ContractStatus { ContractStatusCode = 3, ContractStatusName = "契約中" },
                new ContractStatus { ContractStatusCode = 4, ContractStatusName = "保留・一時停止" },
                new ContractStatus { ContractStatusCode = 5, ContractStatusName = "解約" },
            };
            foreach ( var item in contractStatuses )
            {
                item.UpdateUserCode = 1;
                item.UpdateDate = DateTime.Now;
            }
            context.ContractStatuses.AddRange( contractStatuses );

            var updateMethods = new List<UpdateMethod>
            {
                new UpdateMethod { UpdateMethodCode = 1, UpdateMethodName = "ダウンロード" },
                new UpdateMethod { UpdateMethodCode = 2, UpdateMethodName = "CD発送" },
                new UpdateMethod { UpdateMethodCode = 3, UpdateMethodName = "対応不要" },
            };
            foreach ( var item in updateMethods )
            {
                item.UpdateUserCode = 1;
                item.UpdateDate = DateTime.Now;
            }
            context.UpdateMethods.AddRange( updateMethods );

            var changeTargets = new List<ChangeTarget>
            {
                new ChangeTarget { ChangeTargetCode = 1, ChangeTargetName = "会社系情報" },
                new ChangeTarget { ChangeTargetCode = 2, ChangeTargetName = "発注系情報" },
                new ChangeTarget { ChangeTargetCode = 3, ChangeTargetName = "保守系情報" },
                new ChangeTarget { ChangeTargetCode = 4, ChangeTargetName = "その他" },
            };
            foreach ( var item in changeTargets )
            {
                item.UpdateUserCode = 1;
                item.UpdateDate = DateTime.Now;
            }
            context.ChangeTargets.AddRange( changeTargets );

            var changeTypes = new List<ChangeType>
            {
                new ChangeType { ChangeTypeCode = 1, ChangeTypeName = "新規" },
                new ChangeType { ChangeTypeCode = 2, ChangeTypeName = "変更" },
                new ChangeType { ChangeTypeCode = 3, ChangeTypeName = "削除" },
                new ChangeType { ChangeTypeCode = 4, ChangeTypeName = "その他" },
            };
            foreach ( var item in changeTypes )
            {
                item.UpdateUserCode = 1;
                item.UpdateDate = DateTime.Now;
            }
            context.ChangeTypes.AddRange( changeTypes );

            context.SaveChangesWithoutMetaFields();
        }

        /// <summary>
        /// Companies・Departments・Employeesへの初期データ投入
        /// </summary>
        public static void InsertCompanyDepartmentEmployee( SampleCRMContext context )
        {
            context.Companies.Add( new Company { CompanyCode = 1, IsClientCompany = false, CompanyName = "株式会社サンプル", CustomersContractCode = 1, PhoneticName = "カブシキガイシャサンプル", CompanyStatusCode = 1, IsNeedVisiting = false, UpdateUserCode = 1, UpdateDate = DateTime.Now } );

            context.Departments.Add( new Department { DepartmentCode = 1, CompanyCode = 1, IsMain = true, DepartmentName = "営業チーム", PhoneticName = "エイギョウチーム", PostalCode = "100-0001", Prefecture = "東京都", City = "千代田区千代田", Block = "1-12-3", Building = "ビル3F", TelNumber = "01-2345-6789", DisplayOrder = 1, UpdateUserCode = 1, UpdateDate = DateTime.Now } );

            var employees = new List<Employee>
            {
                new() { EmployeeCode = 1, DepartmentCode = 1, IsMain = true, IsEnrollment = true, EmployeeName = "鈴木一郎", DisplayOrder = 1 },
                new() { EmployeeCode = 2, DepartmentCode = 1, IsMain = false, IsEnrollment = true, EmployeeName = "佐藤二郎", DisplayOrder = 2 },
                new() { EmployeeCode = 3, DepartmentCode = 1, IsMain = false, IsEnrollment = true, EmployeeName = "本田三郎", DisplayOrder = 3 },
                new() { EmployeeCode = 4, DepartmentCode = 1, IsMain = false, IsEnrollment = true, EmployeeName = "鈴木四郎", DisplayOrder = 4 },
            };
            foreach ( var item in employees )
            {
                item.UpdateUserCode = 1;
                item.UpdateDate = DateTime.Now;
            }
            context.Employees.AddRange( employees );

            context.SaveChangesWithoutMetaFields();
        }
    }
}
