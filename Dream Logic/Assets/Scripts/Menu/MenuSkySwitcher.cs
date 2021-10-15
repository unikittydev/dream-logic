using System.Collections;
using UnityEngine;

namespace Game.Menu
{
    public class MenuSkySwitcher : MonoBehaviour
    {
        [SerializeField]
        private Camera cam;

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

        public void SetDaytime(float time)
        {
            sun.intensity = time;
            sun.transform.rotation = Quaternion.Lerp(nightTargetRotation.rotation, dayTargetRotation.rotation, time);
            cam.backgroundColor = skyGradient.Evaluate(time);
        }

        public Coroutine SwitchDaytime(float from, float to)
        {
            return StartCoroutine(SwitchDaytime_Internal(from, to));
        }

        private IEnumerator SwitchDaytime_Internal(float from, float to)
        {
            if (from < to)
                yield return new WaitForSeconds(startDelay);

            float counter = 0f;

            while (counter < switchTime)
            {
                float currTime = Mathf.Lerp(from, to, counter / switchTime);

                SetDaytime(currTime);

                counter += Time.deltaTime;
                yield return null;
            }
        }
    }
}

