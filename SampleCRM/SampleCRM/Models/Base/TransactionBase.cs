using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SampleCRM.Models.Base
{
    /// <summary>
    /// トランザクション系テーブルの基本クラス
    /// </summary>
    public abstract class TransactionBase : TableBase
    {
        [Required]
        [Display( Name = "削除日時" )]
        [Comment( "削除日時" )]
        [Precision( 0 )]
        [DataType( DataType.DateTime )]
        public DateTime DeleteDate { get; internal set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TransactionBase()
        {
            // DateTime.MaxValueを「論理削除されていない状態」と定義する。
            // レコード作成時に「論理削除されていない状態」をセットする。
            DeleteDate = DateTime.MaxValue;
        }

        /// <summary>
        /// 論理削除実行
        /// </summary>
        public void SoftDelete()
        {
            // 「論理削除されていない状態」から「論理削除されている状態」への遷移時、「論理削除を実行した日時」を入れる。
            DeleteDate = DateTime.Now;
        }

        /// <summary>
        /// 論理削除差し戻し
        /// </summary>
        public void UnSoftDelete()
        {
            // 「論理削除されている状態」から「論理削除されていない状態」に戻す。
            DeleteDate = DateTime.MaxValue;
        }

        /// <summary>
        /// 論理削除されているかどうか
        /// </summary>
        /// <remarks>
        /// DeleteDateがDateTime.MaxValueでなければ「論理削除されている状態」。
        /// </remarks>
        public bool IsSoftDeleted()
        {
            // C#側とDB側で小数点以下の有効桁数が違うため、秒の整数部までで比較する。
            return DeleteDate.ToString( "yyyy/MM/dd HH:mm:ss" ) != DateTime.MaxValue.ToString( "yyyy/MM/dd HH:mm:ss" );
        }
    }
}
