using UnityEngine;

public class OptionData : MonoBehaviour
{

    public int ClickedOption { get; private set; }

    public void Setup(int option)
    {
        ClickedOption = option;
    }

}
