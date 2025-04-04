using UnityEngine;

// Đánh dấu lớp là Serializable để có thể chỉnh sửa trực tiếp trong Inspector của Unity
[System.Serializable]
public class Dialogue
{
    [Space(10)]
    // Sprite của NPC liên quan đến đoạn hội thoại
    // Đây là hình ảnh của NPC sẽ hiển thị khi NPC nói
    public Sprite npcSprite;

    [Space(10)]
    // Tên của NPC đang nói
    // Tên này sẽ được hiển thị khi NPC nói trong hội thoại
    public string name;

    [Space(10)]
    // Các câu thoại mà NPC sẽ nói
    // Các câu này có thể được nhập và chỉnh sửa trong Inspector
    [TextArea(3, 10)] // Thẻ TextArea giúp hiển thị vùng nhập văn bản nhiều dòng với tối thiểu 3 dòng và tối đa 10 dòng
    public string[] sentences;
}
