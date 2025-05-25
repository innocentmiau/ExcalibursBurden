using UnityEngine;

public class Florest2Player : MonoBehaviour
{
    
    [SerializeField] private Florest2Scene florest2Scene;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Trigger") && other.gameObject.name.Equals("LeaveTrigger")) florest2Scene.HitNextSceneTrigger();
    }
}
