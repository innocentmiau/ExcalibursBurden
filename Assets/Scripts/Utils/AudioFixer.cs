using System;
using System.Collections;
using Managers;
using UnityEngine;

public class AudioFixer : MonoBehaviour
{

    private AudioSource _audioSource;
    
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        StartCoroutine(Setup());
    }

    private GameManager _gameManager;
    private IEnumerator Setup()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            try
            {
                _gameManager = GameObject.Find("Managers").GetComponent<GameManager>();
                if (_gameManager != null) break;
            }
            catch (Exception e)
            {
                break;
            }
        }

        float volume = _gameManager != null ? _gameManager.Volume : 1f;
        _audioSource.volume = volume;
        _audioSource.Play();
    }
}
