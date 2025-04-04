using UnityEngine;

public class SFXManager : MonoBehaviour
{
    // Biến instance dùng để tạo mô hình Singleton, giúp truy cập dễ dàng từ các script khác
    public static SFXManager instance;

    [Space(10)]
    // Mảng chứa các nguồn âm thanh (AudioSource) cho hiệu ứng âm thanh (SFX)
    public AudioSource[] soundEffects;

    private void Awake()
    {
        // Gán instance này là đối tượng hiện tại của SFXManager
        instance = this;
    }

    // Hàm phát hiệu ứng âm thanh tại vị trí được chỉ định
    public void PlaySFX(int sfxToPlay)
    {
        // Dừng âm thanh trước để đảm bảo phát lại từ đầu
        soundEffects[sfxToPlay].Stop();
        // Phát hiệu ứng âm thanh
        soundEffects[sfxToPlay].Play();
    }

    // Hàm dừng hiệu ứng âm thanh tại vị trí được chỉ định
    public void StopSFX(int sfxToPlay)
    {
        // Dừng hiệu ứng âm thanh tương ứng
        soundEffects[sfxToPlay].Stop();
    }

    // Hàm phát hiệu ứng âm thanh với cao độ (pitch) ngẫu nhiên
    public void PlaySFXPitched(int sfxToPlay)
    {
        // Đặt cao độ ngẫu nhiên trong khoảng từ 0.8 đến 1.2
        soundEffects[sfxToPlay].pitch = Random.Range(0.8f, 1.2f);
        // Phát hiệu ứng âm thanh với cao độ mới
        PlaySFX(sfxToPlay);
    }

    // Hàm dừng tất cả hiệu ứng âm thanh
    public void StopAllSFX()
    {
        // Lặp qua tất cả các hiệu ứng âm thanh và dừng từng cái một
        foreach (AudioSource sfx in soundEffects)
        {
            sfx.Stop();
        }
    }
}
