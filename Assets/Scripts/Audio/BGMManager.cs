using UnityEngine;

public class BGMManager : MonoBehaviour
{
    // Biến instance dùng để tạo mô hình Singleton, giúp truy cập dễ dàng từ các script khác
    public static BGMManager instance;

    [Space(10)]
    // Mảng chứa các nguồn âm thanh (AudioSource) cho nhạc nền
    public AudioSource[] music;

    private void Awake()
    {
        // Gán instance này là đối tượng hiện tại của BGMManager
        instance = this;
    }

    // Hàm dừng phát nhạc nền tại vị trí được chỉ định
    public void StopBGM(int bgmToStop)
    {
        // Dừng nhạc nền tại chỉ mục (index) tương ứng trong mảng music
        music[bgmToStop].Stop();
    }
}
