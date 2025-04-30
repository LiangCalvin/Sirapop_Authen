namespace IISAuthen.Repositories.Interface
{
    public interface IEncrypt
    {
        string EncryptAES(string plaintext);
        T DecryptAES<T>(string ciphertext);
    }
}