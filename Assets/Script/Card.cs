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
    public bool delay = false;

    private IEnumerator coroutine;
    private Coroutine myCoroutine;
    
    

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        background = back.GetComponentInChildren<SpriteRenderer>(); // �޸� ���� ������ ���ؼ�

        coroutine = autoClose();
    }



    public void Setting(int number)
    {
        idx = number;
        frontImage.sprite = Resources.Load<Sprite>($"rtan{idx}");
    }

    public void OpenCard()
    {
        if(GameManager.Instance.secondCard != null)
        {
            return;
        }

        audioSource.PlayOneShot(clip);
        anim.SetBool("isOpen", true);
        front.SetActive(true);
        back.SetActive(false);


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
        CloseCardInvoke2();
        GameManager.Instance.firstCard = null;
        Debug.Log("Coroutine ����");
    }

    IEnumerator autoClose()
    {
        yield return new WaitForSeconds(5.0f);
        CloseCardInvoke2();
        GameManager.Instance.firstCard = null;
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

    
    public void CancelClose()
    {
        CancelInvoke("CloseCardInvoke2");
    }

    void CloseCardInvoke()
    {
        anim.SetBool("isOpen", false);
        if (!matched_fail)
        {
            matched_fail = true;
            background.color = new Color(135 / 255f, 135 / 255f, 135 / 255f, 1f);
        }
        front.SetActive(false);
        back.SetActive(true);
    }
    void CloseCardInvoke2()
    {
        anim.SetBool("isOpen", false);
        if (!matched_fail)
        {
            matched_fail = true;
            background.color = new Color(135 / 255f, 135 / 255f, 135 / 255f, 1f);
        }
        front.SetActive(false);
        back.SetActive(true);
    }
}
