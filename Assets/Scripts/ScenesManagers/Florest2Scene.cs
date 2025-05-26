using Characters;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Florest2Scene : MonoBehaviour
{
    
    
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private OptionsCanvasManager optionsCanvasManager;

    [SerializeField] private GameObject deleteCollider;
    
    private void Start()
    {
        playerManager.SetCanMove(true);
    }
    
    public void HitNextSceneTrigger()
    {
        SceneManager.LoadSceneAsync("Florest_3_Excalibur");
    }

    private int _currentStep = 0;
    public bool AlreadyTalked { get; private set; } = false;
    public void TalkingToEctor(NPCManager talkingNpc)
    {
        AlreadyTalked = true;
        TalkTo(talkingNpc, _currentStep);
        Destroy(deleteCollider);
    }
    
    private void TalkTo(NPCManager talkingNpc, int currentStep)
    {
        //if (option == 0) optionsCanvasManager.PlayEctorTalk0();
        optionsCanvasManager.StartConversation(talkingNpc, currentStep);
    }
    
}
