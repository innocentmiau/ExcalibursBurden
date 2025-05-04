using System.Collections;
using Characters;
using UnityEngine;
using UnityEngine.UI;

public class LancelotSkill1 : MonoBehaviour
{

    [SerializeField] private float timeActivate = 2f;

    private Image _fillBar;
    private SkillTrigger _skillTrigger;
    private void Start()
    {
        _skillTrigger = gameObject.AddComponent<SkillTrigger>();
        _fillBar = transform.Find("Canvas").Find("LoadingBar").Find("Fill").GetComponent<Image>();
        StartCoroutine(ActivateSkill());
    }

    private IEnumerator ActivateSkill()
    {
        float elapsed = 0f;
        while (elapsed < timeActivate)
        {
            elapsed += Time.deltaTime;
            _fillBar.fillAmount = (elapsed / timeActivate);
            yield return null;
        }
        if (_skillTrigger.IsPlayerInTrigger() && _skillTrigger.GetPlayerObject() != null)
        {
            GameObject playerOBj = _skillTrigger.GetPlayerObject();
            if (playerOBj.TryGetComponent(out HealthSystem healthSystem))
            {
                healthSystem.TakeDamage(5f);
            }
        }

        yield return null;
        Destroy(gameObject);
    }
    
}
