using UnityEngine;

namespace Game
{
    /// <summary>
    /// Ходящий персонаж.
    /// </summary>
    public class WalkableCharacter : MonoBehaviour
    {
        private CharacterController cc;

        private Transform model;

        [SerializeField]
        private float height;
        [SerializeField]
        private float riseTime;
        private float riseCounter;

        private float startHeight;

        private float walkMultiplier = 1f;

        private bool rise = true;

        private void Awake()
        {
            cc = GetComponent<CharacterController>();
            model = transform.GetChild(0);
            startHeight = model.localPosition.y;
        }

        private void Update()
        {
            walkMultiplier = cc != null && !cc.isGrounded ? 0f : 1f;

            if (riseCounter > riseTime)
            {
                riseCounter = 0f;
                rise = !rise;
            }

            Vector3 pos = model.localPosition;
            pos.y = rise ?
                Mathf.Lerp(startHeight, startHeight + height * walkMultiplier, riseCounter / riseTime) :
                Mathf.Lerp(startHeight + height * walkMultiplier, startHeight, riseCounter / riseTime);
            model.localPosition = pos;

            riseCounter += Time.deltaTime;
        }
    }
}
