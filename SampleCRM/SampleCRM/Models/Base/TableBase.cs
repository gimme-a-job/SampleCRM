using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SampleCRM.Models.Base
{
    /// <summary>
    /// マスタ系とトランザクション系テーブルの基本クラス
    /// </summary>
    public abstract class TableBase
    {
        [Required]
        [Display( Name = "前回更新者コード" )]
        [Comment( "前回更新者コード" )]
        public int UpdateUserCode { get; set; }

        [Required]
        [Display( Name = "前回更新日時" )]
        [Comment( "前回更新日時" )]
        [Precision( 0 )]
        [DataType( DataType.DateTime )]
        public DateTime UpdateDate { get; set; }
    }
}
