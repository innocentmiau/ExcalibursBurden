using System.IO;
using UnityEngine;

namespace Important
{
    
    public class SaveSystem
    {
        private string _saveFilePath;

        public SaveSystem(bool startFreshForDebug = true)
        {
            _saveFilePath = Application.persistentDataPath + "/savefile.json";
            if (startFreshForDebug)
            {
                Debug.Log("<color=#e6e27a>[Start fresh for debug]</color> <color=#65fc4e>Enabled</color>:\n<color=#ff4f4f>Deleting current Save File</color>");
                File.Delete(_saveFilePath);
            }
        }

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
        public int checkPoint;
    }
}