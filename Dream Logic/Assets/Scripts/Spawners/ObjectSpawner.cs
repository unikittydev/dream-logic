using Game.Dream;
using System.Collections.Generic;
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
        private float voidHeight;

        private List<GameObject> objects = new List<GameObject>();

        public void Create(ObjectSpawnerSettings newSettings, float voidHeight)
        {
            settings = newSettings;
            this.voidHeight = voidHeight;
            DreamGame.pool.AddPool(newSettings.prefabs, Mathf.CeilToInt(DreamGame.cycle.totalTime / newSettings.spawnTime), DreamGame.environment);
        }

        public void Clear()
        {
            foreach (var go in objects)
                if (go.activeSelf)
                    DreamGame.pool.AddObject(go);
            DreamGame.pool.RemovePool(settings.prefabs, true);
        }

        private void Update()
        {
            TrySpawnObject();
            CheckVoidObjects();
        }

        private void TrySpawnObject()
        {
            counter += Time.deltaTime * DreamGame.difficulty.objectSpawnFrequencyMultiplier;

            if (counter > settings.spawnTime)
            {
                counter = 0f;

                Vector2 circleOffset = Random.insideUnitCircle.normalized * Random.Range(settings.minDistance, settings.maxDistance);
                Vector3 fallOffset = DreamGame.player.tr.forward * Mathf.Sqrt(2f * settings.height * DreamGame.player.speed / Physics.gravity.magnitude);
                Vector3 position = DreamGame.player.tr.position + fallOffset + new Vector3(circleOffset.x, settings.height, circleOffset.y);

                GameObject go = DreamGame.pool.GetRandomObject(settings.prefabs, DreamGame.environment, position);
                objects.Add(go);
            }
        }

        private void CheckVoidObjects()
        {
            foreach (var obj in objects)
                if (obj.transform.position.y < voidHeight)
                    DreamGame.pool.AddObject(obj);
        }
    }
}
