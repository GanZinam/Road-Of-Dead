﻿using UnityEngine;
using System.Collections;

public class Monster1 : Monster
{
    public override void init()
    {
        hp = 100;
        speed = 0.5f;
    }

    public override void attack()
    {
        GM.GameManager.getInstance().hpBar.value -= 0.01f;
    }

    public override void move()
    {
        gameObject.transform.Translate(Vector2.right * Time.smoothDeltaTime * speed);
    }

    public override void removeAtVector()
    {
        GM.GameManager.getInstance().v_monster1.Remove(gameObject);
    }
}