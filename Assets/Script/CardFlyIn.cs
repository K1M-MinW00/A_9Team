using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardFlyIn : MonoBehaviour
{
    public Transform startPoint; // ������ (ȭ�� ��)
    public Transform fixedEndPoint; // ������ EndPoint ��ġ (ȭ�� �߾�)
    public float speed = 1f;

    private Transform endPoint; // ���� (ȭ�� �߾�)

    void Start()
    {
        // �ʱ� ��ġ ����
        transform.position = startPoint.position;

        // ������ EndPoint ��ġ�� ����
        endPoint = fixedEndPoint;

        // speef ���� �����Ͽ� ������ �ӵ� ����
        speed = 2.5f; // ������ �ӵ��� 2.5��� ������ ����

        // GameObject Ȱ��ȭ
        startPoint.gameObject.SetActive(true);
        endPoint.gameObject.SetActive(true);

        // �ڷ�ƾ ����
        StartCoroutine(FlyIn());
    }

    IEnumerator FlyIn()
    {
        float t = 0f;

        while (t <= 1f)
        {
            t += speed * Time.deltaTime;

            // Bezier Curve ��� : 't' ���� �̿��Ͽ� ������ �����ӿ� ���� ������ ��� ���
            Vector3 position = Mathf.Pow(1 - t, 2) * startPoint.position +
                               2 * (1 - t) * t * endPoint.position +
                               Mathf.Pow(t, 2) * endPoint.position;

            // ī�� ��ġ ������Ʈ
            transform.position = position;

            // ���� �����ӱ��� ���
            yield return null;
        }

        // ������ �Ϸ� �� GameObject ��Ȱ��ȭ
        startPoint.gameObject.SetActive(false);
        endPoint.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
