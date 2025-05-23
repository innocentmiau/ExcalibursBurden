using UnityEngine;

public class KayDuelScene : MonoBehaviour
{

    [SerializeField] private PlayerManager playerManager;

    private void Start()
    {
        playerManager.SetCanMove(true);
    }

    public void KayLost()
    {
        Debug.Log("a");
    }
    
}
