﻿using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    public LayerMask collisionMask;
    float speed = 10f;
    float damage = 1f;
    float lifeTime = 1;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

	// Update is called once per frame
	void Update () {
        float moveDistance = speed * Time.deltaTime;
        CheckCollisions(moveDistance);
        transform.Translate(Vector3.forward * moveDistance);
	}

    void CheckCollisions(float moveDistance)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, moveDistance, collisionMask, QueryTriggerInteraction.Ignore))
        {
            OnHitObject(hit);
        }
    }

    void OnHitObject(RaycastHit hit)
    {
        IDamagable damageableObject = hit.collider.GetComponent<IDamagable>();
        if (damageableObject != null)
        {
            damageableObject.TakeHit(damage, hit);
        }
        Destroy(gameObject);
    }
}
