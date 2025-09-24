using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class AESEncryption
{
    // ʹ����Կ��������������Կ��IV
    private static readonly byte[] Salt = new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 };
    private const string SecretKey = "OnsIslandBeSucc"; // ����Ϊ����Ϸ��Ψһ��Կ

    public static string Encrypt(string plainText)
    {
        byte[] encrypted;

        using (Aes aesAlg = Aes.Create())
        {
            // ʹ��Rfc2898DeriveBytes������������Կ
            Rfc2898DeriveBytes keyDerivation = new Rfc2898DeriveBytes(SecretKey, Salt);
            aesAlg.Key = keyDerivation.GetBytes(32); // 256λ��Կ
            aesAlg.IV = keyDerivation.GetBytes(16);  // 128λIV

            // ����������
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            // �����ڴ���
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        // д���������ݵ���
                        swEncrypt.Write(plainText);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }
        }

        // ����Base64����ļ�������
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
                // ʹ��Rfc2898DeriveBytes������������Կ
                Rfc2898DeriveBytes keyDerivation = new Rfc2898DeriveBytes(SecretKey, Salt);
                aesAlg.Key = keyDerivation.GetBytes(32); // 256λ��Կ
                aesAlg.IV = keyDerivation.GetBytes(16);  // 128λIV

                // ����������
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // �����ڴ���
                using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // ��ȡ���ܺ������
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
        catch (CryptographicException e)
        {
            Debug.LogError("����ʧ��: " + e.Message);
            return null;
        }
        catch (FormatException e)
        {
            Debug.LogError("���ݸ�ʽ����: " + e.Message);
            return null;
        }

        return plaintext;
    }

    // ���������Կ����ѡ�����ڸ��߼��İ�ȫ����
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