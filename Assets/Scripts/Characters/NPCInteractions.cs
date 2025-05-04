using Managers;
using TMPro;
using UnityEngine;

namespace Characters
{
    public class NPCInteractions : MonoBehaviour
    {

        [SerializeField] private RectTransform pressInteractPanel;

        private MessagesManager _messagesManager;
        private void Start()
        {
            pressInteractPanel.gameObject.SetActive(false);
            _messagesManager = GameObject.Find("Managers").GetComponent<MessagesManager>();
        }

        public void ShowToInteract()
        {
            pressInteractPanel.gameObject.SetActive(true);
            string txt = _messagesManager.GetTextByID(7);
            txt = txt.Replace(" E ", " <color=white>E</color> ");
            pressInteractPanel.Find("Text").GetComponent<TMP_Text>().text = txt;
        }

        public void HideToInteract()
        {
            pressInteractPanel.gameObject.SetActive(false);
        }
    }
}