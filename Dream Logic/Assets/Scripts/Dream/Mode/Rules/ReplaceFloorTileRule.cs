using UnityEngine;

namespace Game.Dream
{
    public class ReplaceFloorTileRule : DreamRule
    {
        private FloorSpawner spawner;

        [SerializeField]
        private FloorTile replacedTilePrefab;

        [SerializeField]
        private float avgReplaceTime = .75f;
        [SerializeField]
        private float replaceTimeOffset = .375f;
        private float replaceTime;

        private float replaceCounter;

        private void Awake()
        {
            replaceTime = avgReplaceTime;
            spawner = FindObjectOfType<FloorSpawner>(true);
        }

        private void Update()
        {
            replaceCounter += Time.deltaTime;
            if (replaceCounter > replaceTime)
            {
                spawner.ReplaceRandomTile(replacedTilePrefab, false);
                replaceCounter = 0f;
                replaceTime = avgReplaceTime + Random.Range(-replaceTimeOffset, replaceTimeOffset);
            }
        }
    }
}
