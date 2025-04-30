namespace Important
{
    public class Data
    {

        public int Map { get; private set; }
        public int Checkpoint { get; private set; }
        private SaveSystem _saveSystem;
        
        public Data()
        {
            _saveSystem = new SaveSystem();
            SaveData loadedData = _saveSystem.LoadGame();
            if (loadedData == null)
            {
                SaveData saveData = new SaveData();
                saveData.map = 0;
                saveData.checkPoint = 0;
                _saveSystem.SaveGame(saveData);
                loadedData = _saveSystem.LoadGame();
            }
            
            Checkpoint = loadedData.checkPoint;
        }
        
        public void SaveData()
        {
            SaveData saveData = new SaveData();
            saveData.map = Map;
            saveData.checkPoint = Checkpoint;
            _saveSystem.SaveGame(saveData);
        }
    }
}