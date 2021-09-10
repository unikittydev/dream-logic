using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public static class EventManager
    {
        private static readonly Dictionary<Type, UnityEvent<Component, IEventData>> eventDict = new Dictionary<Type, UnityEvent<Component, IEventData>>();

        public static UnityEvent<Component, PlayerHitData> OnPlayerHit { get; } = new UnityEvent<Component, PlayerHitData>();
        public static UnityEvent<ScoreChangedData> OnScoreChange { get; } = new UnityEvent<ScoreChangedData>();

        public static void PostEvent<T>(Component sender, T data) where T : IEventData
        {
            if (eventDict.ContainsKey(typeof(T)))
                eventDict[typeof(T)].Invoke(sender, data);
        }
    }
}
