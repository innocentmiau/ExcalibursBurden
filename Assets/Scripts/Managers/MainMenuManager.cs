using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Important;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    
    public class MainMenuManager : MonoBehaviour
    {

        [SerializeField] private Transform canvas;
        [SerializeField] private RectTransform optionsMenu;
        [SerializeField] private TMP_Dropdown languagesDropdown;

        private void Start()
        {
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

            languagesDropdown.value = 0;
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
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("House_1");
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }

        public void ClickCloseOptionsMenuButton()
        {
            optionsMenu.gameObject.SetActive(false);
        }

        public void ClickButtonOptions()
        {
            optionsMenu.gameObject.SetActive(true);
        }

        public void UpdateLanguageUpdate()
        {
            try
            {
                MessagesManager man = GameObject.Find("Managers").GetComponent<MessagesManager>();
                List<Languages> lan = Enum.GetValues(typeof(Languages)).Cast<Languages>().ToList();
                string languageName = lan[languagesDropdown.value].ToString().ToLower();
                languageName = languageName.Replace(" ", "_");
                man.LoadNewLanguage(languageName);
                foreach (TMPLanguageText eachTxt in FindObjectsByType<TMPLanguageText>(FindObjectsSortMode.None))
                {
                    eachTxt.RefreshText();
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
        
    }

}
