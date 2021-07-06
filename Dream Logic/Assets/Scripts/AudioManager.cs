using System;
using System.Collections;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// Менеджер звука.
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager _instance;
        public static AudioManager instance => _instance;

        [SerializeField]
        private float fadeTime;

        private AudioSource currentThemeSource;
        private AudioSource nextThemeSource;

        private Sound currentTheme;
        [SerializeField]
        private Sound menuTheme;

        [SerializeField]
        private Sound[] soundEffects;

        private void Awake()
        {
            if (_instance == null)
                _instance = this;
            else if (_instance != this)
            {
                Destroy(gameObject);
                return;
            }

            currentThemeSource = gameObject.AddComponent<AudioSource>();
            nextThemeSource = gameObject.AddComponent<AudioSource>();

            foreach (var sound in soundEffects)
                AddAudioSource(sound);
            PlayTheme(menuTheme);

            DontDestroyOnLoad(gameObject);
        }

        public void PlayTheme(Sound settings)
        {
            SetAudioSource(nextThemeSource, currentTheme);
            StartCoroutine(FadeCoroutine(currentTheme, false, fadeTime));
            currentTheme = settings;
            SetAudioSource(currentThemeSource, currentTheme);
            StartCoroutine(FadeCoroutine(currentTheme, true, fadeTime));
        }

        public void Play(string name)
        {
            Sound settings = Array.Find(soundEffects, sound => sound.name == name);
            if (settings == null)
                Debug.LogWarning($"Sound {name} not found.");
            else
                settings.source.Play();
        }

        public void FadeAudio(Sound settings)
        {
            StartCoroutine(FadeCoroutine(settings, false, fadeTime));
        }

        private IEnumerator FadeCoroutine(Sound settings, bool enabled, float time)
        {
            if (settings == null)
                yield break;
            if (enabled)
                settings.source.Play();

            float counter = 0f;
            float startVolume = enabled ? 0f : settings.source.volume, endVolume = enabled ? settings.maxVolume : 0f;

            while (counter < time)
            {
                settings.source.volume = Mathf.Lerp(startVolume, endVolume, counter / time);
                counter += Time.deltaTime;
                yield return null;
            }

            if (!enabled)
                settings.source.Stop();
        }

        private void AddAudioSource(Sound sound)
        {
            var source = gameObject.AddComponent<AudioSource>();
            SetAudioSource(source, sound);
        }

        private void SetAudioSource(AudioSource source, Sound sound)
        {
            if (sound == null)
                return;

            source.clip = sound.clip;
            source.volume = sound.maxVolume;
            source.loop = sound.loop;

            sound.source = source;
        }
    }
}
