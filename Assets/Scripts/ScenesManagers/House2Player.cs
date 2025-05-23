using UnityEngine;

public class House2Player : MonoBehaviour
{
    
    [SerializeField] private House2Scene house2Scene;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Trigger") && other.gameObject.name.Equals("LeaveTrigger")) house2Scene.HitNextSceneTrigger();
    }
}
