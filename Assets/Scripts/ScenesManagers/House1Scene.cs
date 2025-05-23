using UnityEngine;
using UnityEngine.SceneManagement;

public class House1Scene : MonoBehaviour
{

    
    [SerializeField] private PlayerManager playerManager;

    private void Start()
    {
        playerManager.SetCanMove(true);
    }
    
    public void HitNextSceneTrigger()
    {
        SceneManager.LoadSceneAsync("House_2");
    }
    
}
