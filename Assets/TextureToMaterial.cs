using UnityEngine;

public class TextureToMaterial : MonoBehaviour
{
    public Texture2D texture; // Gán từ Inspector
    public Renderer targetRenderer; // Gán từ Inspector

    private void Start()
    {
        // Tạo một Material mới từ Shader "Standard"
        Material newMaterial = new Material(Shader.Find("Standard"));

        // Gán texture vào Material (bạn có thể chọn các kênh khác nhau của material, như Albedo)
        newMaterial.mainTexture = texture;

        // Gán material vào Renderer của đối tượng
        if (targetRenderer != null)
        {
            targetRenderer.material = newMaterial;
        }
        else
        {
            Debug.LogError("Renderer not assigned!");
        }
    }
}
