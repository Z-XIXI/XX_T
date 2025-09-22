using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveDataManager
{
    // ��ȡSteamƽ̨�ʵ��Ĵ洢·��
    public static string GetSavePath()
    {
        string path;

        // ʹ��Application.persistentDataPath��Ϊ����·��
        path = Application.persistentDataPath;

        // ����������ļ��н�һ����֯����
        path = Path.Combine(path, "SaveData");

        // ȷ��Ŀ¼����
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        return path;
    }
    public static void SaveGame(GameData data, string fileName = "savegame.dat")
    {
        string fullPath = Path.Combine(SaveDataManager.GetSavePath(), fileName);

        // ��Vector3��Unity��������ת��Ϊ�����л���ʽ
        SerializableGameData serializableData = new SerializableGameData(data);

        // ���л�ΪJSON
        string jsonData = JsonUtility.ToJson(serializableData, true);

        // ʹ��AES��������
        string encryptedData = AESEncryption.Encrypt(jsonData);

        if (encryptedData == null)
        {
            Debug.LogError("����ʧ�ܣ��޷�������Ϸ");
            return;
        }

        // д���ļ�
        File.WriteAllText(fullPath, encryptedData);

        Debug.Log("��Ϸ�ѱ���: " + fullPath);
    }
    public static bool IsExistsGameSave(string fileName = "savegame.dat")
    {
        string fullPath = Path.Combine(SaveDataManager.GetSavePath(), fileName);
        if (File.Exists(fullPath))
            return true;
        return false;
    }
    public static GameData LoadGame(string fileName = "savegame.dat")
    {
        string fullPath = Path.Combine(SaveDataManager.GetSavePath(), fileName);

        if (!File.Exists(fullPath))
        {
            Debug.LogWarning("�浵�ļ�������: " + fullPath);
            return null;
        }

        try
        {
            // ��ȡ��������
            string encryptedData = File.ReadAllText(fullPath);

            // ��������
            string jsonData = AESEncryption.Decrypt(encryptedData);

            if (jsonData == null)
            {
                Debug.LogError("����ʧ�ܣ����ܴ浵����");
                return null;
            }

            // �����л�
            SerializableGameData serializableData = JsonUtility.FromJson<SerializableGameData>(jsonData);

            // ��֤����������
            if (!SaveDataManager.ValidateSaveData(serializableData))
            {
                Debug.LogWarning("�浵������֤ʧ�ܣ���������");
                return null;
            }

            return serializableData.ToGameData();
        }
        catch (System.Exception e)
        {
            Debug.LogError("���ش浵ʧ��: " + e.Message);
            return null;
        }
    }

    /// <summary>
    /// ���ݴ浵
    /// </summary>
    public static bool BackupSave(string fileName = "savegame.dat")
    {
        string sourcePath = Path.Combine(SaveDataManager.GetSavePath(), fileName);
        string backupPath = Path.Combine(SaveDataManager.GetSavePath(), $"{fileName}.backup");

        if (!File.Exists(sourcePath))
        {
            return false;
        }

        try
        {
            File.Copy(sourcePath, backupPath, true);
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("����ʧ��: " + e.Message);
            return false;
        }
    }

    /// <summary>
    /// �ӱ��ݻָ�
    /// </summary>
    public static bool RestoreFromBackup(string fileName = "savegame.dat")
    {
        string sourcePath = Path.Combine(SaveDataManager.GetSavePath(), fileName);
        string backupPath = Path.Combine(SaveDataManager.GetSavePath(), $"{fileName}.backup");

        if (!File.Exists(backupPath))
        {
            return false;
        }

        try
        {
            File.Copy(backupPath, sourcePath, true);
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("�ָ�����ʧ��: " + e.Message);
            return false;
        }
    }

    public static bool ValidateSaveData(SerializableGameData data)
    {
        if (data == null || !data.IsValidateData()) return false;

        // ���汾�����ԣ���ѡ��
        if (!IsVersionCompatible(data.gameVersion))
        {
            Debug.LogWarning($"�浵�汾 {data.gameVersion} �뵱ǰ�汾 {Application.version} ���ܲ�����");
            // ���������Ӱ汾Ǩ���߼�
        }

        return true;
    }

    private static bool IsVersionCompatible(string savedVersion)
    {
        // �򵥵İ汾�����Լ��
        // ���Ը�����Ҫʵ�ָ����ӵ��߼�
        if (string.IsNullOrEmpty(savedVersion)) return false;

        // �������汾����ͬ�����
        string[] currentParts = Application.version.Split('.');
        string[] savedParts = savedVersion.Split('.');

        if (currentParts.Length > 0 && savedParts.Length > 0)
        {
            return currentParts[0] == savedParts[0];
        }

        return false;
    }
}
