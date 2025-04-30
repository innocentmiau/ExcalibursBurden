using System;
using System.Collections.Generic;
using Important;
using UnityEngine;

namespace Managers
{
    public class MessagesManager : MonoBehaviour
    {

        private Dictionary<string, string> _textInCurrentLanguage;

        public string GetText(TextHex pickText)
        {
            string text = _textInCurrentLanguage.GetValueOrDefault(((int)pickText).ToString(), null);
            if (text == null) return GetText(TextHex.NOT_FOUND);
            return text;
        }
        
        private void Start()
        {
            LoadNewLanguage();
        }

        public void LoadNewLanguage(string language = "default")
        {
            TextAsset jsonData = Resources.Load<TextAsset>("Messages/" + language);
            if (jsonData == null)
            {
                jsonData = Resources.Load<TextAsset>("Messages/default");
                if (jsonData == null)
                {
                    Debug.LogWarning("Couldn't find the desired language file, why? idk, maybe fix.");
                    return;
                }
            }
            LanguageMessages data = JsonUtility.FromJson<LanguageMessages>(jsonData.text);
            if (data == null)
            {
                Debug.LogError("Failed to parse JSON or phrases dictionary is null");
                return;
            }
            
            _textInCurrentLanguage = new Dictionary<string, string>();
            foreach (string text in data.texts)
            {
                string[] split = text.Split(" ");
                _textInCurrentLanguage.Add(split[0], text.Substring(split[0].Length + 1));
            }

            foreach (string key in _textInCurrentLanguage.Keys)
            {
                Debug.Log($"Test '{key}': {_textInCurrentLanguage[key]}");
            }
        }
    }
}