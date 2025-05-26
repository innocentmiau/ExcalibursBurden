using System.Collections;
using Characters;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScenesManagers
{
    public class Florest3Scene : MonoBehaviour
    {
        
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private RuntimeAnimatorController newPlayerAnimator;
        [SerializeField] private SpriteRenderer renderToChange;
        [SerializeField] private Sprite spriteChangeTo;
        [SerializeField] private GameObject enableAfter;
        [SerializeField] private GameObject takeOut;
        [SerializeField] private MerlinExtras merlinExtras;
        [SerializeField] private OptionsCanvasManager optionsCanvasManager;
        [SerializeField] private OptionsCanvasManager optionsCanvasManager2;
        [SerializeField] private NPCManager ectorNpc;
        [SerializeField] private Transform canvas;
        [SerializeField] private AudioSource backgroundSource;
        
        private void Start()
        {
            playerManager.SetCanMove(true);
        }

        public void TakeOutExtra()
        {
            if (takeOut.activeSelf) takeOut.SetActive(false);
        }
        
        public void ArthurPulledSword()
        {
            renderToChange.sprite = spriteChangeTo;
            playerManager.transform.GetComponent<Animator>().runtimeAnimatorController = newPlayerAnimator;
            TalkingToEctor(ectorNpc);
            //merlinExtras.Appear();
            //TalkingToNpc(merlinExtras.GetComponent<NPCManager>());
        }
    
        public void HitNextSceneTrigger()
        {
            playerManager.SetCanMove(false);
            StartCoroutine(EndAnimation());
        }

        public void BeginMerlin()
        {
            playerManager.SetCanMove(false);
            merlinExtras.Appear();
            StartCoroutine(TalkToMerlin());
        }

        public void EndMerlin()
        {
            enableAfter.SetActive(true);
            merlinExtras.Disappear();
        }

        private IEnumerator TalkToMerlin()
        {
            yield return new WaitForSeconds(1f);
            TalkTo2(merlinExtras.GetComponent<NPCManager>(), 0);
        }
        
        private void TalkTo2(NPCManager talkingNpc, int currentStep)
        {
            //if (option == 0) optionsCanvasManager.PlayEctorTalk0();
            optionsCanvasManager2.StartConversation(talkingNpc, currentStep);
        }
        
        public bool AlreadyTalkedEctor { get; private set; } = false;
        public void TalkingToEctor(NPCManager talkingNpc)
        {
            AlreadyTalkedEctor = true;
            TalkTo(talkingNpc, 5);
        }
    
        private void TalkTo(NPCManager talkingNpc, int currentStep)
        {
            //if (option == 0) optionsCanvasManager.PlayEctorTalk0();
            optionsCanvasManager.StartConversation(talkingNpc, currentStep);
        }

        
        private IEnumerator EndAnimation()
        {
            Transform allBlack = canvas.Find("AllBlack");
            allBlack.gameObject.SetActive(true);
            CanvasGroup cg = allBlack.GetComponent<CanvasGroup>();
            float elapsed = 0;
            cg.alpha = 0f;
            float startValue = backgroundSource.volume;
            while (elapsed < 1.5f)
            {
                backgroundSource.volume = Mathf.Lerp(startValue, 0f, (elapsed / 1.5f));
                cg.alpha = Mathf.Lerp(0f, 1f, (elapsed / 1.5f));
                elapsed += Time.deltaTime;
                yield return null;
            }
            cg.alpha = 1f;
            SceneManager.LoadSceneAsync("Merlin_Talk");
        }
        
    }
}