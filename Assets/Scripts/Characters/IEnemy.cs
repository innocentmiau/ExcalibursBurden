using UnityEngine;

namespace Characters
{
    public interface IEnemy
    {

        GameObject AttackPrefab { get; set; }
        bool Cast();
        float AttackSpeed { get; set; }

    }
}