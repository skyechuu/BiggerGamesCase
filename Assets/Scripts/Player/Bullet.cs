﻿using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] LayerMask collisionMask;

    readonly float speed = 10;

    float maxLifetime = 1;
    float currentLifetime = 0;
    int damage = 1;

    public void OnEnable()
    {
        currentLifetime = 0;
    }

    void Update()
    {
        ControlLifeTime();

        float moveDistance = speed * Time.deltaTime;
        transform.Translate(Vector3.up * moveDistance);
    }
    
    public void Despawn()
    {
        gameObject.SetActive(false);
    }

    void ControlLifeTime()
    {
        if (currentLifetime >= maxLifetime)
            Despawn();
        else
            currentLifetime += Time.deltaTime;
    }

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    public int GetDamage()
    {
        return damage;
    }
}
