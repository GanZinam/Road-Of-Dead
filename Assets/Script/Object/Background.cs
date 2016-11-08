using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour
{
    public float BG_speed;

    void Update()
    {
        if (!GM.GameManager.getInstance().pause)
        {
            transform.position -= new Vector3(BG_speed * Time.smoothDeltaTime, 0);

            if (transform.localPosition.x <= -6513f)
            {
                transform.localPosition = new Vector3(-384.5f, 0, 0);
            }
        }
    }
}
