using System.Security.Claims;

namespace SampleCRM.Common
{
    /// <summary>
    /// DBContextでユーザーIDを取得するためのサービス
    /// </summary>
    public class UserResolverService
    {
        private IHttpContextAccessor HttpContextAccessor { get; }

        public UserResolverService( IHttpContextAccessor httpContextAccessor )
        {
            HttpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 現在ログイン中ユーザーのユーザーIDを取得する
        /// </summary>
        /// <returns>ユーザーがログイン済の場合はユーザーIDを、未ログインの場合はNullを返す</returns>
        /// <exception cref="Exception"></exception>
        /// <remarks>
        /// Program.csでの処理完了前(SeedData投入時など)にアクセスすると、HttpContextが取得できないため例外となる。
        /// またユーザーIDを整数(int)型に変換できない場合も例外となる。
        /// </remarks>
        public int? GetUserCode()
        {
            if ( HttpContextAccessor.HttpContext!.User is { } user )
            {
                return user.Identity!.IsAuthenticated switch
                {
                    true => int.Parse( user.Claims.First( x => x.Type.Equals( ClaimTypes.NameIdentifier ) ).Value ),

                    // 未ログイン時にもログイン処理の際にDBアクセスが走るが、この時はまだIDが取れないためNullを返す
                    false => null,
                };
            }
            else
            {
                throw new Exception( "httpContext.Userがnullです。" );
            }
        }
    }
}
