using Characters;
using Managers;
using UnityEngine;

public class KayDuelScene : MonoBehaviour
{

    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private OptionsCanvasManager optionsCanvasManager;
    [SerializeField] private NPCManager kayNPCManager;


    private void Start()
    {
        playerManager.SetCanMove(true);
    }

    public void KayLost()
    {
        TalkTo(kayNPCManager, 1008);
    }
    
    public void TalkTo(NPCManager talkingNpc, int currentStep)
    {
        //if (option == 0) optionsCanvasManager.PlayEctorTalk0();
        optionsCanvasManager.StartConversation(talkingNpc, currentStep);
    }
    
}
