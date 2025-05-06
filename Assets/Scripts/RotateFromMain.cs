using UnityEngine;

public class RotateFromMain : MonoBehaviour
{

    [SerializeField] private Transform trans;
    [SerializeField] private RectTransform rectt;
    
    void Update()
    {
        rectt.localRotation = trans.rotation;
    }
}
