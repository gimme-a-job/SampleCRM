using SampleCRM.Models.TableModels;
using System.ComponentModel.DataAnnotations;

namespace SampleCRM.Models.ViewModels
{
    /// <summary>
    /// Roleの表示、追加、変更、削除用のModel
    /// </summary>
    public class RoleViewModel
    {
        [Required( ErrorMessage = "{0}は必須です。" )]
        [StringLength( 32, ErrorMessage = "{0} は {2} 文字以上", MinimumLength = 3 )]
        [Display( Name = "ロール名" )]
        public string Name { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <remarks>
        /// これを消すとController側でのBind時にエラーを起こしたため残している
        /// </remarks>
        public RoleViewModel() { }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="role"></param>
        /// <remarks>
        /// TableModelをViewModelに変換する
        /// </remarks>
        public RoleViewModel( IdentityRole role ) => this.Name = role.Name;

        /// <summary>
        /// ViewModelをTableModelに変換する
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// TableModel側にコンストラクタを追加したくないため、こちらで行っている
        /// </remarks>
        public IdentityRole GetTableModel() => new IdentityRole
        {
            Name = this.Name,
        };
    }
}
