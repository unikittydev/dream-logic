using UnityEngine;

public enum ElementFlag
{
    Air,
    Earth,
    Water,
    Fire,
    Light,
    Darkness
}

public class Elements : MonoBehaviour
{
    public BaseElement[] baseElements;
    public MixedElement[] mixedElements;

    private TextureCreator tc;

    private void OnValidate()
    {
        tc = GetComponent<TextureCreator>();
        tc.ResetTexture();

        for (int i = 0; i < tc.resolution; i++)
        {
            for (int j = 0; j < tc.resolution; j++)
            {
                Vector2 viewportPos = FindTwoNearestElements(i, j, out int e1, out int e2);
                //PaintPixel(i, j);
                float sqrDist1 = (baseElements[e1].unitPosition - viewportPos).magnitude;
                float sqrDist2 = (baseElements[e2].unitPosition - viewportPos).magnitude;
                tc.texture.SetPixel(i, j, Color.Lerp(baseElements[e1].color, baseElements[e2].color, sqrDist1 / (sqrDist1 + sqrDist2)));
            }
        }

        tc.texture.Apply();
    }

    private void PaintPixel(int i, int j)
    {
        Vector2 unitPos = (new Vector2(i, j) / tc.resolution - .5f * Vector2.one) * 2f;

        int minIndex = 0;
        for (int k = 1; k < baseElements.Length; k++)
        {
            if ((baseElements[k].unitPosition - unitPos).sqrMagnitude < (baseElements[minIndex].unitPosition - unitPos).sqrMagnitude)
                minIndex = k;
        }

        tc.texture.SetPixel(i, j, baseElements[minIndex].color);
    }

    private Vector2 FindTwoNearestElements(int i, int j, out int e1, out int e2)
    {
        Vector2 unitPos = (new Vector2(i, j) / tc.resolution - .5f * Vector2.one) * 2f;

        int[] indices = new int[baseElements.Length];
        for (int k = 0; k < indices.Length; k++)
            indices[k] = k;

        for (int x = 0; x < baseElements.Length; x++)
        {
            for (int y = 0; y < baseElements.Length - x - 1; y++)
            {
                if ((baseElements[indices[y]].unitPosition - unitPos).sqrMagnitude > (baseElements[indices[y + 1]].unitPosition - unitPos).sqrMagnitude)
                {
                    int temp = indices[y];
                    indices[y] = indices[y + 1];
                    indices[y + 1] = temp;
                }
            }
        }

        e1 = indices[0];
        e2 = indices[1];

        return unitPos;
    }
}
