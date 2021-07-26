using UnityEngine;

namespace Magic.Core
{
    /// <summary>
    /// Отношения двух элементов между собой.
    /// </summary>
    [CreateAssetMenu(fileName = "Element1-Element2", menuName = "Magic/Relation")]
    public class ElementRelation : ScriptableObject
    {
        /// <summary>
        /// Сильный элемент.
        /// </summary>
        public Element left;
        /// <summary>
        /// Слабый элемент.
        /// </summary>
        public Element right;

        /// <summary>
        /// Вес сильного элемента над слабым.
        /// </summary>
        [Range(0f, 1f)]
        public float weight;
    }
}
