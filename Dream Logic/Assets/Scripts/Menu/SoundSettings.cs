using UnityEngine;

public class SoundSettings : MonoBehaviour
{
    private static float _effects;
    public static float effects
    {
        get => _effects;
        set => _effects = Mathf.Clamp01(value);
    }

    private static float _music;
    public static float music
    {
        get => _music;
        set => _music = Mathf.Clamp01(value);
    }
}
