using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public static Card Instance;

    public int idx = 0;
    public int id = 0;

    public SpriteRenderer frontImage;
    public SpriteRenderer background; // ī���� �޸� (���� ������ ���ؼ�)
    public GameObject front;
    public GameObject back;
    public AudioClip clip;
    public AudioSource audioSource;

    public Animator anim;

    bool matched_fail = false;
    public static bool time_started = false;

    private Coroutine myCoroutine;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        background = back.GetComponentInChildren<SpriteRenderer>(); // �޸� ���� ������ ���ؼ�

        anim = anim.GetComponent<Animator>();
    }

    public void Setting(int number)
    {
        idx = number;
        id = GetInstanceID();
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

    public void CardCreatAnim()
    {
        anim.SetBool("Game Play", true);
    }

    public void OpenCard()
    {
        if(GameManager.Instance.secondCard != null || time_started == false)
        {
            return;
        }

        if (GameManager.Instance.firstCard != null && GameManager.Instance.firstCard.id == id)
            return;

        audioSource.PlayOneShot(clip);
        anim.SetBool("isOpen", true);

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
        GameManager.Instance.firstCard = null;
        GameManager.Instance.secondCard = null;
        Destroy(gameObject);
    }

    public void CloseCard()
    {
        Invoke("CloseCardInvoke", 0.5f);
    }

    void CloseCardInvoke()
    {
        GameManager.Instance.firstCard = null;
        GameManager.Instance.secondCard = null;
        anim.SetBool("isOpen", false);
        if (!matched_fail)
        {
            matched_fail = true;
            background.color = new Color(135 / 255f, 135 / 255f, 135 / 255f, 1f);
        }

    }
}
