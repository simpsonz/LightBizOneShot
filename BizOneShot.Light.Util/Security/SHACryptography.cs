using System;
using System.Security.Cryptography;
using System.Text;

namespace BizOneShot.Light.Util.Security
{
    public class SHACryptography
    {
        private readonly SHA256CryptoServiceProvider _SHA256Provider;

        public SHACryptography()
        {
            _SHA256Provider = new SHA256CryptoServiceProvider();
        }

        public string Encrypt(string encryptValue)
        {
            var hashValue = _SHA256Provider.ComputeHash(Encoding.Default.GetBytes(encryptValue));
            return Convert.ToBase64String(hashValue);
        }

        public string EncryptString(string encryptValue)
        {
            var hashValue = _SHA256Provider.ComputeHash(Encoding.Default.GetBytes(encryptValue));
            var sb = new StringBuilder();
            for (var i = 0; i < hashValue.Length; i++)
            {
                sb.Append(hashValue[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}