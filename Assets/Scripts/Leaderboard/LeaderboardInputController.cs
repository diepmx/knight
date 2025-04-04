using TMPro;
using UnityEngine;

public class LeaderboardInputController : MonoBehaviour
{
    [Space(10)]
    // Tham chiếu đến TMP_InputField để nhập tên người chơi
    public TMP_InputField inputField;
    // Giới hạn số ký tự tối đa trong trường nhập liệu
    public int maxCharacters = 10;

    void Start()
    {
        // Đặt giới hạn số ký tự cho trường nhập liệu
        inputField.characterLimit = maxCharacters;

        // Thêm listener để phát hiện thay đổi trong văn bản của trường nhập liệu và loại bỏ khoảng trắng
        inputField.onValueChanged.AddListener(delegate { RemoveSpaces(); });
    }

    // Hàm loại bỏ khoảng trắng khỏi văn bản trong trường nhập liệu
    void RemoveSpaces()
    {
        // Thay thế tất cả các khoảng trắng trong văn bản nhập liệu bằng chuỗi rỗng
        inputField.text = inputField.text.Replace(" ", "");
    }
}
