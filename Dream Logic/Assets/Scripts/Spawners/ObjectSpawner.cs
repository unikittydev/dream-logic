using Game.Dream;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// Спавнер противников.
    /// </summary>
    public class ObjectSpawner : MonoBehaviour
    {
        [SerializeField]
        private ObjectSpawnerSettings settings;

        private float counter;

        public void Refresh(ObjectSpawnerSettings newSettings)
        {
            settings = newSettings;
        }

        private void Update()
        {
            counter += Time.deltaTime;

            if (counter >= settings.frequency)
            {
                counter = 0f;

                Vector2 circleOffset = Random.insideUnitCircle.normalized * Random.Range(settings.minDistance, settings.maxDistance);
                Vector3 fallOffset = DreamSimulation.player.tr.forward * Mathf.Sqrt(2f * settings.height * DreamSimulation.player.speed / Physics.gravity.magnitude);
                Vector3 position = DreamSimulation.player.tr.position + fallOffset + new Vector3(circleOffset.x, settings.height, circleOffset.y);

                Instantiate(settings.prefabs[Random.Range(0, settings.prefabs.Length)], position, Quaternion.identity, DreamSimulation.environment);
            }
        }
    }
}
