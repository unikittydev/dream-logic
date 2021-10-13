using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace Game.Dream
{
    /// <summary>
    ///  ласс, выполн€ющий плавную смену тем.
    /// </summary>
    public class DreamThemeSwitcher : MonoBehaviour
    {
        private const string themesLabel = "Dream Theme";

        private DreamCycle cycle;

        [SerializeField]
        private AssetReference defaultThemeRef;
        private DreamTheme defaultTheme;

        private DreamTheme _currentTheme;
        public DreamTheme currentTheme => _currentTheme;

        [SerializeField]
        private CinemachineVirtualCamera currentVirtualCamera;
        [SerializeField]
        private CinemachineVirtualCamera nextVirtualCamera;

        [SerializeField]
        private Volume currentVolume;
        [SerializeField]
        private Volume nextVolume;
        [SerializeField]
        private Volume switchVolume;

        [SerializeField]
        private ObjectSpawner objectSpawnerPrefab;
        [SerializeField]
        private FloorSpawner floorSpawnerPrefab;

        [SerializeField]
        private Transform environment;

        private List<GameObject> oldSpawners;
        private List<GameObject> newSpawners;

        private List<IResourceLocation> themesLocations = new List<IResourceLocation>();

        private void Awake()
        {
            cycle = GetComponent<DreamCycle>();

            var handle = Addressables.LoadResourceLocationsAsync(themesLabel);
            handle.Completed += _ =>
            {
                themesLocations.AddRange(handle.Result);
            };

            var defaultThemeHandle = defaultThemeRef.LoadAssetAsync<DreamTheme>();
            defaultThemeHandle.WaitForCompletion();
            defaultTheme = defaultThemeHandle.Result;
        }

        public IEnumerator LoadRandomTheme()
        {
            var handle = Addressables.LoadAssetAsync<DreamTheme>(themesLocations[Random.Range(0, themesLocations.Count)]);
            yield return handle;
            _currentTheme = handle.Result;
            LoadSpawners(currentTheme.objectSpawnerSettings, currentTheme.floorSpawnerSettings);
        }

        public void SetDefaultTheme()
        {
            _currentTheme = defaultTheme;
            LoadSpawners(defaultTheme.objectSpawnerSettings, defaultTheme.floorSpawnerSettings);
            SwitchTheme();
        }

        public void SwitchTheme()
        {
            AudioManager.instance.Play("theme.switch");
            AudioManager.instance.PlayTheme(currentTheme.themeSound);
            //StartCoroutine(SwitchEffect(1f));
            StartCoroutine(SetSkyColor(currentTheme.skyColor, 2f));
            SetCameraSettings(currentTheme.cameraAngle, currentTheme.cameraDistance);
            //StartCoroutine(SetActiveVolume(currentTheme.postprocessing, 2f));
            //ClearEnvironment();
            ReplacePlayer(currentTheme.playerPrefab);
            ReplaceSpawners();
        }

        private IEnumerator SwitchEffect(float switchTime)
        {
            float switchCounter = 0f;

            switchVolume.weight = 0f;
            switchVolume.enabled = true;
            while (switchCounter <= switchTime * .5f)
            {
                switchVolume.weight = Mathf.Clamp01(switchCounter / switchTime);
                switchCounter += Time.deltaTime;
                yield return null;
            }
            while (switchCounter <= switchTime)
            {
                switchVolume.weight = Mathf.Clamp01(1f - switchCounter / switchTime);
                switchCounter += Time.deltaTime;
                yield return null;
            }
            switchVolume.enabled = false;
        }

        private IEnumerator SetSkyColor(Color newColor, float switchTime)
        {
            var cam = Camera.main;
            Color oldColor = cam.backgroundColor;

            float switchCounter = 0f;
            while (switchCounter < switchTime)
            {
                cam.backgroundColor = Color.Lerp(oldColor, newColor, switchCounter / switchTime);

                switchCounter += Time.deltaTime;
                yield return null;
            }
        }

        private void SetCameraSettings(float angle, float distance)
        {
            nextVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance = distance;
            nextVirtualCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.Value = angle;
            nextVirtualCamera.Priority = 2;
            currentVirtualCamera.Priority = 1;

            var temp = currentVirtualCamera;
            currentVirtualCamera = nextVirtualCamera;
            nextVirtualCamera = temp;
        }

        private IEnumerator SetActiveVolume(VolumeProfile newProfile, float switchTime)
        {
            currentVolume.weight = 0f;
            nextVolume.weight = 1f;

            nextVolume.profile = currentVolume.profile;
            currentVolume.profile = newProfile;

            nextVolume.enabled = true;

            float switchCounter = 0f;
            while (switchCounter <= switchTime)
            {
                switchCounter += Time.deltaTime;

                currentVolume.weight = Mathf.Clamp01(switchCounter / switchTime);
                nextVolume.weight = 1f - currentVolume.weight;

                yield return null;
            }

            currentVolume.weight = 1f;
            nextVolume.weight = 0f;
            nextVolume.enabled = false;
        }

        private void ClearEnvironment()
        {
            for (int i = 0; i < environment.childCount; i++)
            {
                Destroy(environment.GetChild(i).gameObject);
            }
        }

        private void ReplacePlayer(PlayerController newPlayer)
        {
            var player = DreamGame.player;

            Vector3 position = player.tr.position;
            Quaternion rotation = player.tr.rotation;
            Transform parent = player.tr.parent;

            Destroy(player.gameObject);
            DreamGame.player = player = Instantiate(newPlayer, position, rotation, parent);

            currentVirtualCamera.LookAt = nextVirtualCamera.LookAt = player.tr;
            currentVirtualCamera.Follow = nextVirtualCamera.Follow = player.tr;
        }

        private void LoadSpawners(ObjectSpawnerSettings[] objSettings, FloorSpawnerSettings floorSettings)
        {
            newSpawners = new List<GameObject>();

            for (int i = 0; i < objSettings.Length; i++)
            {
                if (objSettings[i].spawnerCreateChance > Random.value)
                {
                    var spawner = Instantiate(objectSpawnerPrefab);
                    spawner.gameObject.SetActive(false);
                    newSpawners.Add(spawner.gameObject);
                    spawner.Create(objSettings[i], -Mathf.Abs(floorSettings.smoothTime));
                }
            }

            var floorSpawner = Instantiate(floorSpawnerPrefab);
            floorSpawner.gameObject.SetActive(false);
            newSpawners.Add(floorSpawner.gameObject);
            floorSpawner.Create(floorSettings);
        }

        private void ReplaceSpawners()
        {
            if (oldSpawners != null)
                for (int i = 0; i < oldSpawners.Count; i++)
                {
                    if (oldSpawners[i].TryGetComponent<FloorSpawner>(out var floor))
                        floor.Clear();
                    if (oldSpawners[i].TryGetComponent<ObjectSpawner>(out var obj))
                        obj.Clear();

                    Destroy(oldSpawners[i].gameObject);
                }
            if (newSpawners != null)
                for (int i = 0; i < newSpawners.Count; i++)
                    newSpawners[i].gameObject.SetActive(true);
            oldSpawners = newSpawners;
        }
    }
}
