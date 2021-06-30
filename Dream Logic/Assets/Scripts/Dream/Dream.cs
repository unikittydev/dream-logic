using System.Collections;
using UnityEngine;

namespace Game.Dream
{
    /// <summary>
    /// Сон.
    /// </summary>
    public class Dream : MonoBehaviour
    {
        private float maxTime;
        private float timeCounter;

        private DreamTheme theme;
        private DreamBehaviour rules;

        public void Play(DreamTheme theme, DreamBehaviour rules)
        {
            ApplyTheme(theme);
            ApplyRules(rules);

            timeCounter = 0f;
            //maxTime = rules.settings.averageTime;
        }

        public void Stop()
        {

        }

        private void ApplyTheme(DreamTheme theme)
        {
            this.theme = theme;

            StartCoroutine(LerpCameraState(theme.cameraAngle, theme.cameraDistance));
        }

        private IEnumerator LerpCameraState(float angle, float distance)
        {
            float epsilon = .1f;
            float rotateSpeed = .1f, distanceSpeed = .01f;

            var cam = Camera.main.GetComponent<Cinemachine.CinemachineVirtualCamera>();
            var follow = cam.GetCinemachineComponent<Cinemachine.CinemachineFramingTransposer>();

            while (Mathf.DeltaAngle(angle, cam.transform.rotation.eulerAngles.x) > epsilon && Mathf.Abs(follow.m_CameraDistance - distance) > epsilon)
            {
                Vector3 rot = cam.transform.rotation.eulerAngles;
                float newAngle = Mathf.LerpAngle(rot.x, angle, rotateSpeed * Time.deltaTime);
                rot.x = newAngle;
                cam.transform.rotation = Quaternion.Euler(rot);

                follow.m_CameraDistance = Mathf.LerpUnclamped(follow.m_CameraDistance, distance, distanceSpeed * Time.deltaTime);

                yield return null;
            }
        }

        private void ApplyRules(DreamBehaviour rules)
        {
            this.rules = rules;
        }

        private void Update()
        {
            if (timeCounter >= maxTime)
            {
                DreamSimulation.StartNewDreamCycle();
            }
            else
            {
                timeCounter += Time.deltaTime;
            }
        }
    }
}
