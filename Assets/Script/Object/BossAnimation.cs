﻿using UnityEngine;
using System.Collections;

public class BossAnimation : MonoBehaviour
{
    public int hp;             // 몬스터 hp
    bool item_check;                // 아이템 안에 들어와있나 아닌가 체크


    public Sprite[] attackAnimation;
    public Sprite[] dieAnimation;
    public Sprite[] eyeMoveAnimation;
    public Sprite[] stayAnimation;

    public Sprite[] weaponAnimation;

    UI2DSprite bossAnim;
    [SerializeField]
    UI2DSprite bossWeapon;
    [SerializeField]
    GameObject weaponObj;

    [SerializeField]
    BoxCollider2D[] bossCollider;

    bool canMove = true;        // 앞뒤로 움직이기 관리
    bool isAttacking = false;   // 공격 관리

    bool isUpDown = false;
    float UpDownTime;
    float limitTime;

    float moveSpeed = 0.07f;

    void Start()
    {
        hp = 6000000;

        item_check = false;

        UpDownTime = 0;
        limitTime = Random.Range(1f, 3f);

        transform.localPosition = new Vector3(-820, -100);

        weaponObj.transform.localPosition = new Vector3(-550, 115);
        weaponObj.SetActive(false);

        if (Random.Range(0, 2).Equals(0))
            isUpDown = false;
        else
            isUpDown = true;

        canMove = true;
        bossAnim = GetComponent<UI2DSprite>();

        StartCoroutine("eyeMove", 0);
        StartCoroutine("attackPattern",0);

    }


    void Update()
    {
        if (!isAttacking)
        {
            if (canMove)
            {
                transform.position += new Vector3(moveSpeed * Time.deltaTime, 0);
                if (transform.localPosition.x >= 220)
                {
                    isAttacking = true;
                    StopCoroutine("eyeMove");
                    StopCoroutine("stayMove");
                    weaponObj.SetActive(true);
                    StartCoroutine("weapon", 0);
                }
            }
            else
            {
                transform.position -= new Vector3(1.2f * Time.deltaTime, 0);
                if (transform.localPosition.x <= -100)
                {
                    canMove = true;
                }
            }

            UpDownTime += Time.deltaTime;

            if (isUpDown)
            {
                transform.position += new Vector3(0, 0.1f * Time.deltaTime);
                if (transform.localPosition.y >= -130)
                {
                    UpDownTime = 0;
                    limitTime = Random.Range(1.0f, 5.0f);
                    isUpDown = false;
                }
            }
            else
            {
                transform.position -= new Vector3(0, 0.1f * Time.deltaTime);

                if (transform.localPosition.y <= -250)
                {
                    UpDownTime = 0;
                    limitTime = Random.Range(1.0f, 5.0f);
                    isUpDown = true;
                }
            }

            if (UpDownTime >= limitTime)
            {
                UpDownTime = 0;
                limitTime = Random.Range(2.0f, 6.0f);

                if (isUpDown)
                    isUpDown = false;
                else
                    isUpDown = true;
            }
        }
    }

    IEnumerator eyeMove(int idx)
    {
        yield return new WaitForSeconds(0.1f);

        if (idx.Equals(11))
        {
            StartCoroutine("stayMove", 0);
        }
        else
        {
            bossAnim.sprite2D = eyeMoveAnimation[idx];

            StartCoroutine("eyeMove", ++idx);
        }
    }

    IEnumerator stayMove(int idx)
    {
        yield return new WaitForSeconds(0.1f);

        if (idx.Equals(11))
        {
            StartCoroutine("eyeMove", 0);
        }
        else
        {
            bossAnim.sprite2D = stayAnimation[idx];

            StartCoroutine("stayMove", ++idx);
        }
    }

    IEnumerator attack(int idx)
    {
        yield return new WaitForSeconds(0.1f);

        if (idx.Equals(11))
        {
            canMove = false;
            isAttacking = false;
            StartCoroutine("eyeMove", 0);
        }
        else
        {
            bossAnim.sprite2D = attackAnimation[idx];

            StartCoroutine("attack", ++idx); 
            if (idx.Equals(9))
            {
                GM.GameManager.getInstance().hpBar.value -= 0.2f;
            }
        }
    }

    IEnumerator death(int idx)
    {
        yield return new WaitForSeconds(0.1f);

        if (idx.Equals(10))
        {
            // 죽음
            Destroy(gameObject);
            GM.GameManager.getInstance().BossDeath = true;
            GM.GameManager.getInstance().boss = null;
        }
        else
        {
            bossAnim.sprite2D = dieAnimation[idx];

            StartCoroutine("death", ++idx);
        }
    }

    IEnumerator attackPattern(int idx)
    {
        yield return new WaitForSeconds(10);

        if (idx.Equals(4))
            idx = 0;

        if (idx.Equals(0))
        {
            // 일반 좀비 4명 소환
            for (int i = 0; i < 4; i++)
            {
                GM.GameManager.getInstance().monsterSpawnPos.x = Random.Range(-650, -700);      //-760 ~ -650
                GM.GameManager.getInstance().monsterSpawnPos.y = Random.Range(28, -310);
                GM.GameManager.getInstance().Monster_1_creat();

            }
        }
        else if (idx.Equals(1))
        {
            // 체력 좀비 3마리 소환

            for (int i = 0; i < 3; i++)
            {
                GM.GameManager.getInstance().monsterSpawnPos.x = Random.Range(-650, -700);      //-760 ~ -650
                GM.GameManager.getInstance().monsterSpawnPos.y = Random.Range(28, -310);
                GM.GameManager.getInstance().Monster_3_creat();
            }
        }
        else if (idx.Equals(2))
        {
            // 공격 좀비 3마리 소환
            for (int i = 0; i < 3; i++)
            {
                GM.GameManager.getInstance().monsterSpawnPos.x = Random.Range(-650, -700);      //-760 ~ -650
                GM.GameManager.getInstance().monsterSpawnPos.y = Random.Range(28, -310);
                GM.GameManager.getInstance().Monster_2_creat();
            }
        }
        else
        {
            // 밸런스 좀비 2마리 소환

            for (int i = 0; i < 2; i++)
            {
                GM.GameManager.getInstance().monsterSpawnPos.x = Random.Range(-650, -700);      //-760 ~ -650
                GM.GameManager.getInstance().monsterSpawnPos.y = Random.Range(28, -310);
                GM.GameManager.getInstance().Monster_4_creat();
            }
        }

        StartCoroutine("attackPattern", ++idx);
    }

    IEnumerator weapon(int idx)
    {
        yield return new WaitForSeconds(0.1f);

        if (idx.Equals(7))
        {
            weaponObj.transform.localPosition = new Vector3(-360, 140);
            weaponObj.SetActive(false);
            StartCoroutine("attack", 0);
        }
        else
        {
            weaponObj.transform.localPosition += new Vector3(75, 0);

            bossWeapon.sprite2D = weaponAnimation[idx];

            StartCoroutine("weapon", ++idx);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            if (gameObject.transform.localPosition.y + other.gameObject.transform.localPosition.y + 200 <= 380 &&
                gameObject.transform.localPosition.y + other.gameObject.transform.localPosition.y + 200 >= 300)
            {
                // 추가 데미지
                hp -= 10000;
            }

            // 총알이 충돌됬을시 지우지 않고 안보이는 곳으로 이동시킨후 다시 사용
            hp -= GM.GameManager.getInstance().Bullet_Damege;
            other.gameObject.transform.localPosition = new Vector3(-1500, -2000);

            deathCheck();
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
     * @brief : 폭탄으로 몬스터 죽이기
     */
    public void boom_item_die()
    {
        Debug.Log(item_check);
        if (item_check)
        {
            Debug.Log("White");
            hp -= 150;
            gameObject.GetComponent<UI2DSprite>().color = Color.white;

            deathCheck();
        }
    }

    public void deathCheck()
    {
        if (hp <= 0)
        {
            if (PlayerPrefs.GetInt("Q_0_IS").Equals(1))
            {
                int k = PlayerPrefs.GetInt("Q_0_MONSTER_KILL");
                k++;
                PlayerPrefs.SetInt("Q_0_MONSTER_KILL", k);
            }
            
            bossCollider[0].enabled = false;
            bossCollider[1].enabled = false;

            StartCoroutine("death", 0);
        }
    }
}