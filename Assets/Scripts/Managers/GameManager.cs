using System.Collections;
using Important;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    
    public class GameManager : MonoBehaviour
    {

        private Data _data;
    
        void Start()
        {
            DontDestroyOnLoad(gameObject);
            _data = new Data();
            
            // load all data from resources here in the future
            // then load main menu scene in async also.
            ChangeScene();
        }

        private void ChangeScene()
        {
            StartCoroutine(LoadMainMenuAsync());
        }

        private IEnumerator LoadMainMenuAsync()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainMenu");
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }

        void Update()
        {
        
        }
    }
    
}

