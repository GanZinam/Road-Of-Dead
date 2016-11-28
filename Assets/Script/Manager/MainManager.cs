using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace GM
{
    public class MainManager : MonoBehaviour
    {
        public GameObject pop;              // info 팝업

        public Image[] images;              // 좀비들 이미지
        public Text mapTxt;                 // 맵 이름
        public Text questTxt;               // 퀘스트 내용
        public Text chatTxt;                // 채팅 내용
        public GameObject CustomBT;        // 커스텀 버튼

        public Sprite[] zombieTypes;        // 좀비 타입들

        public GameObject[] Map_Canvas;     // Map_Canvas 3개  0. LastTown  1. Volt 
        public GameObject Basic_Canvas;     // BasicCanvas

        public GameObject[] custom_Type;    // 0. Netro 1. Spray 2. Car 3. Tire
        public Sprite[] Netro_onj;          // 0. None 1. Netro
        public Sprite[] Spray_onj;          // 0. None 1. Spray(Star)
        public Sprite[] Car_onj;            // 0. normal 1. normal(yellow)
        public Sprite[] Tire_onj;           // 0. normal 1. Big

        public GameObject[] Car_Custom;     
        public GameObject[] Car_Netro;
        public GameObject[] Car_Spray;
        public GameObject[] Car_Tire;

        public Image[] Car_Obj;             // 차가 가지고있는 것들

        public GameObject[] Map_BT;         // 0. LastTown 1. Volt 2. Beretta 3. None 4. None
        public GameObject[] Map_Close_BT;   // 0. LastTown 1. Volt 2. Beretta 3. None 4. None

        string chatTemp = "";
        [SerializeField]
        UnityEngine.UI.Text chatText;
        [SerializeField]
        UnityEngine.UI.Text charName;
        float sayingSpeed = 0.05f;
        byte nowScriptPart = 0;                 // 현재 대본 파트
        public List<string> chatScript;         // 채팅 대본
        public GameObject chatCanvas;

        public Animator buttonAnim;
        int chatTxtCount = 0;
        
        [SerializeField]
        Text[] myQuestTxt;
        [SerializeField]
        GameObject basicCanvas;

        [SerializeField]
        GameObject nowPos;

        [SerializeField]
        Transform[] mapPos;





        void Start()
        {
            nowPos.transform.localPosition = mapPos[PlayerPrefs.GetInt("NowMyPos")].localPosition;


            checkMap();
            
            checkQuest();

            checkCar();

        }

        public void CheckWorldMap()
        {
            for (int i = 0; i < 5; i++)
            {
                if(PlayerPrefs.GetInt(string.Format("Map_{0}", i )).Equals(1))
                {
                    Map_Close_BT[i].SetActive(false);
                    Map_BT[i].SetActive(true);
                }
                else
                {
                    Map_Close_BT[i].SetActive(true);
                    Map_BT[i].SetActive(false);
                }
                
            }
        }


        void checkQuest()
        {
            if (PlayerPrefs.GetInt("Q_0_IS").Equals(1))
                PlayerInfo.quest[0] = 1;

            int qNum = 0;

            for (int i =0; i< PlayerInfo.quest.Length; i++)
            {
                if (!PlayerInfo.quest[i].Equals(0))
                {
                    if (PlayerInfo.quest[i].Equals(1))
                    {
                        myQuestTxt[qNum].text = "좀비 처치  ( " + PlayerPrefs.GetInt("Q_0_MOSNTER_KILL") + " / 100 )"; 
                    }
                }
            }
        }

        void checkMap()
        {
            Debug.Log(PlayerPrefs.GetInt("NowMyPos"));
            Basic_Canvas.SetActive(true);
            for(int i = 0; i<3;i++)
            {
                if (PlayerPrefs.GetInt("NowMyPos").Equals(i))
                {
                    Map_Canvas[i].SetActive(true);
                }
                else
                {
                    Map_Canvas[i].SetActive(false);
                }
            }
        }

        void checkCar()
        {
            Car_Custom[0].SetActive(false);
            Car_Custom[1].SetActive(false);
            Car_Tire[0].SetActive(false);
            Car_Tire[1].SetActive(false);
            Car_Tire[2].SetActive(false);
            Car_Tire[3].SetActive(false);

            for (int i = 0; i < 4; i++)
            {
                if (PlayerPrefs.GetInt(string.Format("Nitro_{0}", i)).Equals(1))
                {
                    if (i.Equals(1))
                    {
                        Car_Netro[0].SetActive(true);
                        Car_Netro[1].SetActive(true);
                    }
                    else 
                    {
                        Car_Netro[0].SetActive(false);
                        Car_Netro[1].SetActive(false);
                    }
                }
                if (PlayerPrefs.GetInt(string.Format("Spray_{0}", i)).Equals(1))
                {
                    if (i.Equals(1))
                    {
                        Car_Spray[0].SetActive(true);
                        Car_Spray[1].SetActive(true);
                    }
                    else
                    {
                        Car_Spray[0].SetActive(false);
                        Car_Spray[1].SetActive(false);
                    }
                }
                if (PlayerPrefs.GetInt(string.Format("Car_{0}", i)).Equals(1))
                {
                    Car_Custom[i].SetActive(true);
                }
                if (PlayerPrefs.GetInt(string.Format("Tire_{0}", i)).Equals(1))
                {
                    if(i.Equals(1))
                    {
                        Car_Tire[1].SetActive(true);
                        Car_Tire[3].SetActive(true);
                    }
                    else if(i.Equals(0))
                    {
                        Car_Tire[0].SetActive(true);
                        Car_Tire[2].SetActive(true);
                    }
                }
            }
        }
        
        public void CustomCheck()
        {
            for (int i = 0; i < 4; i++)
            {
                if(PlayerPrefs.GetInt(string.Format("Nitro_{0}", i)).Equals(1))
                {
                    Car_Obj[0].sprite = Netro_onj[i];
                }
                if (PlayerPrefs.GetInt(string.Format("Spray_{0}", i)).Equals(1))
                {
                    Car_Obj[1].sprite = Spray_onj[i];
                }
                if (PlayerPrefs.GetInt(string.Format("Car_{0}", i)).Equals(1))
                {
                    Car_Obj[2].sprite = Car_onj[i];
                }
                if (PlayerPrefs.GetInt(string.Format("Tire_{0}", i)).Equals(1))
                {
                    Car_Obj[3].sprite = Tire_onj[i];
                }
            }
        }

        /**
         * @brief : 맵 
         * @param i : 이동할 위치
         */
        public void mapBT(int i)
        {
            if (i != PlayerPrefs.GetInt("NowMyPos"))
            {
                PlayerInfo.loadNum = i;
                Debug.Log("next_map_num : " + PlayerInfo.loadNum);
                mapTxt.text = "";
                if (PlayerInfo.quest.Equals(1))
                {
                    questTxt.text = "좀비 100마리 처치!";
                }
                chatTxt.text = "";

                images[0].sprite = zombieTypes[0];
                images[1].sprite = zombieTypes[1];
                images[2].sprite = zombieTypes[2];
                images[3].sprite = zombieTypes[3];

                pop.SetActive(true);
            }
        }

        /**
         * @brief : 팝업 닫기 
         */
        public void popExitBT()
        {
            pop.SetActive(false);
        }

        /**
         * @brief : 뒤로 가기
         */
        public void BackBT()
        {
            checkMap();
        }

        /**
         * @brief : 게임 시작 
         */
        public void gameStartBT()
        {
            PlayerInfo.hp = 1000;

            Application.LoadLevel("InGameScene");
        }

        /**
         * @brief : Nitro 선택
         * @param i : 몇번째 Nitro인지
         */
        public void Nitro(int i)
        {
            for (int j = 0; j < 4; j++)
                PlayerPrefs.SetInt(string.Format("Nitro_{0}", j), 0);
            PlayerPrefs.SetInt(string.Format("Nitro_{0}", i), 1);
            Car_Obj[0].sprite = Netro_onj[i];
            //Car_Obj[0].sprite = Resources.Load<Sprite>("Nitro"+i)as Sprite;

        }
        /**
         * @brief : Spray 선택
         * @param i : 몇번째 Spray인지
         */
        public void Spray(int i)
        {
            for (int j = 0; j < 4; j++)
                PlayerPrefs.SetInt(string.Format("Spray_{0}", j), 0);
            PlayerPrefs.SetInt(string.Format("Spray_{0}", i), 1);
            Car_Obj[1].sprite = Spray_onj[i];
        }
        /**
         * @brief : Car 선택
         * @param i : 몇번째 Car인지
         */
        public void Car(int i)
        {
            for (int j = 0; j < 4; j++)
                PlayerPrefs.SetInt(string.Format("Car_{0}", j), 0);
            PlayerPrefs.SetInt(string.Format("Car_{0}", i), 1);

            Car_Obj[2].sprite = Car_onj[i];
        }
        /**
         * @brief : Tire 선택
         * @param i : 몇번째 Tire인지
         */
        public void Tire(int i)
        {
            for (int j = 0; j < 4; j++)
                PlayerPrefs.SetInt(string.Format("Tire_{0}", j), 0);
            PlayerPrefs.SetInt(string.Format("Tire_{0}", i), 1);

            Car_Obj[3].sprite = Tire_onj[i];
        }



        #region _CHAT_


        /**
         * @brief : 채팅 기능
         * @param loadDataTxt : 불러온 대사 파트 ( 캐릭터 번호 / 캐릭터 표정 / 캐릭터 위치 / 캐릭터 대사 )
         */
        void chatting(string loadDataTxt)
        {
            int strIdx = 0;

            int readWhat = 0;   // 어디 파트를 읽고 있는지
            int charIdx = 0;
            int charEmote = 0;

            while (strIdx < loadDataTxt.Length)
            {
                if (loadDataTxt[strIdx].Equals('/'))
                {
                    if (readWhat.Equals(0))         // 캐릭터 번호
                    {
                        charIdx = System.Convert.ToInt32(chatTemp);
                        chatTemp = "";
                    }
                    else if (readWhat.Equals(1))    // 캐릭터 일러스트 번호
                    {
                        charEmote = System.Convert.ToInt32(chatTemp);
                        chatTemp = "";
                    }
                    else if (readWhat.Equals(2))    // 캐릭터 이름
                    {
                        switch (charIdx)
                        {
                            case 0:
                                charName.text = "<자칭> 섻수 마스터"; break;
                            case 1:
                                charName.text = "지크"; break;
                            default:
                                charName.text = ""; break;
                        }

                        chatText.text = "";
                        StartCoroutine(chatSaying(chatTemp));   // 대화 코루틴 시작
                    }

                    //chatTemp = "";
                    readWhat++;
                }
                else
                {
                    chatTemp += loadDataTxt[strIdx];
                }
                strIdx++;
            }
        }

        bool isRed = false; 
        bool isGreen = false;
        bool isChatFinish = false;

        /**
         * @brief : 대화
         * @param chat : 대사
         */
        IEnumerator chatSaying(string chat)
        {
            int i = 0;

            yield return new WaitForSeconds(0.2f);

            while (i < chat.Length)
            {
                yield return new WaitForSeconds(sayingSpeed);
                if (chat[i].Equals('@'))
                {
                    chatTxtCount++;
                    yield return chatText.text += "\n";
                }
                else if (chat[i].Equals('#'))
                    isRed = true;
                else if (chat[i].Equals('%'))
                    isGreen = true;
                else if (chat[i].Equals('$'))
                {
                    // 수락 거절
                    if (!isChatFinish)
                        buttonAnim.SetTrigger("BUTTON");
                }
                else
                {
                    chatTxtCount++;
                    if (isRed)
                    {
                        chatTxtCount++;
                        chatText.text += "<b><color=#E74C3C>" + chat[i] + "</color></b>";
                        isRed = false;
                        yield return chatText.text;
                    }
                    else if (isGreen)
                    {
                        chatTxtCount++;
                        chatText.text += "<b><color=#1ABC9C>" + chat[i] + "</color></b>";
                        isGreen = false;
                        yield return chatText.text;
                    }
                    else
                    {
                        yield return chatText.text += chat[i];
                    }
                }

                i++;
            }
        }



        /**
         * @brief : 말하고 있는지 아닌지 체크
         */
        public void chatCheck()
        {
            if (!chatTemp.Length.Equals(chatTxtCount))/* chatTemp.Equals(chatText.text))*/
            {
                // 말하는 도중이면 빠르게 말하도록 설정
                sayingSpeed = 0;

                if (isChatFinish)
                {
                    chatTemp = "";
                    chatText.text = "";
                    sayingSpeed = 0.05f;

                    chatScript.Clear();
                    chatCanvas.SetActive(false);
                    basicCanvas.SetActive(true);
                    CustomBT.SetActive(true);
                }
            }
            else //if (isChatFinish)
            {
                Debug.Log("FINIS");
                // 아니라면 다음 대사로 넘어감
                chatTxtCount = 0;
                chatTemp = "";
                chatText.text = "";
                sayingSpeed = 0.05f;
                
                if (nowScriptPart < chatScript.Count - 1)
                {
                    chatting(chatScript[++nowScriptPart]);
                }
                else
                {
                    chatScript.Clear();
                    chatCanvas.SetActive(false);
                    basicCanvas.SetActive(true);
                    CustomBT.SetActive(true);
                }
            }
        }

        public void YES()
        {
            if (PlayerPrefs.GetInt("Q_0_IS").Equals(0))
            {
                PlayerPrefs.SetInt("Q_0_IS", 1);
                PlayerPrefs.SetInt("Q_0_MONSTER_KILL", 0);

                isChatFinish = true;
                buttonAnim.SetTrigger("DONE");

                chatCheck();

                checkQuest();
            }
        }

        public void NO()
        {
            isChatFinish = true;
            buttonAnim.SetTrigger("DONE");

            basicCanvas.SetActive(true);
            CustomBT.SetActive(true);
        }

        public void showMission(int type)
        {
            if (type.Equals(0) && PlayerPrefs.GetInt("Q_0_IS").Equals(0))
            {
                basicCanvas.SetActive(false);

                chatScript.Clear();
                nowScriptPart = 0;
                chatTxtCount = 0;
                isChatFinish = false;

                chatScript.Add("0/0/이봐, 자네/");
                chatScript.Add("0/0/자네 혹시 베르타 주변에서 출몰한다는 #카#코 #데#빌 이라고 아는가?/");
                chatScript.Add("0/0/그 놈 때문에 골머리가 썩는다는데.../");
                chatScript.Add("0/0/#카#코 #데#빌 을 #처#치 한다면 베르타에 있는 내 친구 %김%동%원 이@%1%0%0%0%0%볼%트 를 줄 걸세$/"); 

                chatting(chatScript[nowScriptPart]);
            }
            else
            {
                Debug.Log("IS ALREADY HAVE");
                chatCanvas.SetActive(false);
                basicCanvas.SetActive(true);
                CustomBT.SetActive(true);
            }
        }

        #endregion
    }
}