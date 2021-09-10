using UnityEngine;

namespace Game
{
    public interface IEventData
    {

    }

    public class PlayerHitData : IEventData
    {
        public ControllerColliderHit hit;
    }

    public class ScoreChangedData : IEventData
    {
        public float value;
    }
}