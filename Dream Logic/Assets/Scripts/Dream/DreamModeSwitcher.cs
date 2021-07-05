using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Dream
{
    /// <summary>
    /// Класс, выполняющий смену режима игры.
    /// </summary>
    public class DreamModeSwitcher : MonoBehaviour
    {
        private Dictionary<DreamModeFlag, Type> modeComponents = new Dictionary<DreamModeFlag, Type>() 
        {
            { DreamModeFlag.Blank,           null },
            { DreamModeFlag.RunAvoid,       typeof(RunAvoidDream) },
            { DreamModeFlag.FallingFloor,   typeof(FallingFloorDream) },
            { DreamModeFlag.RunCatch,       typeof(RunCatchDream) },
        };

        private Dictionary<DreamModeFlag, string> modeDesc = new Dictionary<DreamModeFlag, string>()
        {
            { DreamModeFlag.Blank, "" },
            { DreamModeFlag.RunAvoid, "Беги и уклоняйся!" },
            { DreamModeFlag.FallingFloor, "Смотри под ноги!" },
            { DreamModeFlag.RunCatch, "Беги и лови!" },
        };

        private DreamModeFlag _currMode;

        private void OnDisable()
        {
            if (gameObject.TryGetComponent<DreamMode>(out var mode))
                Destroy(mode);
        }
        
        public string GetCurrentModeDescription()
        {
            return modeDesc[_currMode];
        }

        public void SetDefaultMode()
        {
            SetMode(DreamModeFlag.Blank);
        }

        public void SetAllowedMode(DreamTheme theme)
        {
            var flag = GetAllowedModeFlag(theme.allowedModes);
            SetMode(flag);
        }

        private void SetMode(DreamModeFlag flag)
        {
            _currMode = flag;

            if (gameObject.TryGetComponent<DreamMode>(out var mode))
                Destroy(mode);

            Type componentType = modeComponents[flag];
            if (componentType != null)
                gameObject.AddComponent(componentType);
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
