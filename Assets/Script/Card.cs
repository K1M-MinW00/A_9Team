using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int idx = 0;
    public SpriteRenderer frontImage;
    public SpriteRenderer background; // ī���� �޸� (���� ������ ���ؼ�)
    public GameObject front;
    public GameObject back;
    public AudioClip clip;
    public AudioSource audioSource;

    public Animator anim;

    bool matched_fail = false;

    private Coroutine myCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        background = back.GetComponentInChildren<SpriteRenderer>(); // �޸� ���� ������ ���ؼ�

    }

    public void Setting(int number)
    {
        idx = number;
        string objectName = gameObject.name; // ���� ������Ʈ �̸��� �޾ƿͼ� Ȯ���Ұſ���
        //Debug.Log(objectName);

        if (objectName == "Card1 MW(Clone)") // ���� ������Ʈ �̸��� MW ���
        {
            //Debug.Log(" �ο�� ī�� ã��");
            frontImage.sprite = Resources.Load<Sprite>($"MW/MW_{idx}"); //MW ���� ���ο� �ִ� mw_idx ������ ����Ʈ�� ����
        }
        else if (objectName == "Card2 JW(Clone)")
        {
            frontImage.sprite = Resources.Load<Sprite>($"JW/JW_{idx}");
        }
        else if (objectName == "Card3 SW(Clone)")
        {
            frontImage.sprite = Resources.Load<Sprite>($"SW/SW_{idx}");
        }
        else if (objectName == "Card4 LH(Clone)")
        {
            frontImage.sprite = Resources.Load<Sprite>($"LH/LH_{idx}");
        }
    }

    public void OpenCard()
    {
        if(GameManager.Instance.secondCard != null)
        {
            return;
        }

        audioSource.PlayOneShot(clip);
        anim.SetBool("isOpen", true);

        // Flip �ִϸ��̼ǿ��� SetActive �����ؼ� ���� �ʿ� ����
        //front.SetActive(true);
        //back.SetActive(false);

        if (GameManager.Instance.firstCard == null)
        {
            GameManager.Instance.firstCard = this;
            GameManager.Instance.firstCard.StartCoroutineExample();
        }
        else
        {
            GameManager.Instance.secondCard = this;
            GameManager.Instance.firstCard.StopCoroutineExample();
            GameManager.Instance.Matched();
        }
    }

    public void StartCoroutineExample()
    {
        myCoroutine = StartCoroutine(MyCoroutine());
    }

    public void StopCoroutineExample()
    {
        if (myCoroutine != null)
        {
            StopCoroutine(myCoroutine);
            myCoroutine = null;
            Debug.Log("�ڷ�ƾ ������");
        }
    }

    IEnumerator MyCoroutine()
    {
        Debug.Log("Coroutine ����");
        yield return new WaitForSeconds(5.0f);
        CloseCardInvoke();
        GameManager.Instance.firstCard = null;
        Debug.Log("Coroutine ����");
    }
    public void DestroyCard()
    {
        Invoke("DestoryCardInvoke", 0.5f);
    }

    void DestoryCardInvoke()
    {
        Destroy(gameObject);
    }

    public void CloseCard()
    {
        Invoke("CloseCardInvoke", 0.5f);
    }

    void CloseCardInvoke()
    {
        anim.SetBool("isOpen", false);
        if (!matched_fail)
        {
            matched_fail = true;
            background.color = new Color(135 / 255f, 135 / 255f, 135 / 255f, 1f);
        }

        // flip �ִϸ��̼ǿ��� SetActive �����ؼ� ���� �ʿ� ����
        // front.SetActive(false);
        // back.SetActive(true);
    }
}
