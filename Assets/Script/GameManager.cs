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
    public AudioSource audioSource;
    public AudioClip clip;
    // ��ſ� - miss, finish, fail �߰�
    public AudioClip miss;
    public AudioClip finish;
    public AudioClip fail;

    public Animator timer_anim;
    float time = 80.0f;
    public int cardCount = 0;

    
    int MatchCount = 0;

    int time_score = 0;
    int match_score = 0;
    int match_cnt_score = 0;
    int Score = 0;

    public GameObject endPanel;

    public Text CountTxt; //��ġ ī��Ʈ �ؽ�Ʈ
    public Text ScoreTxt;
    public Text BestScoreTxt;

    AudioSource audioSource_tictok; // 5�� ������ �� ����� AudioSource ������Ʈ�� ���� ���� audioSource_tictok
    public AudioClip clip_tictok;
    float warning_time = 5.0f; // ��� ��Ÿ�� �ð�
    bool is_tictok = false; // clip_tictok �� �÷��̵ǰ� �ִ���


    string key = "BestTime";
    string skey = "BestScore";

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
        timer_anim = timeTxt.GetComponentInChildren<Animator>();

        timer_anim.SetBool("isWarning", false);
        Camera.main.backgroundColor = new Color(90 / 255f, 90 / 255f, 255 / 255f);
        timeTxt.transform.localScale = new Vector3(1f, 1f, 1f);

        audioSource_tictok = GetComponent<AudioSource>();
        audioSource_tictok.clip = clip_tictok;


        

        Score = 0;
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
                timer_anim.SetBool("isWarning", true);

                is_tictok = true;
                audioSource_tictok.loop = true;
                audioSource_tictok.Play();
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
            Time.timeScale = 0.0f;
        }



    }

    public void Matched()
    {
        CancelInvoke();
        // ��ſ� - �̹����� ���� ���� �̸� ǥ��, ���� �߰�
        // ������������ �ο��� ������ ������ �ش� �κ� �ڵ� ������ �ʿ��մϴ�!! (����)
        if (firstCard.idx == secondCard.idx)
        {
            if (firstCard.idx < 2)
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

            // ��Ī ���� �� ����
            match_score += 2;

            if (cardCount == 0)
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
            time -= 1;

            firstCard.CloseCard();
            secondCard.CloseCard();
        }
        firstCard = null;
        secondCard = null;
        MatchCount++;
    }


    void EndPanel()
    {
        // �ð� ����
        if (time >= 15.0f)
            time_score = 50;
        else if (time >= 12.0f)
            time_score = 45;
        else if (time >= 10.0f)
            time_score = 40;
        else if (time > 0.0f)
            time_score = 30;
        else
            time_score = 0;

        // ��Ī �õ� Ƚ�� ����
        if (cardCount == 0)
        {
            if (MatchCount <= 16)
                match_cnt_score = 34;

            else if (MatchCount <= 20)
                match_cnt_score = 30;

            else if (MatchCount <= 24)
                match_cnt_score = 26;

            else if (MatchCount <= 28)
                match_cnt_score = 22;

            else if (MatchCount <= 32)
                match_cnt_score = 18;
            else
                match_cnt_score = 0;
        }

        // ���� ���
        Score = time_score + match_cnt_score + match_score;

        BestScoreTxt.text = Score.ToString("N2");
        CountTxt.text = MatchCount.ToString();

        ScoreTxt.text = Score.ToString();

        // �ִܽð� �Ǵ�
        if (PlayerPrefs.HasKey(key))
        {
            float best = PlayerPrefs.GetFloat(key);

            if (best < time)
            {
                PlayerPrefs.SetFloat(key, time);
                bestTimeTxt.text = time.ToString("N2");
            }
            else
                bestTimeTxt.text = best.ToString("N2");
        }
        else
        {
            PlayerPrefs.SetFloat(key, time);
            bestTimeTxt.text = time.ToString("N2");
        } 


        // �ְ����� �Ǵ�
        if (PlayerPrefs.HasKey(skey))
        {
            float best = PlayerPrefs.GetFloat(skey);

            if (best < Score)
            {
                PlayerPrefs.SetFloat(skey, Score);
                BestScoreTxt.text = Score.ToString();
            }
            else
                BestScoreTxt.text = best.ToString();
        }
        else
        {
            PlayerPrefs.SetFloat(skey, Score);
            BestScoreTxt.text = Score.ToString();
        }

    }
}
