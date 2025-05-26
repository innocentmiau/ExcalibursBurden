using System;
using UnityEngine;
using UnityEngine.UI;

public class BlinkOnCanvas : MonoBehaviour
{

    [SerializeField] private Image image;
    [SerializeField] private float minValue = 0.5f;
    [SerializeField] private float time = 1f;
    [SerializeField] private float startValue = 1f;

    private Color _color;
    private void Start()
    {
        _color = image.color;
        _elapsed = startValue;
    }

    private float _elapsed = 1f;
    private bool _goDown = true;
    void Update()
    {
        if (_goDown)
        {
            _elapsed -= Time.deltaTime;
            if (_elapsed <= minValue) _goDown = false;
        }
        else
        {
            _elapsed += Time.deltaTime;
            if (_elapsed >= startValue) _goDown = true;
        }

        float value = Mathf.Max(_elapsed / time, minValue);
        value = Mathf.Min(value, startValue);
        _color.a = value;
        image.color = _color;
    }
    
}
