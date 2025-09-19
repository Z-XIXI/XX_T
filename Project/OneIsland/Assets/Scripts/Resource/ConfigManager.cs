using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class ConfigManage
{
    static Dictionary<string, object> cfg_list = new Dictionary<string, object>();
    /// <summary>
    /// 从指定路径读取JSON文件并返回字符串
    /// </summary>
    /// <param name="filePath">JSON文件的完整路径</param>
    /// <returns>JSON字符串（读取失败返回null）</returns>
    public static string LoadJsonAsString(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            Debug.LogError("文件路径不能为空！");
            return null;
        }

        try
        {
            if (!File.Exists(filePath))
            {
                Debug.LogError($"文件不存在：{filePath}");
                return null;
            }

            return File.ReadAllText(filePath);
        }
        catch (Exception e)
        {
            Debug.LogError($"读取JSON文件失败：{e.Message}");
            return null;
        }
    }

    /// <summary>
    /// 从指定路径读取并解析JSON文件
    /// </summary>
    /// <typeparam name="T">目标数据类型</typeparam>
    /// <param name="filePath">JSON文件的完整路径</param>
    /// <returns>解析后的对象（失败返回默认值）</returns>
    public static T LoadJson<T>(string filePath)
    {
        //Application.dataPath + "\\JsonCfg\\" + filePath + ".json";
        string fileRootPath = Path.Combine(Application.dataPath, "GameScripts", "Config", "JsonCfg", filePath + ".json");
        string jsonData = LoadJsonAsString(fileRootPath);
        if (string.IsNullOrEmpty(jsonData)) return default;

        try
        {
            return JsonUtility.FromJson<T>(jsonData);
        }
        catch (Exception e)
        {
            Debug.LogError($"JSON解析失败：{e.Message}");
            return default;
        }
    }

    public static object GetCfg<T>(string filePath)
    {
        if (!cfg_list.ContainsKey(filePath))
        {
            var data = LoadJson<T>(filePath);
            cfg_list[filePath] = data;
        }
        return cfg_list[filePath];
    }
}
