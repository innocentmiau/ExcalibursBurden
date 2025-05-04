using UnityEngine;

namespace Managers
{
    public class Game0Manager : MonoBehaviour
    {

        [SerializeField] private OptionsCanvasManager optionsCanvasManager;

        public void TalkToEctor(int option)
        {
            if (option == 0) optionsCanvasManager.PlayEctorTalk0();
        }
        
    }
}