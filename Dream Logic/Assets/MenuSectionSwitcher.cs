using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Menu
{
    public class MenuSectionSwitcher : MonoBehaviour
    {
        [SerializeField]
        private PhysicsRaycaster raycaster;

        [SerializeField]
        private MenuSection currentSection;

        [SerializeField]
        private float switchTime;

        public void SwitchSection(MenuSection section)
        {
            StartCoroutine(SwitchSection_Internal(currentSection, section));
        }

        private IEnumerator SwitchSection_Internal(MenuSection from, MenuSection to)
        {
            raycaster.enabled = false;

            foreach (var item in from.onScreenUI)
                GameUI.FadeUI(item, false, 0f, switchTime);

            yield return new WaitForSeconds(switchTime);

            to.virtualCamera.Priority++;
            from.virtualCamera.Priority--;

            while (CinemachineCore.Instance.IsLive(from.virtualCamera))
                yield return null;

            foreach (var item in to.onScreenUI)
                GameUI.FadeUI(item, true, 1f, switchTime);

            yield return new WaitForSeconds(switchTime);

            currentSection = to;

            raycaster.enabled = true;
        }
    }
}
