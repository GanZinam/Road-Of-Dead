using UnityEngine;
using System.Collections;

public class Monster2 : Monster
{
    public override void init()
    {
        hp = 80;
        speed = 0.75f;
    }

    public override void attack()
    {
        GM.GameManager.getInstance().hpBar.value -= 0.015f;
    }

    public override void move()
    {
        gameObject.transform.Translate(Vector2.right * Time.smoothDeltaTime * 0.7f);
    }

    public override void removeAtVector()
    {
        GM.GameManager.getInstance().v_monster2.Remove(gameObject);
    }
}