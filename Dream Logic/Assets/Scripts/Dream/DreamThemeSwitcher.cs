using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering;

namespace Game.Dream
{
    /// <summary>
    ///  ласс, выполн€ющий плавную смену тем.
    /// </summary>
    public class DreamThemeSwitcher : MonoBehaviour
    {
        private const string themesLabel = "Dream Theme";

        [SerializeField]
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

        private Transform environment;

        private List<ObjectSpawner> objectSpawners;

        private DreamTheme[] allThemes;

        private void Awake()
        {
            Addressables.LoadAssetsAsync<DreamTheme>(themesLabel, null).Completed += objects =>
            {
                Debug.Log(objects.Result.Count);
                allThemes = objects.Result as DreamTheme[];
            };

            objectSpawners = new List<ObjectSpawner>(FindObjectsOfType<ObjectSpawner>());

            environment = GameObject.FindGameObjectWithTag(GameTags.environment).transform;
        }

        public void SetDefaultTheme()
        {
            SwitchThemes(defaultTheme);
        }

        public void SetRandomTheme()
        {
            SwitchThemes(allThemes[Random.Range(0, allThemes.Length)]);
        }

        private void SwitchThemes(DreamTheme newTheme)
        {
            AudioManager.instance.Play("theme.switch");
            AudioManager.instance.PlayTheme(newTheme.themeSound);
            _currentTheme = newTheme;
            StartCoroutine(SwitchEffect(1f));
            StartCoroutine(SetSkyColor(newTheme.skyColor, 2f));
            SetCameraSettings(newTheme.cameraAngle, newTheme.cameraDistance);
            StartCoroutine(SetActiveVolume(newTheme.postprocessing, 2f));
            DreamSimulation.floorSpawner.Refresh(newTheme.floorSpawnerSettings);
            ClearEnvironment();
            ReplacePlayer(newTheme.playerPrefab);
            ReplaceSpawners(newTheme.objectSpawnerSettings);
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
            var player = DreamSimulation.player;

            Vector3 position = player.tr.position;
            Quaternion rotation = player.tr.rotation;
            Transform parent = player.tr.parent;

            Destroy(player.gameObject);
            DreamSimulation.player = player = Instantiate(newPlayer, position, rotation, parent);

            currentVirtualCamera.LookAt = nextVirtualCamera.LookAt = player.tr;
            currentVirtualCamera.Follow = nextVirtualCamera.Follow = player.tr;
        }

        private void ReplaceSpawners(ObjectSpawnerSettings[] settings)
        {
            for (int i = 0; i < objectSpawners.Count; i++)
            {
                Destroy(objectSpawners[i].gameObject);
            }
            objectSpawners.Clear();
            for (int i = 0; i < settings.Length; i++)
            {
                if (settings[i].spawnerCreateChance > Random.value)
                {
                    var spawner = Instantiate(objectSpawnerPrefab);
                    objectSpawners.Add(spawner);
                    spawner.Refresh(settings[i]);
                }
            }
        }
    }
}
