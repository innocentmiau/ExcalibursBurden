using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    public class CanvasImageAnimation : MonoBehaviour
    {

        [SerializeField] private float frameTime;
        [SerializeField] private Sprite[] sprites;
        private Image _image;

        private void Awake()
        {
            _image = transform.GetComponent<Image>();
        }

        private void Start()
        {
            StartCoroutine(Animation());
        }

        private IEnumerator Animation()
        {
            int count = 0;
            while (true)
            {
                _image.sprite = sprites[count];
                yield return new WaitForSeconds(frameTime);
                count++;
                if (count >= sprites.Length) count = 0;
            }
        }
    
    }

}