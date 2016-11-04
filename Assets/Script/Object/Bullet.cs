using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    /**
     * @brief : 총알 이동 방향 회전
     * @param pos : 돌릴 값
     */
    public void SetRotation(Vector3 pos)
    {
        float angle = Mathf.Atan2(pos.y - transform.localPosition.y, pos.x - transform.localPosition.x) * Mathf.Rad2Deg;
        transform.localEulerAngles = new Vector3(0, 0, angle - 90);
    }

    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * 10);
    }
}
