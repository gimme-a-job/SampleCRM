namespace SampleCRM.Common
{
    public static class CookieUtility
    {
        public static string GetValueFromCookie( HttpRequest request, string key )
        {
            if ( request.Cookies.TryGetValue( key, out string cookieValue ) )
            {
                return cookieValue;
            }
            return null;
        }

        public static TEnum? GetEnumValueFromCookie<TEnum>( HttpRequest request ) where TEnum : struct, Enum
        {
            if ( request.Cookies.TryGetValue( typeof( TEnum ).Name, out string cookieValue ) )
            {
                if ( Enum.TryParse<TEnum>( cookieValue, out TEnum parsedValue ) )
                {
                    return parsedValue;
                }
            }
            return null;
        }

        public static void SaveValueToCookie( HttpResponse response, string key, string value )
        {
            var options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays( 14 ) // 有効期限を14日に設定、適当
            };
            response.Cookies.Append( key, value, options );
        }

        public static void SaveEnumValueToCookie<TEnum>( HttpResponse response, TEnum enumValue ) where TEnum : struct, Enum
            => SaveValueToCookie( response, typeof( TEnum ).Name, enumValue.ToString() );
    }
}
