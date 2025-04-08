using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    // Singleton instance của DialogueManager
    public static DialogueManager instance;

    [Space(10)]
    // Animator dùng để xử lý các animation của hộp thoại
    public Animator animator;

    [Space(10)]
    // Image dùng để hiển thị hình ảnh của NPC
    public Image npcImage;

    // Text dùng để hiển thị tên của NPC
    public TMP_Text nameText;

    // Text dùng để hiển thị các câu thoại của NPC
    public TMP_Text dialogueText;

    [Space(10)]
    // Biến boolean kiểm tra xem hộp thoại hiện tại có đang mở hay không
    public bool isDialogueOpen = false;

    [Space(10)]
    // Queue lưu trữ các câu thoại
    private Queue<string> sentences;

    private void Awake()
    {
        // Đảm bảo chỉ có một instance của DialogueManager
        if (instance != null && instance != this)
        {
            Debug.LogWarning("Multiple instances of DialogueManager detected. Destroying the new instance.");
            Destroy(gameObject);
            return;
        }

        // Thiết lập instance duy nhất của DialogueManager
        instance = this;
        DontDestroyOnLoad(gameObject);

        // Khởi tạo Queue lưu các câu thoại
        sentences = new Queue<string>();
    }

    // Bắt đầu một đoạn hội thoại mới
    public void StartDialogue(Dialogue dialogue)
    {
        if (animator == null || npcImage == null || nameText == null || dialogueText == null)
        {
            Debug.LogError("DialogueManager is missing UI components.");
            return;
        }

        isDialogueOpen = true;
        animator.SetBool("isOpen", true); // Bật animation mở hộp thoại

        // Đặt hình ảnh NPC và tên NPC
        npcImage.sprite = dialogue.npcSprite;
        nameText.text = dialogue.name;

        // Xóa các câu thoại cũ nếu có
        sentences.Clear();

        // Thêm tất cả các câu thoại vào Queue
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        // Hiển thị câu thoại đầu tiên
        DisplayNextSentence();
    }

    // Hiển thị câu thoại tiếp theo trong đoạn hội thoại
    public void DisplayNextSentence()
    {
        // Kiểm tra nếu không còn câu thoại nào trong Queue
        if (sentences.Count == 0)
        {
            EndDialogue(); // Kết thúc đoạn hội thoại nếu không còn câu
            return;
        }

        // Lấy câu thoại tiếp theo từ Queue
        string sentence = sentences.Dequeue();
        // Dừng bất kỳ Coroutine gõ chữ nào đang chạy và bắt đầu gõ chữ câu thoại mới
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    // Coroutine gõ từng chữ cái trong câu thoại
    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = ""; // Làm sạch text hiện tại

        // Lặp qua từng chữ cái trong câu
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter; // Thêm chữ cái vào text hiện tại
            // Tốc độ gõ
            yield return new WaitForSeconds(0.05f);
        }
    }

    // Kết thúc đoạn hội thoại
    void EndDialogue()
    {
        isDialogueOpen = false;
        animator.SetBool("isOpen", false); // Bật animation đóng hộp thoại
    }
}
