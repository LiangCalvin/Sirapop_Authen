using System.Net.Http.Json;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using IISAuthen.Repositories.Interface;
namespace IISAuthen.Repositories
{
    public class Encrypt : IEncrypt
    {
        private readonly string _key;
        public Encrypt(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new Exception("Encrypt NotFound Key Or ConnectionString!");
            }
            _key = key;
        }
        public T DecryptAES<T>(string ciphertext)
        {
            byte[] iv = Convert.FromBase64String(ciphertext.Split(":")[0]);
            byte[] ciphertextBytes = Convert.FromBase64String(ciphertext.Split(":")[1]);
            using (System.Security.Cryptography.Aes aes = System.Security.Cryptography.Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(_key);
                aes.IV = iv;
                aes.Mode = CipherMode.CBC; // Electronic Codebook mode
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                byte[] decryptedBytes = decryptor.TransformFinalBlock(ciphertextBytes, 0, ciphertextBytes.Length);

                string decryptedText = Encoding.UTF8.GetString(decryptedBytes);
                return JsonSerializer.Deserialize<T>(decryptedText)!;            }
        }

        public string EncryptAES(string plaintext)
        {
            byte[] plaintextBytes = Encoding.UTF8.GetBytes(plaintext);
            using (System.Security.Cryptography.Aes aes = System.Security.Cryptography.Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(_key);
                aes.Mode = CipherMode.CBC; // Electronic Codebook mode
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                byte[] encryptedBytes = encryptor.TransformFinalBlock(plaintextBytes, 0, plaintextBytes.Length);

                string ciphertext = Convert.ToBase64String(encryptedBytes);
                return ciphertext;
            }
        }
    }
}