using System.Collections;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// Класс плитки пола.
    /// </summary>
    public class FloorTile : MonoBehaviour
    {
        private const float epsilon = .1f;

        private Transform tr;
        [SerializeField]
        private Transform model;

        private void Awake()
        {
            tr = transform;
        }

        public void Spawn(float height, float offset, float time)
        {
            Vector3 pos = model.position;
            pos.y += offset;
            model.position = pos;

            StartCoroutine(Move(height, time, false));
        }

        public void Despawn(float height, float time)
        {
            StopAllCoroutines();
            StartCoroutine(Move(height, time, true));
        }

        private IEnumerator Move(float height, float time, bool destroy)
        {
            float currSpeed = 0f;
            while (Mathf.Abs(tr.position.y - height) > epsilon)
            {
                float damp = Mathf.SmoothDamp(tr.position.y, height, ref currSpeed, time);
                tr.position = new Vector3(tr.position.x, damp, tr.position.z);
                yield return null;
            }
            if (destroy)
                Destroy(gameObject);
        }
    }
}
