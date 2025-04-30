using System;
using UnityEngine;

namespace Characters
{
    
    public class NPCManager : MonoBehaviour
    {

        [SerializeField] private Character npcObject;
        private HealthSystem _healthSystem;
        
        private void Start()
        {
            gameObject.tag = npcObject.tag;
            if (npcObject.attackable)
            {
                _healthSystem = GetComponent<HealthSystem>();
                if (_healthSystem != null) _healthSystem.Setup(npcObject);
            }
            
        }

        private float _lastAttacked = 0f;
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (!collider.tag.Equals("Weapon") || _healthSystem == null) return;
            if (collider.TryGetComponent(out SwordManager swordManager) && swordManager.StartAttack > _lastAttacked)
            {
                _lastAttacked = swordManager.StartAttack;
                _healthSystem.TakeDamage(swordManager.Damage);
            }
        }
    }
    
}
