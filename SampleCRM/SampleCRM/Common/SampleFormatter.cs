using Microsoft.VisualBasic;
using System.Text.RegularExpressions;
using System.Text;
using System.Runtime.Versioning;

namespace SampleCRM.Common
{
    // for Microsoft.VisualBasic.*
    [SupportedOSPlatform( "windows" )]

    public static class SampleFormatter
    {
        #region 定数

        /// <summary>
        /// 横線の変換対象となる文字ワードパターン
        /// （横線は同じに見えるが違う文字の場合があるため）
        /// </summary>
        public static string LineReplaceTargetPattern { get; } = @"[-－―]";

        #endregion

        /// <summary>
        /// 無駄なスペースをなくしカンマを消したフォーマットにする
        /// </summary>
        public static string ReplaceFormatStringExRemoveComma( string original )
        {
            return RemoveComma( ReplaceSpace( ReplaceFormatString( original ) ) );
        }

        /// <summary>
        /// 無駄なスペースをなくしたフォーマットにする
        /// </summary>
        public static string ReplaceFormatStringEx( string original )
        {
            return ReplaceSpace( ReplaceFormatString( original ) );
        }

        /// <summary>
        /// スペースはそのままのフォーマットにする
        /// </summary>
        public static string ReplaceFormatString( string original )
        {
            if ( string.IsNullOrEmpty( original ) )
                return string.Empty;

            // 半角であるべき文字を半角にする.
            var reNarrow = new Regex( @"[ａ-ｚＡ-Ｚ0-9０-９\s、。，．]" );

            var reVBNarrow = reNarrow.Replace( original, VBReplaceNarrow );

            // 全角であるべき文字を全角にする.
            var reWide = new Regex( @"[ｧ-ﾝﾞﾟ()①-⑳･'=+*/<>~#%:;-]" );

            var reVBWide = reWide.Replace( reVBNarrow, VBReplaceWide );

            // Microsoft.VisualBasic.StrConv 関数でWideを利用しても半角の "\" が全角に変換されない
            reVBWide = reVBWide.Replace( @"\", "￥" );

            // 濁点・半濁点が分割されていることがあるので結合する.
            var combiningVoicedMarks = CombiningVoicedMarks( reVBWide );
            // ゆらぎ除去.
            var absorbFluctuations = AbsorbFluctuations( combiningVoicedMarks );
            // 線の場合分け変換.
            var lineReplaceByCase = LineReplaceByCase( absorbFluctuations );

            return lineReplaceByCase;
        }

        /// <summary>
        /// ReplaceFormatString の拡張メソッド版
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static string GetSampleFormatString( this string original )
        {
            if ( string.IsNullOrEmpty( original ) )
                return string.Empty;

            // 半角であるべき文字を半角にする.
            var reNarrow = new Regex( @"[ａ-ｚＡ-Ｚ0-9０-９\s、。，．]" );

            var reVBNarrow = reNarrow.Replace( original, VBReplaceNarrow );

            // 全角であるべき文字を全角にする.
            var reWide = new Regex( @"[ｦ-ﾝﾞﾟ()①-⑳･'=+*/<>~#%:;-]" );

            var reVBWide = reWide.Replace( reVBNarrow, VBReplaceWide );

            // Microsoft.VisualBasic.StrConv 関数でWideを利用しても半角の "\" が全角に変換されない
            reVBWide = reVBWide.Replace( @"\", "￥" );

            // 濁点・半濁点が分割されていることがあるので結合する.
            var combiningVoicedMarks = CombiningVoicedMarks( reVBWide );
            // ゆらぎ除去.
            var absorbFluctuations = AbsorbFluctuations( combiningVoicedMarks );
            // 線の場合分け変換.
            var lineReplaceByCase = LineReplaceByCase( absorbFluctuations );

            return lineReplaceByCase;
        }

        /// <summary>
        /// 半角にする
        /// </summary>
        public static string VBReplaceNarrow( string original )
        {
            return Strings.StrConv( original, VbStrConv.Narrow );
        }

        /// <summary>
        /// 濁点、半濁点を結合する
        /// </summary>
        private static string CombiningVoicedMarks( string original )
        {
            if ( original.Contains( "゛" ) || original.Contains( "゜" ) )
            {
                // 複数回ReplaceするならStringBuilderを通してReplaceする方がパフォーマンスが良い
                var strBuilder = new StringBuilder( original );
                strBuilder.Replace( "か゛", "が" )
                          .Replace( "き゛", "ぎ" )
                          .Replace( "く゛", "ぐ" )
                          .Replace( "け゛", "げ" )
                          .Replace( "こ゛", "ご" )
                          .Replace( "さ゛", "ざ" )
                          .Replace( "し゛", "じ" )
                          .Replace( "す゛", "ず" )
                          .Replace( "せ゛", "ぜ" )
                          .Replace( "ぞ゛", "ぞ" )
                          .Replace( "た゛", "だ" )
                          .Replace( "ち゛", "ぢ" )
                          .Replace( "つ゛", "づ" )
                          .Replace( "て゛", "で" )
                          .Replace( "と゛", "ど" )
                          .Replace( "は゛", "ば" )
                          .Replace( "ひ゛", "び" )
                          .Replace( "ふ゛", "ぶ" )
                          .Replace( "へ゛", "べ" )
                          .Replace( "ほ゛", "ぼ" )
                          .Replace( "ウ゛", "ヴ" )
                          .Replace( "カ゛", "ガ" )
                          .Replace( "キ゛", "ギ" )
                          .Replace( "ク゛", "グ" )
                          .Replace( "ケ゛", "ゲ" )
                          .Replace( "コ゛", "ゴ" )
                          .Replace( "サ゛", "ザ" )
                          .Replace( "シ゛", "ジ" )
                          .Replace( "ス゛", "ズ" )
                          .Replace( "セ゛", "ゼ" )
                          .Replace( "ソ゛", "ゾ" )
                          .Replace( "タ゛", "ダ" )
                          .Replace( "チ゛", "ヂ" )
                          .Replace( "ツ゛", "ヅ" )
                          .Replace( "テ゛", "デ" )
                          .Replace( "ト゛", "ド" )
                          .Replace( "ハ゛", "バ" )
                          .Replace( "ヒ゛", "ビ" )
                          .Replace( "フ゛", "ブ" )
                          .Replace( "ヘ゛", "ベ" )
                          .Replace( "ホ゛", "ボ" )
                          .Replace( "ワ゛", "ヷ" )
                          .Replace( "ヰ゛", "ヸ" )
                          .Replace( "ゑ゛", "ヹ" )
                          .Replace( "ヲ゛", "ヺ" )
                          .Replace( "は゜", "ぱ" )
                          .Replace( "ひ゜", "ぴ" )
                          .Replace( "ふ゜", "ぷ" )
                          .Replace( "へ゜", "ぺ" )
                          .Replace( "ほ゜", "ぽ" )
                          .Replace( "ハ゜", "パ" )
                          .Replace( "ヒ゜", "ピ" )
                          .Replace( "フ゜", "プ" )
                          .Replace( "ヘ゜", "ペ" )
                          .Replace( "ホ゜", "ポ" );
                return strBuilder.ToString();
            }
            else
            {
                return original;
            }
        }

        /// <summary>
        /// ゆらぎの除去
        /// ローマ数字を分解する（アルファベットのI　アイ　にする）
        /// Ⅰ、Ⅱ、Ⅲだけ対応しておけば十分
        /// その他単位を一文字にしたものは半角表現にする
        /// </summary>
        private static string AbsorbFluctuations( string original )
        {
            // Containsで確認する文字とReplaceで置換する文字の組み合わせが同じなのでContainsの意味が無いのでやらない
            // 複数回ReplaceするならStringBuilderを通してReplaceする方がパフォーマンスが良い
            var strBuilder = new StringBuilder( original );
            strBuilder.Replace( "Ⅰ", "I" )
                      .Replace( "Ⅱ", "II" )
                      .Replace( "Ⅲ", "III" )
                      .Replace( "Ⅳ", "IV" )
                      .Replace( "Ⅴ", "V" )
                      .Replace( "Ⅵ", "VI" )
                      .Replace( "Ⅶ", "VII" )
                      .Replace( "Ⅷ", "VIII" )
                      .Replace( "Ⅸ", "IX" )
                      .Replace( "Ⅹ", "X" )
                      .Replace( "㍉", "mm" )
                      .Replace( "㌢", "cm" )
                      .Replace( "㍍", "m" )
                      .Replace( "㌖", "km" )
                      .Replace( "㌘", "g" )
                      .Replace( "㌔", "kg" )
                      .Replace( "㌧", "t" )
                      .Replace( "㌃", "a" )
                      .Replace( "㌶", "ha" )
                      .Replace( "㍑", "L" )
                      .Replace( "㍗", "W" )
                      .Replace( "㌫", "%" )
                      .Replace( "㌻", "p" )
                      .Replace( "㎜", "mm" )
                      .Replace( "㎝", "cm" )
                      .Replace( "㎞", "km" )
                      .Replace( "㎡", "m2" )
                      .Replace( "㎥", "m3" )
                      .Replace( "㎎", "mg" )
                      .Replace( "㎏", "kg" )
                      .Replace( "ℓ", "L" )
                      .Replace( "㎘", "kL" )
                      .Replace( "㏄", "cc" )
                      .Replace( "№", "No." )
                      .Replace( "㏍", "K.K." )
                      .Replace( "℡", "TEL" )
                      .Replace( "ヶ", "ケ" );
            return strBuilder.ToString();
        }

        /// <summary>
        /// 線文字の置換が必要ならパターンに応じた置換を行う
        /// </summary>
        /// <returns></returns>
        public static string LineReplaceByCase( string original )
        {
            const string WideKanaPattern = @"^[ァ-ヶー]+";
            const string HalfKanaPattern = @"^[｡-ﾟ]+";
            const string AlphanumericPattern = @"^[0-9a-zA-Z]+";
            const string NumericPattern = @"^[0-9]+";
            const string FrontBracketsPattern = @"^[(｢[（「［]+";

            var matches = Regex.Matches( original, LineReplaceTargetPattern );
            if ( matches.Count == 0 )
                return original;

            var strBuilder = new StringBuilder( original );
            var lastIndex = original.Length - 1;
            foreach ( Match match in matches )
            {
                var front = ( match.Index == 0 ) ? string.Empty : original[ match.Index - 1 ].ToString();
                // 「―－」のように対象文字が２つ並んだ場合を想定して１文字ずつ処理
                for ( var i = 0; i < match.Value.Length; ++i )
                {
                    var matchChar = match.Value[ i ];
                    // 前が改行コードの時は実施しない(PDF取込で使用される改行コードのみチェック)
                    if ( front != "\n" )
                    {
                        // 前が全角カタカナ
                        if ( Regex.IsMatch( front, WideKanaPattern ) )
                        {
                            strBuilder.Replace( $"{front}{matchChar}", $"{front}ー" );
                            continue;
                        }
                        // 前が半角カタカナ
                        else if ( Regex.IsMatch( front, HalfKanaPattern ) )
                        {
                            strBuilder.Replace( $"{front}{matchChar}", $"{front}ｰ" );
                            continue;
                        }
                    }
                    else { front = string.Empty; }

                    // 置換文字と同じなら処理しない
                    if ( matchChar != '-' )
                    {
                        // 前が空文字や括弧、後が数字
                        var back = ( match.Index == lastIndex ) ? string.Empty : original[ match.Index + 1 + i ].ToString();
                        var isFrontEmpty = string.IsNullOrWhiteSpace( front );
                        if ( ( isFrontEmpty || Regex.IsMatch( front, FrontBracketsPattern ) ) && Regex.IsMatch( back, NumericPattern ) )
                        {
                            strBuilder.Replace( $"{front}{matchChar}{back}", $"{front}-{back}" );
                            continue;
                        }
                        // 前の文字が空文字でないなら実施
                        else if ( !isFrontEmpty )
                        {
                            // 前後が英数字
                            if ( Regex.IsMatch( front, AlphanumericPattern ) && Regex.IsMatch( back, AlphanumericPattern ) )
                            {
                                strBuilder.Replace( $"{front}{matchChar}{back}", $"{front}-{back}" );
                                continue;
                            }
                        }
                    }
                }
            }
            return strBuilder.ToString();
        }

        /// <summary>
        /// 一致した文字を半角にする
        /// </summary>
        private static string VBReplaceNarrow( Match match )
        {
            return Strings.StrConv( match.Value, VbStrConv.Narrow );
        }

        /// <summary>
        /// 一致した文字を全角にする
        /// </summary>
        private static string VBReplaceWide( Match match )
        {
            return Strings.StrConv( match.Value, VbStrConv.Wide );
        }

        /// <summary>
        /// スペースを削る
        /// </summary>
        public static string ReplaceSpace( string original )
        {
            if ( string.IsNullOrEmpty( original ) )
                return string.Empty;

            return original.Replace( @" ", @"" ).Replace( @"　", @"" );
        }

        /// <summary>
        /// カンマを削る
        /// </summary>
        public static string RemoveComma( string original )
        {
            if ( string.IsNullOrEmpty( original ) )
                return string.Empty;

            return original.Replace( @",", @"" );
        }

        /// <summary>
        /// 数字だけを取り出す
        /// </summary>
        /// <returns></returns>
        public static string ExtractOnlyTheNumbers( string original )
        {
            if ( string.IsNullOrEmpty( original ) )
                return string.Empty;
            return Regex.Replace( original, @"[^0-9]", "" );
        }

        /// <summary>
        /// 先頭の文字から最初の数字の塊部分だけを取り出す.(整数)
        /// 例）第0001号123　といった文字列があったとき、0001を取り出す
        /// </summary>
        /// <returns></returns>
        public static int? FromTheBeginningNumbers( string original )
        {
            if ( string.IsNullOrEmpty( original ) )
                return null;

            // 先頭の文字列から数字を抜き出していく
            var numbersStrBuilder = new StringBuilder();

            var isNumberZone = false;
            foreach ( var originalChar in original )
            {
                if ( isNumberZone )
                {
                    if ( char.IsDigit( originalChar ) )
                    {
                        numbersStrBuilder.Append( originalChar );
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    if ( char.IsDigit( originalChar ) )
                    {
                        isNumberZone = true;
                        numbersStrBuilder.Append( originalChar );
                    }
                }
            }

            // 抜き出された文字が数字になってるか判定
            int resultNumbers = 0;
            if ( !string.IsNullOrEmpty( numbersStrBuilder.ToString() )
                && int.TryParse( numbersStrBuilder.ToString(), out resultNumbers ) )
            {
                return resultNumbers;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 漢数字を数字（半角）に変換し、
        /// 【「数字（半角前提、小数点あり）」「単位」】という文字列の時、「数字（半角前提、小数点あり）」「単位」を分離する
        /// 分離が失敗（数字部分が数字としてみなせなかったら）すべての文字列は単位だったとして返す
        /// </summary>
        public static (string, string) SeparatNumbersUnit( string original )
        {
            if ( string.IsNullOrEmpty( original ) )
                return (string.Empty, string.Empty);

            // 漢数字の対応
            // 横浜市委託・水道局の設計書で取り込めない設計書がある #3513 局所的に必要なため簡易的に行っている。別の場所などで必要なら切り分ける。
            original = original
                .Replace( "一", "1" )
                .Replace( "二", "2" )
                .Replace( "三", "3" )
                .Replace( "四", "4" )
                .Replace( "五", "5" )
                .Replace( "六", "6" )
                .Replace( "七", "7" )
                .Replace( "八", "8" )
                .Replace( "九", "9" )
                .Replace( "十", "10" );

            // 先頭の文字列から数字を抜き出していく
            var numbersStrBuilder = new StringBuilder();
            var unitStrBuilder = new StringBuilder();

            var isUnitZone = false;
            foreach ( var originalChar in original )
            {
                if ( !isUnitZone )
                {
                    if ( char.IsDigit( originalChar ) || originalChar == '.' || originalChar == ',' ) // 小数点の「.」と桁の「,」には対応
                    {
                        numbersStrBuilder.Append( originalChar );
                    }
                    else
                    {
                        // 単位ゾーンの開始.
                        isUnitZone = true;
                        unitStrBuilder.Append( originalChar );
                    }
                }
                else
                {
                    unitStrBuilder.Append( originalChar );
                }
            }

            // 抜き出された文字が数字になってるか判定
            double resultNumbers = 0;
            if ( !string.IsNullOrEmpty( numbersStrBuilder.ToString() )
                && double.TryParse( numbersStrBuilder.ToString(), out resultNumbers ) )
            {
                return (numbersStrBuilder.ToString(), unitStrBuilder.ToString());
            }
            else
            {
                return (string.Empty, original);
            }
        }

        /// <summary>
        /// カタカナに寄せる
        /// </summary>
        /// <returns></returns>
        public static string VBReplaceKatakana( string original )
        {
            if ( string.IsNullOrEmpty( original ) )
                return string.Empty;
            return Strings.StrConv( original, VbStrConv.Katakana );
        }

        /// <summary>
        /// カッコ「()」を取り除いて中身を取得する
        /// </summary>
        /// <returns></returns>
        public static string GetContentsInKakko( string original )
        {
            if ( string.IsNullOrEmpty( original ) )
                return string.Empty;
            return Regex.Match( original, @"(?<=[（]).*?(?=[）])" ).Value;
        }

    }
}
