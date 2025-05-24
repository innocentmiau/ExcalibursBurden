using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{

    [SerializeField] private RectTransform normalAttackPanel;

    private void Start()
    {
        StartCoroutine(NormalAttackCooldown(0f));
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
    
}
