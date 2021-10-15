using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Menu
{
    public class MenuTheme : MonoBehaviour
    {
        [SerializeField]
        private AssetReference themeSettings;

        private IEnumerator Start()
        {
            var handle = themeSettings.LoadAssetAsync<AudioClipSettings>();
            yield return handle;

            while (!AudioManager.instance.isReady)
                yield return null;

            AudioManager.instance.PlayTheme(themeSettings.Asset as AudioClipSettings);
        }

        private void OnDestroy()
        {
            themeSettings.ReleaseAsset();
        }
    }
}
