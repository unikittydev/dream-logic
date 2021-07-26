using System.Runtime.InteropServices;
using UnityEngine;

namespace Magic.Core
{
    /// <summary>
    /// �������������� ��������.
    /// </summary>
    [CreateAssetMenu(fileName = "Element", menuName = "Magic/Element")]
    [StructLayout(LayoutKind.Sequential)]
    [System.Serializable]
    public class Element : ScriptableObject
    {
        /// <summary>
        /// ���������������. ��� ������ ����� ������������ ���������� � ��������� ����� ��� ��������.
        /// </summary>
        [Range(0f, 1f)]
        public float swiftness;
        /// <summary>
        /// ������������. ����������� ���������� ���������� �����.
        /// </summary>
        [Range(0f, 1f)]
        public float volatility;
        /// <summary>
        /// ��������. ��������� ������ ��������� �������.
        /// </summary>
        [Range(0f, 1f)]
        public float hardness;
        /// <summary>
        /// ��������. ��������� ����� ��������� ���������� � ����.
        /// </summary>
        [Range(0f, 1f)]
        public float precision;
        /// <summary>
        /// ����������. ����������� ���������� ��������� ������� ������ ��������� � ������������� ���������� �������.
        /// </summary>
        [Range(0f, 1f)]
        public float absorption;
        /// <summary>
        /// ������������. ����������� �������� ������� ���������� ������� � ����� �����.
        /// </summary>
        [Range(0f, 1f)]
        public float concentration;

        /// <summary>
        /// ����������. ������� ���������� ������� ������� � �������.
        /// </summary>
        public float quantity;

        public static Element operator *(in Element element, in float weight)
        {
            var result = CreateInstance<Element>();
            result.swiftness = element.swiftness * weight;
            result.volatility = element.volatility * weight;
            result.hardness = element.hardness * weight;
            result.precision = element.precision * weight;
            result.absorption = element.absorption * weight;
            result.concentration = element.concentration * weight;
            return result;
            
        }
        public static Element operator /(in Element element, in float weight)
        {
            var result = CreateInstance<Element>();
            result.swiftness = element.swiftness / weight;
            result.volatility = element.volatility / weight;
            result.hardness = element.hardness / weight;
            result.precision = element.precision / weight;
            result.absorption = element.absorption / weight;
            result.concentration = element.concentration / weight;
            return result;
        }

        public static Element operator +(in Element left, in Element right)
        {
            var result = CreateInstance<Element>();
            result.swiftness = left.swiftness + right.swiftness;
            result.volatility = left.volatility + right.volatility;
            result.hardness = left.hardness + right.hardness;
            result.precision = left.precision + right.precision;
            result.absorption = left.absorption + right.absorption;
            result.concentration = left.concentration + right.concentration;
            return result;
        }
    }
}
