using UnityEngine;
using System.Collections;

//Random behaviours einbauen. CurvedBall, Staight un kp
public class Ball : MonoBehaviour {

    Vector3 startingPosition = Vector3.zero;
    Vector3 startingSpeed;
    Rigidbody rb;
    float maxSpeed;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        maxSpeed = GameManager.Instance.MaxBallSpeed;
        startingSpeed = new Vector3(GameManager.Instance.BallStartSpeed, 0 , 0f);
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

    private void Update()
    {
        if (Mathf.Abs(rb.velocity.x) < 7.0f)
        {
            if(rb.velocity.x <= 0)
            {
                rb.AddForce(new Vector3(2, 0, 0), ForceMode.Impulse);
            }
            else if(rb.velocity.x >0)
            {
                rb.AddForce(new Vector3(2, 0, 0), ForceMode.Impulse);
            }
        }


    }


    void OnCollisionEnter(Collision collision)
    {
        bool isPlayer;
        if (isPlayer = collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Obsticle"))
        {
            //if we collide with a player, we become faster
            rb.velocity *= 1.1f;
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);

            //we also gain a little speed away from his center
            float addSpeed = (this.gameObject.transform.position.x - collision.gameObject.transform.position.x);
            rb.velocity += new Vector3(addSpeed, 0, 0);

            if (!isPlayer)
                return;

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
