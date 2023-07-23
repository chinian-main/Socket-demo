using System;
using System.Collections;
using System.Collections.Generic;
using SocketGameProtocol;
using UnityEngine;

public class GunController : MonoBehaviour
{
    private SpriteRenderer gunRenderer;
    private GameObject bullet;
    private Transform firePos;
  
    private FireRequest fireRequest;
    private void Awake()
    {
        if (fireRequest == null)
        {
            fireRequest = transform.gameObject.AddComponent<FireRequest>();
        }
        firePos = transform.Find("Fire").transform;
        gunRenderer = GetComponent<SpriteRenderer>();
        bullet = (GameObject)Resources.Load("Prefab/Bullet");
    }
    private void Start()
    {
        // fireRequest = transform.GetComponent<FireRequest>();

        if (bullet == null) Debug.Log("bullet空");
    }
    private void Update()
    {
        LookAt2D();
        Fire();
    }
    void LookAt2D()
    {
        Vector3 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        if (transform.eulerAngles.z >= 90 && transform.eulerAngles.z <= 270)
        {
            gunRenderer.flipY = true;
        }
        else
        {
            gunRenderer.flipY = false;
        }
    }
    void Fire()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 m_mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            m_mousePosition.z = 0;
            float m_fireAngle = Vector3.Angle(m_mousePosition - transform.position, Vector3.up);
            if (m_mousePosition.x > transform.position.x)
            {
                m_fireAngle = -m_fireAngle;
            }
            // GameObject g=Instantiate(bullet,firePos.position,Quaternion.identity);
            GameObject g = GameFace.Face.BulletPool.Pop(firePos.position, Quaternion.identity);
            g.GetComponent<Bullet>().onRecycleEvent += GameFace.Face.BulletPool.Push;
            g.GetComponent<Bullet>().Init();
            g.transform.eulerAngles = new Vector3(0, 0, m_fireAngle + 90);
            Vector2 velocity = (m_mousePosition - firePos.position).normalized * 20;
            g.GetComponent<Rigidbody2D>().velocity = velocity;
            // bullet 

            fireRequest.SendRequest(firePos.position, m_fireAngle + 90, m_mousePosition);
        }

    }


}
