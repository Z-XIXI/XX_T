using System.Collections;
using UnityEngine;

public class GameDataManager : SingletonClass<GameDataManager>
{
    private GameData gameData;
    public void LoadGameData()
    {
        if (!SaveDataManager.IsExistsGameSave())
        {
            //新游戏
            gameData = GameData.CreateFallbackData();
        }
        else
        {
            gameData = SaveDataManager.LoadGame();
            //如果加载失败，则用备份数据
            if (gameData == null)
            {
                if (SaveDataManager.RestoreFromBackup())
                    gameData = SaveDataManager.LoadGame();
                else
                    gameData = GameData.CreateFallbackData(); // 备份数据不存在，只能开始新游戏
            }
        }

        Debug.Log(gameData.vitality);
    }
    public void SaveGameData()
    {
        Debug.Log(gameData.ToString());
        SaveDataManager.SaveGame(gameData);
    }
    /// <summary>
    /// 修改数据
    /// </summary>
    private void ChangeValue<T>(ref T changeValue, T value, GameDataValueType valueType)
    {
        var oldValue = changeValue;
        changeValue = value;
        EventSystem.Instance.Fire<GameDataValueType, T, T>(GlobalEvents.GameDtatChange, valueType, changeValue, oldValue);
    }
    /// <summary>
    /// 活力
    /// </summary>
    public float Vitality
    {
        get => gameData.vitality;
        set => this.ChangeValue(ref gameData.vitality, value, GameDataValueType.Vitality);
    }



#if UNITY_EDITOR
    public void ResetGameData()
    {
        Debug.LogError("重置游戏数据");
        gameData = GameData.CreateFallbackData();
    }
#endif
}
