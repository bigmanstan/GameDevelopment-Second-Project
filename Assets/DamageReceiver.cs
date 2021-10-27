using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReceiver : MonoBehaviour, IEntity
{
    //This script keep track of player HP
    public float playerHP = 100;
    public MouseController1 playerController;


    public void ApplyDamage(float points)
    {
        print("here");
        playerHP -= points;

        if (playerHP <= 0)
        {
            //Player is dead
            playerController.canMove = false;
            playerHP = 0;
        }
    }
}