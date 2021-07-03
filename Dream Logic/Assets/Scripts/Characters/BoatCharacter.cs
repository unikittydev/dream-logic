using UnityEngine;

namespace Game
{
    /// <summary>
    /// �������� � �����.
    /// </summary>
    public class BoatCharacter : MonoBehaviour
    {
        [SerializeField]
        private Transform leftPaddle;
        [SerializeField]
        private Transform rightPaddle;

        [SerializeField]
        private float rowingSpeed;

        private void Update()
        {
            RotatePaddle(leftPaddle, 1f);
            RotatePaddle(rightPaddle, -1f);
        }

        private void RotatePaddle(Transform paddle, float factor)
        {
            var rot = paddle.eulerAngles;
            rot.z += rowingSpeed * factor * Time.deltaTime;
            paddle.eulerAngles = rot;
        }
    }
}

