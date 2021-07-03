using Game.Dream;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// Коллайдер игрока в режиме "Беги и уклоняйся!"
    /// </summary>
    public class RunAvoidCollider : MonoBehaviour
    {
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (hit.gameObject.CompareTag(GameTags.enemy))
            {
                DreamSimulation.WakeUp();
            }
        }
    }
}
