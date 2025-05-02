using UnityEngine;

namespace Characters
{
    public class LancelotManager : MonoBehaviour, IEnemy
    {
        
        public GameObject AttackPrefab { get; set; }
        public bool Cast()
        {
            return false;
        }
        public float AttackSpeed { get; set; }
    }
}