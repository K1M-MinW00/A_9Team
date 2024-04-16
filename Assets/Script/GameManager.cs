using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Card firstCard;
    public Card secondCard;
    // ��ſ� - timeTxt, nameTxt �߰�
    public Text timeTxt;
    public Text nameTxt;
    // ������ - bestTimeTxt �߰�
    public Text bestTimeTxt;
    public GameObject endTxt;
    public AudioSource audioSource;
    public AudioClip clip;
    // ��ſ� - miss, finish, fail �߰�
    public AudioClip miss;
    public AudioClip finish;
    public AudioClip fail;
    public int cardCount = 0;
    float time = 30.0f;
    int MatchCount = 0;
    int Score = 0;

    public GameObject endPanel;

    public Text CountTxt; //��ġ ī��Ʈ �ؽ�Ʈ
    public Text ScoreTxt;
    public Text EndTimeTxt;

    AudioSource audioSource_tictok; // 5�� ������ �� ����� AudioSource ������Ʈ�� ���� ���� audioSource_tictok
    public AudioClip clip_tictok;
    float warning_time = 5.0f; // ��� ��Ÿ�� �ð�
    bool is_tictok = false; // clip_tictok �� �÷��̵ǰ� �ִ���

    Color originalColor = new Color(90 / 255f, 90 / 255f, 255 / 255f);
    Color targetColor = Color.red;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        audioSource = GetComponent<AudioSource>();

        Camera.main.backgroundColor = new Color(90 / 255f, 90 / 255f, 255 / 255f);
        timeTxt.transform.localScale = new Vector3(1f, 1f, 1f);

        audioSource_tictok = GetComponent<AudioSource>();
        audioSource_tictok.clip = clip_tictok;

        // ������ - ����� �ִ� ��� �ҷ�����
        float bestTime = PlayerPrefs.GetFloat("BestTime", float.MaxValue);

        // UI�� �ִ� ��� ǥ��
        bestTimeTxt.text = "Best Time: " + bestTime.ToString("N2");

        // ���� ����� �ִ� ��Ϻ��� ������ �� 
        if (time < bestTime) // time�� bestTime���� ������ �ִ� ��� �߻�
        {
            // ���� ����� �ִ� ��Ϻ��� �����ٸ� �ִ� ��� ������Ʈ
            bestTime = time;

            // �ִ� ��� ����
            PlayerPrefs.SetFloat("BestTime", bestTime);

            // UI�� �ִ� ��� ǥ��
            timeTxt.text = "Best Time: " + bestTime.ToString("N2");
        }
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        timeTxt.text = time.ToString("N2");

        if (time <= warning_time) // ���
        {
            if (!is_tictok) // tictok �����
            {
                is_tictok = true;
                audioSource_tictok.loop = true;
                audioSource_tictok.Play();
            }

            // �ð� UI �� ��ȭ �ֱ�
            for (int i = 1; i <= (int)warning_time; ++i)
            {
                float t = (time) / 0.6f;
                float scaleValue = Mathf.Sin(t * Mathf.PI); // 0���� 1��, �׸��� �ٽ� 0���� ��ȭ
                timeTxt.transform.localScale = Vector3.Lerp(Vector3.one, new Vector3(1.5f, 1.5f, 1f), scaleValue);
                timeTxt.color = Color.Lerp(Color.white, Color.red, scaleValue);
            }
        }

        if (time < 0.0f)
        {
            // ��ſ� - fail ���� �߰�
            time = 0f;
            timeTxt.text = time.ToString("N2");

            endPanel.SetActive(true);
            audioSource_tictok.Stop(); // tictok ����� ����
            EndPanel();
            //audioSource.PlayOneShot(fail);            
            Time.timeScale = 0.0f;
        }

        Score = ((int)time * 1000) - (MatchCount * 50);
    }

    public void Matched()
    {
        // ��ſ� - �̹����� ���� ���� �̸� ǥ��, ���� �߰�
        if (firstCard.idx == secondCard.idx)
        {
            if(firstCard.idx < 2)
            {
                nameTxt.text = "��ο�";
            }
            else if (firstCard.idx >= 2 && firstCard.idx < 4)
            {
                nameTxt.text = "��ſ�";
            }
            else if (firstCard.idx >= 4 && firstCard.idx < 6)
            {
                nameTxt.text = "������";
            }
            else 
            {
                nameTxt.text = "������";
            }
            audioSource.PlayOneShot(clip);

            firstCard.DestroyCard();
            secondCard.DestroyCard();
            cardCount -= 2;
            if(cardCount == 0)
            {
                Time.timeScale = 0.0f;
                endPanel.SetActive(true);
                EndPanel();
                audioSource.PlayOneShot(finish);

            }
        }
        else
        {
            nameTxt.text = "����!";
            audioSource.PlayOneShot(miss);

            firstCard.CloseCard();
            secondCard.CloseCard();
        }

        firstCard = null;
        secondCard = null;
        MatchCount++;
    }


    void EndPanel()
    {
        EndTimeTxt.text = time.ToString("N2");
        CountTxt.text = MatchCount.ToString();

        ScoreTxt.text = Score.ToString();


    }
}
