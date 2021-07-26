using UnityEngine;

namespace Magic.Core
{
    /// <summary>
    /// ��������� ���� ��������� ����� �����.
    /// </summary>
    [CreateAssetMenu(fileName = "Element1-Element2", menuName = "Magic/Relation")]
    public class ElementRelation : ScriptableObject
    {
        /// <summary>
        /// ������� �������.
        /// </summary>
        public Element left;
        /// <summary>
        /// ������ �������.
        /// </summary>
        public Element right;

        /// <summary>
        /// ��� �������� �������� ��� ������.
        /// </summary>
        [Range(0f, 1f)]
        public float weight;
    }
}
