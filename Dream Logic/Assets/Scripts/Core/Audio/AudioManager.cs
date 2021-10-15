using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game
{
    public class AudioManager : MonoBehaviour
    {
        private const string EFFECTS_VOLUME_KEY = "EFFECTS_VOLUME";
        private const string MUSIC_VOLUME_KEY = "MUSIC_VOLUME";

        private static AudioManager _instance;
        public static AudioManager instance => _instance;

        private float _effectsVolume;
        public float effectsVolume
        {
            get => _effectsVolume;
            set
            {
                _effectsVolume = Mathf.Clamp01(value);
                PlayerPrefs.SetFloat(EFFECTS_VOLUME_KEY, value);
            }
        }

        private float _musicVolume;
        public float musicVolume
        {
            get => _musicVolume;
            set
            {
                _musicVolume = Mathf.Clamp01(value);
                PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, value);
            }
        }

        private bool _isReady;
        public bool isReady => _isReady;

        [SerializeField]
        private AssetReference sourcePrefabReference;
        private AudioSource sourcePrefab;
        [SerializeField]
        private AssetReference[] effects;

        [SerializeField]
        private int poolCapacity;

        [SerializeField]
        private float themeFadeSpeed;

        private AudioSource currThemeSource;

        private List<AudioSource> effectSources = new List<AudioSource>();

        private void Awake()
        {
            if (instance == null)
            {
                _instance = this;
                StartCoroutine(Init());
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }

        private void SetSettings()
        {
            if (!PlayerPrefs.HasKey(EFFECTS_VOLUME_KEY))
                PlayerPrefs.SetFloat(EFFECTS_VOLUME_KEY, 1f);
            if (!PlayerPrefs.HasKey(MUSIC_VOLUME_KEY))
                PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, 1f);

            effectsVolume = PlayerPrefs.GetFloat(EFFECTS_VOLUME_KEY);
            musicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY);
        }

        private IEnumerator Init()
        {
            SetSettings();
            yield return StartCoroutine(AddPool());
            yield return StartCoroutine(LoadEffects());
            //yield return StartCoroutine(LoadAssetAndPlay(mainTheme, PlayTheme));
            _isReady = true;
        }

        private IEnumerator AddPool()
        {
            var handle = sourcePrefabReference.LoadAssetAsync<GameObject>();
            yield return handle;
            sourcePrefab = (sourcePrefabReference.Asset as GameObject).GetComponent<AudioSource>();
            GamePool.instance.AddPool(sourcePrefab, poolCapacity, transform);
        }

        private IEnumerator LoadEffects()
        {
            foreach (var effect in effects)
                yield return effect.LoadAssetAsync<AudioClipSettings>();
        }

        public void PlaySound(string name)
        {
            PlaySound(Array.Find(effects, x => x.Asset.name == name).Asset as AudioClipSettings);
        }

        public void PlaySound(AudioClipSettings sound)
        {
            AudioSource source = GetSoundSource(sound.clip);

            source.pitch = 1f + UnityEngine.Random.Range(-sound.pitchRandomOffset, sound.pitchRandomOffset);
            source.loop = sound.loop;
            source.volume = sound.maxVolume * effectsVolume;

            source.PlayOneShot(sound.clip);
        }

        private AudioSource GetSoundSource(AudioClip clip)
        {
            foreach (var source in effectSources)
                if (source.clip == clip)
                    return source;
            var instance = GamePool.instance.GetObject(sourcePrefab, transform);
            instance.clip = clip;
            effectSources.Add(instance);
            return instance;
        }

        public void PlayTheme(AssetReference reference)
        {
            StartCoroutine(LoadAssetAndPlay(reference, PlayTheme));
        }

        private IEnumerator LoadAssetAndPlay(AssetReference reference, Action<AudioClipSettings> callback)
        {
            if (reference.Asset == null)
            {
                var handle = reference.LoadAssetAsync<AudioClipSettings>();
                yield return handle;
                callback(handle.Result);
            }
            else
                callback(reference.Asset as AudioClipSettings);
        }

        public void PlayTheme(AudioClipSettings theme)
        {
            AudioSource source = GamePool.instance.GetObject(sourcePrefab, transform);
            
            source.clip = theme.clip;
            source.loop = theme.loop;
            source.volume = theme.maxVolume * musicVolume;

            if (currThemeSource != null)
                StartCoroutine(FadeSource(currThemeSource, false, themeFadeSpeed));
            StartCoroutine(FadeSource(source, true, themeFadeSpeed));

            currThemeSource = source;
        }

        private IEnumerator FadeSource(AudioSource source, bool enable, float time)
        {
            float startVolume = enable ? 0f : source.volume, endVolume = source.volume - startVolume;

            float counter = 0f;

            if (enable)
                source.Play();

            while (counter < time)
            {
                source.volume = Mathf.Lerp(startVolume, endVolume, counter / time);
                counter += Time.deltaTime;
                yield return null;
            }

            if (!enable)
            {
                source.Stop();
                GamePool.instance.AddObject(source);
            }
        }
    }
}

