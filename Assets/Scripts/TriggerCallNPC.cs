using UnityEngine;

public class TriggerCallNPC : MonoBehaviour
{

    [SerializeField] private Transform npcTransform;


    public Transform GetNpcTransform() => npcTransform;

}
