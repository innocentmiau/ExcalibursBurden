using System.Collections;
using UnityEngine;

public class WalkingBehindEffect : MonoBehaviour
{

    [SerializeField] private SpriteRenderer spriteToEffect;
    [SerializeField] private float time = 0.5f;

    public void PlayerIsBehind()
    {
        if (_appeCoro != null) StopCoroutine(_appeCoro);
        //spriteToEffect.color = new Color(1f, 1f, 1f, 0.75f);
        _disaCoro = StartCoroutine(Disappearing(0.75f));
    }

    private Coroutine _disaCoro;
    private IEnumerator Disappearing(float value)
    {
        float elapsed = 0;
        Color c = spriteToEffect.color;
        float startValue = c.a;
        while (elapsed < time)
        {
            float alpha = Mathf.Lerp(startValue, value, (elapsed / time));
            c.a = alpha;
            spriteToEffect.color = c;
            elapsed += Time.deltaTime;
            yield return null;
        }
        c.a = value;
        spriteToEffect.color = c;
    }
    
    public void PlayerIsNotBehind()
    {
        if (_disaCoro != null) StopCoroutine(_disaCoro);
        //spriteToEffect.color = new Color(1f, 1f, 1f, 1f);
        _appeCoro = StartCoroutine(Appearing(1f));
    }

    private Coroutine _appeCoro;
    private IEnumerator Appearing(float value)
    {
        float elapsed = 0;
        Color c = spriteToEffect.color;
        float startValue = c.a;
        while (elapsed < time)
        {
            float alpha = Mathf.Lerp(startValue, value, (elapsed / time));
            c.a = alpha;
            spriteToEffect.color = c;
            elapsed += Time.deltaTime;
            yield return null;
        }
        c.a = value;
        spriteToEffect.color = c;
    }

}
