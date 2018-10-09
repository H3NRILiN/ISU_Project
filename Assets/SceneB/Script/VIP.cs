using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VIP : MonoBehaviour {

    public string m_HealthTextPrefix = "目前生命值: ";

    public float m_BaseHealth = 100;

    public Text m_HealthText;

    public Color m_DamagedColor=new Color(255,0,0);

    float m_CurrentHealth;

    Color m_OriginColor;

    Animator m_Animaor;

    // Use this for initialization
    void Start () {
        m_OriginColor = GetComponent<MeshRenderer>().material.color;
        m_Animaor = GetComponent<Animator>();
        m_CurrentHealth = m_BaseHealth;
        SetText();
    }
	

	// Update is called once per frame
	void Update () {
        WhenDamaged();
    }


    public void TakeDamage(float damage)
    {
        m_CurrentHealth -= damage;
        //Debug.Log("損失了 "+damage+" 點傷害,剩餘生命值 "+m_CurrentHealth+" 點");
        SetText();
        gameObject.GetComponent<MeshRenderer>().material.color = m_DamagedColor;
        m_Animaor.Play("VIPshake1");
    }


    public void SetText()
    {
        m_HealthText.text = m_HealthTextPrefix + m_CurrentHealth;
    }


    void WhenDamaged()
    {
        gameObject.GetComponent<MeshRenderer>().material.color = Color.Lerp(gameObject.GetComponent<MeshRenderer>().material.color, m_OriginColor, 0.1f);
    }
}
