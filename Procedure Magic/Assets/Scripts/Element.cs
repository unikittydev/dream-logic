using System.Runtime.InteropServices;
using UnityEngine;

namespace Magic.Core
{
    /// <summary>
    /// Характеристики элемента.
    /// </summary>
    [CreateAssetMenu(fileName = "Element", menuName = "Magic/Element")]
    [StructLayout(LayoutKind.Sequential)]
    [System.Serializable]
    public class Element : ScriptableObject
    {
        /// <summary>
        /// Стремительность. Как быстро может перемещаться заклинание и насколько резки его движения.
        /// </summary>
        [Range(0f, 1f)]
        public float swiftness;
        /// <summary>
        /// Изменчивость. Способность заклинания измененять форму.
        /// </summary>
        [Range(0f, 1f)]
        public float volatility;
        /// <summary>
        /// Твёрдость. Насколько сложно повредить элемент.
        /// </summary>
        [Range(0f, 1f)]
        public float hardness;
        /// <summary>
        /// Точность. Насколько легко направить заклинание в цель.
        /// </summary>
        [Range(0f, 1f)]
        public float precision;
        /// <summary>
        /// Поглощение. Способность заклинания поглощать энергию слабых элементов и противостоять поглощению сильных.
        /// </summary>
        [Range(0f, 1f)]
        public float absorption;
        /// <summary>
        /// Концентрация. Способность собирать большое количество энергии в одной точке.
        /// </summary>
        [Range(0f, 1f)]
        public float concentration;

        /// <summary>
        /// Количество. Сколько магической энергии вложено в элемент.
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
