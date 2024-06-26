using SampleCRM.Common;
using SampleCRM.Models.Base;
using SampleCRM.Models.TableModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SampleCRM.Contexts
{
    public class SampleCRMContext
        : IdentityDbContext<IdentityUser, IdentityRole, int, IdentityUserClaim, IdentityUserRole, IdentityUserLogin, IdentityRoleClaim, IdentityUserToken>
    {
        private UserResolverService UserResolverService { get; }

        public SampleCRMContext( DbContextOptions<SampleCRMContext> options, UserResolverService userResolverService ) : base( options )
            => UserResolverService = userResolverService;

        /// <summary>
        /// 共通列に値を設定し、DBへの変更を確定する。
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess"></param>
        /// <returns></returns>
        public override int SaveChanges( bool acceptAllChangesOnSuccess )
        {
            SetMetaFields();

            return base.SaveChanges( acceptAllChangesOnSuccess );
        }

        /// <summary>
        /// 共通列に値を設定せずに、DBへの変更を確定する。
        /// </summary>
        /// <remarks>
        /// オーバーライドしたSaveChangesが呼ばれることを回避する
        /// </remarks>
        public int SaveChangesWithoutMetaFields() => base.SaveChanges( true );

        /// <summary>
        /// 共通列に値を設定し、DBへの変更を確定する。
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<int> SaveChangesAsync( bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default )
        {
            SetMetaFields();

            return await base.SaveChangesAsync( acceptAllChangesOnSuccess, cancellationToken );
        }

        /// <summary>
        /// 共通列（更新者・更新日付）に値をセットする
        /// </summary>
        private void SetMetaFields()
        {
            var userCode = UserResolverService.GetUserCode();

            foreach ( var entry in this.ChangeTracker.Entries<TableBase>() )
            {
                // 未変更または削除したレコードの更新者・更新日時が更新されることを防ぐ
                if ( !( entry.State == EntityState.Added || entry.State == EntityState.Modified ) )
                {
                    return;
                }

                // 未ログイン時にもログイン処理の際にDBアクセスが走るが、この時はまだUserが取れないためスルーする
                if ( userCode is not null )
                {
                    entry.Entity.UpdateUserCode = userCode.Value;
                }
                entry.Entity.UpdateDate = DateTime.Now;
            }
        }

        // TODO: ViewModelを生成してスキャフォールディングを実施するときには、当クラスのTableModelへの参照に一旦Models.TableModels.を追加し、スキャフォールディングが終了したら元に戻す。
        // （例：DbSet<ConstructionKind>は、一旦DbSet<Models.TableModels.ConstructionKind>としてスキャフォールディングし、終わったらDbSet<ConstructionKind>に戻す。）

        // ⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺
        // Entityの属性で指定できない内容はFluent APIで定義する
        // ⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺
        protected override void OnModelCreating( ModelBuilder modelBuilder )
        {
            base.OnModelCreating( modelBuilder );

            // 高速化のためIndexを張る
            modelBuilder.Entity<Company>().HasIndex( i => i.CompanyStatusCode );
            modelBuilder.Entity<Department>().HasIndex( i => i.CompanyCode );
            modelBuilder.Entity<Employee>().HasIndex( i => i.DepartmentCode );
            modelBuilder.Entity<Order>().HasIndex( i => i.DeliveryStatusCode );
            modelBuilder.Entity<Payment>().HasIndex( i => i.PaymentMethodCode );
            modelBuilder.Entity<Contract>().HasIndex( i => i.OrderCode );
            modelBuilder.Entity<Contract>().HasIndex( i => i.ContractStatusCode );
            modelBuilder.Entity<Contract>().HasIndex( i => i.UpdateMethodCode );
            modelBuilder.Entity<Contract>().HasIndex( i => i.SystemKindCode );
            modelBuilder.Entity<Contract>().HasIndex( i => i.CDShippingDepartmentCode );
            modelBuilder.Entity<CDShippingLog>().HasIndex( i => i.ContractCode );

            modelBuilder.Entity<DataLicense>().HasIndex( i => i.ProjectKindCode );
            modelBuilder.Entity<DataLicense>().HasIndex( i => i.LicenseDistrictCode );
            modelBuilder.Entity<DataLicense>().HasIndex( i => i.LicenseStatusCode );
            modelBuilder.Entity<SystemLicense>().HasIndex( i => i.LicenseStatusCode );
            modelBuilder.Entity<OffLineLicenseIssuanceHistory>().HasIndex( i => i.SystemLicenseCode );

            modelBuilder.Entity<MaintenanceFeeBill>().HasIndex( i => i.BillingDepartmentCode );

            // 複合主キーのあるテーブルの主キーを設定する(EF6から属性による設定が出来なくなった)
            modelBuilder.Entity<ClientEmployeeToOrder>().HasKey( c => new { c.EmployeeCode, c.OrderCode } );
            modelBuilder.Entity<SalesEmployeeToOrder>().HasKey( c => new { c.EmployeeCode, c.OrderCode } );
            modelBuilder.Entity<OrderToPayment>().HasKey( c => new { c.OrderCode, c.PaymentCode } );

            modelBuilder.Entity<ContractToSystemLicense>().HasKey( c => new { c.ContractCode, c.SystemLicenseCode } );
            modelBuilder.Entity<ContractToDataLicense>().HasKey( c => new { c.ContractCode, c.DataLicenseCode } );

            modelBuilder.Entity<ContractToMaintenanceFee>().HasKey( c => new { c.ContractCode, c.MaintenanceFeeCode } );
            modelBuilder.Entity<MaintenanceFeeToBill>().HasKey( c => new { c.MaintenanceFeeCode, c.BillCode } );
            modelBuilder.Entity<MaintenanceFeeBillToClearance>().HasKey( c => new { c.ClearanceCode, c.BillCode } );

            // マスタ系テーブルのNameカラムにユニーク制約を付加。
            modelBuilder.Entity<CompanyStatus>().HasIndex( u => u.CompanyStatusName ).IsUnique();
            modelBuilder.Entity<DeliveryStatus>().HasIndex( u => u.DeliveryStatusName ).IsUnique();
            modelBuilder.Entity<PaymentMethod>().HasIndex( u => u.PaymentMethodName ).IsUnique();
            modelBuilder.Entity<CompanyStatus>().HasIndex( u => u.CompanyStatusName ).IsUnique();
            modelBuilder.Entity<UpdateMethod>().HasIndex( u => u.UpdateMethodName ).IsUnique();
            modelBuilder.Entity<SystemKind>().HasIndex( u => u.SystemKindName ).IsUnique();
            modelBuilder.Entity<LicenseStatus>().HasIndex( u => u.LicenseStatusName ).IsUnique();

            modelBuilder.Entity<ChangeType>().HasIndex( u => u.ChangeTypeName ).IsUnique();
            modelBuilder.Entity<ChangeTarget>().HasIndex( u => u.ChangeTargetName ).IsUnique();
            modelBuilder.Entity<GUIDVersion>().HasIndex( u => u.VersionName ).IsUnique();

            // トランザクション系テーブルの論理削除されているレコードをデフォルトで無視するようにするには、
            // HasQueryFilter（Global Query Filters）を使う。
            // ただし、無視されるレコードを参照したい箇所にいちいち.IgnoreQueryFilters()を記述する必要が出る。
            // そのため、一覧表示画面（Index）だけから論理削除されたレコードを弾きたいなど、
            // 参照したい箇所より無視したい箇所のほうが少ない場合はHasQueryFilterを設定しないほうが楽かもしれない。
            // （参照したい箇所と無視したい箇所のどちらが多くなるかは画面構築を始めてみないと不明。）
            // 以下はCompanyテーブルの例。
            //modelBuilder.Entity<Company>().HasQueryFilter( x => x.DeleteDate == DateTime.MaxValue );

            #region 外部キー制約の設定
            // マスタ系直下のトランザクション系テーブル。
            // （トランザクション系側で使われているものはマスタ系側から物理削除させない。）
            modelBuilder.Entity<Company>()
                .HasOne<CompanyStatus>()
                .WithMany()
                .HasForeignKey( f => f.CompanyStatusCode )
                .OnDelete( DeleteBehavior.NoAction );
            modelBuilder.Entity<Order>()
                .HasOne<DeliveryStatus>()
                .WithMany()
                .HasForeignKey( f => f.DeliveryStatusCode )
                .OnDelete( DeleteBehavior.NoAction );
            modelBuilder.Entity<Payment>()
                .HasOne<PaymentMethod>()
                .WithMany()
                .HasForeignKey( f => f.PaymentMethodCode )
                .OnDelete( DeleteBehavior.NoAction );
            modelBuilder.Entity<Contract>()
                .HasOne<ContractStatus>()
                .WithMany()
                .HasForeignKey( f => f.ContractStatusCode )
                .OnDelete( DeleteBehavior.NoAction );
            modelBuilder.Entity<Contract>()
                .HasOne<UpdateMethod>()
                .WithMany()
                .HasForeignKey( f => f.UpdateMethodCode )
                .OnDelete( DeleteBehavior.NoAction );
            modelBuilder.Entity<Contract>()
                .HasOne<SystemKind>()
                .WithMany()
                .HasForeignKey( f => f.SystemKindCode )
                .OnDelete( DeleteBehavior.NoAction );

            modelBuilder.Entity<SystemLicense>()
                .HasOne<LicenseStatus>()
                .WithMany()
                .HasForeignKey( f => f.LicenseStatusCode )
                .OnDelete( DeleteBehavior.NoAction );
            modelBuilder.Entity<OffLineLicenseIssuanceHistory>()
                .HasOne<LicenseStatus>()
                .WithMany()
                .HasForeignKey( f => f.LicenseStatusCode )
                .OnDelete( DeleteBehavior.NoAction );
            modelBuilder.Entity<DataLicense>()
                .HasOne<LicenseStatus>()
                .WithMany()
                .HasForeignKey( f => f.LicenseStatusCode )
                .OnDelete( DeleteBehavior.NoAction );

            modelBuilder.Entity<ChangeLog>()
                .HasOne<IdentityUser>()
                .WithMany()
                .HasForeignKey( f => f.ChangeUserCode )
                .OnDelete( DeleteBehavior.NoAction );
            modelBuilder.Entity<ChangeLog>()
                .HasOne<ChangeType>()
                .WithMany()
                .HasForeignKey( f => f.ChangeTypeCode )
                .OnDelete( DeleteBehavior.NoAction );
            modelBuilder.Entity<ChangeLog>()
                .HasOne<ChangeTarget>()
                .WithMany()
                .HasForeignKey( f => f.ChangeTargetCode )
                .OnDelete( DeleteBehavior.NoAction );

            // マスタ系とマスタ系に挟まれた中間テーブル。
            // （RoleからはON DELETE NO ACTION。）
            // 基底クラスの処理に任せるとON DELETE CASCADEになってしまうので、ON DELETE NO ACTIONに上書きする。
            modelBuilder.Entity<IdentityUserRole>()
                .HasOne<IdentityRole>()
                .WithMany()
                .HasForeignKey( f => f.RoleId )
                .OnDelete( DeleteBehavior.NoAction );

            // トランザクション系とトランザクション系に挟まれた中間テーブル。
            // （上流側の親からはON DELETE NO ACTION、下流側の親からはON DELETE CASCADE。）
            modelBuilder.Entity<ClientEmployeeToOrder>()
                .HasOne<Employee>()
                .WithMany()
                .HasForeignKey( f => f.EmployeeCode )
                .OnDelete( DeleteBehavior.NoAction );
            modelBuilder.Entity<ClientEmployeeToOrder>()
                .HasOne<Order>()
                .WithMany()
                .HasForeignKey( f => f.OrderCode )
                .OnDelete( DeleteBehavior.Cascade );
            modelBuilder.Entity<SalesEmployeeToOrder>()
                .HasOne<Employee>()
                .WithMany()
                .HasForeignKey( f => f.EmployeeCode )
                .OnDelete( DeleteBehavior.NoAction );
            modelBuilder.Entity<SalesEmployeeToOrder>()
                .HasOne<Order>()
                .WithMany()
                .HasForeignKey( f => f.OrderCode )
                .OnDelete( DeleteBehavior.Cascade );
            modelBuilder.Entity<OrderToPayment>()
                .HasOne<Order>()
                .WithMany()
                .HasForeignKey( f => f.OrderCode )
                .OnDelete( DeleteBehavior.NoAction );
            modelBuilder.Entity<OrderToPayment>()
                .HasOne<Payment>()
                .WithMany()
                .HasForeignKey( f => f.PaymentCode )
                .OnDelete( DeleteBehavior.Cascade );
            modelBuilder.Entity<ContractToMaintenanceFee>()
                .HasOne<Contract>()
                .WithMany()
                .HasForeignKey( f => f.ContractCode )
                .OnDelete( DeleteBehavior.NoAction );
            modelBuilder.Entity<ContractToMaintenanceFee>()
                .HasOne<MaintenanceFee>()
                .WithMany()
                .HasForeignKey( f => f.MaintenanceFeeCode )
                .OnDelete( DeleteBehavior.Cascade );
            modelBuilder.Entity<MaintenanceFeeToBill>()
                .HasOne<MaintenanceFee>()
                .WithMany()
                .HasForeignKey( f => f.MaintenanceFeeCode )
                .OnDelete( DeleteBehavior.NoAction );
            modelBuilder.Entity<MaintenanceFeeToBill>()
                .HasOne<MaintenanceFeeBill>()
                .WithMany()
                .HasForeignKey( f => f.BillCode )
                .OnDelete( DeleteBehavior.Cascade );
            modelBuilder.Entity<MaintenanceFeeBillToClearance>()
                .HasOne<MaintenanceFeeBill>()
                .WithMany()
                .HasForeignKey( f => f.BillCode )
                .OnDelete( DeleteBehavior.NoAction );
            modelBuilder.Entity<MaintenanceFeeBillToClearance>()
                .HasOne<MaintenanceFeeClearance>()
                .WithMany()
                .HasForeignKey( f => f.ClearanceCode )
                .OnDelete( DeleteBehavior.Cascade );
            #endregion
        }

        // ⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺
        // ユーザー認証関連
        // ⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺
        public override DbSet<IdentityUser> Users { get; set; } = default!;

        public override DbSet<IdentityUserClaim> UserClaims { get; set; } = default!;

        public override DbSet<IdentityUserLogin> UserLogins { get; set; } = default!;

        public override DbSet<IdentityUserToken> UserTokens { get; set; } = default!;

        public override DbSet<IdentityUserRole> UserRoles { get; set; } = default!;

        public override DbSet<IdentityRole> Roles { get; set; } = default!;

        public override DbSet<IdentityRoleClaim> RoleClaims { get; set; } = default!;

        // ⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺
        // 会社・部署・担当者関連
        // ⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺
        public DbSet<CompanyStatus> CompanyStatuses { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Employee> Employees { get; set; }

        // ⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺
        // 担当者と発注を繋ぐ中間テーブル
        // ⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺
        public DbSet<ClientEmployeeToOrder> ClientEmployeeToOrders { get; set; }

        public DbSet<SalesEmployeeToOrder> SalesEmployeeToOrders { get; set; }

        // ⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺
        // 発注・支払い・契約関連
        // ⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺
        public DbSet<DeliveryStatus> DeliveryStatuses { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<PaymentMethod> PaymentMethods { get; set; }

        public DbSet<Payment> Payments { get; set; }

        public DbSet<OrderToPayment> OrderToPayments { get; set; }

        public DbSet<ContractStatus> ContractStatuses { get; set; }

        public DbSet<UpdateMethod> UpdateMethods { get; set; }

        public DbSet<SystemKind> SystemKinds { get; set; }

        public DbSet<Contract> Contracts { get; set; }

        public DbSet<CDShippingLog> CDShippingLogs { get; set; }

        // ⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺
        // ライセンス関連
        // ⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺
        public DbSet<ContractToSystemLicense> ContractToSystemLicenses { get; set; }

        public DbSet<ContractToDataLicense> ContractToDataLicenses { get; set; }

        public DbSet<LicenseStatus> LicenseStatuses { get; set; }

        public DbSet<SystemLicense> SystemLicenses { get; set; }

        public DbSet<OffLineLicenseIssuanceHistory> OffLineLicenseIssuanceHistories { get; set; }

        public DbSet<DataLicense> DataLicenses { get; set; }

        // ⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺
        // 保守料関連
        // ⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺
        public DbSet<MaintenanceFee> MaintenanceFees { get; set; }

        public DbSet<ContractToMaintenanceFee> ContractToMaintenanceFees { get; set; }

        public DbSet<MaintenanceFeeBill> MaintenanceFeeBills { get; set; }

        public DbSet<MaintenanceFeeToBill> MaintenanceFeeToBills { get; set; }

        public DbSet<MaintenanceFeeClearance> MaintenanceFeeClearances { get; set; }

        public DbSet<MaintenanceFeeBillToClearance> MaintenanceFeeBillToClearances { get; set; }

        // ⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺
        // 変更履歴関連
        // ⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺
        public DbSet<ChangeLog> ChangeLogs { get; set; }

        public DbSet<ChangeTarget> ChangeTargets { get; set; }

        public DbSet<ChangeType> ChangeTypes { get; set; }

        // ⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺
        // バージョン
        // ⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺⸺
        public DbSet<GUIDVersion> GUIDVersions { get; set; }

        // TODO: ここにViewModelが追加されていたら消す。（スキャフォールディング時、勝手に追加されてしまう。消し忘れるとInitialSQLが失敗する。）
    }
}
