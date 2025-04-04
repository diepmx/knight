using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BonusLevelCrossfade : MonoBehaviour
{
    [Space(10)]
    // Animator for crossfade effect.
    // Chứa animator để điều khiển hiệu ứng crossfade giữa các màn chơi
    public Animator crossfadeAnimator;

    [Space(10)]
    // Duration of transition.
    // Thời gian chuyển cảnh
    public float transitionTime = 1f;

    [Space(10)]
    // AudioSource for music.
    // Chứa nguồn âm thanh (nhạc nền)
    public AudioSource audioSource;
    // Duration for fading out music.
    // Thời gian giảm dần âm lượng nhạc
    public float musicFadeOutTime = 1f;

    [Space(10)]
    // Reference to the SpriteRenderer of the player.
    // Tham chiếu đến SpriteRenderer của nhân vật người chơi
    public SpriteRenderer playerSpriteRenderer;

    // Check for trigger collision.
    // Kiểm tra khi nào nhân vật va chạm với trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Stairs"))
        {
            // Khi nhân vật va chạm với cầu thang, bắt đầu quá trình chuyển cảnh
            StartCoroutine(NextLevelSequence());
        }
    }

    // Initiate crossfade to the next level.
    // Bắt đầu quá trình chuyển cảnh sang màn tiếp theo
    IEnumerator NextLevelSequence()
    {
        // Đặt lại kích thước ban đầu của người chơi
        PlayerController.instance.transform.localScale = Vector3.one;

        // Thực hiện scale nhỏ lại và mờ dần
        yield return StartCoroutine(ScaleDownPlayer());

        // Tải màn chơi tiếp theo
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 2));
        // Mờ dần âm thanh nền
        StartCoroutine(FadeOutMusic());
    }

    // Coroutine to load the next level with a crossfade effect.
    // Coroutine tải màn chơi tiếp theo sau khi hoàn thành hiệu ứng crossfade
    IEnumerator LoadLevel(int levelIndex)
    {
        // Bắt đầu hiệu ứng crossfade
        crossfadeAnimator.SetTrigger("Start");

        // Đợi thời gian chuyển cảnh
        yield return new WaitForSeconds(transitionTime);

        // Tải màn chơi tiếp theo
        SceneManager.LoadScene(levelIndex);
    }

    // Coroutine to fade out the music.
    // Coroutine để giảm dần âm thanh của nhạc nền
    IEnumerator FadeOutMusic()
    {
        // Nếu chưa có AudioSource, lấy AudioSource từ GameObject
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Lưu lại âm lượng ban đầu của nhạc
        float startVolume = audioSource.volume;

        // Giảm âm lượng từ từ cho đến khi âm thanh tắt hẳn
        while (audioSource.volume > 0)
        {
            // Giảm dần âm lượng theo thời gian
            audioSource.volume -= startVolume * Time.deltaTime / musicFadeOutTime;
            yield return null;
        }

        // Dừng nhạc khi âm lượng bằng 0
        audioSource.Stop();
    }

    // Coroutine to scale down the player with fade out.
    // Coroutine để thu nhỏ nhân vật và mờ dần
    IEnumerator ScaleDownPlayer()
    {
        Vector3 targetScale = new Vector3(0.5f, 0.5f, 1f); // Kích thước đích của nhân vật
        float scaleSpeed = 1f; // Tốc độ thu nhỏ
        float fadeOutDuration = 1f; // Thời gian mờ dần
        Color originalColor = playerSpriteRenderer.color; // Màu ban đầu của nhân vật
        float initialAlpha = originalColor.a; // Độ trong suốt ban đầu của nhân vật
        float fadeOutStartTime = Time.time; // Thời điểm bắt đầu hiệu ứng mờ dần

        // Thu nhỏ nhân vật và mờ dần cho đến khi đạt kích thước đích và độ mờ dần là 0
        while (PlayerController.instance.transform.localScale.x > targetScale.x || playerSpriteRenderer.color.a > 0)
        {
            // Thu nhỏ nhân vật
            PlayerController.instance.transform.localScale -= new Vector3(scaleSpeed, scaleSpeed, 0f) * Time.deltaTime;

            // Tính toán độ trong suốt mới để mờ dần
            float newAlpha = Mathf.Lerp(initialAlpha, 0f, Mathf.Clamp01((Time.time - fadeOutStartTime) / fadeOutDuration));
            playerSpriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha);

            yield return null;
        }

        // Đảm bảo nhân vật đạt kích thước và độ mờ dần cuối cùng
        PlayerController.instance.transform.localScale = targetScale;
        playerSpriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
    }
}
