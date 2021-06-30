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
        public void Spawn(float height, float time)
        {
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
            while (Mathf.Abs(transform.position.y - height) > epsilon)
            {
                float damp = Mathf.SmoothDamp(transform.position.y, height, ref currSpeed, time);
                transform.position = new Vector3(transform.position.x, damp, transform.position.z);
                yield return null;
            }
            if (destroy)
                Destroy(gameObject);
        }
    }
}
