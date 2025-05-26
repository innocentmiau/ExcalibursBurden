using System;
using System.Collections;
using System.Collections.Generic;
using Characters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        private PlayerManager _playerManager;
        private void Start()
        {
            CheckVariables();
        }

        private bool _canPickOption = false;
        private bool _canSkip = false;
        private bool _skipNextFrame = false;
        private bool _clickedPreviously = false;
        private Coroutine _deleteClickCo;
        private void Update()
        {
            if (_canPickOption)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1)) ClickButton(1);
                if (Input.GetKeyDown(KeyCode.Alpha2)) ClickButton(2);
                if (Input.GetKeyDown(KeyCode.Alpha3)) ClickButton(3);
                if (Input.GetKeyDown(KeyCode.Alpha4)) ClickButton(4);
            }
            
            if (Input.GetKeyDown(KeyCode.Space) && _skipNextFrame == false && _canSkip)
            {
                if (_clickedPreviously == false)
                {
                    _clickedPreviously = true;
                    if (_deleteClickCo != null) StopCoroutine(_deleteClickCo);
                    _deleteClickCo = StartCoroutine(DeletePreviousClick(0.2f));
                }
                else
                {
                    _skipNextFrame = true;
                }
            }
        }
        
        private IEnumerator DeletePreviousClick(float time)
        {
            yield return new WaitForSeconds(time);
            _clickedPreviously = false;
        }

        private NPCManager _latestNpcManager = null;
        public void StartConversation(NPCManager npcManager, int talkingStep)
        {
            CheckVariables();
            _playerManager.SetCanMove(false);
            //List<string> options = new List<string>();
            //options.Add(_messagesManager.GetTextByID(1003));
            (int id, int npcTalk, Dictionary<int, int> answers) = npcManager.GetTalkStuff(talkingStep);
            if (id == -2)
            {
                transform.gameObject.SetActive(false);
                _playerManager.SetCanMove(true);
                return;
            }
            transform.gameObject.SetActive(true);
            _latestNpcManager = npcManager;
            ClearCurrentOptions();
            StartCoroutine(UpdateOptions(id, npcTalk, answers));
        }
        
        private void ClickButton(int option)
        {
            if (_latestNpcManager == null) return;
            Debug.Log("Clicked " + option);
            _canPickOption = false;
            
            StartConversation(_latestNpcManager, _clickingOptions[option-1].ClickedOption);
            /*
            if (_current == 0) {
                PlayEctorTalk1();
                return;
            }
            if (_current == 1) transform.gameObject.SetActive(false);
            */
        }
        
        /*
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
        */

        private void CheckVariables()
        {
            if (_canvasGroup == null) _canvasGroup = transform.GetComponent<CanvasGroup>();
            if (_otherTMP == null) _otherTMP = transform.Find("OtherSide").Find("TextPanel").Find("Text").GetComponent<TMP_Text>();
            if (_textsGroup == null) _textsGroup = transform.Find("PlayerSide").Find("TextsGroup");
            
            GameObject managers = GameObject.Find("Managers");
            if (_messagesManager == null) _messagesManager = managers.GetComponent<MessagesManager>();
            if (_gameManager == null) _gameManager = managers.GetComponent<GameManager>();
            if (_playerManager == null) _playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
        }

        private IEnumerator UpdateOptions(int id, int npcTalk, Dictionary<int, int> answers)
        {
            _canPickOption = false;
            TMP_Text otherTMP = transform.Find("OtherSide").Find("TextPanel").Find("Text").GetComponent<TMP_Text>();
            string text = _messagesManager.GetTextByID(npcTalk);
            if (!_gameManager.InstantLoadText)
            {
                _canSkip = true;
                for (int i = 0; i < text.Length; i++)
                {
                    string newText = text.Substring(0,i) + "<color=#00000000>" + text.Substring(i);
                    otherTMP.text = newText;
                    // Maybe later add delay between words to look more real??? perhaps???
                    //if (text[i].Equals(' ')) yield return new WaitForSeconds(0.1f);
                    if (_skipNextFrame)
                    {
                        _skipNextFrame = false;
                        break;
                    }
                    yield return new WaitForSeconds(0.05f);
                }
                _canSkip = false;
            }
            otherTMP.text = text;
            if (answers == null)
            {
                answers = new Dictionary<int, int>();
                answers.Add(-1, -1);
            }
            ShowOptions(answers);
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

        private List<OptionData> _clickingOptions = new List<OptionData>();
        private void ShowOptions(Dictionary<int, int> options)
        {
            int count = 1;
            _clickingOptions.Clear();
            foreach (int key in options.Keys)
            {
                Transform trans = Instantiate(arthurOptionPrefab, _textsGroup).transform;
                OptionData optionData = trans.gameObject.AddComponent<OptionData>();
                optionData.Setup(options[key]);
                _clickingOptions.Add(optionData);
                trans.Find("Text").GetComponent<TMP_Text>().text = key != -1 ? _messagesManager.GetTextByID(key) : "<color=#e0b275>" + _messagesManager.GetTextByID(8);
                trans.Find("KeyPanel").Find("Text").GetComponent<TMP_Text>().text = count.ToString();
                if (key == -1)
                {
                    if (trans.TryGetComponent(out Image image))
                    {
                        image.color = new Color(0.6f, 0.38f, 0.18f, 0.65f);
                    }
                    if (trans.Find("KeyPanel").TryGetComponent(out Image image2))
                    {
                        image2.color = new Color(0.5f, 0.3f, 0.1f, 1f);
                    }
                }
                count++;
            }
        }
    }
}