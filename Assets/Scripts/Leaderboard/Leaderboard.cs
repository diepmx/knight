using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class Leaderboard : MonoBehaviour
{
    // Tạo instance để truy cập từ các script khác
    public static Leaderboard instance;
    private LeaderboardDisplay leaderboardDisplay;

    // Mã công khai để lấy dữ liệu từ Dreamlo
    private const string publicCode = "65eb5bc08f40bcbe8897ef97";
    // URL API của Dreamlo
    private const string webURL = "http://dreamlo.com/lb/";

    // Danh sách lưu trữ bảng xếp hạng
    public Highscore[] highscoresList;

    private void Awake()
    {
        // Gán instance để truy cập dễ dàng từ nơi khác
        instance = this;
        // Lấy component hiển thị leaderboard
        leaderboardDisplay = GetComponent<LeaderboardDisplay>();
    }

    // Thêm điểm số mới vào bảng xếp hạng
    public static void AddNewHighscore(string username, int score)
    {
        instance.StartCoroutine(instance.UploadNewHighscore(username, score));
    }

    // Coroutine để tải điểm lên server
    IEnumerator UploadNewHighscore(string username, int score)
    {
        // Tạo URL để gửi điểm số lên Dreamlo
        string url = webURL + publicCode + "/add/" + UnityWebRequest.EscapeURL(username) + "/" + score;

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            // Gửi request và chờ phản hồi
            yield return webRequest.SendWebRequest();

            // Kiểm tra xem có lỗi khi gửi không
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Lỗi khi tải điểm: " + webRequest.error);
            }
            else
            {
                Debug.Log("Tải điểm thành công!");
                // Nếu tải thành công, cập nhật lại bảng xếp hạng
                DownloadHiscores();
            }
        }
    }

    // Tải bảng xếp hạng từ server
    public void DownloadHiscores()
    {
        StartCoroutine(DownloadHighscoresFromDatabase());
    }

    // Coroutine tải bảng xếp hạng từ Dreamlo
    IEnumerator DownloadHighscoresFromDatabase()
    {
        string url = webURL + publicCode + "/pipe/";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            // Gửi request và chờ phản hồi
            yield return webRequest.SendWebRequest();

            // Kiểm tra xem có lỗi khi tải không
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Lỗi khi tải bảng xếp hạng: " + webRequest.error);
            }
            else
            {
                // Nếu thành công, xử lý dữ liệu và hiển thị bảng xếp hạng
                FormatHighscores(webRequest.downloadHandler.text);
                leaderboardDisplay.OnHiscoresDownloaded(highscoresList);
            }
        }
    }

    // Xử lý dữ liệu từ server thành danh sách điểm số
    void FormatHighscores(string textStream)
    {
        // Tách dữ liệu thành từng dòng
        string[] entries = textStream.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        // Tạo danh sách highscores
        highscoresList = new Highscore[entries.Length];

        for (int i = 0; i < entries.Length; i++)
        {
            // Mỗi dòng có dạng: username|score
            string[] entryInfo = entries[i].Split('|');

            // Lấy tên người chơi
            string username = entryInfo[0];
            // Lấy điểm số và chuyển thành số nguyên
            int score = int.Parse(entryInfo[1]);

            // Lưu dữ liệu vào danh sách
            highscoresList[i] = new Highscore(username, score);
        }
    }
}

// Cấu trúc dữ liệu lưu thông tin một điểm số trên bảng xếp hạng
public struct Highscore
{
    public string username; // Tên người chơi
    public int score; // Điểm số

    // Constructor để khởi tạo điểm số
    public Highscore(string _username, int _score)
    {
        username = _username;
        score = _score;
    }
}
