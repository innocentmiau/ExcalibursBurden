using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScenesManagers
{
    public class Florest3Scene : MonoBehaviour
    {
        
        [SerializeField] private PlayerManager playerManager;
        
        private void Start()
        {
            playerManager.SetCanMove(true);
        }
    
        public void HitNextSceneTrigger()
        {
            SceneManager.LoadSceneAsync("Kay_Duel2");
        }
        
    }
}