using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KI : MonoBehaviour
{
    Transform ball;
    Vector3 defaultPos;
    float speed;
    Rigidbody rb;


    void Start()
    {
        ball = GameManager.Instance.Ball.transform;
        defaultPos = transform.position;
        speed = GameManager.Instance.KiSpeed;
        rb = ball.gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!ball)
            return;
        Move();
    }

    void Move()
    {
        if(rb.velocity.x >= 0)
        {
            if (ball.position.y != transform.position.y && (Mathf.Abs(ball.position.y - transform.position.y) > 0.2))
            {
                if (ball.position.y > transform.position.y)
                {
                    transform.Translate(new Vector3(0, 0.001f * speed, 0), Space.World);
                }
                else
                {
                    transform.Translate(new Vector3(0, 0.001f * speed, 0) * -1, Space.World);
                }
            }
        }
        else if(rb.velocity.x <0)
        {

            if(defaultPos.y > transform.position.y)
            {
                transform.Translate(new Vector3(0, 0.001f * speed * 0.5f, 0), Space.World);
            }
            else
            {
                transform.Translate(new Vector3(0, 0.001f * speed * 0.5f, 0) * -1, Space.World);
            }

            
        }

        
    }
}
