using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    
    public class MainMenuManager : MonoBehaviour
    {

        [SerializeField] private Transform canvas;

        public void ClickButtonPlay()
        {
            canvas.Find("Panel").gameObject.SetActive(false);
            StartCoroutine(LoadGameAsync());
        }

        private IEnumerator LoadGameAsync()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Game0");
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
        
    }

}
