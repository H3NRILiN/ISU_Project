using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyGameManager : MonoBehaviour {

    // Use this for initialization
    private void Awake()
    {
        
    }
    private void Start()
    {
    }
    public void ResetScene()
    {
        SceneManager.LoadScene("A");
    }
}
