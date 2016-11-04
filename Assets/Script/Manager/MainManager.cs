using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

        /**
         * @brief : 맵 
         * @param i : 이동할 위치
         */
        public void mapBT(int i)
        {
            mapTxt.text = "";
            questTxt.text = "";
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
    }
}