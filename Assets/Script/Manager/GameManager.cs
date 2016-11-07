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

        //@ 게임 진행 
        [HideInInspector]
        public int waveNum = 0;             // 현 웨이브 수
        [HideInInspector]
        public bool pause = false;          // 게임 일시정지
        [HideInInspector]
        public bool isGameEnd = false;      // 게임 결과창이 난 후
        [HideInInspector]
        public bool plzShaking = false;     // 흔들어야 될때
        public UILabel touch_screen;        // 몬스터가 붙었을때 화면을 터치하시오 txt      
        

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

        //@ 총알
        [HideInInspector]
        public bool Reload = false;         // 총 장전중인지 아닌지
        [HideInInspector]
        public int countBullet = 250;       // 총알 총 갯수

        //@ 몬스터 벡터
        [HideInInspector]
        public List<GameObject> v_monster1 = new List<GameObject>();
        [HideInInspector]
        public List<GameObject> v_monster2 = new List<GameObject>();

        [SerializeField]
        Sprite[] itemSpr;

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
            myItem[0].type = -1;
            myItem[0].num = 0;
            myItem[1].type = -1;
            myItem[1].num = 0;

            v_monster1.Clear();
            v_monster2.Clear();

            waveNum = 0;
            pause = true;
            isGameEnd = false;
            plzShaking = false;

            needShakeCount = 0;
            monsterKill = 0;
            nStickMonster = 0;

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

            itemCostTxt[0].text = 10 + "";
            itemCostTxt[1].text = 20 + "";
            itemCostTxt[2].text = 30 + "";
            itemCostTxt[3].text = 40 + "";
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
        }


        /**
         * @brief : 돈 증가
         * @param m : 증가시킬 양
         */
        public void getMoney(int m)
        {
            monsterKill++;  // 돈이 증가 된다 라는 것은 몬스터가 한마리 죽었다는 것

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
        void inputItemSlotTxt(int i, int numItem)
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
    }
}