using UnityEngine;
using TMPro;

public class CharacterSelectHintUI : MonoBehaviour
{
    public static CharacterSelectHintUI instance;

    public TMP_Text hintText;

    private void Awake()
    {
        instance = this;
        HideHint(); // Ẩn ban đầu
    }

    public void ShowHint()
    {
        hintText.text = "Nhấn [E] để đổi nhân vật";
    }

    public void HideHint()
    {
        hintText.text = "";
    }
}
