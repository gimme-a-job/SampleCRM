using Microsoft.AspNetCore.Authorization;
using static SampleCRM.Identity.RoleNames;

namespace SampleCRM.Identity
{
    /// <summary>
    /// 各ページにアクセスする際の権限をチェックするポリシーを格納
    /// </summary>
    public static class AuthorizationPolicies
    {
        /// <summary>
        /// 何かしらロールが割り当てられているユーザーであれば許可
        /// </summary>
        /// <remarks>
        /// 退職者など、何の権限も無いユーザーを弾くことを想定
        /// </remarks>
        public static Action<AuthorizationPolicyBuilder> AnyRolePolicy
            => policy => policy.RequireRole( Reader, Editor, UserAdministrator, GlobalAdministrator );

        /// <summary>
        /// 編集権限があるかどうか
        /// </summary>
        public static Action<AuthorizationPolicyBuilder> EditorPolicy
            => policy => policy.RequireRole( Editor, UserAdministrator, GlobalAdministrator );

        /// <summary>
        /// ユーザーの管理権限があるかどうか
        /// </summary>
        public static Action<AuthorizationPolicyBuilder> UserAdministratorPolicy
            => policy => policy.RequireRole( UserAdministrator, GlobalAdministrator );

        /// <summary>
        /// 全体管理者のみ許可
        /// </summary>
        public static Action<AuthorizationPolicyBuilder> GlobalAdministratorOnlyPolicy
            => policy => policy.RequireRole( GlobalAdministrator );
    }

    /// <summary>
    /// 各ロール名を格納
    /// </summary>
    /// <remarks>
    /// このクラスからRoleManagerにアクセスするのが難しくDBからの取得ができないため、こちらで宣言している。
    /// </remarks>
    public static class RoleNames
    {
        /// <summary>
        /// ロール無し
        /// </summary>
        /// <remarks>
        /// ロールを何もアサインしない場合の表示に使う
        /// DBには含まれていないため注意
        /// </remarks>
        public const string NotAssigned = "ロール無し";

        /// <summary>
        /// 閲覧者
        /// </summary>
        public const string Reader = "閲覧者";

        /// <summary>
        /// 編集者
        /// </summary>
        public const string Editor = "編集者";

        /// <summary>
        /// ユーザー権限管理者
        /// </summary>
        public const string UserAdministrator = "ユーザー権限管理者";

        /// <summary>
        /// 全体管理者
        /// </summary>
        public const string GlobalAdministrator = "全体管理者";
    }
}
