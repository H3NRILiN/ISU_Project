using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public GameObject VIP;

    public float m_RotateSpeed = 30f;
    public float m_MoveSpeed = .5f;
    public float m_Damage = 5;

    Rigidbody m_rigidbody;

    Quaternion m_RotateAngle = Quaternion.identity;

    Vector3 m_Movement = Vector3.zero;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();

        m_RotateAngle = Random.rotation;
        transform.rotation = m_RotateAngle;
    }


    // Use this for initialization
    void Start () {
		
	}
	

	// Update is called once per frame
	void Update () {
        Rotate();
    }


    private void FixedUpdate()
    {
        MoveToVIP();
    }


    void Rotate()
    {
        transform.Rotate(transform.forward, m_RotateSpeed * Time.deltaTime);
    }


    void MoveToVIP()
    {
        //往VIP的方向飛去
        m_Movement = VIP.transform.position - transform.position;
        m_Movement = m_Movement.normalized;
        m_rigidbody.position += m_Movement * m_MoveSpeed * Time.deltaTime;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<VIP>() != null)
        {
            VIP vip = other.GetComponent<VIP>();
            vip.TakeDamage(m_Damage);
            Destroy(this.gameObject);
        }
    }
}
