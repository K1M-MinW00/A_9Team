using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class Board : MonoBehaviour
{
    //public Transform cards;
    public GameObject card;

    int[] arr = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 };
    float howManyCard = 0f;

    float Xmore = 0;
    float Ymore = 0;
    float distance = 1.25f; // ��� 3���� �������� 3���ʹ� ī�� �������� �޶����� �Ÿ� ������ �����Դϴ�!

    // Start is called before the first frame update
    void Start()
    {
         
        if (card.name == "Card1 MW") // �ο��, 8��Ʈ 16��
        {
            howManyCard = 7f;
        }
        else if (card.name == "Card2 JW") // ����, 10��Ʈ 20��
        {
            Ymore = 0.6f;
            howManyCard = 9f;
            arr = new int[] { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9 };
        }
        else if (card.name == "Card3 SW") // �ſ��, 12��Ʈ 24��
        {
            Xmore = 0.19f;
            Ymore = 0.89f;
            howManyCard = 11f;
            distance = 1.125f;
            gameObject.transform.localScale = new Vector3(0.9f, 0.9f, 1); // ī�� �߰��� ���� Board ������ ����

            arr = new int[] { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10, 11, 11 };
        }
        else if ( card.name == "Card4 LH") // ������, 14��Ʈ 28��
        {
            Xmore = 0.38f;
            Ymore = 1.1f;
            howManyCard = 13f;
            distance = 1.0f;
            gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 1); // ī�� �߰��� ���� Board ������ ����

            arr = new int[] { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10, 11, 11, 12, 12, 13, 13 };
        }

        arr = arr.OrderBy(x => Random.Range(0f, howManyCard)).ToArray();

        GameManager.Instance.cardCount = arr.Length;

        StartCoroutine(CardCreate());
    }

    IEnumerator CardCreate()
    {
        for (int i = 0; i < (howManyCard + 1) * 2; i++)
        {
            GameObject go = Instantiate(card, this.transform);

            float x = (i % 4) * distance - 1.875f + Xmore;
            float y = (i / 4) * distance - 2.85f - Ymore;
            go.transform.position = new Vector2(x, y);
            go.GetComponent<Card>().Setting(arr[i]);

            Debug.Log("ī����� : " + i);
            yield return null;
        }

        yield break;
    }

}
