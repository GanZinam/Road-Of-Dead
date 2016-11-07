using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour
{
    public int hp = 10;             // 몬스터 hp
    public float speed = 0.5f;      // 몬스터 이동 속도

    bool need_;

    float nowtime;
    float DelayTime;


    void Start()
    {
        need_ = false;
        DelayTime = 0.5f;

        StartCoroutine(update());
    }

    public IEnumerator update()
    {
        yield return null;

        if (!need_)
        {
            if (hp <= 0)
            {
                GM.GameManager.getInstance().getMoney(100);
                removeAtVector();
                Destroy(gameObject);        //가까이 없을때만 공격 당해서 죽음
            }

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
            yield return new WaitForSeconds(0.5f);      //0.5초당 한번씩 공격
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
    }

    /**
     * @brief : 터치로 몬스터 죽이기
     */
    public void attack_move()
    {
        if (need_ && GM.GameManager.getInstance().Die)
        {
            // 현재 자동차에 붙어서 공격중이며 죽어야 할 대상일시 오브젝트를 지워 준다.
            GM.GameManager.getInstance().touch_screen.gameObject.SetActive(false);   //몬스터가 붙었을때 화면을 터치하시오 txt 삭제
            Destroy(gameObject);
            removeAtVector();
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