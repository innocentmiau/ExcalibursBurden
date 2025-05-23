using System;
using UnityEngine;

public class House1Player : MonoBehaviour
{

    [SerializeField] private House1Scene house1Scene;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Trigger") && other.gameObject.name.Equals("LeaveTrigger")) house1Scene.HitNextSceneTrigger();
    }
}
