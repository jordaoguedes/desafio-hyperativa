using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;

namespace DesafioHyperativa.Repository.Util;

public static class Extension
{
    private static string key = "x$arzz{%wok<%!!g-1ou2l):^$m9&1]y";

    public static byte[] EncryptFileToBytes(IFormFile inputFile)
    {
        try
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.GenerateIV();

                using (MemoryStream msOutput = new MemoryStream())
                {
                    msOutput.Write(aesAlg.IV, 0, aesAlg.IV.Length);

                    using (ICryptoTransform encryptor = aesAlg.CreateEncryptor())
                    using (CryptoStream csEncrypt = new CryptoStream(msOutput, encryptor, CryptoStreamMode.Write))
                    using (Stream inputFileStream = inputFile.OpenReadStream())
                    {
                        inputFileStream.CopyTo(csEncrypt);
                    }

                    return msOutput.ToArray();
                }
            }
        }
        catch (Exception)
        {
            throw new Exception("Erro ao encriptar dados");
        }
    }
    public static IFormFile DecryptBytesToFormFile(byte[] encryptedBytes)
    {
        try
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);

                byte[] iv = new byte[aesAlg.BlockSize / 8];
                Array.Copy(encryptedBytes, iv, iv.Length);
                aesAlg.IV = iv;

                using (ICryptoTransform decryptor = aesAlg.CreateDecryptor())
                using (MemoryStream msOutput = new MemoryStream())
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msOutput, decryptor, CryptoStreamMode.Write))
                    {
                        csDecrypt.Write(encryptedBytes, iv.Length, encryptedBytes.Length - iv.Length);
                    }

                    byte[] decryptedFileBytes = msOutput.ToArray();

                    return new FormFile(new MemoryStream(decryptedFileBytes), 0, decryptedFileBytes.Length, "file", "fileName");
                }
            }
        }
        catch (Exception)
        {
            throw new Exception("Erro ao decriptar dados");
        }
    }

    public static string Encrypt(string text)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(key);
            byte[] fixedIV = new byte[aesAlg.BlockSize / 8];
            aesAlg.IV = fixedIV;

            using (MemoryStream msOutput = new MemoryStream())
            {
                msOutput.Write(aesAlg.IV, 0, aesAlg.IV.Length);

                using (ICryptoTransform encryptor = aesAlg.CreateEncryptor())
                using (CryptoStream csEncrypt = new CryptoStream(msOutput, encryptor, CryptoStreamMode.Write))
                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(text);
                }

                return Convert.ToBase64String(msOutput.ToArray());
            }
        }
    }

    public static string Decrypt(string encryptedText)
    {
        try
        {
            using (Aes aesAlg = Aes.Create())
            {
                byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
                byte[] iv = new byte[aesAlg.IV.Length];
                Array.Copy(encryptedBytes, iv, iv.Length);

                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = iv;

                using (MemoryStream msOutput = new MemoryStream())
                {
                    using (ICryptoTransform decryptor = aesAlg.CreateDecryptor())
                    using (CryptoStream csDecrypt = new CryptoStream(msOutput, decryptor, CryptoStreamMode.Write))
                    {
                        csDecrypt.Write(encryptedBytes, iv.Length, encryptedBytes.Length - iv.Length);
                    }

                    return Encoding.UTF8.GetString(msOutput.ToArray());
                }
            }
        }
        catch(Exception) 
        {
            throw;
        }
    }
}
