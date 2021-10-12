using UnityEngine;

public class MenuLightSwitcher : MonoBehaviour
{
    private static readonly string emissionKeyword = "_EMISSION";
    private static readonly int emissionColorId = Shader.PropertyToID("_EmissionColor");

    [SerializeField]
    private GameObject nightlight;
    [SerializeField]
    private MeshRenderer nightlightRenderer;
    [SerializeField]
    private GameObject roomLight;

    [SerializeField]
    private float nightlightOnThreshold;
    [SerializeField]
    private float roomlightOnThreshold;
    [SerializeField]
    private float roomlightOffThreshold;

    private void Awake()
    {
        nightlightRenderer.material.EnableKeyword(emissionKeyword);
    }

    public void SetDaytime(float time, bool turnRoomlightOn)
    {
        if (1f - time > nightlightOnThreshold)
        {
            nightlight.SetActive(true);
            nightlightRenderer.material.SetColor(emissionColorId, Color.white);
        }
        else
        {
            nightlight.SetActive(false);
            nightlightRenderer.material.SetColor(emissionColorId, Color.black);
        }

        roomLight.SetActive(/*turnRoomlightOn && 1f - time > roomlightOnThreshold &&*/ 1f - time < roomlightOffThreshold);
    }
}
