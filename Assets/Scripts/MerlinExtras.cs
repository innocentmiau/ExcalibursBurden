using UnityEngine;

public class MerlinExtras : MonoBehaviour
{

    private Animator _animator;

    public void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Appear()
    {
        //transform.gameObject.SetActive(true);
        transform.Find("MerlinCanvas").gameObject.SetActive(true);
        _animator.SetBool("shown", true);
    }
    
    public void FullyDisappear() {
        transform.gameObject.SetActive(false);
    }
    
    public void Disappear()
    {
        _animator.SetBool("shown", false);
    }
    
}
