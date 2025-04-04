using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleCrossfade : MonoBehaviour
{
    [Space(10)]
    // Animator for the crossfade transition
    // Animator để thực hiện hiệu ứng crossfade chuyển cảnh
    public Animator animator;

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

    // Method called when the start button is pressed (e.g., when the player clicks "Start")
    // Phương thức được gọi khi người chơi nhấn nút "Start"
    public void OnPressStart()
    {
        // Start the crossfade animation and load the next level
        // Bắt đầu hiệu ứng crossfade và tải màn tiếp theo
        NextLevelCrossfade();
    }

    // Initiates the crossfade animation and loads the next level after a delay.
    // Bắt đầu hiệu ứng crossfade và tải màn tiếp theo sau một khoảng thời gian
    public void NextLevelCrossfade()
    {
        // Start the crossfade animation and load the next level
        // Bắt đầu Coroutine để tải màn tiếp theo với hiệu ứng crossfade
        StartCoroutine(loadLevel(SceneManager.GetActiveScene().buildIndex + 1));

        // Fade out the background music
        // Bắt đầu Coroutine để làm mờ nhạc nền
        StartCoroutine(FadeOutMusic());
    }

    // Coroutine to load a level with a crossfade animation.
    // Coroutine để tải màn chơi tiếp theo với hiệu ứng crossfade
    IEnumerator loadLevel(int levelIndex)
    {
        // Trigger the crossfade animation
        // Kích hoạt hiệu ứng crossfade
        animator.SetTrigger("Start");

        // Wait for the specified transition time
        // Chờ trong khoảng thời gian chuyển cảnh
        yield return new WaitForSeconds(transitionTime);

        // Wait for the music fade out time to ensure music fades out before loading the level
        // Chờ thêm thời gian làm mờ nhạc nền để đảm bảo nhạc nền mờ đi trước khi chuyển màn
        yield return new WaitForSeconds(musicFadeOutTime);

        // Load the next scene
        // Tải màn chơi tiếp theo
        SceneManager.LoadScene(levelIndex);
    }

    // Coroutine to fade out the background music.
    // Coroutine để giảm dần âm thanh nhạc nền
    IEnumerator FadeOutMusic()
    {
        // If the audio source is not assigned, try to get it from the component
        // Nếu chưa có AudioSource, lấy AudioSource từ GameObject
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Store the initial volume of the audio source
        // Lưu lại âm lượng ban đầu của nhạc nền
        float startVolume = audioSource.volume;

        // Gradually decrease the volume of the background music
        // Giảm dần âm lượng của nhạc nền cho đến khi âm lượng bằng 0
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / musicFadeOutTime;
            yield return null;
        }

        // Stop the music when the volume reaches zero
        // Dừng nhạc khi âm lượng đạt 0
        audioSource.Stop();
    }
}
