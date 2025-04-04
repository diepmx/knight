using System.Collections;
using UnityEngine;

public class LeaderboardDisplay : MonoBehaviour
{
    [Space(10)]
    // Mảng chứa các phần tử TextMeshProUGUI để hiển thị bảng xếp hạng
    public TMPro.TextMeshProUGUI[] highscoreText;

    // Tham chiếu đến lớp Leaderboard
    private Leaderboard leaderboard;

    void Start()
    {
        // Khởi tạo tất cả các ô hiển thị với dòng chữ "Đang tải..."
        for (int i = 0; i < highscoreText.Length; i++)
        {
            highscoreText[i].text = i + 1 + ". Đang tải...";
        }

        // Lấy tham chiếu đến thành phần Leaderboard
        leaderboard = GetComponent<Leaderboard>();

        // Bắt đầu làm mới bảng xếp hạng theo chu kỳ
        StartCoroutine("RefreshHighscores");
    }

    // Hàm được gọi khi dữ liệu bảng xếp hạng đã được tải về từ máy chủ
    public void OnHiscoresDownloaded(Highscore[] highscoresList)
    {
        // Cập nhật dữ liệu hiển thị bảng xếp hạng với thông tin mới
        for (int i = 0; i < highscoreText.Length; i++)
        {
            highscoreText[i].text = i + 1 + ". ";

            if (highscoresList.Length > i)
            {
                // Chuyển đổi điểm số thành định dạng phút:giây (MM:SS)
                float minutes = Mathf.FloorToInt(highscoresList[i].score / 60f);
                float seconds = Mathf.FloorToInt(highscoresList[i].score % 60);

                // Định dạng thời gian tốt nhất theo MM:SS
                string bestTime = minutes.ToString("00") + ":" + seconds.ToString("00");

                // Hiển thị tên người chơi và thời gian tốt nhất trong bảng xếp hạng
                highscoreText[i].text += highscoresList[i].username + " - " + bestTime;
            }
        }
    }

    // Coroutine để làm mới bảng xếp hạng theo chu kỳ
    IEnumerator RefreshHighscores()
    {
        // Liên tục làm mới bảng xếp hạng
        while (true)
        {
            // Tải dữ liệu bảng xếp hạng mới nhất từ máy chủ
            leaderboard.DownloadHiscores();

            // Đợi một khoảng thời gian trước khi làm mới lại (mặc định 10 giây)
            yield return new WaitForSeconds(10);
        }
    }
}
