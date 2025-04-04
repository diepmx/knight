using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class HubPauseController : MonoBehaviour
{
    public static HubPauseController instance;

    [Space(10)]
    // GameObject cho màn hình tạm dừng (pause screen)
    public GameObject pauseScreen;
    // Nút mặc định được chọn khi màn hình tạm dừng được kích hoạt
    public GameObject defaultSelectedButton;
    // Tên của cảnh menu chính sẽ được tải
    public string mainMenuName;

    private void Awake()
    {
        // Đảm bảo chỉ có một instance duy nhất của HubPauseController
        instance = this;
    }

    // Phương thức gọi khi nút tạm dừng được nhấn
    public void OnPressPause()
    {
        // Gọi phương thức tạm dừng hoặc tiếp tục trò chơi
        PauseUnpause();
    }

    // Phương thức tạm dừng hoặc tiếp tục trò chơi
    public void PauseUnpause()
    {
        // Nếu màn hình tạm dừng không hiển thị, tiến hành tạm dừng trò chơi
        if (pauseScreen.activeSelf == false)
        {
            // Dừng tất cả hiệu ứng âm thanh khi tạm dừng
            SFXManager.instance.StopAllSFX();

            // Kích hoạt màn hình tạm dừng
            pauseScreen.SetActive(true);
            // Đặt Time.timeScale về 0 để tạm dừng trò chơi
            Time.timeScale = 0f;
            // Đặt nút được chọn mặc định trong EventSystem
            EventSystem.current.SetSelectedGameObject(defaultSelectedButton);
        }
        else
        {
            // Nếu màn hình tạm dừng đang hiển thị, tiếp tục trò chơi
            pauseScreen.SetActive(false);
            // Đặt Time.timeScale về 1 để tiếp tục trò chơi
            Time.timeScale = 1f;
            // Đặt nút được chọn trong EventSystem là null (không có nút được chọn)
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    // Phương thức quay lại menu chính
    public void GoToMainMenu()
    {
        // Tải cảnh menu chính
        SceneManager.LoadScene(mainMenuName);
        // Đảm bảo Time.timeScale được đặt lại về 1 để thời gian tiếp tục bình thường
        Time.timeScale = 1f;
    }

    // Phương thức thoát khỏi trò chơi
    public void Quitgame()
    {
        // Thoát khỏi ứng dụng
        Application.Quit();
    }
}
