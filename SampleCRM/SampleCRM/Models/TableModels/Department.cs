using SampleCRM.Models.Base;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleCRM.Models.TableModels
{
    /// <summary>
    /// 部署
    /// </summary>
    [Table( "T_Departments" )]
    public class Department : TransactionBase
    {
        [Key]
        [DatabaseGenerated( DatabaseGeneratedOption.Identity )]
        [Display( Name = "部署コード" )]
        [Comment( "部署コード" )]
        public int DepartmentCode { get; set; }

        [Required]
        [Display( Name = "会社コード" )]
        [Comment( "会社コード" )]
        public int CompanyCode { get; set; }

        [Required]
        [Display( Name = "代表部署かどうか" )]
        [Comment( "代表部署かどうか" )]
        public bool IsMain { get; set; }

        /// <summary>
        /// ヤマト運輸の発送先と保守料請求先部署として部署テーブルを使う際には
        /// 会社名(お届け先名) + 改行コード + 部門名1という形で登録し、改行コードが入っているかどうかで判断する
        /// なので登録する際と表示する際には気をつけること
        /// なお部門名1の文字数制限は「25文字/50文字(ｶﾅ)」となっている
        /// </summary>
        [StringLength( 128 )]
        [Display( Name = "部署名" )]
        [Comment( "部署名" )]
        public string DepartmentName { get; set; }

        /// <summary>
        /// ヤマト運輸の発送先として使う場合には
        /// 半角ｶﾅのみで50文字が最大となる
        /// </summary>
        [StringLength( 128 )]
        [RegularExpression( @"\A[ァ-ヶ ー]+\z", ErrorMessage = "全角カタカナのみ入力できます。" )]
        [Display( Name = "部署名フリガナ" )]
        [Comment( "部署名フリガナ" )]
        public string PhoneticName { get; set; }

        /// <summary>
        /// ヤマト運輸のシステムではお届け先会社部門1と部門2を登録出来るが、その最大文字数がどちらも25文字
        /// CD発送先として登録する詳細部署名はヤマト運輸の部門2に該当する
        /// CD発送先として登録する場合には25文字を超えたかどうかのチェックが必須になる
        /// </summary>
        [StringLength( 128 )]
        [Display( Name = "詳細部署名" )]
        [Comment( "詳細部署名" )]
        public string DetailedDepartmentName { get; set; }

        [StringLength( 8 )]
        [DataType( DataType.PostalCode )]
        [RegularExpression( @"\d{3}-\d{4}", ErrorMessage = "○○○-○○○○で記述してください。" )]
        [Display( Name = "郵便番号" )]
        [Comment( "郵便番号" )]
        public string PostalCode { get; set; }

        [StringLength( 4 )]
        [Display( Name = "都道府県" )]
        [Comment( "都道府県" )]
        public string Prefecture { get; set; }

        /// <summary>
        /// CD発送先を登録する際には番地もこちらに含める(建物-部屋番号は含めない)
        /// </summary>
        [StringLength( 32 )]
        [Display( Name = "市区町村" )]
        [Comment( "市区町村" )]
        public string City { get; set; }

        /// <summary>
        /// CD発送先を登録する際には番地を登録しない
        /// </summary>
        [StringLength( 16 )]
        [Display( Name = "番地" )]
        [Comment( "番地" )]
        public string Block { get; set; }

        [StringLength( 32 )]
        [Display( Name = "建物名-部屋番号" )]
        [Comment( "建物名-部屋番号" )]
        public string Building { get; set; }

        //[DataType(DataType.PhoneNumber)]
        [Phone( ErrorMessage = "正しい電話番号ではありません。" )]
        [StringLength( 16 )]
        [Display( Name = "電話番号" )]
        [Comment( "電話番号" )]
        public string TelNumber { get; set; }

        //[DataType(DataType.PhoneNumber)]
        [Phone( ErrorMessage = "正しいFAX番号ではありません。" )]
        [StringLength( 16 )]
        [Display( Name = "FAX番号" )]
        [Comment( "FAX番号" )]
        public string FaxNumber { get; set; }

        //[DataType(DataType.EmailAddress)]
        [EmailAddress( ErrorMessage = "正しいメールアドレスではありません。" )]
        [StringLength( 255 )]
        [Display( Name = "メールアドレス" )]
        [Comment( "メールアドレス" )]
        public string Email { get; set; }

        [StringLength( 2048 )]
        [DataType( DataType.MultilineText )]
        [Display( Name = "備考" )]
        [Comment( "備考" )]
        public string Note { get; set; }

        // 部署コードと同じ値に設定する
        // TODO: 可能であればSHOW TABLE STATUS LIKE 'Departments' を実行し、Auto_increment列の値が次の値となるのでそれを挿入する
        [Required]
        [Display( Name = "表示順" )]
        [Comment( "表示順" )]
        public int DisplayOrder { get; set; }
    }
}
