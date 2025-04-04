using TMPro;
using UnityEngine;

public class LeaderboardController : MonoBehaviour
{
    [Space(10)]
    // Tham chiếu đến TMP_InputField để nhập tên người chơi
    public TMP_InputField inputField;

    // Biến lưu giá trị thời gian tốt nhất (best timer)
    private int bestTimer;

    // Hàm gọi khi người chơi muốn tải điểm số của họ lên bảng xếp hạng
    public void UploadScore()
    {
        // Lấy giá trị thời gian tốt nhất từ GameController
        bestTimer = GameController.instance.bestTimer;

        // Gọi phương thức AddNewHighscore từ lớp Leaderboard để gửi điểm
        Leaderboard.AddNewHighscore(inputField.text, bestTimer);
    }
}
