using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Important;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class GlobalStuff : MonoBehaviour
    {

        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private Transform canvasTransform;
        [SerializeField] private TMP_Dropdown languagesDropdown;
        [SerializeField] private Slider volumeSlider;
        [SerializeField] private TMP_Text volumeTMP;
        [SerializeField] private AudioSource backgroundSource;

        private void Start()
        {
            canvasTransform.gameObject.SetActive(false);
            FixOptionsMenu();
        }

        public void PressedEsc()
        {
            canvasTransform.gameObject.SetActive(!canvasTransform.gameObject.activeSelf);
            playerManager.SetCanMove(!canvasTransform.gameObject.activeSelf);
            if (canvasTransform.gameObject.activeSelf) FixOptionsMenu();
        }

        public void FixOptionsMenu()
        {
            if (backgroundSource == null)
            {
                foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Music"))
                {
                    if (obj.name.Contains("BackgroundMusic"))
                    {
                        backgroundSource = obj.GetComponent<AudioSource>();
                        break;
                    }
                }
            }
            
            languagesDropdown.options.Clear();
            foreach (Languages language in Enum.GetValues(typeof(Languages)))
            {
                string lanName = language.ToString();
                string[] lanData = lanName.Split("_");
                List<string> texts = new List<string>();
                foreach (string s in lanData)
                {
                    texts.Add(s.Substring(0, 1).ToUpper() + s.Substring(1).ToLower());
                }

                languagesDropdown.options.Add(new TMP_Dropdown.OptionData(string.Join(" ", texts), null, Color.white));
            }
            
            int selectedLan = 0;
            try
            {
                GameManager man = GameObject.Find("Managers").GetComponent<GameManager>();
                volumeSlider.value = man.Volume;
                selectedLan = man.SelectedLanguage;
            }
            catch (Exception e)
            {
                volumeSlider.value = 1f;
            }

            
            languagesDropdown.value = selectedLan;
            languagesDropdown.RefreshShownValue();
        }

        public void ClickCloseOptionsMenuButton()
        {
            canvasTransform.gameObject.SetActive(false);
        }

        private Coroutine _updatingVolumeCoro;
        public void UpdatedVolumeSlider()
        {
            volumeTMP.text = $"{volumeSlider.value:F2}";
            if (_updatingVolumeCoro != null) StopCoroutine(_updatingVolumeCoro);
            _updatingVolumeCoro = StartCoroutine(UpdatingVolume());
        }

        private IEnumerator UpdatingVolume()
        {
            yield return new WaitForSeconds(0.1f);
            float value = volumeSlider.value;
            foreach (AudioSource eachSource in FindObjectsByType<AudioSource>(FindObjectsSortMode.None))
            {
                eachSource.volume = value;
            }
            try
            {
                GameManager man = GameObject.Find("Managers").GetComponent<GameManager>();
                man.UpdateVolume(value);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        public void UpdateLanguageUpdate()
        {
            try
            {
                GameObject mana = GameObject.Find("Managers");
                MessagesManager man = mana.GetComponent<MessagesManager>();
                List<Languages> lan = Enum.GetValues(typeof(Languages)).Cast<Languages>().ToList();
                string languageName = lan[languagesDropdown.value].ToString().ToLower();
                languageName = languageName.Replace(" ", "_");
                man.LoadNewLanguage(languageName);
                foreach (TMPLanguageText eachTxt in FindObjectsByType<TMPLanguageText>(FindObjectsSortMode.None))
                {
                    eachTxt.RefreshText();
                }
                GameManager gameManager = mana.GetComponent<GameManager>();
                gameManager.UpdateSelectedLanguage(languagesDropdown.value);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
        
    }
}