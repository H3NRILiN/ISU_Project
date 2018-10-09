using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Answer : MonoBehaviour {
    public int m_CorrectAns;

    public int m_AnsNum;

    public bool m_IsCorrect = false;

    public void ApplyAns()
    {
        if (m_CorrectAns == m_AnsNum)
        {
            Debug.Log("答對");
        }
        else
            Debug.Log("答錯");
    }
}
