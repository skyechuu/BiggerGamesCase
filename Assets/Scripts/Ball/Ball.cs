﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour
{
    [SerializeField] float bounceHeight = 8f;
    [SerializeField] float horizontalSpeed = 1f;
    [SerializeField] int hitPoint = 1;
    float gravity = -9.8f;
    bool isLeft = false;

    bool isBallReady = false;
    bool isParent = false;
    float velocityY;
    float velocityX;

    void Awake()
    {
        GetComponent<Rigidbody>().isKinematic = true;
    }

    void Start()
    {
        gravity = GameController.GetGameData().gravity;
    }
    
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
            Bounce();
#endif

        CheckBallReadiness();

        Move();
    }

    void OnEnable()
    {
        ResetBall();
        CheckBallReadiness();
    }

    void OnTriggerEnter(Collider collider)
    {
        switch (collider.gameObject.tag)
        {
            case "Ground":
                Bounce();
                break;
            case "Wall_R":
                SidewayBounce(true);
                break;
            case "Wall_L":
                SidewayBounce(false);
                break;
            case "Bullet":
                OnHitByBullet(collider);
                break;
        }
    }

    void Move()
    {
        // Calcualte horizontal velocity
        velocityX = horizontalSpeed * ((isLeft) ? -1 : 1);

        if (isBallReady)
        {
            // Add gravity to vertical velocity
            velocityY += gravity * Time.deltaTime;
        }


        // Move the ball
        transform.position += (Vector3.up * velocityY + transform.right * velocityX) * Time.deltaTime;
    }

    /// <summary>
    /// Bounces ball by calculating kinematic equation with gravity and ball's bounceHeight and calls corresponding callback function at the end.
    /// </summary>
    void Bounce()
    {
        Bounce(bounceHeight);
    }

    /// <summary>
    /// Bounces ball by calculating kinematic equation with gravity and desired bounceHeight and calls corresponding callback function at the end.
    /// </summary>
    public void Bounce(float bounceHeight)
    {
        if (!isBallReady)
            return;
        
        float targetBounceHeight = bounceHeight;
        float currentHeight = transform.position.y;

        // Prevent throwing ball out of game area with bounce
        if (currentHeight + targetBounceHeight > GameConstants.MAX_GAME_AREA_HEIGHT)
            targetBounceHeight = GameConstants.MAX_GAME_AREA_HEIGHT - currentHeight;

        float bounceVel = Mathf.Sqrt(-2 * gravity * targetBounceHeight);
        velocityY = bounceVel;

        OnBounce();
    }

    /// <summary>
    /// Changes direciton of ball's horizontal velocity and calls corresponding callback function at the end.
    /// </summary>
    /// <param name="newIsLeft">New state of isLeft variable as boolean.</param>
    void SidewayBounce(bool newIsLeft)
    {
        isLeft = newIsLeft;

        OnSidewayBounce();
    }

    void OnBounce()
    {
        // callback for sound, gfx, etc...
        //Debug.Log("Boink!");
    }

    void OnSidewayBounce()
    {
        // callback for sound, gfx, etc...
        //Debug.Log("Boink!");
    }

    void OnHitByBullet(Collider collider)
    {
        Bullet bullet = collider.GetComponent<Bullet>();
        bullet.Despawn();

        hitPoint -= bullet.GetDamage();

        if(hitPoint <= 0)
        {
            transform.gameObject.SetActive(false);

            if (isParent)
            {
                Split();
            }
        }
    }

    void Split()
    {
        //var ChildL = BallSpawner.instance.SpawnBall(this, 5, true, 3, bounceHeight);
        //ChildL.Bounce(GameConstants.SPLIT_BOUNCE_HEIGHT);
        //var ChildR = BallSpawner.instance.SpawnBall(this, 5, false, 3, bounceHeight);
        //ChildR.Bounce(GameConstants.SPLIT_BOUNCE_HEIGHT);
        //isParent = false;
    }

    /// <summary>
    /// Check if the ball ready to drop and bounce to platform after it spawned. Prevents hitting the side walls.
    /// </summary>
    void CheckBallReadiness()
    {
        if (isBallReady)
            return;

        float platformLeft = -GameConstants.MAX_HORIZONTAL_DIMENSION_FOR_BALL;
        float platformRight = GameConstants.MAX_HORIZONTAL_DIMENSION_FOR_BALL;
        Vector3 pos = transform.position;
        Vector3 size = transform.localScale;

        isBallReady = (pos.x >= platformLeft + size.x) && (pos.x <= platformRight - size.x);
    }

    private void ResetBall()
    {
        isBallReady = false;
        velocityX = 0;
        velocityY = 0;
    }

    /// <summary>
    /// Sets direction of horizontal velocity.
    /// </summary>
    /// <param name="direction">Set 'true' for Left, 'false' for Right.</param>
    public void SetIsLeft(bool direction)
    {
        isLeft = direction;
    }

    public void SetBounceHeight(float bounceHeight)
    {
        this.bounceHeight = bounceHeight;
    }

    public void SetHorizontalSpeed(float horizontalSpeed)
    {
        this.horizontalSpeed = horizontalSpeed;
    }

    public void SetHitPoint(int hitPoint)
    {
        this.hitPoint = hitPoint;
    }

    public void SetIsParent(bool isParent)
    {
        this.isParent = isParent;
    }

}