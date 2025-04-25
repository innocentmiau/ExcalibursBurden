using System.IO;
using UnityEngine;

namespace Important
{
    
    public class SaveSystem
    {
        private string _saveFilePath;

        public SaveSystem() => _saveFilePath = Application.persistentDataPath + "/savefile.json";

        public void SaveGame(SaveData data)
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(_saveFilePath, json);
            //Debug.Log("Game saved to: " + _saveFilePath);
        }

        public SaveData LoadGame()
        {
            if (File.Exists(_saveFilePath))
            {
                string json = File.ReadAllText(_saveFilePath);
                SaveData data = JsonUtility.FromJson<SaveData>(json);
                //Debug.Log("Game loaded from: " + _saveFilePath);
                return data;
            }
            else
            {
                Debug.LogWarning("Save file not found!");
                return null;
            }
        }
    }

    [System.Serializable]
    public class SaveData
    {
        // VARIABLES TO LOAD FROM/SAVE FILE.
    }
}