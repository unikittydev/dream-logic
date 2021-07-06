using System.Collections;
using UnityEngine;

namespace Game.Dream
{
    /// <summary>
    /// 
    /// </summary>
    public class ThinkingDream : DreamMode
    {
        private readonly string[] randomThoughts = new string[]
        {
            "�������� ������� �������� �������� ����� �������� ������� ����� ��������",
            "������� ������, � ���� ������. ���... ��� ������ ����? ���� �����.",
            "������ ����� �� ���� �� 10 �����, � �������� �� 45?",
            "�����, ������, ���������� ��� ��� ������!",
            "����� ���-������ ����? ���!",
            "�������� ����� ��� ������? ��� �������?",
            "��� ��� �� �����?",
            "������ - ��� \"������\" ��������",
            "������������ ������������� �������� - ��� CAT. ���!",
        };

        private float waitTime = 5f;

        private float scoreMultiplier = 1.75f;

        private float messWordWeight = .25f;

        private void Start()
        {
            StartCoroutine(Think());
        }

        private void Update()
        {
            DreamSimulation.score += scoreMultiplier * Time.deltaTime;
        }

        private IEnumerator Think()
        {
            while (enabled)
            {
                DreamSimulation.ui.DisplayDescription("", Random.value > messWordWeight ? GetRandomWord() : GetIncomprehensibleWord());
                yield return new WaitForSeconds(waitTime);
            }
        }

        private string GetIncomprehensibleWord()
        {
            char[] word = GetRandomWord().ToCharArray();
            for (int i = 0; i < word.Length / 4; i++)
            {
                int index1 = Random.Range(0, word.Length);
                int index2 = Random.Range(0, word.Length);

                char t = word[index1];
                word[index1] = word[index2];
                word[index2] = t;
            }
            return new string(word);
        }

        private string GetRandomWord()
        {
            return randomThoughts[Random.Range(0, randomThoughts.Length)];
        }
    }
}
