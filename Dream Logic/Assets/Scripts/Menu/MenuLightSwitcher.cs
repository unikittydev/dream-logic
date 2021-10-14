using System.Collections;
using UnityEngine;

namespace Game.Menu
{
    public class MenuLightSwitcher : MonoBehaviour
    {
        private static readonly string emissionKeyword = "_EMISSION";
        private static readonly int emissionColorId = Shader.PropertyToID("_EmissionColor");

        [SerializeField]
        private float switchTime;
        [Header("Roomlight")]
        [SerializeField]
        private Light roomLight;
        [SerializeField]
        private float roomlightOffDelay;
        [SerializeField]
        private float roomlightSwitchSpeed;
        [SerializeField]
        private float roomlightIntensity;
        [Header("Nightlight")]
        [SerializeField]
        private Light nightlight;
        [SerializeField]
        private MeshRenderer nightlightRenderer;
        [SerializeField]
        private float nightlightOnDelay;
        [SerializeField]
        private float nightlightSwitchSpeed;
        [SerializeField]
        private float nightlightIntensity;
        [SerializeField]
        private Color onEmissionColor;

        private void Awake()
        {
            nightlightRenderer.material.EnableKeyword(emissionKeyword);
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        public Coroutine SetDaytime(float fromDaytime, float toDaytime)
        {
            return StartCoroutine(SetDaytime_Internal(fromDaytime, toDaytime));
        }

        private IEnumerator SetDaytime_Internal(float fromDaytime, float toDaytime)
        {
            float counter = 0f;

            bool nightOn = fromDaytime < toDaytime, roomOn = fromDaytime > toDaytime;

            while (counter < switchTime)
            {
                float currTime = switchTime * Mathf.Lerp(fromDaytime, toDaytime, 1f - counter / switchTime);

                if (roomOn && currTime > roomlightOffDelay)
                {
                    StartCoroutine(SwitchLight(roomLight, roomlightIntensity, 0f, roomlightSwitchSpeed));
                    roomOn = false;
                }
                else if (!roomOn && currTime < roomlightOffDelay)
                {
                    StartCoroutine(SwitchLight(roomLight, 0f, roomlightIntensity, roomlightSwitchSpeed));
                    roomOn = true;
                }

                if (nightOn && currTime > nightlightOnDelay)
                {
                    StartCoroutine(SwitchLight(nightlight, 0f, nightlightIntensity, nightlightSwitchSpeed));
                    StartCoroutine(SwitchColor(nightlightRenderer.material, emissionColorId, onEmissionColor, nightlightSwitchSpeed));
                    nightOn = false;
                }
                else if (!nightOn && currTime < nightlightOnDelay)
                {
                    StartCoroutine(SwitchLight(nightlight, nightlightIntensity, 0f, nightlightSwitchSpeed));
                    StartCoroutine(SwitchColor(nightlightRenderer.material, emissionColorId, Color.black, nightlightSwitchSpeed));
                    nightOn = true;
                }

                counter += Time.deltaTime;
                yield return null;
            }
        }

        private IEnumerator SwitchLight(Light light, float from, float to, float speed)
        {
            if (to > from)
                light.gameObject.SetActive(true);

            float counter = 0f;

            while (counter < speed)
            {
                light.intensity = Mathf.Lerp(from, to, counter / speed);
                counter += Time.deltaTime;
                yield return null;
            }

            if (to < from)
                light.gameObject.SetActive(false);
        }

        private IEnumerator SwitchColor(Material material, int nameID, Color to, float speed)
        {
            float counter = 0f;

            Color from = material.GetColor(nameID);
            while (counter < speed)
            {
                material.SetColor(nameID, Color.Lerp(from, to, counter / speed));
                counter += Time.deltaTime;
                yield return null;
            }
        }
    }
}
