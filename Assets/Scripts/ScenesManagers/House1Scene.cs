using System;
using System.Collections;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class House1Scene : MonoBehaviour
{

    
    [SerializeField] private Transform canvas;
    [SerializeField] private PlayerManager playerManager;

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

        PlayStartMoment();

    }
    
    public void HitNextSceneTrigger()
    {
        SceneManager.LoadSceneAsync("House_2");
    }

    public void PlayStartMoment()
    {
        StartCoroutine(StartAnimation());
    }

    public void ShowTipBox(string text)
    {
        Transform tbt = canvas.Find("TipBox");
        tbt.gameObject.SetActive(true);
        CanvasGroup cg = tbt.GetComponent<CanvasGroup>();
        TMP_Text tmp = tbt.Find("Text").GetComponent<TMP_Text>();
        tmp.text = text;
        if (_tipBoxDisappear != null) StopCoroutine(_tipBoxDisappear);
        _tipBoxDisappear = StartCoroutine(DisappearingTipBox(cg, 8f, 5f));
    }

    private Coroutine _tipBoxDisappear;

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
        if (text.Contains("Ector"))
        {
            text = text.Replace("Ector", "<color=#78db53>Ector</color>");
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
        string text = _messagesManager.GetTextByID(1000);
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
        ShowTipBox(_messagesManager.GetTextByID(1001));
    }
    
    
    
}
