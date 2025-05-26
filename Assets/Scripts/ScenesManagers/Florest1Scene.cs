using System;
using System.Collections;
using Characters;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Florest1Scene : MonoBehaviour
{

    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private OptionsCanvasManager optionsCanvasManager;
    [SerializeField] private Transform canvas;
    
    private MessagesManager _messagesManager;
    private void Start()
    {
        try
        {
            _messagesManager = GameObject.Find("Managers").GetComponent<MessagesManager>();
        }
        catch (Exception e)
        {
            return;
        }

        PlayStartMoment();

    }

    public void PlayStartMoment()
    {
        StartCoroutine(StartAnimation());
    }
    
    public void HitNextSceneTrigger()
    {
        SceneManager.LoadSceneAsync("Florest_2");
    }

    private int _currentStep = 0;
    public bool AlreadyTalked { get; private set; } = false;
    public void TalkingToEctor(NPCManager talkingNpc)
    {
        AlreadyTalked = true;
        TalkTo(talkingNpc, _currentStep);
    }
    
    private void TalkTo(NPCManager talkingNpc, int currentStep)
    {
        //if (option == 0) optionsCanvasManager.PlayEctorTalk0();
        optionsCanvasManager.StartConversation(talkingNpc, currentStep);
    }
    private IEnumerator DisappearingTipBox(CanvasGroup canvasG, float stay, float fadeout)
    {
        yield return new WaitForSeconds(stay);
        float elapsed = 0f;
        while (elapsed < fadeout)
        {
            yield return null;
            elapsed += Time.deltaTime;
            canvasG.alpha = Mathf.Lerp(1f, 0f, (elapsed / fadeout));
        }
        canvasG.alpha = 0f;
        canvasG.gameObject.SetActive(false);
    }

    // colorful names for npcs? maybe?
    private string FormatTextWithNpcColors(string text)
    {
        if (text.Contains("Chapter 1"))
        {
            text = text.Replace("Chapter 1", "<color=#ffcf4a>Chapter 1</color>");
        }
        return text;
    }
    
    private IEnumerator StartAnimation()
    {
        Transform allBlack = canvas.Find("AllBlack");
        CanvasGroup cg = allBlack.GetComponent<CanvasGroup>();
        allBlack.gameObject.SetActive(true);
        cg.alpha = 1f;
        TMP_Text tmp = allBlack.Find("Text").GetComponent<TMP_Text>();
        string text = _messagesManager.GetTextByID(1011);
        for (int i = 0; i < text.Length; i++)
        {
            string startText = FormatTextWithNpcColors(text.Substring(0, i));
            string newText = startText + "<color=#00000000>" + text.Substring(i);
            tmp.text = newText;
            yield return new WaitForSeconds(2f/text.Length);
        }
        tmp.text = FormatTextWithNpcColors(text);
        yield return new WaitForSeconds(1f);

        float elapsed = 0;
        while (elapsed < 1.5f)
        {
            cg.alpha = Mathf.Lerp(1f, 0f, (elapsed / 1.5f));
            elapsed += Time.deltaTime;
            yield return null;
        }
        allBlack.gameObject.SetActive(false);
        playerManager.SetCanMove(true);
        
    }
    
}
