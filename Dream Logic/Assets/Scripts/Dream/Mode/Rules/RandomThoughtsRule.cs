using UnityEngine;

namespace Game.Dream
{
    public class RandomThoughtsRule : DreamRule
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

        [SerializeField]
        private float waitTime = 5f;
        private float waitCounter;

        [SerializeField]
        private float messWordWeight = .25f;

        private void Update()
        {
            if (waitCounter >= waitTime)
            {
                waitCounter = 0;
                DreamGame.ui.DisplayDescription(string.Empty, Random.value > messWordWeight ? GetRandomWord() : GetIncomprehensibleWord());
            }
            waitCounter += Time.deltaTime;
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
