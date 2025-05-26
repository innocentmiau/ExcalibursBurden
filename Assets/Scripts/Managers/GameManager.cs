using System.Collections;
using System.Linq;
using Important;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    
    public class GameManager : MonoBehaviour
    {
        [System.Serializable]
        public struct Resolution16x9
        {
            public int width;
            public int height;
            public string name;
        
            public Resolution16x9(int w, int h, string n)
            {
                width = w;
                height = h;
                name = n;
            }
        }
    
        private static readonly Resolution16x9[] resolutions16x9 = {
            new Resolution16x9(854, 480, "480p"),
            new Resolution16x9(960, 540, "qHD"),
            new Resolution16x9(1024, 576, "576p"),
            new Resolution16x9(1280, 720, "720p HD"),
            new Resolution16x9(1366, 768, "WXGA"),
            new Resolution16x9(1600, 900, "HD+"),
            new Resolution16x9(1920, 1080, "1080p Full HD"),
            new Resolution16x9(2048, 1152, "2K"),
            new Resolution16x9(2560, 1440, "1440p QHD"),
            new Resolution16x9(3200, 1800, "QHD+"),
            new Resolution16x9(3840, 2160, "4K UHD"),
            new Resolution16x9(5120, 2880, "5K"),
            new Resolution16x9(7680, 4320, "8K UHD")
        };

        [SerializeField] private Transform canvas;
        [Header("Results")]
        private Resolution16x9 bestResolution;
        [SerializeField] private Resolution nativeResolution;
        
        public bool IsGamePaused { get; private set; }
        public void PauseGame()
        {
            IsGamePaused = true;
        }
        public void UnPauseGame()
        {
            IsGamePaused = false;
        }
        
        public float Volume { get; private set; }
        public void UpdateVolume(float value) => Volume = value;
        
        private Data _data;

        private MessagesManager _messagesManager;
        public int SelectedLanguage { get; private set; } = 0;
        public void UpdateSelectedLanguage(int value) => SelectedLanguage = value;
        public bool InstantLoadText { get; private set; } = false;
        public void UpdateInstantLoadText(bool value) => InstantLoadText = value;
        void Start()
        {
            FindAndApplyBest16X9Resolution();
            DontDestroyOnLoad(gameObject);
            _data = new Data();
            UpdateVolume(1f);

            _messagesManager = transform.GetComponent<MessagesManager>();
            
            // load all data from resources here in the future
            // then load main menu scene in async also.
            if (SceneManager.GetActiveScene().name.Equals("Loading")) ChangeScene();
        }

        private void ChangeScene()
        {
            StartCoroutine(LoadMainMenuAsync());
        }

        private IEnumerator LoadMainMenuAsync()
        {
            yield return new WaitForSeconds(0.5f);
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainMenu");
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }

        public void PlayStartMoment()
        {
            StartCoroutine(StartAnimation());
        }

        public void ShowTipBox(string text)
        {
            Transform tbt = canvas.Find("TipBox");
            tbt.gameObject.SetActive(true);
            CanvasGroup cg = tbt.GetComponent<CanvasGroup>();
            TMP_Text tmp = tbt.Find("Text").GetComponent<TMP_Text>();
            tmp.text = text;
            if (_tipBoxDisappear != null) StopCoroutine(_tipBoxDisappear);
            _tipBoxDisappear = StartCoroutine(DisappearingTipBox(cg, 8f, 5f));
        }

        private Coroutine _tipBoxDisappear;

        private IEnumerator DisappearingTipBox(CanvasGroup canvasG, float stay, float fadeout)
        {
            yield return new WaitForSeconds(stay);
            float elapsed = 0f;
            while (elapsed < fadeout)
            {
                yield return null;
                elapsed += Time.deltaTime;
                canvasG.alpha = Mathf.Lerp(1f, 0f, (elapsed / fadeout));
            }
            canvasG.alpha = 0f;
            canvasG.gameObject.SetActive(false);
        }

        // colorful names for npcs? maybe?
        private string FormatTextWithNpcColors(string text)
        {
            if (text.Contains("Ector"))
            {
                text = text.Replace("Ector", "<color=#78db53>Ector</color>");
            }
            return text;
        }
        
        private IEnumerator StartAnimation()
        {
            canvas = GameObject.Find("Canvas").transform;
            Transform allBlack = canvas.Find("AllBlack");
            CanvasGroup cg = allBlack.GetComponent<CanvasGroup>();
            allBlack.gameObject.SetActive(true);
            cg.alpha = 1f;
            TMP_Text tmp = allBlack.Find("Text").GetComponent<TMP_Text>();
            string text = _messagesManager.GetTextByID(1000);
            for (int i = 0; i < text.Length; i++)
            {
                string startText = FormatTextWithNpcColors(text.Substring(0, i));
                string newText = startText + "<color=#00000000>" + text.Substring(i);
                tmp.text = newText;
                yield return new WaitForSeconds(2f/text.Length);
            }
            tmp.text = FormatTextWithNpcColors(text);
            yield return new WaitForSeconds(1f);

            float elapsed = 0;
            while (elapsed < 1.5f)
            {
                cg.alpha = Mathf.Lerp(1f, 0f, (elapsed / 1.5f));
                elapsed += Time.deltaTime;
                yield return null;
            }
            allBlack.gameObject.SetActive(false);
            ShowTipBox(_messagesManager.GetTextByID(1001));
        }
    
        private Resolution16x9 FindBest16X9Resolution()
        {
            nativeResolution = Screen.currentResolution;
            Debug.Log($"Native Resolution: {nativeResolution.width}x{nativeResolution.height} @ {nativeResolution.refreshRate}Hz");
        
            Resolution16x9 best = resolutions16x9[0];
            float bestScore = float.MaxValue;
        
            foreach (var res in resolutions16x9)
            {
                int nativePixels = nativeResolution.width * nativeResolution.height;
                int resPixels = res.width * res.height;
                float pixelDiff = Mathf.Abs(nativePixels - resPixels);
            
                float widthDiff = Mathf.Abs(nativeResolution.width - res.width);
                float heightDiff = Mathf.Abs(nativeResolution.height - res.height);
            
                float score = pixelDiff * 0.7f + (widthDiff + heightDiff) * 0.3f;
            
                if (res.width > nativeResolution.width || res.height > nativeResolution.height)
                {
                    score *= 1.5f; // Penalty for exceeding native resolution
                }
            
                Debug.Log($"{res.name} ({res.width}x{res.height}): Score = {score:F0}");
            
                if (score < bestScore)
                {
                    bestScore = score;
                    best = res;
                }
            }
        
            bestResolution = best;
            Debug.Log($"<color=green>Best 16:9 Resolution: {best.name} ({best.width}x{best.height})</color>");
        
            return best;
        }
    
        private void FindAndApplyBest16X9Resolution()
        {
            Resolution16x9 best = FindBest16X9Resolution();
            ApplyResolution(best);
        }
    
        private void ApplyResolution(Resolution16x9 resolution)
        {
            Screen.SetResolution(resolution.width, resolution.height, true);
            Debug.Log($"Applied Resolution: {resolution.name} ({resolution.width}x{resolution.height})");
        }
    }
    
}

