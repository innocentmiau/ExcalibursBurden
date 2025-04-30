using UnityEngine;

public class SwordManager : MonoBehaviour
{

    public bool IsAttacking { get; private set; }
    public float Damage { get; private set; }
    private Collider2D _collider2D;
    
    private void Start()
    {
        _collider2D = GetComponent<BoxCollider2D>();
        if (_collider2D == null)
        {
            this.enabled = false;
            return;
        }
        StopAttacking();
    }

    public float StartAttack { get; private set; }
    public void SetAttacking(float damage)
    {
        _collider2D.enabled = true;
        Damage = damage;
        IsAttacking = true;
        StartAttack = Time.time;
        // for tests purposes
        GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 1f);
    }

    public void StopAttacking()
    {
        _collider2D.enabled = false;
        Damage = 0f;
        IsAttacking = false;
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
    }
}