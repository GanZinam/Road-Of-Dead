using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour
{
    public int hp = 10;             // 몬스터 hp
    public float speed = 0.5f;      // 몬스터 이동 속도

    bool need_;                     // 트럭에 붙어 있나 없나 체크
    bool item_check;                // 아이템 안에 들어와있나 아닌가 체크


    void Start()
    {

        need_ = false;
        item_check = false;

        StartCoroutine(update());
    }

    public IEnumerator update()
    {
        yield return null;
        if (hp <= 0)
        {
            GM.GameManager.getInstance().getMoney(100);
            removeAtVector();
            Destroy(gameObject);        //가까이 없을때만 공격 당해서 죽음
        }
        if (!need_)
        {
            move();     //움직이는 함수

            if (200 <= gameObject.transform.localPosition.x)        //200 = 자동차 끝부분  need 가까이있으면 true되어 공격으로 ㄱ
            {
                GM.GameManager.getInstance().touch_screen.gameObject.SetActive(true);   //몬스터가 붙었을때 화면을 터치하시오 txt 생성
                GM.GameManager.getInstance().plzShaking = true;
                need_ = true;
                GM.GameManager.getInstance().nStickMonster += 1;
            }
        }
        else                                //공격하는부분
        {
            yield return new WaitForSeconds(0.5f);      //0.5초당 한번씩 공격  (몬스터 공격 딜레이 시간)
            attack();
        }
        StartCoroutine(update());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            // 총알이 충돌됬을시 지우지 않고 안보이는 곳으로 이동시킨후 다시 사용
            other.gameObject.transform.localPosition = new Vector3(-1500, -2000);
            hp -= 1;
        }
        if (other.gameObject.CompareTag("Item"))
        {
            // 아이템 범위 안에 들어오면 빨간색으로 변함
            gameObject.GetComponent<UI2DSprite>().color = Color.red;
            item_check = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            // 아이템 범위 밖에 나가면 하얀색으로 변함
            gameObject.GetComponent<UI2DSprite>().color = Color.white;
            item_check = false;
        }
    }

    /**
     * @brief : 터치로 몬스터 죽이기
     */
    public void attack_move()
    {
        if (need_)
        {
            // 현재 자동차에 붙어서 공격중이며 죽어야 할 대상일시 오브젝트를 지워 준다.
            GM.GameManager.getInstance().touch_screen.gameObject.SetActive(false);   //몬스터가 붙었을때 화면을 터치하시오 txt 삭제
            GM.GameManager.getInstance().getMoney(100);                              //몬스터 죽는 만큼 돈
            removeAtVector();
            Destroy(gameObject);
        }
    }

    /**
     * @brief : 폭탄으로 몬스터 죽이기
     */
    public void boom_item_die()
    {
        Debug.Log(1);
        if (item_check)
        {
            hp -= 10;
        }
    }

    /**
     * @brief 몬스터가 데미지를 받을때
     */
    public virtual void attack() { }

    /**
     * @brief 몬스터 이동 관리
     */
    public virtual void move() { }

    /**
     * @brief 몬스터 벡터에서 삭제
     */
    public virtual void removeAtVector() { }
}