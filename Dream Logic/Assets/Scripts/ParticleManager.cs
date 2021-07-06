using UnityEngine;

namespace Game
{
    /// <summary>
    /// Менеджер частиц.
    /// </summary>
    public class ParticleManager : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem _poof;
        private static ParticleSystem poof;

        private void Awake()
        {
            poof = _poof;
        }

        public static void Poof(Vector3 position)
        {
            if (poof == null)
                return;
            poof.transform.position = position;
            poof.Emit(1);
        }
    }
}
