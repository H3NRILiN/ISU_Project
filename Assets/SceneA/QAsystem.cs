using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class QAsystem : MonoBehaviour {
    #region "Debug模式"
    public bool DebugMode = false; 
    public LineRenderer line;
    #endregion
    public float DectectRange = 1;
    public float AnswerRange = 1;
    
    public Transform TrackedObject;
    Transform AnsPos;
    Transform AnsPosTemp;
    public ParticleSystem particle;
    
    float Dist;

    bool IsOnAnsPos;

    Vector3 SmoothDampVel = Vector3.zero;
    // Use this for initialization
    void Start () {
        DataManager.IsAnswering = false;
        IsOnAnsPos = false;
        DataManager.isOnAnsPosStop = false;
        #region "Debug模式"
        line.SetPosition(0, transform.position);
        #endregion
        line.enabled = false;
        ParticaleControl(false);
    }
	
	// Update is called once per frame
	void Update () {
        Dist = Vector3.Distance(transform.position, TrackedObject.position);
        DectectDistance();
    }
    void DectectDistance()
    {
        if (DataManager.IsAnswering != true)
        {
            if (Dist <= DectectRange)
            {
                ParticaleControl(true);
                #region "Debug模式"
                if (DebugMode != false)
                {
                    line.enabled = true;
                    line.SetPosition(1, TrackedObject.position);
                    if (Dist < AnswerRange)
                        Debug.Log("答題");
                }
                #endregion
                particle.gameObject.transform.LookAt(TrackedObject.position);
                if (Dist <= AnswerRange)
                {
                    ParticaleControl(false);
                    DataManager.IsAnswering = true;
                    AnsPos = transform.GetChild(1);
                }
            }
            else
            {
                #region "Debug模式"
                line.enabled = false;
                #endregion
                ParticaleControl(false);
                AnsPos = null;
            }
        }
        else
        {
            #region "Debug模式"
            line.enabled = false;
            #endregion
            ParticaleControl(false);
            AnswerToAnsPos();
        }
    }
    void ParticaleControl(bool setactive)
    {
        particle.gameObject.SetActive(setactive);
    }
    void AnswerToAnsPos()
    {
        if (IsOnAnsPos != true)
        {
            AnsPosTemp = AnsPos;
            TrackedObject.parent = this.transform;
            TrackedObject.position = transform.position;
            IsOnAnsPos = true;
        }
        if (DataManager.isOnAnsPosStop != true)
        {
            if (AnsPos!= null)
                StartCoroutine(MoveToAnsPos());
        }
    }
    IEnumerator MoveToAnsPos()
    {
        TrackedObject.transform.position = Vector3.SmoothDamp(TrackedObject.position, AnsPos.position, ref SmoothDampVel, 1f);
        if (Mathf.Approximately(0, Mathf.Round(Vector3.Distance(TrackedObject.position, AnsPos.position))))
        {
            yield return new WaitForSeconds(1.5f);
            DataManager.isOnAnsPosStop = true;
            TrackedObject.position = AnsPos.position;
        }
    }
    #region "Debug模式"
    private void OnDrawGizmos() 
    {
        if (DebugMode != false)
        {
            Gizmos.color = new Color(0, 0, 1, 0.3f);
            Gizmos.DrawSphere(transform.position, DectectRange);
            Gizmos.color = new Color(1, 0, 0, 0.3f);
            Gizmos.DrawSphere(transform.position, AnswerRange);
        }
    }
    #endregion
}
