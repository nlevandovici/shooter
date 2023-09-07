using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveLoadManager
{
    private static SaveData SaveData;



    public static int WinCount
    {
        get
        {
            return SaveData.winCount;
        }
    }

    public static int LoseCount
    {
        get
        {
            return SaveData.loseCount;
        }
    }



    public static void AddWin()
    {
        SaveData.winCount++;
    }

    public static void AddLose()
    {
        SaveData.loseCount++;
    }



    public static void Load()
    {
        string path = Path.Combine(Application.dataPath, "data.sav");

        if (File.Exists(path))
        {
            try
            {
               SaveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(path));
            }
            catch
            {
                SaveData = new SaveData();
            }
        }
        else
        {
            SaveData = new SaveData();
        }
    }

    public static void Save()
    {
        File.WriteAllText(Path.Combine(Application.dataPath, "data.sav"), JsonUtility.ToJson(SaveData));
    }
}
