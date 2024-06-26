using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SampleCRM.Common
{
    /// <summary>
    /// 属性情報取得ユーティリティ
    /// </summary>
    public static class AttributeUtility
    {
        /// <summary>
        /// 型指定でpublicプロパティの属性を取得する
        /// </summary>
        /// <typeparam name="T">属性</typeparam>
        /// <param name="type">型</param>
        /// <param name="name">属性名</param>
        /// <returns></returns>
        private static T GetPropertyAttribute<T>( Type type, string name ) where T : Attribute
        {
            var property = type.GetProperty( name );
            if ( property == null )
            {
                SampleLog.logger.Error( $"Property is not found => [{name}]" );
                return default;
            }
            var attribute = property.GetCustomAttribute<T>();
            if ( attribute == null )
            {
                SampleLog.logger.Error( $"Attribute is not found => [{name}]" );
                return default;
            }
            return attribute;
        }

        /// <summary>
        /// インスタンスを指定してpublicプロパティの属性を取得
        /// </summary>
        /// <typeparam name="T">属性</typeparam>
        /// <param name="instance"></param>
        /// <param name="name">属性名</param>
        /// <returns></returns>
        public static T GetPropertyAttribute<T>( object instance, string name ) where T : Attribute
        {
            return GetPropertyAttribute<T>( instance.GetType(), name );
        }

        /// <summary>
        /// インスタンスを指定して StringLengthAttribute の最大文字数を取得
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="name">属性名</param>
        /// <returns>最大文字数</returns>
        public static int GetStringMaxLength( this object instance, string name )
        {
            var attribute = GetPropertyAttribute<StringLengthAttribute>( instance, name );
            return attribute.MaximumLength;
        }

        /// <summary>
        /// インスタンスを指定して RegularExpression を取得
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="name">属性名</param>
        /// <returns>正規表現の属性情報</returns>
        public static RegularExpressionAttribute GetRegularExpression( this object instance, string name )
        {
            RegularExpressionAttribute attribute = GetPropertyAttribute<RegularExpressionAttribute>( instance, name );
            return attribute;
        }
    }
}
