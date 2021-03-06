﻿using UnityEngine;
using System.Collections;

public class IntroManager : MonoBehaviour {
    
    public Animator sceneAnim;

	// Use this for initialization
	void Start () 
    {
        if (!GM.GameManager.getInstance().FIRST_START)
        {
            // 데이터 (추후 삭제)
            PlayerPrefs.SetInt("NowMyPos", 0);

            PlayerPrefs.SetInt("Map_0", 1);
            PlayerPrefs.SetInt("Map_1", 1);
            PlayerPrefs.SetInt("Map_2", 0);
            PlayerPrefs.SetInt("Map_3", 0);
            PlayerPrefs.SetInt("Map_4", 0);

            PlayerPrefs.SetInt("Q_0_IS", 0);
            PlayerPrefs.SetInt("Q_0_MONSTER_KILL", 0);

            PlayerPrefs.SetInt("Money", 10000);
            PlayerPrefs.SetInt("Item_0", 0);
            PlayerPrefs.SetInt("Item_1", 0);
            PlayerPrefs.SetInt("Item_2", 0);
            PlayerPrefs.SetInt("Item_3", 0);
            PlayerPrefs.SetInt("Item_4", 0);
            PlayerPrefs.SetInt("Item_5", 0);
            PlayerPrefs.SetInt("Item_6", 0);

            for (int j = 0; j < 4; j++)
            {
                if (j.Equals(0))
                {
                    PlayerPrefs.SetInt(string.Format("Spray_{0}", j), 1);
                    PlayerPrefs.SetInt(string.Format("Tire_{0}", j), 1);
                    PlayerPrefs.SetInt(string.Format("Car_{0}", j), 1);
                    PlayerPrefs.SetInt(string.Format("Nitro_{0}", j), 1);
                }
                else
                {
                    PlayerPrefs.SetInt(string.Format("Spray_{0}", j), 0);
                    PlayerPrefs.SetInt(string.Format("Tire_{0}", j), 0);
                    PlayerPrefs.SetInt(string.Format("Car_{0}", j), 0);
                    PlayerPrefs.SetInt(string.Format("Nitro_{0}", j), 0);
                }
            }
            GM.GameManager.getInstance().FIRST_START = true;
            // ===========================================================
        }
        StartCoroutine(showBT());
        StartCoroutine(soundLion());
	}

    IEnumerator soundLion()
    {
        yield return new WaitForSeconds(1);
        GameObject.Find("_Audio").GetComponent<GM.AudioManager>().introEffect();
        GameObject.Find("_Audio").GetComponent<GM.AudioManager>().introSound();
    }

    [SerializeField]
    GameObject button;
    IEnumerator showBT()
    {
        yield return new WaitForSeconds(5f);
        button.SetActive(true);
    }

    public void Next_Scene()
    {
        sceneAnim.SetTrigger("Change");
        GameObject.Find("_Audio").GetComponent<GM.AudioManager>().StartCoroutine("decreaseServeVolume");
        StartCoroutine(changeScene());
    }

    IEnumerator changeScene()
    {
        yield return new WaitForSeconds(1);
        DontDestroyOnLoad(GameObject.Find("_Audio"));
        Application.LoadLevel("MainScene");
    }
}
