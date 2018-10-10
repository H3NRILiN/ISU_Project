using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneBGameManager : MonoBehaviour {

    public GameObject m_AnswerPrefab;
    public GameObject m_EnemyPrefab;
    public GameObject m_QuestionSlot;
    public GameObject m_AnswerSlot;

    public Text m_RoundEndText;
    public Text m_RoundScoreText;
    public Text m_Question;

    public List<EnemyManager> m_Enemys;

    public List<QASystemProfile> m_QA;

    public VIP m_VIP;

    [HideInInspector]public int m_CurrentEnemyLeft;

    public float m_EnemySpawnSpeed = 1;
    public float m_PrepareRoundDelay = 2;
    public float m_EndRoundDelay = 3;

    public enum ControlMode {MergeCube,Mouse };
    public ControlMode m_ControlMode;

    WaitForSeconds m_EnemySpawnWait;
    WaitForSeconds m_PrepareRoundWait;
    WaitForSeconds m_EndRoundWait;

    float m_VIPCurrentHP;

    int m_CurrentRound;
    int m_GameMaxRounds;

    string m_RoundScore;

    bool m_IsWin;


    // Use this for initialization
    void Start () {

        SetAll();

        StartCoroutine(GameLoop());
    }


	// Update is called once per frame
	void Update () {
        m_VIPCurrentHP = m_VIP.GetComponent<VIP>().m_CurrentHealth;

        if (m_CurrentEnemyLeft < 0)
            m_CurrentEnemyLeft = 0;
        if (m_VIPCurrentHP < 0)
            m_VIPCurrentHP = 0;

    }


    void ControlModeSelect()
    {
        if (m_ControlMode == ControlMode.MergeCube)
        {

        }
        if (m_ControlMode == ControlMode.Mouse)
        {

        }
    }


    //GameLoop一次全部重整畫面狀態>如果場數小於等於m_QA.Count>開始RoundLoop>開始Prepare>開始Playing
         //如果贏了>開始End,場數+1>開始GameLoop
         //如果輸了>開始End>開始GameLoop
    //如果場數大於m_QA.Count>開始GameLoop
    //...
    IEnumerator GameLoop()
    {
        if (m_CurrentRound <= m_QA.Count)
            yield return StartCoroutine(RoundLoop());


    }


    IEnumerator RoundLoop()
    {
        Debug.Log("RoundLoop");
        yield return StartCoroutine(PrepareRound());
        yield return StartCoroutine(PlayingRound());
        yield return StartCoroutine(EndRound());
        StartCoroutine(GameLoop());
    }


    IEnumerator PrepareRound()
    {
        //PrepareRound開時時玩家必須先把mergecube的物件放置到畫面指定地點開始準備遊戲
        Debug.Log("Prepare");
        DisableUI();
        EnableUI();
        Prepare();
        yield return m_PrepareRoundWait;
    }

    
    IEnumerator PlayingRound()
    {
        //PlayingRound在VIP血量少於等於0時 或是 病毒全數消除時結束, 如果VIP死亡會重新開始, 病毒全數消除則進到下一關
        Debug.Log("Playing");

        StartCoroutine(SpawnEnemys());


        while (m_VIPCurrentHP != 0&&m_CurrentEnemyLeft!=0)
        {
            yield return null;
        }
        
    }


    IEnumerator EndRound()
    {
        //顯示最終結果,VIP死亡或是病毒全數消滅,依照剩餘血量給予S A B C D F(失敗)評分
        Debug.Log("End");

        DisableUI();

        for (int i = 0; i < GameObject.FindGameObjectsWithTag("Enemy").Length; i++)
        {
            Destroy(GameObject.FindGameObjectsWithTag("Enemy")[i]);
        }

        m_RoundEndText.text = GetRoundWinLose();

        m_RoundScoreText.text = "評價 " + GetRoundScore();

        yield return m_EndRoundWait;
    }


    IEnumerator EndOfGame()
    {
        yield return null;
    }


    IEnumerator SpawnEnemys()
    {
        if (m_Enemys.ToArray().Length != 0)
        {
            int i = Random.Range(-(m_Enemys.Count - 1), m_Enemys.Count);
            i = Mathf.Abs(i);
            if (m_Enemys[i].m_SpawnPoint.gameObject.activeInHierarchy)
            {
                m_Enemys[i].m_Instane = Instantiate(m_EnemyPrefab);
                m_Enemys[i].m_Instane.GetComponent<Enemy>().VIP = m_VIP.gameObject;
                m_Enemys[i].m_Instane.GetComponent<Enemy>().m_GameManager = this;
                m_Enemys[i].SpawnEnemy();
                yield return m_EnemySpawnWait;
            }
            else
            {
                yield return null;
            }
            if (m_CurrentEnemyLeft >0&&m_VIPCurrentHP>0)
                StartCoroutine(SpawnEnemys());
        }
    }


    void SetAll()
    {
        m_EnemySpawnWait = new WaitForSeconds(m_EnemySpawnSpeed);
        m_PrepareRoundWait = new WaitForSeconds(m_PrepareRoundDelay);
        m_PrepareRoundWait = new WaitForSeconds(m_PrepareRoundDelay);
        m_EndRoundWait = new WaitForSeconds(m_EndRoundDelay);

        m_VIPCurrentHP = m_VIP.m_CurrentHealth;

        m_CurrentRound = 1;
    }


    void EnableUI()
    {
        m_VIP.gameObject.SetActive(true);

        m_RoundEndText.text = string.Empty;
        m_RoundScoreText.text = string.Empty;

        m_QuestionSlot.SetActive(true);

        m_AnswerSlot.SetActive(true);
    }


    void DisableUI()
    {
        m_VIP.gameObject.SetActive(false);

        m_QuestionSlot.SetActive(false);

        m_AnswerSlot.SetActive(false);

        for (int i = 0; i < m_AnswerSlot.transform.childCount; i++)
        {
            Destroy(m_AnswerSlot.transform.GetChild(i).gameObject);
        }
    }


    //a是目前回合數也是QASystemProfile的element位置
    //b是每個答案的element位置
    void Prepare()
    {
        m_VIP.m_CurrentHealth = m_VIP.m_BaseHealth;
        m_VIP.SetText();
        m_GameMaxRounds = m_QA.Count;
        if (m_QA.Count > 0)
        {
            int a = m_CurrentRound-1;
            if (m_QA[a] != null)
            {
                m_Question.text = m_QA[a].m_Question;
                m_CurrentEnemyLeft = m_QA[a].m_EnemyCounts;

                for (int b = 0; b < m_QA[a].m_AnswersContent.Length; b++)
                {
                    GameObject answer = Instantiate(m_AnswerPrefab, m_AnswerSlot.transform);
                    answer.GetComponentInChildren<Text>().text = m_QA[a].m_AnswersContent[b];
                    Answer Ans = answer.GetComponent<Answer>();
                    Ans.m_CorrectAns = m_QA[a].m_CorrectAns;
                    Ans.m_AnsNum = b;
                }
            }
            else
                Debug.LogWarning("QA中的第"+a+"區塊沒東西");
        }
    }


    string GetRoundWinLose()
    {
        string roundwinlose = string.Empty;
        if (m_CurrentEnemyLeft == 0)
        {
            if (m_VIPCurrentHP != 0)
                m_IsWin = true;
            else
                m_IsWin = false;
        }

        if (m_IsWin)
        {
            roundwinlose = "獲勝";
        }
        else
            roundwinlose = "失敗!再來一次吧";


        return roundwinlose;
    }

    
    string GetRoundScore()
    {
        string roundscore=string.Empty;
        

        if (m_VIPCurrentHP > 0)
        {
            if (m_VIPCurrentHP < 10)
                m_VIPCurrentHP = 1;
            else
                m_VIPCurrentHP = ((int)m_VIPCurrentHP / 10) * 10;
        }

        switch ((int)m_VIPCurrentHP)
        {
            case 100:
                roundscore = "S";
                break;
            case 90:
                roundscore = "A";
                break;
            case 80:
            case 70:
                roundscore = "B";
                break;
            case 60:
            case 50:
                roundscore = "C";
                break;
            case 40:
            case 30:
            case 20:
            case 10:
            case 1:
                roundscore = "D";
                break;
            default:
                roundscore = "Fail";
                break;
        }

        return roundscore;
    }

    
}
