using Game;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MenuLightSwitcher), typeof(MenuSkySwitcher))]
public class MenuGameStart : MonoBehaviour
{
    /// <summary>
    /// Относительное время. Значение 0 соответствует ночи, а 1 - дню.
    /// </summary>
    private static float daytime = 0f;

    private MenuSkySwitcher skySwitcher;
    private MenuLightSwitcher lightSwitcher;

    [SerializeField]
    private float switchTime;
    [SerializeField]
    private GameObject navigationParent;

    private void Awake()
    {
        lightSwitcher = GetComponent<MenuLightSwitcher>();
        skySwitcher = GetComponent<MenuSkySwitcher>();

        ToggleTime();
    }

    public void ToggleTime()
    {
        StartCoroutine(ToggleTime_Internal(1f - daytime, switchTime));
    }

    private IEnumerator ToggleTime_Internal(float time, float switchTime)
    {
        float counter = 0f;

        navigationParent.SetActive(time > daytime);

        while (counter < switchTime)
        {
            float currentTime = Mathf.Lerp(daytime, time, counter / switchTime);

            skySwitcher.SetDaytime(currentTime);
            lightSwitcher.SetDaytime(currentTime, time < daytime);

            counter += Time.deltaTime;
            yield return null;
        }

        if (time < daytime)
        {
            counter = 0f;
            while (counter < switchTime)
            {
                float currentTime = -counter / switchTime;

                lightSwitcher.SetDaytime(currentTime, time < daytime);

                counter += Time.deltaTime;
                yield return null;
            }
        }

        daytime = time;
    }
}
