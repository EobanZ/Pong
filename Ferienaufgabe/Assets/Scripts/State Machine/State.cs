using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface State
{
    State handleInput(GameObject go);
    void update(GameObject go);
    void enter(GameObject go , MonoBehaviour mono);
    void exit();
}

public class defaultState : State 
{
    Transform transform;
    Quaternion startRotation = Quaternion.Euler(0, 0, 90);

    IEnumerator resetRotationAfterTime()
    {
        yield return new WaitForSeconds(2.0f);
        RotateToDefault();
    }

    void RotateToDefault()
    {
        transform.localRotation = startRotation;
        //transform.localRotation = Quaternion.Lerp(transform.localRotation, startRotation, Time.deltaTime * 50f);
    }

    public void enter(GameObject go, MonoBehaviour mono)
    {
        transform = go.transform;
        mono.StartCoroutine(resetRotationAfterTime());
        
       //Go back to default position after a delay and lerp
    }

    public void exit()
    {
        //nothing
    }

    public void update(GameObject go)
    {
        //nothing
    }

    public State handleInput(GameObject go)
    {
        if(Input.GetMouseButtonDown(0))
        {
            return new chargingLeftState();
        }
        if (Input.GetMouseButtonDown(1))
        {
            return new chargingRightState();
        }
        else
            return this;


    }

  
}

public class chargingLeftState : State
{
    static Quaternion startRotation;
    public void enter(GameObject go, MonoBehaviour mono)
    {
        
    }

    public void exit()
    {
        //Apply rotation force to rigid body
    }

    public State handleInput(GameObject go)
    {
        if(Input.GetMouseButtonUp(0))
        {
            exit();
            return new defaultState();
        }
        else
        {
            return this;
        }
    }

    public void update(GameObject go)
    {
        Transform transform = go.transform;
        startRotation = transform.rotation;
        transform.localRotation = Quaternion.Lerp(startRotation, Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, 120), 1);
    }
}

public class chargingRightState : State
{
    public void enter(GameObject go, MonoBehaviour mono)
    {
        
    }

    public void exit()
    {
        
    }

    public State handleInput(GameObject go)
    {
        return new defaultState();
    }

    public void update(GameObject go)
    {
        
    }
}
