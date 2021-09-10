using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Dream
{
    /// <summary>
    ///  ласс, выполн€ющий смену режима игры.
    /// </summary>
    public class DreamModeSwitcher : MonoBehaviour
    {
        private const string modesLabel = "Dream Mode";

        private DefaultDreamMode _currentMode;
        public DefaultDreamMode currentMode => _currentMode;

        private DefaultDreamMode[] dreamModes;

        private void Awake()
        {
            var handle = Addressables.LoadAssetsAsync<GameObject>(modesLabel, null);
            var list = handle.WaitForCompletion();
            dreamModes = new DefaultDreamMode[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                dreamModes[i] = list[i].GetComponent<DefaultDreamMode>();
            }
        }

        private void OnDestroy()
        {
            if (gameObject.TryGetComponent<DefaultDreamMode>(out var mode))
                Destroy(mode.gameObject);
        }
        
        public void SetDefaultMode()
        {
            SetMode(DreamModeFlag.Blank);
        }

        public void SetRandomMode(DreamTheme theme)
        {
            var flag = GetAllowedModeFlag(theme.allowedModes);
            SetMode(flag);
        }

        private void SetMode(DreamModeFlag flag)
        {
            if (currentMode != null)
                Destroy(currentMode.gameObject);

            foreach (var mode in dreamModes)
                if (mode.mode == flag)
                {
                    _currentMode = Instantiate(mode, transform);
                    break;
                }
        }

        private DreamModeFlag GetAllowedModeFlag(DreamModeFlag flag)
        {
            var values = new List<DreamModeFlag>(Enum.GetValues(typeof(DreamModeFlag)) as DreamModeFlag[]);
            for (int i = values.Count - 1; i >= 0; i--)
            {
                if (!flag.HasFlag(values[i]))
                {
                    values.RemoveAt(i);
                }
            }
            return values[UnityEngine.Random.Range(0, values.Count)];
        }
    }
}
