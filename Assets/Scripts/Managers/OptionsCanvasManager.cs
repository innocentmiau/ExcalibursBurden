using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class OptionsCanvasManager : MonoBehaviour
    {

        private CanvasGroup _canvasGroup;
        private TMP_Text _otherTMP;
        private Transform _textsGroup;
        [SerializeField] private GameObject arthurOptionPrefab;

        private MessagesManager _messagesManager;
        private GameManager _gameManager;
        private void Start()
        {
            CheckVariables();
        }

        private bool _canPickOption = false;
        private void Update()
        {
            if (!_canPickOption) return;
            if (Input.GetKeyDown(KeyCode.Alpha1)) ClickButton(1);
            if (Input.GetKeyDown(KeyCode.Alpha2)) ClickButton(2);
            if (Input.GetKeyDown(KeyCode.Alpha3)) ClickButton(3);
            if (Input.GetKeyDown(KeyCode.Alpha4)) ClickButton(4);
        }

        private int _current = 0;
        private void ClickButton(int option)
        {
            _canPickOption = false;
            if (_current == 0) {
                PlayEctorTalk1();
                return;
            }
            if (_current == 1) transform.gameObject.SetActive(false);
        }

        public void PlayEctorTalk0()
        {
            CheckVariables();
            List<string> options = new List<string>();
            options.Add(_messagesManager.GetTextByID(1003));
            UpdateOptions(_messagesManager.GetTextByID(1002), options);
            _current = 0;
        }

        public void PlayEctorTalk1()
        {
            CheckVariables();
            List<string> options = new List<string>();
            options.Add(_messagesManager.GetTextByID(1004));
            UpdateOptions(_messagesManager.GetTextByID(1006), options);
            _current = 1;
        }

        private void CheckVariables()
        {
            if (_canvasGroup == null) _canvasGroup = transform.GetComponent<CanvasGroup>();
            if (_otherTMP == null) _otherTMP = transform.Find("OtherSide").Find("TextPanel").Find("Text").GetComponent<TMP_Text>();
            if (_textsGroup == null) _textsGroup = transform.Find("PlayerSide").Find("TextsGroup");
            
            GameObject managers = GameObject.Find("Managers");
            if (_messagesManager == null) _messagesManager = managers.GetComponent<MessagesManager>();
            if (_gameManager == null) _gameManager = managers.GetComponent<GameManager>();
        }

        private void UpdateOptions(string talk, List<string> options)
        {
            transform.gameObject.SetActive(true);
            ClearCurrentOptions();
            TMP_Text otherTMP = transform.Find("OtherSide").Find("TextPanel").Find("Text").GetComponent<TMP_Text>();
            otherTMP.text = talk;
            ShowOptions(options);
            _canPickOption = true;
        }

        private void ClearCurrentOptions()
        {
            if (_textsGroup.childCount > 0)
            {
                foreach (Transform trans in _textsGroup)
                {
                    Destroy(trans.gameObject);
                }
            }
        }
        
        private void ShowOptions(List<string> options)
        {
            int count = 1;
            foreach (string option in options)
            {
                Transform trans = Instantiate(arthurOptionPrefab, _textsGroup).transform;
                trans.Find("Text").GetComponent<TMP_Text>().text = option;
                trans.Find("KeyPanel").Find("Text").GetComponent<TMP_Text>().text = count.ToString();
                count++;
            }
        }
    }
}