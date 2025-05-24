using System;
using System.Collections;
using Important;
using Managers;
using TMPro;
using UnityEngine;

public class TMPLanguageText : MonoBehaviour
{

    [SerializeField] private TextHex textType;
    [SerializeField] private bool forceUpperCase = false;

    private TMP_Text _tmp;
    private MessagesManager _manager;
    private void Awake()
    {
        _tmp = GetComponent<TMP_Text>();
        StartCoroutine(Setup());
    }

    public void RefreshText()
    {
        StartCoroutine(Setup());
    }
    
    private IEnumerator Setup()
    {
        while (true)
        {
            try
            {
                _manager = GameObject.Find("Managers").GetComponent<MessagesManager>();
                if (_manager != null) break;
            }
            catch (Exception e)
            {
                Debug.Log(e);
                yield break;
            }
            yield return new WaitForSeconds(0.01f);
        }

        while (true)
        {
            if (_manager.IsSetup()) break;
            yield return new WaitForSeconds(0.01f);
        }
        SetText(_manager.GetText(textType));
    }
    
    private void SetText(string text)
    {
        if (_tmp != null)
        {
            if (forceUpperCase) text = text.ToUpper();
            _tmp.text = text;
        }
    }
}