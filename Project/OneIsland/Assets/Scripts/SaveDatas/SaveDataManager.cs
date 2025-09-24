using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveDataManager
{
    // 获取Steam平台适当的存储路径
    public static string GetSavePath()
    {
        string path;

        // 使用Application.persistentDataPath作为基础路径
        path = Application.persistentDataPath;

        // 可以添加子文件夹进一步组织数据
        path = Path.Combine(path, "SaveData");

        // 确保目录存在
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        return path;
    }
    public static void SaveGame(GameData data, string fileName = "savegame.dat")
    {
        string fullPath = Path.Combine(SaveDataManager.GetSavePath(), fileName);

        // 将Vector3等Unity特殊类型转换为可序列化格式
        SerializableGameData serializableData = new SerializableGameData(data);

        // 序列化为JSON
        string jsonData = JsonUtility.ToJson(serializableData, true);

        // 使用AES加密数据
        string encryptedData = AESEncryption.Encrypt(jsonData);

        if (encryptedData == null)
        {
            Debug.LogError("加密失败，无法保存游戏");
            return;
        }

        // 写入文件
        File.WriteAllText(fullPath, encryptedData);

        Debug.Log("游戏已保存: " + fullPath);
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
            Debug.LogWarning("存档文件不存在: " + fullPath);
            return null;
        }

        try
        {
            // 读取加密数据
            string encryptedData = File.ReadAllText(fullPath);

            // 解密数据
            string jsonData = AESEncryption.Decrypt(encryptedData);

            if (jsonData == null)
            {
                Debug.LogError("解密失败，可能存档已损坏");
                return null;
            }

            // 反序列化
            SerializableGameData serializableData = JsonUtility.FromJson<SerializableGameData>(jsonData);

            // 验证数据完整性
            if (!SaveDataManager.ValidateSaveData(serializableData))
            {
                Debug.LogWarning("存档数据验证失败，可能已损坏");
                return null;
            }

            return serializableData.ToGameData();
        }
        catch (System.Exception e)
        {
            Debug.LogError("加载存档失败: " + e.Message);
            return null;
        }
    }

    /// <summary>
    /// 备份存档
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
            Debug.LogError("备份失败: " + e.Message);
            return false;
        }
    }

    /// <summary>
    /// 从备份恢复
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
            Debug.LogError("恢复备份失败: " + e.Message);
            return false;
        }
    }

    public static bool ValidateSaveData(SerializableGameData data)
    {
        if (data == null || !data.IsValidateData()) return false;

        // 检查版本兼容性（可选）
        if (!IsVersionCompatible(data.gameVersion))
        {
            Debug.LogWarning($"存档版本 {data.gameVersion} 与当前版本 {Application.version} 可能不兼容");
            // 这里可以添加版本迁移逻辑
        }

        return true;
    }

    private static bool IsVersionCompatible(string savedVersion)
    {
        // 简单的版本兼容性检查
        // 可以根据需要实现更复杂的逻辑
        if (string.IsNullOrEmpty(savedVersion)) return false;

        // 假设主版本号相同则兼容
        string[] currentParts = Application.version.Split('.');
        string[] savedParts = savedVersion.Split('.');

        if (currentParts.Length > 0 && savedParts.Length > 0)
        {
            return currentParts[0] == savedParts[0];
        }

        return false;
    }
}
