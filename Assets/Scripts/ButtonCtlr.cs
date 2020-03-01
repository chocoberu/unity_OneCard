using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCtlr : MonoBehaviour
{
    public PlayerCtrl player;
    public EnemyCtrl enemy;
    private BoxCollider2D box;
    // Start is called before the first frame update
    void Start()
    {
        player = transform.parent.gameObject.GetComponent<PlayerCtrl>();
        box = GetComponent<BoxCollider2D>();
    }
    private void OnMouseDown()
    {
        if(this.gameObject.name.Equals("LeftButton"))
        {
            Debug.Log("LeftButton");
            if (player)
            {
                int temp = player.GetCurrentListOffset();
                if (temp == 0)
                    return;
                else
                {
                    player.leftDown = true;
                    player.SetCurrentListOffset(temp - 1);
                }
            }
            if(enemy)
            {
                int temp = enemy.GetCurrentListOffset();
                if (temp == 0)
                    return;
                else
                {
                    enemy.leftDown = true;
                    enemy.SetCurrentListOffset(temp - 1);
                }
            }
        }
        if(this.gameObject.name.Equals("RightButton"))
        {
            Debug.Log("RightButton");
            if (player)
            {
                int temp = player.GetCurrentListOffset();
                int max = player.GetMaxListOffset();
                if (temp == max)
                    return;
                else
                {
                    player.rightDown = true;
                    player.SetCurrentListOffset(temp + 1);
                }
            }
            if (enemy)
            {
                int temp = enemy.GetCurrentListOffset();
                int max = enemy.GetMaxListOffset();
                if (temp == max)
                    return;
                else
                {
                    enemy.rightDown = true;
                    enemy.SetCurrentListOffset(temp + 1);
                }
            }
        }
    }
}
