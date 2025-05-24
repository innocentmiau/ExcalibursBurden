using System;
using System.Collections.Generic;
using Important;
using UnityEngine;

namespace Managers
{
    public class MessagesManager : MonoBehaviour
    {

        [SerializeField] private Languages selectedLanguage = Languages.ENGLISH;
        
        private Dictionary<string, string> _textDefaultLanguage;
        private Dictionary<string, string> _textInCurrentLanguage;

        public bool IsSetup()
        {
            if (_textInCurrentLanguage == null || _textDefaultLanguage == null) return false;
            return _textDefaultLanguage.Count > 0 && _textInCurrentLanguage.Count > 0;
        }

        public string GetTextByID(int id, bool defaultLanguage = false)
        {
            if (defaultLanguage) return _textDefaultLanguage.GetValueOrDefault(id.ToString(), null);
            string text = _textInCurrentLanguage.GetValueOrDefault(id.ToString(), null);
            if (text == null)
            {
                string defaultText = _textDefaultLanguage.GetValueOrDefault(id.ToString(), null);
                if (defaultText == null) return GetText(TextHex.NOT_FOUND, true);
                return defaultText;
            }
            return text;
        }

        private string AddEmojis(string text)
        {
            foreach (string s in _emojis.Keys)
            {
                if (text.Contains(s)) text = text.Replace(s, _emojis[s]);
            }
            return text;
        }
        
        public string GetText(TextHex pickText, bool defaultLanguage = false)
        {
            if (defaultLanguage) return _textDefaultLanguage.GetValueOrDefault(((int)pickText).ToString(), null);
            string text = _textInCurrentLanguage.GetValueOrDefault(((int)pickText).ToString(), null);
            if (text == null)
            {
                string defaultText = _textDefaultLanguage.GetValueOrDefault(((int)pickText).ToString(), null);
                if (defaultText == null) return GetText(TextHex.NOT_FOUND, true);
                return defaultText;
            }
            return text;
        }

        private Dictionary<string, string> _emojis;
        private void Start()
        {
            // load default language
            _textDefaultLanguage = new Dictionary<string, string>();
            TextAsset jsonData = Resources.Load<TextAsset>("Messages/english");
            if (jsonData == null)
            {
                Debug.LogWarning("Couldn't find the default language file, why? idk, maybe fix.");
                return;
            }
            LanguageMessages data = JsonUtility.FromJson<LanguageMessages>(jsonData.text);
            if (data == null)
            {
                Debug.LogError("Failed to parse JSON or phrases dictionary is null");
                return;
            }
            
            _textDefaultLanguage = new Dictionary<string, string>();
            foreach (string text in data.texts)
            {
                string[] split = text.Split(" ");
                _textDefaultLanguage.Add(split[0], text.Substring(split[0].Length + 1));
            }
            
            SetupEmojis();
            
            //LoadNewLanguage("portuguese");
            LoadNewLanguage(selectedLanguage.ToString().ToLower());
        }

        private void SetupEmojis()
        {
            _emojis = new Dictionary<string, string>();
            _emojis.Add("🔥", "<sprite=0>");
            _emojis.Add("💅", "<sprite=1>");
            _emojis.Add("⚙️", "<sprite=2>");
            _emojis.Add("🎥", "<sprite=3>");
            _emojis.Add("⏳", "<sprite=4>");
            _emojis.Add("‼️", "<sprite=5>");
            _emojis.Add("💯", "<sprite=6>");
            _emojis.Add("💨", "<sprite=7>");
            _emojis.Add("👑", "<sprite=8>");
            _emojis.Add("🕺", "<sprite=9>");
            _emojis.Add("🗿", "<sprite=10>");
            _emojis.Add("😤", "<sprite=11>");
            _emojis.Add("💀", "<sprite=12>");
        }
        
        public void LoadNewLanguage(string language = "english")
        {
            TextAsset jsonData = Resources.Load<TextAsset>("Messages/" + language);
            if (jsonData == null)
            {
                Debug.LogWarning("Couldn't find the desired language file, why? idk, maybe fix.");
                return;
            }
            LanguageMessages data = JsonUtility.FromJson<LanguageMessages>(jsonData.text);
            if (data == null)
            {
                Debug.LogError("Failed to parse JSON or phrases dictionary is null");
                return;
            }

            selectedLanguage = (Languages)Enum.Parse(typeof(Languages), language.ToUpper());
            _textInCurrentLanguage = new Dictionary<string, string>();
            foreach (string text in data.texts)
            {
                string[] split = text.Split(" ");
                string _text = text.Substring(split[0].Length + 1);
                foreach (string s in _emojis.Keys)
                {
                    if (_text.Contains(s)) _text = _text.Replace(s, _emojis[s]);
                }
                _textInCurrentLanguage.Add(split[0], _text);
            }

            foreach (string key in _textInCurrentLanguage.Keys)
            {
                Debug.Log($"Test '{key}': {_textInCurrentLanguage[key]}");
            }
        }
    }
}