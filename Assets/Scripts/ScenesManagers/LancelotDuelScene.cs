using UnityEngine;

namespace ScenesManagers
{
    public class LancelotDuelScene : MonoBehaviour
    {
        [SerializeField] private PlayerManager playerManager;

        private void Start()
        {
            playerManager.SetCanMove(true);

        }
    }
}