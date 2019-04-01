using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {

    float MAX_POWER = 1;

    float powerLeft = 0;
    float powerRight = 0;

    float timer = 0;
    float powerupRate = 0.05f;


    Quaternion startRotation;

    // Use this for initialization
    void Start () {
        startRotation = transform.localRotation;
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        float movement = Input.GetAxis("Mouse Y");
        this.gameObject.transform.Translate(0, movement, 0, Space.World);
        if(transform.position.y > GameManager.Instance.MAX_Y_MOVEMENT)
        {
            transform.position = new Vector3(transform.position.x, GameManager.Instance.MAX_Y_MOVEMENT, transform.position.z);
        }
        if(transform.position.y < -GameManager.Instance.MAX_Y_MOVEMENT)
        {
            transform.position = new Vector3(transform.position.x, -GameManager.Instance.MAX_Y_MOVEMENT, transform.position.z);
        }

     
        if(Input.GetMouseButton(0) && timer >= powerupRate)
        {
            powerRight = 0;
            powerLeft += 0.1f;
            powerLeft = powerLeft >= 1 ? 1 : powerLeft;
            timer = 0;
            transform.localRotation = Quaternion.Lerp(startRotation, Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, 120), powerLeft);
            //transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, (powerLeft * 30) + 90);

        }
        else if(Input.GetMouseButton(1) && timer >= powerupRate)
        {
            powerLeft = 0;
            powerRight += 0.1f;
            powerRight = powerRight >= 1 ? 1 : powerRight;
            timer = 0;
            transform.localRotation = Quaternion.Lerp(startRotation, Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, 60), powerRight);
            //transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, (powerRight * -30) + 90);
        }
        
        if(!Input.GetMouseButton(0) && !Input.GetMouseButton(1))
        {
            powerLeft = 0;
            powerRight = 0;
            transform.localRotation = Quaternion.Lerp(transform.localRotation, startRotation, Time.deltaTime * 50f);
        }
            
         
        


           
           
            
            
       
        
        
      


    }

}
