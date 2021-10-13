using System.Collections;
using UnityEngine;

namespace Game.Menu
{
    public class MenuLightSwitcher : MonoBehaviour
    {
        private static readonly string emissionKeyword = "_EMISSION";
        private static readonly int emissionColorId = Shader.PropertyToID("_EmissionColor");

        [SerializeField]
        private GameObject nightlight;
        [SerializeField]
        private MeshRenderer nightlightRenderer;
        [SerializeField]
        private GameObject roomLight;

        [SerializeField]
        private float nightlightOnDelay;
        [SerializeField]
        private float roomlightOnDelay;
        [SerializeField]
        private float roomlightOffDelay;
        [SerializeField]
        private float switchTime;

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

            while (counter < switchTime)
            {
                float currTime = switchTime * Mathf.Lerp(fromDaytime, toDaytime, 1f - counter / switchTime);

                nightlight.SetActive(currTime > nightlightOnDelay);
                nightlightRenderer.material.SetColor(emissionColorId, currTime > nightlightOnDelay ? Color.white : Color.black);

                roomLight.SetActive(fromDaytime > toDaytime && roomlightOnDelay < currTime && currTime < roomlightOffDelay);

                counter += Time.deltaTime;
                yield return null;
            }
        }
    }
}
