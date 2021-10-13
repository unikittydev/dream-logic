using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Menu
{
    [RequireComponent(typeof(MenuLightSwitcher), typeof(MenuSkySwitcher))]
    public class MenuGameStart : MonoBehaviour
    {
        /// <summary>
        /// Относительное время. Значение 0 соответствует ночи, а 1 - дню.
        /// </summary>
        private static float daytime = 0f;

        private MenuNavigationSwitcher navSwitcher;
        private MenuSkySwitcher skySwitcher;
        private MenuLightSwitcher lightSwitcher;
        [SerializeField]
        private TextFlashing textFlashing;

        private Coroutine toggleTime;

        [SerializeField]
        private AssetReference gameScene;

        private void Awake()
        {
            navSwitcher = GetComponent<MenuNavigationSwitcher>();
            lightSwitcher = GetComponent<MenuLightSwitcher>();
            skySwitcher = GetComponent<MenuSkySwitcher>();

            ToggleTime();
        }

        public void StartGame()
        {
            StartCoroutine(StartGame_Internal(gameScene));
        }

        private IEnumerator StartGame_Internal(AssetReference scene)
        {
            yield return ToggleTime();
            GameSceneLoader.LoadScene(scene);
        }

        public Coroutine ToggleTime()
        {
            if (toggleTime != null)
                StopCoroutine(toggleTime);
            toggleTime = StartCoroutine(ToggleTime_Internal(1f - daytime));
            return toggleTime;
        }

        private IEnumerator ToggleTime_Internal(float time)
        {
            navSwitcher.SwitchNavigationUI(time > daytime);
            var sky = skySwitcher.SetDaytime(daytime, time);
            var light = lightSwitcher.SetDaytime(daytime, time);

            yield return new WaitForSeconds(navSwitcher.switchTime);
            yield return sky;
            yield return light;

            if (time < daytime)
                yield return textFlashing.DisplayText();
            else
                textFlashing.Clear();

            daytime = time;
        }
    }
}
