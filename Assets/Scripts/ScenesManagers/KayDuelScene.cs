using Characters;
using Managers;
using UnityEngine;

public class KayDuelScene : MonoBehaviour
{

    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private OptionsCanvasManager optionsCanvasManager;
    [SerializeField] private NPCManager ectorNpcManager;


    private void Start()
    {
        playerManager.SetCanMove(true);
    }

    public void KayLost()
    {
        TalkTo(ectorNpcManager, 3);
    }
    
    public void TalkTo(NPCManager talkingNpc, int currentStep)
    {
        //if (option == 0) optionsCanvasManager.PlayEctorTalk0();
        optionsCanvasManager.StartConversation(talkingNpc, currentStep);
    }
    
}
