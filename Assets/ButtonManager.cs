using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    // Hàm để bắt đầu game, chuyển sang scene chính của game
    public void PlayGame()
    {
        // Chuyển sang Scene chính của game. Đảm bảo scene cần load đã được thêm trong Build Settings
        SceneManager.LoadScene("Map1");
    }

    // Hàm để thoát game
    public void ExitGame()
    {
        // Thoát game khi build trên máy tính hoặc thiết bị di động
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
