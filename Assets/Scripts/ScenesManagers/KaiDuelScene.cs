using UnityEngine;

public class KaiDuelScene : MonoBehaviour
{

    [SerializeField] private PlayerManager playerManager;

    private void Start()
    {
        playerManager.SetCanMove(true);
    }
}
