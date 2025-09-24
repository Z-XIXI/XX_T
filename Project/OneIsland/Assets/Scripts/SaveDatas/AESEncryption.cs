using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class AESEncryption
{
    // 使用密钥派生函数生成密钥和IV
    private static readonly byte[] Salt = new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 };
    private const string SecretKey = "OnsIslandBeSucc"; // 更改为此游戏的唯一密钥

    public static string Encrypt(string plainText)
    {
        byte[] encrypted;

        using (Aes aesAlg = Aes.Create())
        {
            // 使用Rfc2898DeriveBytes从密码生成密钥
            Rfc2898DeriveBytes keyDerivation = new Rfc2898DeriveBytes(SecretKey, Salt);
            aesAlg.Key = keyDerivation.GetBytes(32); // 256位密钥
            aesAlg.IV = keyDerivation.GetBytes(16);  // 128位IV

            // 创建加密器
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            // 创建内存流
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        // 写入所有数据到流
                        swEncrypt.Write(plainText);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }
        }

        // 返回Base64编码的加密数据
        return Convert.ToBase64String(encrypted);
    }

    public static string Decrypt(string cipherText)
    {
        string plaintext = null;

        try
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            using (Aes aesAlg = Aes.Create())
            {
                // 使用Rfc2898DeriveBytes从密码生成密钥
                Rfc2898DeriveBytes keyDerivation = new Rfc2898DeriveBytes(SecretKey, Salt);
                aesAlg.Key = keyDerivation.GetBytes(32); // 256位密钥
                aesAlg.IV = keyDerivation.GetBytes(16);  // 128位IV

                // 创建解密器
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // 创建内存流
                using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // 读取解密后的数据
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
        catch (CryptographicException e)
        {
            Debug.LogError("解密失败: " + e.Message);
            return null;
        }
        catch (FormatException e)
        {
            Debug.LogError("数据格式错误: " + e.Message);
            return null;
        }

        return plaintext;
    }

    // 生成随机密钥（可选，用于更高级的安全需求）
    public static string GenerateRandomKey(int length = 32)
    {
        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
        {
            byte[] tokenData = new byte[length];
            rng.GetBytes(tokenData);
            return Convert.ToBase64String(tokenData);
        }
    }
}