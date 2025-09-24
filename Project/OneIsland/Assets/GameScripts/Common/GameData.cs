using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public float vitality;  //����
    public float strength;  //����

    public string gameVersion; // ���ڰ汾����
    // ������Ҫ�������Ϸ����...
    public static GameData CreateFallbackData()
    {
        // ����Ĭ�ϵ���Ϸ����
        return new GameData()
        {
            vitality = 100,
            strength = 1,
        };
    }
}

// �����л���������ݽṹ
[System.Serializable]
public class SerializableGameData
{
    public float vitality;
    public float strength;
    //public float[] playerPosition; // ��Vector3ת��Ϊfloat����
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
        gameVersion = Application.version; // ���浱ǰ��Ϸ�汾
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
    ///  ��������Ƿ�Ϸ�
    /// </summary>
    public bool IsValidateData()
    {
        return true;
    }
}
