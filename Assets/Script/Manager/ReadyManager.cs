using UnityEngine;
using System.Collections;

namespace GM
{
    public class ReadyManager : MonoBehaviour
    {
        [SerializeField]
        Animator readyPop;
        [SerializeField]
        Animator startPop;

        /**
         * @brief : 게임 시작 버튼
         */
        public void startBT()
        {
            readyPop.SetBool("Quit", true);
            startPop.SetBool("Start", true);
            StartCoroutine(waitingStart());
            
        }

        /**
         * @brief : 게임 시작 기다리기 (애니메이션 취함)
         */
        IEnumerator waitingStart()
        {
            yield return new WaitForSeconds(1);
            GM.GameManager.getInstance().pause = false;
        }

        /**
         * @brief : 아이템 구매 버튼
         * @param name : 아이템 tag
         */
        public void buyItem(string name)
        {
            int money = 0;


            for (int j = 0; j < 7; j++)
            {
                if (j.Equals(0))
                {
                    money = 300;
                }
                else if (j.Equals(1))
                {
                    money = 300;
                }
                else if (j.Equals(2))
                {
                    money = 300;
                }
                else if (j.Equals(3))
                {
                    money = 300;
                }
                else if (j.Equals(4))
                {
                    money = 500;
                }
                else if (j.Equals(5))
                {
                    money = 1500;
                }
                else if (j.Equals(6))
                {
                    money = 3000;
                }
                if (name.Equals(string.Format("Item_{0}", j)) && PlayerPrefs.GetInt("Money") >= money)
                {
                    setItemPrefData(j, money);
                    break;
                }
            }
        }

        /**
         * @brief : 아이템 데이터 세팅
         * @param i : 바꿔질 아이템 슬롯창 idx
         * @param cost : 아이템 구매 비용
         */
        void setItemPrefData(int i, int cost)
        {
            // 아이템 증가
            PlayerPrefs.SetInt(string.Format("Item_{0}", i), PlayerPrefs.GetInt(string.Format("Item_{0}", i)) + 1);
            GM.GameManager.getInstance().itemManyTxt[i].text = PlayerPrefs.GetInt(string.Format("Item_{0}", i)) + "";

            // 돈 감소
            PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") - cost);
            GM.GameManager.getInstance().nowMyMoneyTxt.text = PlayerPrefs.GetInt("Money") + "";

            if (GM.GameManager.getInstance().myItem[0].type.Equals(i))
                GM.GameManager.getInstance().itemTxt[0].text = PlayerPrefs.GetInt(string.Format("Item_{0}", i)) + "";
            else if (GM.GameManager.getInstance().myItem[1].type.Equals(i))
                GM.GameManager.getInstance().itemTxt[1].text = PlayerPrefs.GetInt(string.Format("Item_{0}", i)) + "";
        }
    }
}