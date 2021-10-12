using UnityEngine;

namespace Game
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

        public void SetDaytime(float time)
        {
            sun.intensity = time;
            sun.transform.rotation = Quaternion.Lerp(dayTargetRotation.rotation, nightTargetRotation.rotation, time);
            camera.backgroundColor = skyGradient.Evaluate(time);
        }
    }
}

