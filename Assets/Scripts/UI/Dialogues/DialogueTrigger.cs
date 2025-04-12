using UnityEngine;
using TMPro;

public class DialogueTrigger : MonoBehaviour
{
    // Đối tượng Dialogue chứa thông tin về đoạn hội thoại
    public Dialogue dialogue;

    // Biến boolean để kiểm tra xem người chơi có trong phạm vi hay không
    public bool isInRange;

    // Gọi khi collider khác đi vào vùng trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInRange = true;

            // ✅ Luôn hiện hint khi lại gần, bất kể hộp thoại đang mở hay không
            HintUI.instance.ShowHint("Nhấn [E] để trò chuyện");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInRange = false;
            HintUI.instance.HideHint();
        }
    }

    private void Update()
    {
        if (isInRange && Input.GetKeyDown(KeyCode.E))
        {
            TriggerDialogue();

            // ✅ Đổi hint khi bắt đầu thoại
            HintUI.instance.ShowHint("Nhấn [Space] để tiếp tục");
        }

        // ✅ Nếu thoại kết thúc và vẫn còn trong vùng, hiện lại hint E
        if (isInRange && !DialogueManager.instance.isDialogueOpen)
        {
            HintUI.instance.ShowHint("Nhấn [E] để trò chuyện");
        }
    }

    // Kích hoạt đoạn hội thoại
    public void TriggerDialogue()
    {
        DialogueManager.instance.StartDialogue(dialogue); // Gọi phương thức StartDialogue để bắt đầu hội thoại
    }
    

}
