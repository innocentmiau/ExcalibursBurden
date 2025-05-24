using System.Collections;
using System.Numerics;
using Characters;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

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
            float distance = Vector3.Distance(playerOBj.transform.position, transform.position);
            if (distance <= 16f)
            {
                if (playerOBj.TryGetComponent(out HealthSystem healthSystem))
                {
                    healthSystem.TakeDamage(5f);
                }
                if (playerOBj.TryGetComponent(out PlayerManager playerManager))
                {
                    playerManager.TookDamage();
                }
            }
        }

        yield return null;
        Destroy(gameObject);
    }
    
}
