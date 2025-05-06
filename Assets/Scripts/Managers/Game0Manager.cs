using Characters;
using UnityEngine;

namespace Managers
{
    public class Game0Manager : MonoBehaviour
    {

        [SerializeField] private OptionsCanvasManager optionsCanvasManager;

        public void TalkTo(NPCManager talkingNpc, int currentStep)
        {
            //if (option == 0) optionsCanvasManager.PlayEctorTalk0();
            optionsCanvasManager.StartConversation(talkingNpc, currentStep);
        }
        
    }
}