using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "New Clip", menuName = "Dream/Audio Clip Settings")]
    public class AudioClipSettings : ScriptableObject
    {
        public AudioClip clip;
        public float pitchRandomOffset;
        public float maxVolume;
        public bool loop;
    }
}

