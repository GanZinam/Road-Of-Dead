using UnityEngine;
using System.Collections;

public class Monster3 : Monster {

    public override void attack()
    {
        GM.GameManager.getInstance().hpBar.value -= 0.01f;
    }

    public override void move()
    {
        gameObject.transform.Translate(Vector2.right * Time.smoothDeltaTime * 0.5f);
    }

    public override void removeAtVector()
    {
        GM.GameManager.getInstance().v_monster3.Remove(gameObject);
    }
}
