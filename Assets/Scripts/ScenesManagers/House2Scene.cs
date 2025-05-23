using UnityEngine;
using UnityEngine.SceneManagement;

public class House2Scene : MonoBehaviour
{
    
    
    [SerializeField] private PlayerManager playerManager;

    private void Start()
    {
        playerManager.SetCanMove(true);
    }
    
    public void HitNextSceneTrigger()
    {
        SceneManager.LoadSceneAsync("Kay_Duel");
    }
}
