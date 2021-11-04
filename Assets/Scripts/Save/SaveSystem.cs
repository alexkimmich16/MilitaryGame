using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using UnityEngine;

public static class SaveSystem
{
    /*
    public static void SaveStats()
    {
        //Debug.Log("save");
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.fun";
        FileStream stream = new FileStream(path, FileMode.Create);
        SaveData data = new SaveData();

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void LoadGameLarge()
    {
        SaveData CurrentData = LoadGame();
        if (CurrentData == null)
        {
            //Debug.Log("Error");
            return;
        }
        UpgradeScript.ReciveLoad(CurrentData.Silver, CurrentData.Gems, CurrentData.Jump, CurrentData.Control, CurrentData.Speed);
        Options.instance.ChangeSlider(CurrentData.SoundVolume, 1);
        Options.instance.ChangeSlider(CurrentData.MusicVolume, 0);
        Options.instance.ChangeBoth(CurrentData.Name);
        LevelStorage.RecieveLevels(CurrentData.Stars);
        PolicyMenu.RecieveBool(CurrentData.ShowTerms);
    }

    public static SaveData LoadGame()
    {
        string path = Application.persistentDataPath + "/player.fun";
        FileStream stream = new FileStream(path, FileMode.Open);
        if (File.Exists(path) && stream.Length > 0)
        {
            //Debug.Log("Exists");

            BinaryFormatter formatter = new BinaryFormatter();
            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();
            return data;
        }
        else
        {
            //Debug.Log("NotFound in" + path);
            return null;
        }
    }
    */
}
