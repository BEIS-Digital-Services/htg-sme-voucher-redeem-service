namespace smevoucherencryption
{
    public interface IEncryptionService
    {
        public string Encrypt(string plainText, string password);
        public string Decrypt(string cipherText, string password);
    }
}