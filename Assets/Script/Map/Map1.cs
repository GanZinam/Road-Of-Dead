using UnityEngine;
using System.Collections;

public class Map1 : MonoBehaviour {

    float Make_speed = 0.8f;           //몬스터 나오는 시간


	void Update () 
    {
        if (!GM.GameManager.getInstance().pause)
        {
            GM.GameManager.getInstance().start_time += Time.deltaTime;
            //@30초 이전에 나오는 몬스터
            if (GM.GameManager.getInstance().start_time <= 30f && GM.GameManager.getInstance().start_time - GM.GameManager.getInstance().Now_time >= Make_speed)
            {
                GM.GameManager.getInstance().Now_time = GM.GameManager.getInstance().start_time;
                
                int rand = Random.Range(1,101);
                int Monster_num = 0;
                if (rand >= 90)
                    Monster_num = 3;
                else if (rand >= 60)
                    Monster_num = 2;
                else
                    Monster_num = 1;

                for (int i = 0; i < Monster_num; i++)
                {
                    Monster_position();
                    GM.GameManager.getInstance().Monster_1_creat();
                }
            }
            //@70초 이전에 나오는 몬스터
            else if (GM.GameManager.getInstance().start_time <= 70f && GM.GameManager.getInstance().start_time - GM.GameManager.getInstance().Now_time >= Make_speed)
            {
                GM.GameManager.getInstance().Now_time = GM.GameManager.getInstance().start_time;

                int rand = Random.Range(1, 101);
                int Monster_num = 0;
                if (rand >= 90)
                    Monster_num = 3;
                else if (rand >= 60)
                    Monster_num = 2;
                else
                    Monster_num = 1;

                int M2_num = Random.Range(0, Monster_num+1);
                int M1_num = Monster_num - M2_num;

                for (int i = 0; i < M1_num; i++)
                {
                    Monster_position();
                    GM.GameManager.getInstance().Monster_1_creat();
                } 
                for (int i = 0; i < M2_num; i++)
                {
                    Monster_position();
                    GM.GameManager.getInstance().Monster_2_creat();
                }
            }
            //@70초 이후에 나오는 몬스터
            else if (GM.GameManager.getInstance().start_time <= 70f && GM.GameManager.getInstance().start_time - GM.GameManager.getInstance().Now_time >= Make_speed)
            {
                GM.GameManager.getInstance().Now_time = GM.GameManager.getInstance().start_time;

                int rand = Random.Range(1, 101);
                int Monster_num = 0;
                int M1_num = 0;
                int M2_num = 0;
                int M3_num = 0;

                if (rand >= 90)
                    Monster_num = 3;
                else if (rand >= 60)
                    Monster_num = 2;
                else
                    Monster_num = 1;
                if (Monster_num >= 2)
                {
                    M3_num = Random.Range(0, 2);
                }
                M2_num = Random.Range(0, Monster_num + 1 - M3_num);
                M1_num = Monster_num - M2_num - M3_num;

                for (int i = 0; i < M1_num; i++)
                {
                    Monster_position();
                    GM.GameManager.getInstance().Monster_1_creat();
                }
                for (int i = 0; i < M2_num; i++)
                {
                    Monster_position();
                    GM.GameManager.getInstance().Monster_2_creat();
                }
                for (int i = 0; i < M2_num; i++)
                {
                    Monster_position();
                    GM.GameManager.getInstance().Monster_3_creat();
                }
            }
        }
	}
    void Monster_position()
    {
        GM.GameManager.getInstance().monsterSpawnPos.x = Random.Range(-650, -700);      //-760 ~ -650
        GM.GameManager.getInstance().monsterSpawnPos.y = Random.Range(28, -310);
    }
}
