using UnityEngine;
using System.Collections;

public class BoomEffect : MonoBehaviour {

    float Boom_TIme;


	void Start () 
    {

	}
	
	void Update () 
    {
        Boom_TIme += Time.smoothDeltaTime;
        if(Boom_TIme >= 0.4f)
        {
            Destroy(gameObject);
        }
	}
}
