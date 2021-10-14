using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Game.Menu
{
    public class LanguageSelect : MonoBehaviour
    {
        private AssetReference currentLanguage;

        [SerializeField]
        private AssetReference[] languages;

        public void SetLanguage(int index)
        {
            StartCoroutine(SetLanguage_Internal(index));
        }

        private IEnumerator SetLanguage_Internal(int index)
        {
            currentLanguage?.ReleaseAsset();

            var handle = languages[index].LoadAssetAsync<Locale>();

            yield return handle;

            LocalizationSettings.SelectedLocale = handle.Result;
            currentLanguage = languages[index];
        }
    }
}
