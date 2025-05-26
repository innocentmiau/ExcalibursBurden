using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Important;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Managers
{
    
    public class MainMenuManager : MonoBehaviour
    {

        [SerializeField] private Transform canvas;
        [SerializeField] private RectTransform optionsMenu;
        [SerializeField] private TMP_Dropdown languagesDropdown;
        [SerializeField] private Slider volumeSlider;
        [SerializeField] private TMP_Text volumeTMP;
        [SerializeField] private AudioSource backgroundSource;
        [SerializeField] private Toggle toggleButton;
        [SerializeField] private Image toggleBackground;
        [SerializeField] private Color toggleDisabled;
        [SerializeField] private Color toggleEnabled;

        private void Start()
        {
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
            
            optionsMenu.gameObject.SetActive(false);

        }

        public void ClickButtonPlay()
        {
            canvas.Find("Panel").gameObject.SetActive(false);
            StartCoroutine(LoadGameAsync());
        }

        private IEnumerator LoadGameAsync()
        {
            float elapsed = 0.5f;
            float startValue = backgroundSource.volume;
            while (true)
            {
                backgroundSource.volume = (startValue * (elapsed * 2));
                elapsed -= Time.deltaTime;
                yield return null;
                if (elapsed <= 0) break;
            }
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("House_1");
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }

        public void ClickQuitButton()
        {
            Application.Quit();
        }

        public void ClickCloseOptionsMenuButton()
        {
            optionsMenu.gameObject.SetActive(false);
        }

        public void ClickButtonOptions()
        {
            optionsMenu.gameObject.SetActive(true);
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

        public void UpdateToggleInstantText()
        {
            Color c = toggleButton.isOn ? toggleEnabled : toggleDisabled;
            toggleBackground.color = c;
            GameManager man = GameObject.Find("Managers").GetComponent<GameManager>();
            man.UpdateInstantLoadText(toggleButton.isOn);
        }
        
    }

}
