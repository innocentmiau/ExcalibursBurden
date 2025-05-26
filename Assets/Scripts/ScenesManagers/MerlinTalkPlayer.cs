using UnityEngine;

namespace ScenesManagers
{
    public class MerlinTalkPlayer : MonoBehaviour
    {

        [SerializeField] private MerlinTalkScene merlinTalkScene;

        private bool _canTalk = false;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E) && _canTalk)
            {
                merlinTalkScene.TalkToMerlin();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Trigger") && other.TryGetComponent(out TriggerCallMerlin triggerCallMerlin))
            {
                _canTalk = true;
                triggerCallMerlin.ShowToInteract();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Trigger") && other.TryGetComponent(out TriggerCallMerlin triggerCallMerlin))
            {
                _canTalk = false;
                triggerCallMerlin.HideToInteract();
            }
        }
    }
}