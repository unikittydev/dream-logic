using UnityEngine;

public class TextureCreator : MonoBehaviour
{
    [Range(1f, 4096f)]
    public int resolution;

    private Material targetMaterial;
    public Texture2D texture;

    public void ResetTexture()
    {
        Texture2D texture = new Texture2D(resolution, resolution)
        {
            filterMode = FilterMode.Point
        };

        for (int i = 0; i < resolution; i++)
        {
            for (int j = 0; j < resolution; j++)
            {
                texture.SetPixel(i, j, Color.black);
            }
        }
        targetMaterial.mainTexture = this.texture = texture;
    }
}
