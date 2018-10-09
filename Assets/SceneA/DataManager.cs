using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager{

    public static bool IsAnswering;
    public static bool isOnAnsPosStop;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	public static void AnsTheQ () {
        Debug.Log(IsAnswering);
	}
}
