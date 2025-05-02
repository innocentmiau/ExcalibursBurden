using System.Collections;
using Important;
using Managers;
using TMPro;
using UnityEngine;

public class TMPLanguageText : MonoBehaviour
{

    [SerializeField] private TextHex textType;

    private TMP_Text _tmp;
    private MessagesManager _manager;
    private void Awake()
    {
        _tmp = GetComponent<TMP_Text>();
        StartCoroutine(Setup());
    }

    private IEnumerator Setup()
    {
        while (true)
        {
            _manager = GameObject.Find("Managers").GetComponent<MessagesManager>();
            if (_manager != null) break;
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
        if (_tmp != null) _tmp.text = text;
    }
}