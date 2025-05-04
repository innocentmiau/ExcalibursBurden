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
            _canvasGroup = transform.GetComponent<CanvasGroup>();
            _otherTMP = transform.Find("OtherSide").Find("TextPanel").Find("Text").GetComponent<TMP_Text>();
            _textsGroup = transform.Find("PlayerSide").Find("TextsGroup");

            GameObject managers = GameObject.Find("Managers");
            _messagesManager = managers.GetComponent<MessagesManager>();
            _gameManager = managers.GetComponent<GameManager>();
            
            Invoke("UpdateOptions", 1f);
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

        private void ClickButton(int option)
        {
            _canPickOption = false;
        }

        private void UpdateOptions()
        {
            ClearCurrentOptions();
            List<string> options = new List<string>();
            options.Add(_messagesManager.GetTextByID(1000));
            options.Add(_messagesManager.GetTextByID(1001));
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