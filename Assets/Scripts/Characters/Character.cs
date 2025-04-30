using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Scriptable Objects/Character")]
public class Character : ScriptableObject
{

    public string name;
    public bool attackable;
    public float maxHealth;
    public string tag;

}
