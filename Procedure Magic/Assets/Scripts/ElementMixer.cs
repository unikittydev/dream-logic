using UnityEngine;

namespace Magic.Core
{
    /// <summary>
    /// Смешиватель элементов.
    /// </summary>
    public class ElementMixer : MonoBehaviour
    {
        [SerializeField]
        private string baseElementsPath;
        [SerializeField]
        private string baseElementRelationsPath;

        [SerializeField]
        private Element testLeft;
        [SerializeField]
        private float testLeftQuantity;
        [SerializeField]
        private Element testRight;
        [SerializeField]
        private float testRightQuantity;
        [SerializeField]
        private Element testResult;

        private static Element[] baseElements;
        private static ElementRelation[] baseElementRelations;

        private void Awake()
        {
            baseElements = Resources.LoadAll<Element>(baseElementsPath);
            baseElementRelations = Resources.LoadAll<ElementRelation>(baseElementRelationsPath);
        }

        private void Update()
        {
            testResult = Mix(testLeft, testLeftQuantity, testRight, testRightQuantity);
        }

        public static Element Mix(Element leftElement, float leftQuantity, Element rightElement, float rightQuantity)
        {
            float leftRelationWeight = 0f;
            float leftWeight = leftQuantity / (leftQuantity + rightQuantity);
            foreach (var relation in baseElementRelations)
            {
                if (relation.left == leftElement && relation.right == rightElement)
                {
                    leftRelationWeight = RelationWeight(leftWeight, relation.weight);
                    break;
                }
                else if (relation.left == rightElement && relation.right == leftElement)
                {
                    leftRelationWeight = RelationWeight(leftWeight, 1f - relation.weight);
                    break;
                }
            }

            Element result = leftElement * leftRelationWeight + rightElement * (1f - leftRelationWeight);

            return result;
        }

        public static Element MixBaseElements(float[] quantities)
        {
            // Нормализуем веса.
            float sum = 0f;
            foreach (var element in quantities)
                sum += element;
            for (int i = 0; i < quantities.Length; i++)
                quantities[i] /= sum;

            /*for (int i = 0; i < baseElements.Length; i++)
            {
                for (int j = 0; j < baseElements.Length; j++)
                {

                }
            }*/

            return ScriptableObject.CreateInstance<Element>();
        }

        private static float RelationWeight(float percent, float weight)
        {
            return percent < .5f ?
                      weight  * 2f * percent :
                (1f - weight) * 2f * percent + 2f * weight - 1f;
        }
    }
}
