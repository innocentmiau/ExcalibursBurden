using ScenesManagers;
using UnityEngine;

public class Florest3Player : MonoBehaviour
{
    
    [SerializeField] private Florest3Scene florest3Scene;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Trigger") && other.gameObject.name.Equals("LeaveTrigger")) florest3Scene.HitNextSceneTrigger();
    }
}
