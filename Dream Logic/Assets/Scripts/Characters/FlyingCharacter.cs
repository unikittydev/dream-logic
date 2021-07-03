using UnityEngine;

namespace Game
{
    /// <summary>
    /// Летающий персонаж.
    /// </summary>
    public class FlyingCharacter : MonoBehaviour
    {
        private Transform model;

        [SerializeField]
        private float height;
        [SerializeField]
        private float riseTime;
        private float riseCounter;

        private float startHeight;

        private bool rise = true;

        private void Awake()
        {
            model = transform.GetChild(0);
            startHeight = model.localPosition.y;
        }

        private void Update()
        {
            if (riseCounter > riseTime)
            {
                riseCounter = 0f;
                rise = !rise;
            }

            Vector3 pos = model.localPosition;
            pos.y = height * Mathf.Sin(Time.time * riseTime + startHeight);
            //pos.y = rise ?
            //    Mathf.Lerp(startHeight, startHeight + height, riseCounter / riseTime) :
            //    Mathf.Lerp(startHeight + height, startHeight, riseCounter / riseTime);
            model.localPosition = pos;

            riseCounter += Time.deltaTime;
        }
    }
}
