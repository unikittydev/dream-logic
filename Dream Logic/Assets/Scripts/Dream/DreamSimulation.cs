using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Dream
{
    /// <summary>
    /// Симуляция снов.
    /// </summary>
    public class DreamSimulation : MonoBehaviour
    {
        private const string themesPath = "Dream Themes/";

        private static PlayerController _player;
        public static PlayerController player => _player;

        private static FloorSpawner _floorSpawner;
        public static FloorSpawner floorSpawner => _floorSpawner;

        [SerializeField]
        private ObjectSpawner _objectSpawnerPrefab;
        private static ObjectSpawner objectSpawnerPrefab;

        private static List<ObjectSpawner> _objectSpawners;
        public static List<ObjectSpawner> objectSpawners => _objectSpawners;

        private static Transform _environment;
        public static Transform environment => _environment;

        private static float timeCounter;
        private static float maxTime = 15f;

        private static DreamTheme[] allThemes;

        private static DreamTheme _currentTheme;
        public static DreamTheme currentTheme => _currentTheme;

        [SerializeField]
        private DreamTheme _transitionTheme;
        private static DreamTheme transitionTheme;

        private void Awake()
        {
            _player = FindObjectOfType<PlayerController>();
            _floorSpawner = FindObjectOfType<FloorSpawner>();
            _objectSpawners = new List<ObjectSpawner>(FindObjectsOfType<ObjectSpawner>());
            _environment = GameObject.FindGameObjectWithTag(GameTags.environment).transform;
            objectSpawnerPrefab = _objectSpawnerPrefab;

            allThemes = Resources.LoadAll<DreamTheme>(themesPath);
            transitionTheme = _transitionTheme;

            timeCounter = maxTime;
        }

        private void Update()
        {
            if (timeCounter >= maxTime)
            {
                StartCoroutine(StartNewDreamCycle());
                timeCounter = 0f;
            }
            else
            {
                timeCounter += Time.deltaTime;
            }
        }

        public static void ApplyDefaultTheme()
        {
            ApplyTheme(transitionTheme);
        }

        public static void ApplyRandomTheme()
        {
            int index = Random.Range(0, allThemes.Length);
            ApplyTheme(allThemes[index]);
        }

        public static void ApplyTheme(DreamTheme theme)
        {
            _currentTheme = theme;

            UpdateCameraState();

            GameObject.FindGameObjectWithTag(GameTags.volume).GetComponent<Volume>().profile = theme.postprocessing;

            ClearEnvironment();
            floorSpawner.Refresh(theme.floorSpawnerSettings);
            ReplacePlayer(theme.playerPrefab);
            ReplaceSpawners(theme.objectSpawnerSettings);
        }

        public static void WakeUp()
        {
            print("You woke up");
            ApplyDefaultTheme();
            timeCounter = float.NegativeInfinity;
        }

        public IEnumerator StartNewDreamCycle()
        {
            ApplyDefaultTheme();
            yield return new WaitForSeconds(3f);
            ApplyRandomTheme();
        }

        private static void ReplacePlayer(PlayerController newPlayer)
        {
            Vector3 position = player.tr.position;
            Quaternion rotation = player.tr.rotation;
            Transform parent = player.tr.parent;

            Destroy(player.gameObject);
            _player = Instantiate(newPlayer, position, rotation, parent);

            var virtualCam = Camera.main.GetComponent<Cinemachine.CinemachineVirtualCamera>();
            virtualCam.LookAt = player.tr;
            virtualCam.Follow = player.tr;
        }

        private static void ClearEnvironment()
        {
            for (int i = 0; i < environment.childCount; i++)
            {
                Destroy(environment.GetChild(i).gameObject);
            }
        }

        private static void ReplaceSpawners(ObjectSpawnerSettings[] settings)
        {
            for (int i = 0; i < objectSpawners.Count; i++)
            {
                Destroy(objectSpawners[i].gameObject);
            }
            objectSpawners.Clear();
            for (int i = 0; i < settings.Length; i++)
            {
                var spawner = Instantiate(objectSpawnerPrefab);
                objectSpawners.Add(spawner);
                spawner.Refresh(settings[i]);
            }
        }

        private static void UpdateCameraState()
        {
            var cam = Camera.main.GetComponent<Cinemachine.CinemachineVirtualCamera>();
            var follow = cam.GetCinemachineComponent<Cinemachine.CinemachineFramingTransposer>();

            Vector3 rot = cam.transform.eulerAngles;
            rot.x = currentTheme.cameraAngle;
            cam.transform.eulerAngles = rot;

            follow.m_CameraDistance = currentTheme.cameraDistance;

            Camera.main.backgroundColor = currentTheme.skyColor;
        }
    }
}
