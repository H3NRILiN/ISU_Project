using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour {

    public Transform Follower;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    void Update()
    {
        transform.position = Follower.position;
    }
}
