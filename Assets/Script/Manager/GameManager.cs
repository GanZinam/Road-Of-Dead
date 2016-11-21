using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GM
{
    public class GameManager : MonoBehaviour
    {
        //@ 아이템
        [HideInInspector]
        public int[] itemNum = new int[2];  // 게임내 가지고 갈 아이템
        public UILabel[] itemTxt;           // 게임내 가지고 갈 아이템 개수 txt
        public UI2DSprite[] itemImg;        // 게임내 가지고 갈 아이템 이미지
        public UILabel[] itemCostTxt;       // 아이템 코스트 txt
        public UILabel[] itemManyTxt;       // 아이템 개수 txt       
        public GameObject[] item_slot;       // 아이템 슬롯 오브젝트                    

        //@ 게임 진행 
        [HideInInspector]
        public int waveNum = 0;             // 현 웨이브 수
        [HideInInspector]
        public bool wave_start = false;             // 웨이브 스타트
        [HideInInspector]
        public bool pause = false;          // 게임 일시정지
        [HideInInspector]
        public bool isGameEnd = false;      // 게임 결과창이 난 후
        [HideInInspector]
        public bool plzShaking = false;     // 흔들어야 될때
        public GameObject touch_screen;        // 몬스터가 붙었을때 화면을 터치하시오 txt
        
        public float start_time;            // 게임 시작 시간
        public float Now_time;              // 마지막 체크 시간
        public float Now_time_w;              // 마지막 체크 시간 (웨이브)

        //@ 게임 UI
        public UISlider hpBar;              // hp 스크롤 바
        public UILabel nowMyMoneyTxt;       // 현재 가지고 있는 돈 txt
        [SerializeField]
        GameObject pointPrefab;             // 웨이브 포인트 prefab

        //@ 몬스터
        [HideInInspector]
        public int needShakeCount = 0;      // 좀비수에 따라 흔들어야 되는 수
        [HideInInspector]
        public int monsterKill = 0;         // 죽인 몬스터 수
        [HideInInspector]
        public int nStickMonster = 0;       // 붙은 몬스터 수 (떼어내야될 수)
        [HideInInspector]
        int M1_count = 0;                   // 몬스터 1 수
        [HideInInspector]
        int M2_count = 0;                   // 몬스터 2 수
        [HideInInspector]
        int M3_count = 0;                   // 몬스터 3 수
        [HideInInspector]
        int M4_count = 0;                   // 몬스터 4 수
        [HideInInspector]
        public Vector3 monsterSpawnPos;     // 몬스터 생성 위치

        //@ 몬스터 생성
        [SerializeField]
        GameObject[] mon;                   // 몬스터 prefab
        [SerializeField]
        GameObject monsterParent;           // 몬스터 부모

        //@ 총알
        [HideInInspector]
        public bool Reload = false;         // 총 장전중인지 아닌지
        [HideInInspector]
        public int countBullet = 200;       // 총알 총 갯수

        //@ 몬스터 벡터
        [HideInInspector]
        public List<GameObject> v_monster1 = new List<GameObject>();
        [HideInInspector]
        public List<GameObject> v_monster2 = new List<GameObject>();
        [HideInInspector]
        public List<GameObject> v_monster3 = new List<GameObject>();
        [HideInInspector]
        public List<GameObject> v_monster4 = new List<GameObject>();

        [SerializeField]
        Sprite[] itemSpr;
        public GameObject cam;              // 메인 카메라

        public ITEM[] myItem = new ITEM[2];

        // 싱글
        private static GameManager instance;
        public static GameManager getInstance()
        {
            if (!instance)
            {
                instance = GameObject.FindObjectOfType(typeof(GameManager)) as GameManager;
                if (!instance)
                    Debug.LogError("GM instance Error");
            }
            return instance;
        }


        public void init()
        {
            countBullet = 200;

            myItem[0].type = -1;
            myItem[0].num = 0;
            myItem[1].type = -1;
            myItem[1].num = 0;

            v_monster1.Clear();
            v_monster2.Clear();
            v_monster3.Clear();
            v_monster4.Clear();

            waveNum = 0;
            wave_start = false;
            pause = true;
            isGameEnd = false;
            plzShaking = false;

            needShakeCount = 0;
            monsterKill = 0;
            nStickMonster = 0;
            start_time = 0;


            nowMyMoneyTxt.text = PlayerPrefs.GetInt("Money") + "";


            for (int i = 0; i < 4; i++)
            {
                itemManyTxt[i].text = PlayerPrefs.GetInt(string.Format("Item_{0}", i)) + "";
            }

            //@ 아이템 초기화
            itemNum[0] = 10;
            itemNum[1] = 4;

            itemTxt[0].text = "";
            itemTxt[1].text = "";

            itemCostTxt[0].text = 300 + "";
            itemCostTxt[1].text = 300 + "";
            itemCostTxt[2].text = 300 + "";
            itemCostTxt[3].text = 300 + "";
            itemCostTxt[4].text = 500 + "";
            itemCostTxt[5].text = 1500 + "";
            itemCostTxt[6].text = 3000 + "";


        }


        /**
         * @brief : 몬스터 흔들기
         */
        public void shakingMonster()
        {
            needShakeCount++;

            //!< 붙어있는 몬스터 수 * 4 만큼 클릭해야됨
            if ((nStickMonster * 4) <= needShakeCount)
            {
                //@ 붙어있는 몬스터들 제거
                int monC = v_monster1.Count;
                for (int i = 0; i < v_monster1.Count; i++)
                {
                    v_monster1[i].SendMessage("attack_move");
                    if (!monC.Equals(v_monster1.Count))
                    {
                        monC = v_monster1.Count;
                        i--;
                    }
                }

                monC = v_monster2.Count;
                for (int i = 0; i < v_monster2.Count; i++)
                {
                    v_monster2[i].SendMessage("attack_move");
                    if (!monC.Equals(v_monster2.Count))
                    {
                        monC = v_monster2.Count;
                        i--;
                    }
                }
                monC = v_monster3.Count;
                for (int i = 0; i < v_monster3.Count; i++)
                {
                    v_monster3[i].SendMessage("attack_move");
                    if (!monC.Equals(v_monster3.Count))
                    {
                        monC = v_monster3.Count;
                        i--;
                    }
                } 
                monC = v_monster4.Count;
                for (int i = 0; i < v_monster4.Count; i++)
                {
                    v_monster4[i].SendMessage("attack_move");
                    if (!monC.Equals(v_monster4.Count))
                    {
                        monC = v_monster4.Count;
                        i--;
                    }
                }
                plzShaking = false;
                needShakeCount = 0;
                nStickMonster = 0;
                Reload = false;
            }
        }
        /**
        * @brief : 폭탄 아이템 안에 들어와있는 몬스터 죽이기
        */
        public void cheak_Monster()
        {
            int monC = v_monster1.Count;
            for (int i = 0; i < v_monster1.Count; i++)
            {
                v_monster1[i].SendMessage("boom_item_die");
                if (!monC.Equals(v_monster1.Count))
                {
                    monC = v_monster1.Count;
                    i--;
                }
            }

            monC = v_monster2.Count;
            for (int i = 0; i < v_monster2.Count; i++)
            {
                v_monster2[i].SendMessage("boom_item_die");
                if (!monC.Equals(v_monster2.Count))
                {
                    monC = v_monster2.Count;
                    i--;
                }
            }
            monC = v_monster3.Count;
            for (int i = 0; i < v_monster3.Count; i++)
            {
                v_monster3[i].SendMessage("boom_item_die");
                if (!monC.Equals(v_monster3.Count))
                {
                    monC = v_monster3.Count;
                    i--;
                }
            }
            monC = v_monster4.Count;
            for (int i = 0; i < v_monster4.Count; i++)
            {
                v_monster4[i].SendMessage("boom_item_die");
                if (!monC.Equals(v_monster4.Count))
                {
                    monC = v_monster4.Count;
                    i--;
                }
            }
        }


        /**
         * @brief : 돈 증가
         * @param m : 증가시킬 양
         */
        public void getMoney(int m)
        {
            monsterKill++;  // 돈이 증가 된다 라는 것은 몬스터가 한마리 죽었다는 것

            if (PlayerInfo.quest.Equals(1))
            {
                // 몬스터 100마리 처치 미션
                if (monsterKill.Equals(100))
                {

                }
            }

            PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + m);
            nowMyMoneyTxt.text = PlayerPrefs.GetInt("Money") + "";
        }

        /**
         * @brief : 아이템 슬롯 설정
         * @param idx : 슬롯에 넣을 아이템 인덱스
         */
        public void setItemSlotThis(string idx)
        {
            int i = 0;
            if (!itemTxt[i].text.Equals(""))
            {
                i++;
                if (!itemTxt[i].text.Equals(""))
                {
                    return;
                }
            }

            for (int j = 0; j < 7; j++)
            {
                if (idx.Equals(string.Format("Item_{0}",j)))
                {
                    inputItemSlotTxt(i, j);
                    break;
                }
            }
        }

        /**
         * @brief : 아이템 슬롯 텍스트
         * @param i : 바꿔질 아이템 슬롯 인덱스
         * @param numItem : 바꿀 아이템 코드 넘버
         */
        public void inputItemSlotTxt(int i, int numItem)
        {
            itemImg[i].sprite2D = itemSpr[numItem];
            itemTxt[i].text = PlayerPrefs.GetInt(string.Format("Item_{0}",numItem)) + "";
            myItem[i].type = numItem;
            myItem[i].num = PlayerPrefs.GetInt(string.Format("Item_{0}", numItem));
        }

        /**
         * @brief : 아이템 슬롯 비우기
         * @param idx : 비울 아이템 슬롯 인덱스
         */
        public void setItemSlotBlank(string idx)
        {
            myItem[System.Convert.ToInt32(idx)].type = -1;
            myItem[System.Convert.ToInt32(idx)].num = 0;

            itemImg[System.Convert.ToInt32(idx)].sprite2D = null;
            itemTxt[System.Convert.ToInt32(idx)].text = "";
        }
        #region _MONSTER_CREAT_

        /**
         * @brief : 1번몬스터 생성
         */
        public void Monster_1_creat()
        {
            M1_count++;

            GameObject obj = NGUITools.AddChild(monsterParent, mon[0]) as GameObject;
            obj.transform.localPosition = monsterSpawnPos;
            obj.transform.localScale = new Vector3(0.3f, 0.3f);

            obj.GetComponent<UI2DSprite>().depth = -(int)monsterSpawnPos.y+50;

            GameManager.getInstance().v_monster1.Add(obj);
        }

        /**
         * @brief : 2번몬스터 생성
         */
        public void Monster_2_creat()
        {
            M2_count++;

            GameObject obj = NGUITools.AddChild(monsterParent, mon[1]) as GameObject;
            obj.transform.localPosition = monsterSpawnPos;
            obj.transform.localScale = new Vector3(0.3f, 0.3f);

            obj.GetComponent<UI2DSprite>().depth = -(int)monsterSpawnPos.y + 50;

            GameManager.getInstance().v_monster2.Add(obj);
        }

        /**
         * @brief : 3번몬스터 생성
         */
        public void Monster_3_creat()
        {
            M3_count++;

            GameObject obj = NGUITools.AddChild(monsterParent, mon[2]) as GameObject;
            obj.transform.localPosition = monsterSpawnPos;
            obj.transform.localScale = new Vector3(0.3f, 0.3f);

            obj.GetComponent<UI2DSprite>().depth = -(int)monsterSpawnPos.y + 50;

            GameManager.getInstance().v_monster3.Add(obj);
        }

        /**
         * @brief : 4번몬스터 생성
         */
        public void Monster_4_creat()
        {
            M4_count++;

            GameObject obj = NGUITools.AddChild(monsterParent, mon[3]) as GameObject;
            obj.transform.localPosition = monsterSpawnPos;
            obj.transform.localScale = new Vector3(0.3f, 0.3f);

            obj.GetComponent<UI2DSprite>().depth = -(int)monsterSpawnPos.y + 50;

            GameManager.getInstance().v_monster4.Add(obj);
        }

         /**
         * @brief : 붙어있는 몬스터가 있는지 없는지 체크
         */
        public void Shaking_check()
        {
            if (GM.GameManager.getInstance().nStickMonster.Equals(0))
            {
                GM.GameManager.getInstance().plzShaking = false;
                touch_screen.gameObject.SetActive(false);
            }
        }
        /**
         * @brief : 아이템 데이터 세팅
         * @param i : 바꿔질 아이템 슬롯창 idx
         */
        public void UseItemData(int i)
        {
            PlayerPrefs.SetInt(string.Format("Item_{0}", i), PlayerPrefs.GetInt(string.Format("Item_{0}", i)) - 1);
            GM.GameManager.getInstance().itemManyTxt[i].text = PlayerPrefs.GetInt(string.Format("Item_{0}", i)) + "";
        }

        #endregion
    }
}