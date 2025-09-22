using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public float vitality;  //活力
    public float strength;  //力量

    public string gameVersion; // 用于版本控制
    // 其他需要保存的游戏数据...
    public static GameData CreateFallbackData()
    {
        // 创建默认的游戏数据
        return new GameData()
        {
            vitality = 100,
            strength = 1,
        };
    }
}

// 可序列化的替代数据结构
[System.Serializable]
public class SerializableGameData
{
    public float vitality;
    public float strength;
    //public float[] playerPosition; // 将Vector3转换为float数组
    public string gameVersion;

    public SerializableGameData() { }

    public SerializableGameData(GameData data)
    {
        vitality = data.vitality;
        strength = data.strength;
        //playerPosition = new float[3] {
        //    data.playerPosition.x,
        //    data.playerPosition.y,
        //    data.playerPosition.z
        //};
        gameVersion = Application.version; // 保存当前游戏版本
    }

    public GameData ToGameData()
    {
        GameData data = new GameData();
        data.vitality = vitality;
        data.strength = strength;
        //data.playerPosition = new Vector3(
        //    playerPosition[0],
        //    playerPosition[1],
        //    playerPosition[2]
        //);
        data.gameVersion = gameVersion;

        return data;
    }

    /// <summary>
    ///  检测数据是否合法
    /// </summary>
    public bool IsValidateData()
    {
        return true;
    }
}
