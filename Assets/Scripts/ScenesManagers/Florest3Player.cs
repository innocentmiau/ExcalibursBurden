using ScenesManagers;
using UnityEngine;
using UnityEngine.UI;

public class Florest3Player : MonoBehaviour
{
    
    [SerializeField] private Florest3Scene florest3Scene;
    [SerializeField] private RectTransform showAtFirst;
    [SerializeField] private Image fillBar;
    [SerializeField] private float timeToComplete = 2f;

    private float _clickedFor = 0f;
    
    private TriggerGetSword _triggerGetSword;
    private void Update()
    {
        if (!_canPullSword || _hasPulledSword) return;
        if (Input.GetKey(KeyCode.E))
        {
            _clickedFor += Time.deltaTime;
            if (_clickedFor >= timeToComplete)
            {
                _hasPulledSword = true;
                florest3Scene.ArthurPulledSword();
                _triggerGetSword.HideToInteract();
                showAtFirst.gameObject.SetActive(false);
            }
        }
        else
        {
            if (_clickedFor > 0f) _clickedFor -= Time.deltaTime;
        }

        UpdateBar();
        /*
        if (Input.GetKeyDown(KeyCode.E))
        {
            _hasPulledSword = true;
            florest3Scene.ArthurPulledSword();
            _triggerGetSword.HideToInteract();
        }*/
    }

    private void UpdateBar()
    {
        float value = Mathf.Max(_clickedFor / timeToComplete, 0f);
        fillBar.fillAmount = Mathf.Min(value, 1f);
    }

    private bool _hasPulledSword = false;
    private bool _canPullSword = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Trigger") && other.TryGetComponent(out TriggerGetSword triggerGetSword))
        {
            if (!showAtFirst.gameObject.activeSelf) showAtFirst.gameObject.SetActive(true);
            florest3Scene.TakeOutExtra();
            if (_hasPulledSword) return;
            triggerGetSword.ShowToInteract();
            _triggerGetSword = triggerGetSword;
            _canPullSword = true;
        }
        if (other.CompareTag("Trigger") && other.gameObject.name.Equals("LeaveTrigger")) florest3Scene.HitNextSceneTrigger();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Trigger") && other.TryGetComponent(out TriggerGetSword triggerGetSword))
        {
            _clickedFor = 0f;
            triggerGetSword.HideToInteract();
            _canPullSword = false;
            showAtFirst.gameObject.SetActive(false);
        }
    }
}
