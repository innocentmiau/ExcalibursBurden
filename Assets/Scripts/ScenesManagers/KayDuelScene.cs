using System;
using System.Collections;
using Characters;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KayDuelScene : MonoBehaviour
{

    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private OptionsCanvasManager optionsCanvasManager;
    [SerializeField] private NPCManager ectorNpcManager;
    [SerializeField] private Transform canvas;
    [SerializeField] private AudioSource backgroundSource;
    
    private MessagesManager _messagesManager;
    private void Start()
    {
        playerManager.SetCanMove(true);
        try
        {
            _messagesManager = GameObject.Find("Managers").GetComponent<MessagesManager>();
        }
        catch (Exception e)
        {
            return;
        }
    }

    private bool _kayDefeated = false;
    public void KayLost()
    {
        TalkTo(ectorNpcManager, 3);
        _kayDefeated = true;
    }
    
    public void TalkTo(NPCManager talkingNpc, int currentStep)
    {
        //if (option == 0) optionsCanvasManager.PlayEctorTalk0();
        optionsCanvasManager.StartConversation(talkingNpc, currentStep);
    }

    private Coroutine _fadeout;
    private void Update()
    {
        if (!_kayDefeated) return;
        if (!optionsCanvasManager.gameObject.activeSelf && _fadeout == null)
        {
            _fadeout = StartCoroutine(StartAnimation());
        }
    }
    private IEnumerator StartAnimation()
    {
        Transform allBlack = canvas.Find("AllBlack");
        allBlack.gameObject.SetActive(true);
        CanvasGroup cg = allBlack.GetComponent<CanvasGroup>();
        float elapsed = 0;
        cg.alpha = 0f;
        float startValue = backgroundSource.volume;
        while (elapsed < 1.5f)
        {
            backgroundSource.volume = Mathf.Lerp(startValue, 0f, (elapsed / 1.5f));
            cg.alpha = Mathf.Lerp(0f, 1f, (elapsed / 1.5f));
            elapsed += Time.deltaTime;
            yield return null;
        }
        cg.alpha = 1f;
        SceneManager.LoadSceneAsync("Florest_1");
    }
}
