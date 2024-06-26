using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SampleCRM.Common
{
    public static class EnumUtility
    {
        // Display属性を上手く拾えない
        public static IEnumerable<TEnum> GetListFromEnum<TEnum>() where TEnum : struct, Enum
            => Enum.GetValues( typeof( TEnum ) ).Cast<TEnum>();

        public static string GetDisplayName( this Enum enumValue )
            => enumValue.GetType()
                            .GetMember( enumValue.ToString() )
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()?
                            .Name ?? enumValue.ToString();

        public static IEnumerable<SelectListItem> GetSelectListFromEnum<TEnum>( TEnum? selectedValue ) where TEnum : struct, Enum
            => Enum.GetValues( typeof( TEnum ) ).Cast<TEnum>().Select( e => new SelectListItem
            {
                Value = e.ToString(),
                Text = e.GetDisplayName(),
                Selected = e.Equals( selectedValue )
            } );
    }
}
