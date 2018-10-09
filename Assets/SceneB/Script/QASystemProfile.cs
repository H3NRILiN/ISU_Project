using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="New QAProfile",menuName ="QASystem")]
public class QASystemProfile : ScriptableObject {
    [TextArea]
    public string m_Question;

    public string[] m_AnswersContent;

    public int m_CorrectAns;
}
