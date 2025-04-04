using UnityEngine;

public class MouseController : MonoBehaviour
{
    // Biến chứa texture cho con trỏ chuột tùy chỉnh
    public Texture2D cursor;

    // Cờ để theo dõi trạng thái chuột có hiển thị hay không
    private bool isMouseVisible;

    void Start()
    {
        // Đặt texture con trỏ chuột tùy chỉnh
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.ForceSoftware);
        // Khởi tạo trạng thái chuột là hiển thị (visible)
        isMouseVisible = true;
    }

    // Phương thức để thay đổi việc hiển thị hoặc ẩn con trỏ chuột
    public void OnShowMouse()
    {
        // Đảo ngược trạng thái của chuột (hiển thị/ẩn)
        isMouseVisible = !isMouseVisible;

        // Nếu chuột hiện tại đang hiển thị
        if (isMouseVisible)
        {
            // Đặt con trỏ chuột là visible (hiển thị)
            Cursor.visible = true;
            // Đặt trạng thái khóa con trỏ là None (không khóa, chuột có thể tự do di chuyển)
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            // Nếu chuột hiện tại đang ẩn
            // Đặt con trỏ chuột là invisible (ẩn)
            Cursor.visible = false;
            // Đặt trạng thái khóa con trỏ là Locked (khóa, chuột bị hạn chế trong cửa sổ game)
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
