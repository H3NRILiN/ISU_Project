using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneBGameManager : MonoBehaviour {
    public Text m_Question;

    public GameObject m_AnswerPrefab;
    public GameObject m_EnemyPrefab;
    public GameObject m_VIP;

    public Transform m_AnswerPerfabParent;

    public List<EnemyManager> m_Enemys;

    public List<QASystemProfile> m_QA;

    WaitForSeconds m_EnemySpawnWait;
    WaitForSeconds m_PrepareRoundWait;
    WaitForSeconds m_PlayingRoundWait;
    WaitForSeconds m_EndRoundWait;

    public float m_EnemySpawnSpeed = 5;
    public float m_PrepareRoundDelay = 2;

    int m_CurrentRound;
    int m_GameMaxRounds;

    // Use this for initialization
    void Start () {
        m_EnemySpawnWait = new WaitForSeconds(m_EnemySpawnSpeed);
        m_PrepareRoundWait = new WaitForSeconds(m_PrepareRoundDelay);
        m_CurrentRound = 1;
        GenerateQA();
        StartCoroutine(SpawnEnemys());
    }
	
	// Update is called once per frame
	void Update () {
    }
    IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundLoop());


    }


    IEnumerator RoundLoop()
    {
        yield return StartCoroutine(PrepareRound());
        yield return StartCoroutine(PlayingRound());
        yield return StartCoroutine(EndRound());
    }


    IEnumerator PrepareRound()
    {
        yield return m_PrepareRoundWait;
    }

    
    IEnumerator PlayingRound()
    {
        yield return null;
    }


    IEnumerator EndRound()
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
                m_Enemys[i].m_Instane.GetComponent<Enemy>().VIP = m_VIP;
                m_Enemys[i].SpawnEnemy();
                yield return m_EnemySpawnWait;
            }
            else
            {
                yield return null;
            }
            StartCoroutine(SpawnEnemys());
        }
    }


    void GenerateQA()
    {
        m_GameMaxRounds = m_QA.Count;
        if (m_QA.Count > 0)
        {
            int a = m_CurrentRound-1;
            if (m_QA[a] != null)
            {
                m_Question.text = m_QA[a].m_Question;

                for (int b = 0; b < m_QA[a].m_AnswersContent.Length; b++)
                {
                    GameObject answer = Instantiate(m_AnswerPrefab, m_AnswerPerfabParent);
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

}
