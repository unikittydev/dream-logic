using UnityEngine;

namespace Game
{
    /// <summary>
    /// Параметры звука.
    /// </summary>
    [CreateAssetMenu(fileName = "New Clip", menuName = "Dream/Audio Clip Settings")]
    public class Sound : ScriptableObject
    {
        public new string name;
        public AudioClip clip;
        public float maxVolume;
        public bool loop;

        [HideInInspector]
        public AudioSource source;
    }
}
