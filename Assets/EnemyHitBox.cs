using MikeNspired.XRIStarterKit;
using UnityEngine;

public class EnemyHitBox : MonoBehaviour,IDamageable
{
    private Health health;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }  
    public void TakeDamage(float damage, GameObject damager)
    {
        health.TakeDamage(damage);
    }
}
