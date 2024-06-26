using System.Security.Cryptography;

namespace SampleCRM.Common
{
    /// <summary>
    /// 暗号化
    /// </summary>
    /// <remarks></remarks>
    public class Encryptor
    {
        private static int _saltSize = 32;
        private static int _hashSize = 32;

        public void Create( string target, out string salt, int iteration, out string hash )
        {
            byte[] saltBytes = CreateSalt( _saltSize );

            byte[] hashBytes = CreatePBKDF2Hash( target, saltBytes, _hashSize, iteration );

            salt = Convert.ToBase64String( saltBytes );
            hash = Convert.ToBase64String( hashBytes );
        }

        public string GetHash( string target, string salt, int iteration )
        {
            byte[] saltBytes = Convert.FromBase64String( salt );

            byte[] hashBytes = CreatePBKDF2Hash( target, saltBytes, _hashSize, iteration );

            return Convert.ToBase64String( hashBytes );
        }

        private static byte[] CreateSalt( int size )
        {
            var bytes = new Byte[ size ];

            RandomNumberGenerator.Create().GetBytes( bytes );

            return bytes;
        }

        private static byte[] CreatePBKDF2Hash( string password, byte[] salt, int size, int iteration )
        {
            using ( var rfc2898DeriveBytes = new Rfc2898DeriveBytes( password, salt, iteration ) )
            {
                return rfc2898DeriveBytes.GetBytes( size );
            }
        }
    }
}
