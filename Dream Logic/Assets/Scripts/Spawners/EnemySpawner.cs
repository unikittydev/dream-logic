using UnityEngine;

namespace Game
{
    /// <summary>
    /// Спавнер противников.
    /// </summary>
    public class EnemySpawner : MonoBehaviour
    {
        private PlayerController player;

        private Transform objectParent;

        [SerializeField]
        private EnemySpawnerSettings settings;

        private float counter;

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag(GameTags.player).GetComponent<PlayerController>();
            objectParent = GameObject.FindGameObjectWithTag(GameTags.objects).transform;
        }

        private void Update()
        {
            counter += Time.deltaTime;

            if (counter >= settings.frequency)
            {
                counter = 0f;

                Vector2 circleOffset = Random.insideUnitCircle.normalized * Random.Range(settings.minDistance, settings.maxDistance);
                Vector3 fallOffset = player.tr.forward * Mathf.Sqrt(2f * settings.height * player.speed / Physics.gravity.magnitude);
                Vector3 position = player.tr.position + fallOffset + new Vector3(circleOffset.x, settings.height, circleOffset.y);

                Instantiate(settings.prefabs[Random.Range(0, settings.prefabs.Length)], position, Quaternion.identity, objectParent);
            }
        }
    }
}
