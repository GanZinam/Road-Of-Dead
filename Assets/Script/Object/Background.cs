using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour
{
    public float speed = 0;

    void Update()
    {
        if (!GM.GameManager.getInstance().pause)
        {
            transform.position -= new Vector3(speed * Time.deltaTime, 0);

            if (transform.localPosition.x <= -5470)
            {
                transform.localPosition = new Vector3(0, 0, 0);
            }
        }
    }
}
