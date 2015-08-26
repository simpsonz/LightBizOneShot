using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Security;
using System.Security.Cryptography;

namespace BizOneShot.Light.Util.Security
{
    public class SHACryptography
    {
        private SHA256CryptoServiceProvider _SHA256Provider;

        public SHACryptography()
        {
            this._SHA256Provider = new SHA256CryptoServiceProvider();
        }

        public string Encrypt(string encryptValue)
        {
            byte[] hashValue = this._SHA256Provider.ComputeHash(Encoding.Default.GetBytes(encryptValue));
            return Convert.ToBase64String(hashValue);
        }

        public string EncryptString(string encryptValue)
        {
            byte[] hashValue = this._SHA256Provider.ComputeHash(Encoding.Default.GetBytes(encryptValue));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashValue.Length; i++)
            {
                sb.Append(hashValue[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
