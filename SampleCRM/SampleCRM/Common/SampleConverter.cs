using System.Text;

namespace SampleCRM.Common
{
    public static class SampleConverter
    {
        /// <summary>
        /// 空文字の場合に「-」に変換する
        /// IndexやDetailページで使用する事
        /// CreateやEditで使用するのは想定外の文字列が入るので禁止
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string ToInvalidStrIsNullOrEmpty( this string target )
            => string.IsNullOrEmpty( target ) ? "－" : target;

        /// <summary>
        /// エクセルの列表記を整数表現からローマ字表現に変換
        /// A列を0番目として扱う
        /// </summary>
        /// <param name="index">整数表現のエクセル列番号</param>
        /// <returns>A1表記の列名</returns>
        public static string C1ToA1( this int index )
        {
            index++;

            // C1形式の場合は1未満はありえない
            if ( index < 1 )
            {
                return string.Empty;
            }

            var stringBuilder = new StringBuilder();

            while ( index > 0 )
            {
                index--;
                stringBuilder.Insert( 0, (char)( index % 26 + 'A' ) );
                index /= 26;
            }

            return stringBuilder.ToString();
        }
    }
}
