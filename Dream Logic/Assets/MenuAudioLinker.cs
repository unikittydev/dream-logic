using UnityEngine;

namespace Game.Menu
{
    public class MenuAudioLinker : MonoBehaviour
    {
        public void PlaySound(string name) => AudioManager.instance.PlaySound(name);
    }
}
