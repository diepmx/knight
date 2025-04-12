using TMPro;
using UnityEngine;

public class HintUI : MonoBehaviour
{
    public static HintUI instance;

    public TMP_Text hintText;

    private void Awake()
    {
        instance = this;
        hintText.text = "";
    }

    public void ShowHint(string message)
    {
        hintText.text = message;
    }

    public void HideHint()
    {
        hintText.text = "";
    }
}
