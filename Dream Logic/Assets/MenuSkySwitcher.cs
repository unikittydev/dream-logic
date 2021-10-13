using System.Collections;
using UnityEngine;

namespace Game.Menu
{
    public class MenuSkySwitcher : MonoBehaviour
    {
        [SerializeField]
        private new Camera camera;

        [SerializeField]
        private Light sun;
        [SerializeField]
        private Transform dayTargetRotation;
        [SerializeField]
        private Transform nightTargetRotation;

        [SerializeField]
        private Gradient skyGradient;

        [SerializeField]
        private float startDelay;
        [SerializeField]
        private float switchTime;

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        public Coroutine SetDaytime(float from, float to)
        {
            return StartCoroutine(SetDaytime_Internal(from, to));
        }

        private IEnumerator SetDaytime_Internal(float from, float to)
        {
            yield return new WaitForSeconds(startDelay);

            float counter = 0f;

            while (counter < switchTime)
            {
                float currTime = Mathf.Lerp(from, to, counter / switchTime);

                sun.intensity = currTime;
                sun.transform.rotation = Quaternion.Lerp(nightTargetRotation.rotation, dayTargetRotation.rotation, currTime);
                camera.backgroundColor = skyGradient.Evaluate(currTime);

                counter += Time.deltaTime;
                yield return null;
            }
        }
    }
}

