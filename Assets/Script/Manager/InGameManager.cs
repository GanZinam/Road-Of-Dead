using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace GM
{
    public class InGameManager : MonoBehaviour
    {
        //@ 시스템
        float nowtime;                      // 현재 시각
        float DelayTime;                    // 딜레이 시각
        bool TimeStart = false;
        public UISlider progressBar;        // 게임진행 과정 스크롤 바
        bool item_finish = false;           // 아이템 관련

        //@@ 시스템 - txt
        [SerializeField]
        UILabel nowWaveTxt;                 // 현재 웨이브 txt
        [SerializeField]
        UILabel[] killLabel;                // 죽인 몬스터 txt

        //@@ 시스템 - 애니메이션
        [SerializeField]
        Animator waveAnim;                  // 웨이브 애니메이션
        [SerializeField]
        Animator resultAnim;                // 결과창 애니메이션

        //@ 아이템
        bool[] push_item = new bool[7];     // 아이템 눌렀는지             0.수류탄 1.화염병 2.최류탄 3.섬광탄 4.힐 5.전체폭격 6.아나뽕  
        [HideInInspector]
        public int item_num1;               // 아이템 1번에 선택된 아이템 번호  
        [HideInInspector]
        public int item_num2;               // 아이템 2번에 선택된 아이템 번호                                                                         
        public GameObject item_use_dark;    // 아이템 사용시 어두워짐
        
        [SerializeField]
        GameObject mousePositem;            // 터치한 아이템 위치 표시해주는 오브젝트

        //@ 포지션
        Vector2 touchPos;                   // 내가 터치한 위치
        [SerializeField]
        Transform shootPos;                 // 총 쏠 기준점

        //@ 총 관리
        bool Reload_Check = false;          // 총알 갈고있을때 버튼 클릭 ㄴ
        float Reload_Time = 1f;           // 재장전 시간
        float Reload_NowTime;
        float Bullet_num;                   // 발사된 총알수  

        //@ 총 애니메이션
        [SerializeField]
        Animator reloadAnim;                // 재장전 애니메이션
        [SerializeField]
        Animator shootAnim;                 // 발사 애니메이션 ( 총구 화염 )

        //@ 총알
        int nBullIdx = 0;                   // 총알 오브젝트풀 idx
        [SerializeField]
        UILabel bulletsNumTxt;              // 현재 총알 수 txt
        [SerializeField]
        GameObject[] bulletsObjs;           // 총알 오브젝트 풀
        [SerializeField]
        GameObject mousePosObj;             // 터치한 총알 위치 표시해주는 오브젝트

        //@맵
        [SerializeField]
        GameObject[] map;                   // 맵 prefab

        [SerializeField]
        GameObject gunObj;

        void Start()
        {
            PlayerPrefs.SetInt("Money", 1000);
            PlayerPrefs.SetInt("Item_0", 0);
            PlayerPrefs.SetInt("Item_1", 1);
            PlayerPrefs.SetInt("Item_2", 0);
            PlayerPrefs.SetInt("Item_3", 0);
            PlayerPrefs.SetInt("Item_4", 0);
            PlayerPrefs.SetInt("Item_5", 0);
            PlayerPrefs.SetInt("Item_6", 0);


            GameManager.getInstance().pause = true;
            init();
        }

        /**
         * @brief : 게임 초기화
         */
        public void init()
        {
            GameManager.getInstance().init();
            GameObject obj = NGUITools.AddChild(null, map[PlayerInfo.loadNum]) as GameObject;      //맵 불러오기
            DelayTime = 0.0f;       //총알 연사속도
        }


        void Update()
        {
            if (!GameManager.getInstance().pause)
            {
                #region _MONSTER_CREATE_
               
                //@ 몬스터 생성
                if (Input.GetKeyDown(KeyCode.F1))
                {
                    GM.GameManager.getInstance().Monster_1_creat();
                }
                else if (Input.GetKeyDown(KeyCode.F2))
                {
                    GM.GameManager.getInstance().Monster_2_creat();
                    
                }
                else if (Input.GetKeyDown(KeyCode.F3))
                {
                    GM.GameManager.getInstance().Monster_3_creat();
                    
                }
                else if (Input.GetKeyDown(KeyCode.F4))
                {
                    GM.GameManager.getInstance().Monster_4_creat();
                   
                }
                #endregion

                #region _SHOOT_BULLET_
    
                nowtime += Time.smoothDeltaTime;
                
                
                if (Input.GetMouseButtonDown(0))
                {
                    if (GameManager.getInstance().plzShaking)
                    {
                        GameManager.getInstance().shakingMonster();
                    }
                }
                else if (Input.GetMouseButton(0))
                {
                    touchPos = new Vector2(1280.0f / Screen.width * Input.mousePosition.x, 720.0f / Screen.height * Input.mousePosition.y);
                    if (720.0f / Screen.height * Input.mousePosition.y <= 292)
                    {
                        if (push_item[0] || push_item[1] || push_item[2] || push_item[3])
                        {
                            item_finish = true;

                            mousePositem.SetActive(true);
                            mousePositem.transform.localPosition = touchPos - new Vector2(640, 360);

                        }
                        else if (item_finish.Equals(false))

                        {
                            if (DelayTime <= nowtime)
                            {
                                nowtime = 0f;
                                mousePosObj.SetActive(true);
                                mousePosObj.transform.localPosition = touchPos - new Vector2(640, 360);

                                 
                                float angle = Mathf.Atan2(touchPos.y - gunObj.transform.localPosition.y, touchPos.x - gunObj.transform.localPosition.x) * Mathf.Rad2Deg;
                                gunObj.transform.localEulerAngles = new Vector3(0, 0, -angle + 35);


                                if (!GameManager.getInstance().Reload)
                                    delay(touchPos);       //총알
                            }
                        }
                    }
                    else
                    {
                        mousePosObj.SetActive(false);
                        mousePositem.SetActive(false);
                    }
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    mousePosObj.SetActive(false);
                    if (item_finish)
                    {
                        GameManager.getInstance().cheak_Monster();
                        item_finish = false;
                        item_use_dark.SetActive(false);
                        mousePositem.SetActive(false);
                        for (int i = 0; i < 4; i++)
                        {
                            if (push_item[i])
                                item_exit(i);
                        }
                    }
                }
                #endregion

                //@ 게임 진행 프로그래스바
                if (!GameManager.getInstance().isGameEnd)
                    progressBar.value += Time.deltaTime * 0.01f;

                if (progressBar.value >= (0.3f) && GameManager.getInstance().waveNum.Equals(0))
                {
                    nowWaveTxt.text = "WAVE 1";
                    waveAnim.SetTrigger("wave");
                    GameManager.getInstance().waveNum++;
                }
                else if (progressBar.value >= (0.7f) && GameManager.getInstance().waveNum.Equals(1))
                {
                    nowWaveTxt.text = "WAVE 2";
                    waveAnim.SetTrigger("wave");
                    GameManager.getInstance().waveNum++;
                }
                else if (progressBar.value >= 1)
                {
                    gameEnd(true);  // 게임 성공 (종료)
                }

                // 게임 실패 (종료)
                if (GM.GameManager.getInstance().hpBar.value <= 0)
                    gameEnd(false);
            }

            //@ 장전 체크
            if (GameManager.getInstance().Reload && TimeStart)
            {
                Reload_NowTime += Time.smoothDeltaTime;
                if (Reload_NowTime >= Reload_Time)
                {
                    reloadAnim.SetTrigger("Load");

                    GameManager.getInstance().countBullet = 250;
                    bulletsNumTxt.text = GameManager.getInstance().countBullet + "";

                    Reload_NowTime = 0.0f;
                    GameManager.getInstance().Reload = false;
                    TimeStart = false;
                    Reload_Check = false;
                    Bullet_num = 0f;
                }
            }
        }

        /**
         * @brief : 총알 발사
         * @param pos : 포지션
         */
        void delay(Vector2 pos)
        {
            if (GameManager.getInstance().countBullet >= 0 && !TimeStart && !GameManager.getInstance().plzShaking)
            {
                shootAnim.SetTrigger("Shoot");
                bulletsNumTxt.text = GameManager.getInstance().countBullet + "";

                // 오브젝트 풀 형식으로 변경함
                bulletsObjs[nBullIdx].transform.position = shootPos.position;
                bulletsObjs[nBullIdx].transform.GetComponent<Bullet>().SetRotation(pos);

                // 현재 오브젝트 풀 20개로 제한해 둠
                nBullIdx++;
                if (nBullIdx >= 20)
                    nBullIdx = 0;

                // 총알 수
                GameManager.getInstance().countBullet--;
                Bullet_num++;
                if (GameManager.getInstance().countBullet.Equals(0))
                    reloadAnim.SetTrigger("plzReload");     // 재장전 요청 애니메이션
            }
            else
            {
                if (!GameManager.getInstance().Reload)
                {
                    Reload_NowTime = 0.0f;
                    GameManager.getInstance().Reload = true;
                }
            }
        }

        /**
         * @brief : 재장전 버튼
         */
        public void ReloadBt()
        {
            if (!Reload_Check && !GameManager.getInstance().countBullet.Equals(250))
            {
                reloadAnim.SetTrigger("Reload");    // 재장전 애니메이션

                Reload_Check = true;
                TimeStart = true;
                delay(touchPos);
            }
        }

        /**
         * @brief : 아이템 1 사용 버튼
         */
        public void item0_Bt()
        {
            if (!GameManager.getInstance().pause)
            {
                switch (item_num1)
                {
                    case 0:
                        item0(1);
                        break;
                    case 1:
                        item1(1);
                        break;
                    case 2:
                        item2(1);
                        break;
                    case 3:
                        item3(1);
                        break;
                    case 4:
                        item4(1);
                        break;
                    case 5:
                        item5(1);
                        break;
                    case 6:
                        item6(1);
                        break;
                    default:
                        break;
                }
            }
        }

        /**
         * @brief : 아이템 2 사용 버튼 ( 피 회복 )
         */
        public void item1_Bt()
        {
            if (!GameManager.getInstance().pause)
            {
                switch (item_num2)
                {
                    case 0:
                        item0(2);
                        break;
                    case 1:
                        item1(2);
                        break;
                    case 2:
                        item2(2);
                        break;
                    case 3:
                        item3(2);
                        break;
                    case 4:
                        item4(2);
                        break;
                    case 5:
                        item5(2);
                        break;
                    case 6:
                        item6(2);
                        break;
                    default:
                        break;
                }
            }
        }

        /**
         * @brief : 게임 중지
         * @param isClear : 클리어 했는지 ( true == 클리어 , false == 실패 )
         */
        public void gameEnd(bool isClear)
        {
            GameManager.getInstance().isGameEnd = true;
            if (isClear)
            {
                killLabel[1].text = "KILL : " + GameManager.getInstance().monsterKill;
                resultAnim.SetTrigger("Success"); // 성공
            }
            else
            {
                killLabel[0].text = "KILL : " + GameManager.getInstance().monsterKill;
                resultAnim.SetTrigger("Fail");
            }
        }

        /**
         * @brief : 게임 클리어 후 버튼
         */
        public void gameClearBT()
        {
            Application.LoadLevel("MainScene");
        }

        /**
         * @brief : 게임 실패 후 버튼
         */
        public void gameFailBT()
        {
            Application.LoadLevel("MainScene");
        }

        /**
         * @brief : 아이템 선택 버튼 클릭
         * @param choice : 선택된 아이템 번호
         */
        void item_intro(int choice)
        {
            for (int i = 0; i < 7; i++)
            {
                if (choice.Equals(i))
                {
                    push_item[i] = true;
                }
            }
        }

        /**
         * @brief : 아이템 사용시
         * @param choice : 선택된 아이템 번호
         */
        void item_exit(int choice)
        {
            for (int i = 0; i < 7; i++)
            {
                if (choice.Equals(i))
                {
                    push_item[i] = false;
                }
            }
        }

        #region _ITEM_USE_
        /**
         * @brief : 아이템 0번 수류탄
         * @param num : 아이템 칸번호 (1 or 2)
         */
        public void item0(int num)
        {
            if (!GameManager.getInstance().pause)
            {
                item_use_dark.SetActive(true);
                item_intro(0);
                //GM.GameManager.getInstance().item_Image
            }
        }

        /**
         * @brief : 아이템 1번 화염병
         * @param num : 아이템 칸번호 (1 or 2)
         */
        public void item1(int num)
        {
            if (!GameManager.getInstance().pause)
            {
                item_use_dark.SetActive(true);
                item_intro(1);
            }
        }
        /**
         * @brief : 아이템 2번 최류탄
         * @param num : 아이템 칸번호 (1 or 2)
         */
        public void item2(int num)
        {
            if (!GameManager.getInstance().pause)
            {
                item_use_dark.SetActive(true);
                item_intro(2);
            }
        }
        /**
         * @brief : 아이템 3번 섬광탄
         * @param num : 아이템 칸번호 (1 or 2)
         */
        public void item3(int num)
        {
            if (!GameManager.getInstance().pause)
            {
                item_use_dark.SetActive(true);
                item_intro(3);

            }
        }
        /**
         * @brief : 아이템 4번 힐
         * @param num : 아이템 칸번호 (1 or 2)
         */
        public void item4(int num)
        {
            if (!GameManager.getInstance().pause)
            {
                item_intro(4);
                GameManager.getInstance().hpBar.value += 0.1f;
                item_exit(4);
            }
        }
        /**
         * @brief : 아이템 5번 전체폭격
         * @param num : 아이템 칸번호 (1 or 2)
         */
        public void item5(int num)
        {
            if (!GameManager.getInstance().pause)
            {
                item_intro(5);
                item_exit(5);
            }
        }
        /**
         * @brief : 아이템 6번 아나뽕 (공속UP , 공격력UP)
         * @param num : 아이템 칸번호 (1 or 2)
         */
        public void item6(int num)
        {
            if (!GameManager.getInstance().pause)
            {
                item_intro(6);
                item_exit(6);
            }
        }

       
        #endregion



    }
}