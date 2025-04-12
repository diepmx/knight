using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.ParticleSystem;

public class HubCrossfade : MonoBehaviour
{
    [Header("Particle System sẽ bị huỷ khi chuyển màn")]
    public ParticleSystem[] particlesToStop; // Gán 2 cái ParticleSystem bạn muốn tắt
    public float preFadeDelay = 0.3f; // Thời gian delay trước khi bắt đầu crossfade


    [Space(10)]
    // Animator for the crossfade transition
    // Animator để thực hiện hiệu ứng crossfade chuyển cảnh
    public Animator crossfadeAnimator;

    // Animator for the door opening animation
    // Animator để thực hiện hiệu ứng mở cửa
    public Animator doorAnimator;

    [Space(10)]
    // Duration of the crossfade transition
    // Thời gian thực hiện hiệu ứng chuyển cảnh crossfade
    public float transitionTime = 1f;

    [Space(10)]
    // Audio source for background music
    // Nguồn âm thanh dành cho nhạc nền
    public AudioSource audioSource;

    // Time to fade out the music
    // Thời gian để làm mờ âm thanh nhạc nền
    public float musicFadeOutTime = 1f;

    // Method called when the player enters a collider trigger (e.g., door)
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra xem đối tượng va chạm có phải là cửa (tag "Door")
        if (other.CompareTag("Door"))
        {
            // Bắt đầu hiệu ứng crossfade và tải màn chơi tiếp theo
            NextLevelCrossfade();

            // Kích hoạt hiệu ứng mở cửa
            doorAnimator.SetTrigger("Open");

            // Phát âm thanh hiệu ứng mở cửa
            SFXManager.instance.PlaySFX(1);
        }
    }

    // Initiates the crossfade animation and loads the next level after a delay.
    // Bắt đầu hiệu ứng crossfade và tải màn tiếp theo sau một khoảng thời gian
    public void NextLevelCrossfade()
    {
        // Chọn scene ngẫu nhiên từ danh sách
        string[] levelNames = { "Level One", "Level Two", "Level Three" }; // <-- thêm tên các scene bạn muốn
        string randomLevel = levelNames[Random.Range(0, levelNames.Length)];

        StartCoroutine(loadLevel(randomLevel));
        StartCoroutine(FadeOutMusic());
    }


    // Coroutine to load a level with a crossfade animation.
    // Coroutine để tải màn chơi tiếp theo với hiệu ứng crossfade
    IEnumerator loadLevel(string levelName)
    {
        // 👉 Ngừng tất cả particle trước khi crossfade
        foreach (var ps in particlesToStop)
        {
            if (ps != null)
            {
                ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
        }

        // 👉 Chờ chút rồi mới bắt đầu crossfade (cho cảm giác “tắt rồi fade”)
        yield return new WaitForSeconds(preFadeDelay);

        crossfadeAnimator.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelName);
    }




    // Coroutine to fade out the background music.
    // Coroutine để giảm dần âm thanh nhạc nền
    IEnumerator FadeOutMusic()
    {
        // Nếu chưa có AudioSource, lấy AudioSource từ GameObject
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Lưu lại âm lượng ban đầu của nhạc nền
        float startVolume = audioSource.volume;

        // Giảm dần âm lượng của nhạc nền cho đến khi âm lượng bằng 0
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / musicFadeOutTime;
            yield return null;
        }

        // Dừng nhạc khi âm lượng đạt 0
        audioSource.Stop();
    }
}
