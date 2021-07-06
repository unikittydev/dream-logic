using UnityEngine;

namespace Game.Dream
{
    /// <summary>
    /// Режим "Смотри под ноги!"
    /// </summary>
    public class FallingFloorDream : DreamMode
    {
        private FloorTile corruptedTilePrefab;

        private float scoreMultiplier = 1.5f;

        private float voidHeight = -10f;

        private float avgReplaceTime = .75f;
        private float replaceTimeOffset = .375f;
        private float replaceTime;

        private float replaceCounter;

        private void Awake()
        {
            corruptedTilePrefab = Resources.Load<FloorTile>("Prefabs/Void Tile");
            replaceTime = avgReplaceTime;
        }

        private void Update()
        {
            DreamSimulation.score += scoreMultiplier * Time.deltaTime;
            CheckPlayer();
            TryReplaceTile();
        }

        private void CheckPlayer()
        {
            if (DreamSimulation.player.tr.position.y < voidHeight)
            {
                DreamSimulation.WakeUp();
            }
        }

        private void TryReplaceTile()
        {
            replaceCounter += Time.deltaTime;
            if (replaceCounter > replaceTime)
            {
                DreamSimulation.floorSpawner.ReplaceRandomTile(corruptedTilePrefab, false);
                replaceCounter = 0f;
                replaceTime = avgReplaceTime + Random.Range(-replaceTimeOffset, replaceTimeOffset);
            }
        }
    }
}
