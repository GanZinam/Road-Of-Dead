using UnityEngine;
using System.Collections;

public class BossAnimation : MonoBehaviour
{
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

    bool canMove = true;        // 앞뒤로 움직이기 관리
    bool isAttacking = false;   // 공격 관리

    bool isUpDown = false;
    float UpDownTime;
    float limitTime;


    void Start()
    {
        UpDownTime = 0;
        limitTime = Random.Range(1f, 3f);

        if (Random.Range(0, 2).Equals(0))
            isUpDown = false;
        else
            isUpDown = true;

        canMove = true;
        bossAnim = GetComponent<UI2DSprite>();

        StartCoroutine("eyeMove", 0);

        //weaponObj.SetActive(true);
        //StartCoroutine("weapon", 0);
    }


    void Update()
    {
        if (!isAttacking)
        {
            if (canMove)
            {
                transform.position += new Vector3(0.3f * Time.deltaTime, 0);
                if (transform.localPosition.x >= 140)
                {
                    isAttacking = true;
                    StopCoroutine("eyeMove");
                    StopCoroutine("stayMove");
                    StartCoroutine("attack", 0);
                }
            }
            else
            {
                transform.position -= new Vector3(1 * Time.deltaTime, 0);
                if (transform.localPosition.x <= -300)
                {
                    Debug.Log("ATTACK");
                    canMove = true;
                }
            }

            UpDownTime += Time.deltaTime;
            
            if (isUpDown)
            {
                transform.position += new Vector3(0, 0.1f * Time.deltaTime);
                if (transform.localPosition.y >= -10)
                    isUpDown = false;
            }
            else
            {
                transform.position -= new Vector3(0, 0.1f * Time.deltaTime);

                if (transform.localPosition.y <= -170)
                    isUpDown = true;
            }

            if (UpDownTime >= limitTime)
            {
                UpDownTime = 0;
                limitTime = Random.Range(1f, 3f);

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
        }
    }

    IEnumerator death(int idx)
    {
        yield return new WaitForSeconds(0.1f);

        if (idx.Equals(10))
        {
            // 죽음
            Destroy(gameObject);
        }
        else
        {
            bossAnim.sprite2D = dieAnimation[idx];

            StartCoroutine("death", ++idx);
        }
    }

    IEnumerator attackPattern(int idx)
    {
        yield return new WaitForSeconds(15);

        if (idx.Equals(4))
            idx = 0;

        if (idx.Equals(0))
        {
            // 일반 좀비 4명 소환
        }
        else if (idx.Equals(1))
        {
            // 체력 좀비 3마리 소환
        }
        else if (idx.Equals(2))
        {
            // 공격 좀비 3마리 소환
        }
        else
        {
            // 밸런스 좀비 2마리 소환
        }

        StartCoroutine("attackPattern", ++idx);
    }

    IEnumerator weapon(int idx)
    {
        yield return new WaitForSeconds(0.1f);

        if (idx.Equals(7))
        {
            weaponObj.SetActive(false);
        }
        else
        {
            bossWeapon.sprite2D = weaponAnimation[idx];

            StartCoroutine("weapon", ++idx);
        }
    }
}
