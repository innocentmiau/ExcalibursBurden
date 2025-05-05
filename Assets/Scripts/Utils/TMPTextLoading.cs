using System.Collections;
using Important;
using Managers;
using TMPro;
using UnityEngine;

namespace Utils
{
    
    public class TMPTextLoading : MonoBehaviour
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
            StartCoroutine(ChangeText(text));
        }

        private IEnumerator ChangeText(string text)
        {
            int indexStart = text.IndexOf("...");
            int count = 0;
            while (true)
            {
                string newText = text.Substring(0, (indexStart + count)) 
                                 + "<color=#FFFFFF00>" + text.Substring((indexStart + count), (3-count))  + "</color>"+ text.Substring((indexStart+3));
                _tmp.text = newText;
                _tmp.ForceMeshUpdate();
                yield return new WaitForSeconds(0.2f);
                count++;
                if (count > 3) count = 0;
            }
        }
        
    }

}