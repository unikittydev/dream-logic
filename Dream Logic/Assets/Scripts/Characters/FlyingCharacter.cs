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
        }

        private void Start()
        {
            startHeight = model.localPosition.y;
        }

        private void Update()
        {
            Fly();
            UpdateFlyCounter();
        }

        private void UpdateFlyCounter()
        {
            if (riseCounter > riseTime)
            {
                riseCounter = 0f;
                rise = !rise;
            }

            riseCounter += Time.deltaTime;
        }

        private void Fly()
        {
            Vector3 pos = model.localPosition;
            pos.y = rise ?
                Mathf.Lerp(startHeight, startHeight + height, riseCounter / riseTime) :
                Mathf.Lerp(startHeight + height, startHeight, riseCounter / riseTime);
            model.localPosition = pos;
        }
    }
}
