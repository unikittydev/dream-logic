using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Dream
{
    /// <summary>
    /// Сон.
    /// </summary>
    public class Dream : MonoBehaviour
    {
        public float maxTime { get; set; }
        public float timeCounter { get; set; }

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

            Camera.main.backgroundColor = theme.skyColor;

            GameObject.FindGameObjectWithTag(GameTags.volume).GetComponent<Volume>().profile = theme.postprocessing;

            // игрок
            // враги
            // препятствия
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
    }
}
