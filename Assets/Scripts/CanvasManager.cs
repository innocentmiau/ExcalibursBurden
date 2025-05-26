using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{

    [SerializeField] private RectTransform normalAttackPanel;
    [SerializeField] private RectTransform defensePanel;

    private void Start()
    {
        StartCoroutine(NormalAttackCooldown(0f));
        StartCoroutine(DefenseCooldown(0f));
    }

    public void StartCooldownDefense(float cooldown)
    {
        StartCoroutine(DefenseCooldown(cooldown));
    }
    
    public void StartCooldownNormalAttack(float cooldown)
    {
        StartCoroutine(NormalAttackCooldown(cooldown));
    }

    private IEnumerator NormalAttackCooldown(float cooldown)
    {
        Image imageIcon = normalAttackPanel.Find("Image").GetComponent<Image>();
        Image imageFill = normalAttackPanel.Find("Fill").GetComponent<Image>();
        TMP_Text tmp = normalAttackPanel.Find("Text").GetComponent<TMP_Text>();
        imageIcon.color = new Color(1f, 1f, 1f, 0.25f);
        imageFill.fillAmount = 0f;
        float elapsed = 0f;
        while (elapsed < cooldown)
        {
            float v = (elapsed / cooldown);
            tmp.text = $"{(cooldown-elapsed):F1}s";
            imageFill.fillAmount = v;
            yield return null;
            elapsed += Time.deltaTime;
        }
        imageIcon.color = new Color(1f, 1f, 1f, 1f);
        imageFill.fillAmount = 1f;
        tmp.text = "";
    }

    private IEnumerator DefenseCooldown(float cooldown)
    {
        Image imageIcon = defensePanel.Find("Image").GetComponent<Image>();
        Image imageFill = defensePanel.Find("Fill").GetComponent<Image>();
        TMP_Text tmp = defensePanel.Find("Text").GetComponent<TMP_Text>();
        imageIcon.color = new Color(1f, 1f, 1f, 0.25f);
        imageFill.fillAmount = 0f;
        float elapsed = 0f;
        while (elapsed < cooldown)
        {
            float v = (elapsed / cooldown);
            tmp.text = $"{(cooldown-elapsed):F1}s";
            imageFill.fillAmount = v;
            yield return null;
            elapsed += Time.deltaTime;
        }
        imageIcon.color = new Color(1f, 1f, 1f, 1f);
        imageFill.fillAmount = 1f;
        tmp.text = "";
    }
    
}
