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

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        background = back.GetComponentInChildren<SpriteRenderer>(); // �޸� ���� ������ ���ؼ�
    }

    public void Setting(int number)
    {
        idx = number;
        frontImage.sprite = Resources.Load<Sprite>($"rtan{idx}");
    }

    public void OpenCard()
    {
        audioSource.PlayOneShot(clip);
        anim.SetBool("isOpen", true);

        // Flip �ִϸ��̼ǿ��� SetActive �����ؼ� ���� �ʿ� ����
        //front.SetActive(true);
        //back.SetActive(false);

        if (GameManager.Instance.firstCard == null)
        {
            GameManager.Instance.firstCard = this;
        }
        else
        {
            GameManager.Instance.secondCard = this;
            GameManager.Instance.Matched();
        }
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
