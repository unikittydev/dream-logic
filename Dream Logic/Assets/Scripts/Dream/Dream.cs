using UnityEngine;

/// <summary>
/// Абстрактный класс сна.
/// </summary>
public abstract class Dream : MonoBehaviour
{
    protected abstract void OnEnable();

    protected abstract void OnDisable();
}
