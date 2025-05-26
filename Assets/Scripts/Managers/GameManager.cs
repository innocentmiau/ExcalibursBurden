using System;
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
        public Resolution16x9[] GetResolutions() => resolutions16x9;

        [SerializeField] private Transform canvas;
        [Header("Results")]
        private Resolution16x9 bestResolution;
        public Resolution16x9 GetBestResolution() => bestResolution;
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
        
        public Resolution16x9 SelectedResolution { get; private set; }
        public void UpdateResolution(Resolution16x9 resolution16X9)
        {
            SelectedResolution = resolution16X9;
            Screen.SetResolution(resolution16X9.width, resolution16X9.height, true);
            Debug.Log($"Applied Resolution: {resolution16X9.name} ({resolution16X9.width}x{resolution16X9.height})");
        }
        public float Volume { get; private set; }
        public void UpdateVolume(float value) => Volume = value;
        
        private Data _data;
        private bool _isResolutionDone = false;

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

        private void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift))
            {
                HandleAdmin();
            }
        }

        private void HandleAdmin() 
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                int current = SceneManager.GetActiveScene().buildIndex;
                Debug.Log("Current scene: " + current);
                if (current > 1)
                {
                    SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex - 1);
                }
                // behind scene
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                // next scene
                int current = SceneManager.GetActiveScene().buildIndex;
                Debug.Log("Current scene: " + current);
                if (current < 9)
                {
                    SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
                }
            }
        }

        private void ChangeScene()
        {
            StartCoroutine(LoadMainMenuAsync());
        }

        private IEnumerator LoadMainMenuAsync()
        {
            yield return new WaitForSeconds(0.5f);
            while (!_isResolutionDone)
            {
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(0.5f);
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainMenu");
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
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
            
                if (res.width > nativeResolution.width || res.height > nativeResolution.height) score *= 1.5f;
            
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
            _isResolutionDone = true;
        }
    
        private void ApplyResolution(Resolution16x9 resolution)
        {
            UpdateResolution(resolution);
        }
    }
    
}

