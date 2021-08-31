using UnityEngine;

namespace Game
{
    public class GameTouchscreen : MonoBehaviour
    {
        private void Awake()
        {
            if (!Application.isMobilePlatform)
            {
                Destroy(gameObject);
            }
        }
    }
}
