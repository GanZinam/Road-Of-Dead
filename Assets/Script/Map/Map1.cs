using UnityEngine;
using System.Collections;

public class Map1 : MonoBehaviour {

    float Make_speed;               //몬스터 나오는 시간
    float Make_speed_w;             //몬스터 나오는 시간 (웨이브)
    int Wave_num;                   //웨이브 수

    void Start()
    {
        Make_speed = 0.8f;
        Make_speed_w = 0.5f;
        Wave_num = 0;
        GM.GameManager.getInstance().wave1_time = 0.3f;
        GM.GameManager.getInstance().wave2_time = 0.7f;
    }

	void Update () 
    {
        if (!GM.GameManager.getInstance().pause && !GM.GameManager.getInstance().isGameEnd)
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
            else if (GM.GameManager.getInstance().start_time >= 70f && GM.GameManager.getInstance().start_time - GM.GameManager.getInstance().Now_time >= Make_speed)
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
            
            //@웨이브1
            if(GM.GameManager.getInstance().wave_start &&GM.GameManager.getInstance().start_time - GM.GameManager.getInstance().Now_time_w >= Make_speed_w&& GM.GameManager.getInstance().waveNum.Equals(1))
            {
                GM.GameManager.getInstance().Now_time_w = GM.GameManager.getInstance().start_time;
                Wave_num++;

                switch(Wave_num)
                {
                    case 1:
                        for (int i = 0; i < 3; i++)
                        {
                            Monster_position();
                            GM.GameManager.getInstance().Monster_3_creat();
                        }
                            break;
                    case 2:
                            Monster_position();
                            GM.GameManager.getInstance().Monster_1_creat();
                        break;
                    case 3:
                        Monster_position();
                        GM.GameManager.getInstance().Monster_1_creat();
                        Monster_position();
                        GM.GameManager.getInstance().Monster_2_creat();
                        Monster_position();
                        GM.GameManager.getInstance().Monster_1_creat();
                        break;
                    case 4:
                        Monster_position();
                        GM.GameManager.getInstance().Monster_3_creat();
                        Wave_num = 0;
                        GM.GameManager.getInstance().wave_start = false;
                        break;
                    default:
                        break;
                }
            }
            //@웨이브2
            else if (GM.GameManager.getInstance().wave_start && GM.GameManager.getInstance().start_time - GM.GameManager.getInstance().Now_time_w >= Make_speed_w && GM.GameManager.getInstance().waveNum.Equals(2))
            {
                GM.GameManager.getInstance().Now_time_w = GM.GameManager.getInstance().start_time;
                Wave_num++;

                switch (Wave_num)
                {
                    case 1:
                        for (int i = 0; i < 2; i++)
                        {
                            Monster_position();
                            GM.GameManager.getInstance().Monster_4_creat();
                        }
                        break;
                    case 2:
                        Monster_position();
                        GM.GameManager.getInstance().Monster_1_creat();
                        Monster_position();
                        GM.GameManager.getInstance().Monster_2_creat();
                        Monster_position();
                        GM.GameManager.getInstance().Monster_1_creat();
                        break;
                    case 3:
                        Monster_position();
                        GM.GameManager.getInstance().Monster_1_creat();
                        Monster_position();
                        GM.GameManager.getInstance().Monster_1_creat();
                        break;
                    case 4:
                        Monster_position();
                        GM.GameManager.getInstance().Monster_3_creat();
                        Monster_position();
                        GM.GameManager.getInstance().Monster_3_creat();
                        Wave_num = 0;
                        GM.GameManager.getInstance().wave_start = false;
                        break;
                    default:
                        break;
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
