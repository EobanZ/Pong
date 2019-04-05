using UnityEngine;
using System.Collections;

//Random behaviours einbauen. CurvedBall, Staight un kp
public class Ball : MonoBehaviour {

    Vector3 startingPosition = Vector3.zero;
    Vector3 startingSpeed = new Vector3(8f,0f,0f);
    Rigidbody rb;
    float maxSpeed;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        maxSpeed = GameManager.Instance.MaxBallSpeed;
        reset();
    }

    public void reset() {
        rb.velocity = Vector3.zero;
        this.gameObject.transform.position = startingPosition;
        Invoke("resetStartingSpeed", 1);
    }

    void resetStartingSpeed() {
        rb.velocity = startingSpeed;
    }


    void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("Player"))
        {
            //if we collide with a player, we become faster
            rb.velocity *= 1.1f;
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);

            //we also gain a little speed away from his center
            float addSpeed = (this.gameObject.transform.position.x - collision.gameObject.transform.position.x);
            rb.velocity += new Vector3(addSpeed, 0, 0);


            if (transform.position.x > 0)
                GameManager.Instance.LastContact = ePlayerType.ki;
            else
                GameManager.Instance.LastContact = ePlayerType.player;         
        }
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Border"))
        {
            if (other.gameObject.transform.position.x < 0.0f)
                GameManager.Instance.OnBorderCollision(ePlayerType.player);
            else
                GameManager.Instance.OnBorderCollision(ePlayerType.ki);
        }
    }
}
