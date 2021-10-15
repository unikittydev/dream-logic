using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GamePool : MonoBehaviour
    {
        private readonly Dictionary<Object, Queue<Object>> pool = new Dictionary<Object, Queue<Object>>();

        private static GamePool _instance;
        public static GamePool instance => _instance;
        private void Awake()
        {
            if (instance == null)
            {
                _instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }

        public void AddPool<T>(T prefab, int capacity, Transform parent) where T : Object
        {
            if (!pool.ContainsKey(prefab))
            {
                Queue<Object> queue = new Queue<Object>(capacity);

                for (int i = 0; i < capacity; i++)
                {
                    T instance = CreateNewObject(prefab, Vector3.zero, Quaternion.identity, parent);
                    queue.Enqueue(instance);
                }

                pool.Add(prefab, queue);
            }
        }

        public void AddPool<T>(T[] prefabs, int capacity, Transform parent) where T : Object
        {
            foreach (var prefab in prefabs)
                AddPool(prefab, capacity, parent);
        }

        public void RemovePool<T>(T prefab, bool destroyObjects = false) where T : Object
        {
            if (pool.ContainsKey(prefab))
            {
                if (destroyObjects)
                    foreach (var instance in pool[prefab])
                    {
                        GameObject go = GetGameObject(instance);
                        if (!go.activeSelf)
                            Destroy(go);
                    }
                pool.Remove(prefab);
            }
        }

        public void RemovePool<T>(T[] prefabs, bool destroyObjects = false) where T : Object
        {
            foreach (var prefab in prefabs)
                RemovePool(prefab, destroyObjects);
        }

        public void Clear(bool destroyObjects = true)
        {
            if (destroyObjects)
                foreach (var kv in pool)
                {
                    foreach (var instance in kv.Value)
                    {
                        Destroy(GetGameObject(instance));
                    }
                }
            pool.Clear();
        }

        public void AddObject<T>(T obj) where T : Object
        {
            GameObject go = GetGameObject(obj);
            if (!HasPool(obj))
                Destroy(go);
            else
            {
                go.SetActive(false);
                go.transform.position = Vector3.zero;
                pool[GetPrefab(obj)].Enqueue(obj);
            }
        }

        public T GetObject<T>(T prefab) where T : Object
        {
            return GetObject(prefab, null, Vector3.zero, Quaternion.identity);
        }

        public T GetObject<T>(T prefab, Transform parent) where T : Object
        {
            return GetObject(prefab, parent, Vector3.zero, Quaternion.identity);
        }

        public T GetObject<T>(T prefab, Transform parent, Vector3 position) where T : Object
        {
            return GetObject(prefab, parent, position, Quaternion.identity);
        }

        public T GetObject<T>(T prefab, Transform parent, Vector3 position, Quaternion rotation) where T : Object
        {
            Object instance;
            if (!pool.ContainsKey(prefab))
                pool.Add(prefab, new Queue<Object>());
            if (pool[prefab].Count == 0)
            {
                instance = Instantiate(prefab, position, rotation, parent);
                instance.name = prefab.name;
            }
            else
            {
                instance = pool[prefab].Dequeue();
                GameObject go = GetGameObject(instance);
                go.SetActive(true);
                go.transform.parent = parent;
                go.transform.position = position;
                go.transform.rotation = rotation;
            }
            return instance as T;
        }

        public T GetRandomObject<T>(T[] prefabs) where T : Object
        {
            return GetObject(GetRandomPrefab(prefabs), null, Vector3.zero, Quaternion.identity);
        }

        public T GetRandomObject<T>(T[] prefabs, Transform parent) where T : Object
        {
            return GetObject(GetRandomPrefab(prefabs), parent, Vector3.zero, Quaternion.identity);
        }

        public T GetRandomObject<T>(T[] prefabs, Transform parent, Vector3 position) where T : Object
        {
            return GetObject(GetRandomPrefab(prefabs), parent, position, Quaternion.identity);
        }

        public T GetRandomObject<T>(T[] prefabs, Transform parent, Vector3 position, Quaternion rotation) where T : Object
        {
            return GetObject(GetRandomPrefab(prefabs), parent, position, rotation);
        }

        public bool HasPool<T>(T obj) where T : Object
        {
            foreach (var kv in pool)
                if (obj.name == kv.Key.name)
                    return true;
            return false;
        }

        private Object GetPrefab<T>(T obj) where T : Object
        {
            foreach (var kv in pool)
                if (obj.name == kv.Key.name)
                    return kv.Key;
            Debug.LogError($"Prefab of name {obj.name} is not found.");
            return null;
        }

        private T GetRandomPrefab<T>(T[] prefabs) where T : Object
        {
            return prefabs[Random.Range(0, prefabs.Length)];
        }

        private GameObject GetGameObject(Object obj)
        {
            if (obj is GameObject go)
            {
                return go;
            }
            else if (obj is Behaviour c)
            {
                return c.gameObject;
            }
            Debug.LogError($"Type {obj.GetType().Name} is incorrect.");
            return null;
        }

        private T CreateNewObject<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent) where T : Object
        {
            T instance = Instantiate(prefab, position, rotation, parent);
            GetGameObject(instance).SetActive(false);
            instance.name = prefab.name;
            return instance;
        }
    }
}
