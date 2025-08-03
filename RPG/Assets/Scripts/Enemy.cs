using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public Enemy_IdleState idleState;
    public Enemy_MoveState moveState;
    [Header("Movement Detail")]
    public float idleTime = 2;
    public float moveSpeed = 1.4f;//�ƶ��ٶ�
    [Range(0,2)]
    public float moveAnimSpeedMultiplier = 1;
}
