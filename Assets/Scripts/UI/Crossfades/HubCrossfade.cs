using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HubCrossfade : MonoBehaviour
{
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
        string[] levelNames = { "Level One", "Level Two" }; // <-- thêm tên các scene bạn muốn
        string randomLevel = levelNames[Random.Range(0, levelNames.Length)];

        StartCoroutine(loadLevel(randomLevel));
        StartCoroutine(FadeOutMusic());
    }


    // Coroutine to load a level with a crossfade animation.
    // Coroutine để tải màn chơi tiếp theo với hiệu ứng crossfade
    IEnumerator loadLevel(string levelName)
    {
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
