using UnityEngine;

namespace Managers
{
    public class GlobalStuff : MonoBehaviour
    {

        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private Transform canvasTransform;

        private void Start()
        {
            canvasTransform.gameObject.SetActive(false);
        }

        public void PressedEsc()
        {
            canvasTransform.gameObject.SetActive(!canvasTransform.gameObject.activeSelf);
            playerManager.SetCanMove(!canvasTransform.gameObject.activeSelf);
        }
    }
}