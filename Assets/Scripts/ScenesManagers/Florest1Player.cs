using UnityEngine;

public class Florest1Player : MonoBehaviour
{
    [SerializeField] private Florest1Scene florest1Scene;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Trigger") && other.gameObject.name.Equals("LeaveTrigger")) florest1Scene.HitNextSceneTrigger();
    }
}
