using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace GM
{
    public class MainManager : MonoBehaviour
    {
        int whereMe = 0;                // 현재 내 위치
        public GameObject pop;          // info 팝업

        public Image[] images;          // 좀비들 이미지
        public Text mapTxt;             // 맵 이름
        public Text questTxt;           // 퀘스트 내용
        public Text chatTxt;            // 채팅 내용

        public Sprite[] zombieTypes;    // 좀비 타입들
        public GameObject[] rocker;

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

        void Start()
        {
            PlayerPrefs.SetInt("Map_0", 1);
            PlayerPrefs.SetInt("Map_1", 1);
            PlayerPrefs.SetInt("Map_2", 0);
            PlayerPrefs.SetInt("Map_3", 0);
            PlayerPrefs.SetInt("Map_4", 0);

            for (int i = 0; i < 5; i++)
            {
                if (PlayerPrefs.GetInt(string.Format("Map_{0}", i)).Equals(1))
                {
                    rocker[i].SetActive(false);
                }
            }

            PlayerInfo.quest = 1;

        }

        /**
         * @brief : 맵 
         * @param i : 이동할 위치
         */
        public void mapBT(int i)
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

        /**
         * @brief : 팝업 닫기 
         */
        public void popExitBT()
        {
            pop.SetActive(false);
        }

        /**
         * @brief : 게임 시작 
         */
        public void gameStartBT()
        {
            PlayerInfo.hp = 1000;

            Application.LoadLevel("InGameScene");
        }

        public void Nitro(int i)
        {
            for (int j = 0; j < 4; j++)
                PlayerPrefs.SetInt(string.Format("Nitro_{0}", j), 0);
            PlayerPrefs.SetInt(string.Format("Nitro_{0},i"), 1);
        }

        public void Engine(int i)
        {
            for (int j = 0; j < 4; j++)
                PlayerPrefs.SetInt(string.Format("Engine_{0}", j), 0);
            PlayerPrefs.SetInt(string.Format("Engine_{0},i"), 1);
        }

        public void Car(int i)
        {
            for (int j = 0; j < 4; j++)
                PlayerPrefs.SetInt(string.Format("Car_{0}", j), 0);
            PlayerPrefs.SetInt(string.Format("Car_{0},i"), 1);
        }

        public void Tire(int i)
        {
            for (int j = 0; j < 4; j++)
                PlayerPrefs.SetInt(string.Format("Tire_{0}", j), 0);
            PlayerPrefs.SetInt(string.Format("Tire_{0},i"), 1);
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
                else
                {
                    chatTxtCount++;
                    if (isRed)
                    {
                        chatTxtCount++;
                        chatText.text += "<b><color=#a52a2aff>" + chat[i] + "</color></b>";
                        isRed = false;
                        yield return chatText.text;
                    }
                    else
                    {
                        yield return chatText.text += chat[i];
                    }
                }

                i++;
            }
            if (!isChatFinish)
                buttonAnim.SetTrigger("BUTTON");
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
                }
            }
            else if (isChatFinish)
            {
                // 아니라면 다음 대사로 넘어감
                chatTemp = "";
                chatText.text = "";
                sayingSpeed = 0.05f;

                Debug.Log("FDS");
                if (nowScriptPart < chatScript.Count - 1)
                {
                    chatting(chatScript[++nowScriptPart]);
                }
                else
                {
                    chatScript.Clear();
                    chatCanvas.SetActive(false);
                }
            }
        }

        public void YES()
        {
            isChatFinish = true;
            buttonAnim.SetTrigger("DONE");
            chatCheck();
        }

        public void NO()
        {
            buttonAnim.SetTrigger("DONE");
            chatScript.Clear();
            chatTemp = "";
            chatText.text = "";
            sayingSpeed = 0.05f;
        }

        public void showMission()
        {
            chatScript.Clear();
            nowScriptPart = 0;
            chatTxtCount = 0;
            isChatFinish = false;

            chatScript.Add("0/0/주변에 좀비들이 많아 식량을 구하기가 너무 힘드네..@옆 마을로 가는김에 좀비 #1#0#0마리만 없애줄수 있겠나?/");
            chatScript.Add("0/0/고맙네/");
            chatting(chatScript[nowScriptPart]);
        }

        #endregion
    }
}