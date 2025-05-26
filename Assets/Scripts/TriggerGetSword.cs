using System;
using Managers;
using TMPro;
using UnityEngine;

public class TriggerGetSword : MonoBehaviour
{
    
    [SerializeField] private RectTransform pressInteractPanel;

    private MessagesManager _messagesManager;
    private void Start()
    {
        pressInteractPanel.gameObject.SetActive(false);
        try
        {
            _messagesManager = GameObject.Find("Managers").GetComponent<MessagesManager>();
            if (_messagesManager != null) return;
        }
        catch (Exception e)
        {
            return;
        }
    }

    public void ShowToInteract()
    {
        pressInteractPanel.gameObject.SetActive(true);
        if (_messagesManager == null) return;
        string txt = _messagesManager.GetTextByID(13);
        txt = txt.Replace(" E ", " <color=white>E</color> ");
        pressInteractPanel.Find("Text").GetComponent<TMP_Text>().text = txt;
    }

    public void HideToInteract()
    {
        pressInteractPanel.gameObject.SetActive(false);
    }
    
}
