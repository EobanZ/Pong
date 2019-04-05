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
    Rigidbody rb;

    IEnumerator resetRotationAfterTime()
    {
        yield return new WaitForSeconds(2.0f);
        RotateToDefault();
    }

    void RotateToDefault()
    {
        transform.localRotation = Quaternion.Euler(0, 0, 90);
        rb.angularDrag = 10000;
        rb.angularVelocity = Vector3.zero;
       
    }

    public void enter(GameObject go, MonoBehaviour mono)
    {
        transform = go.transform;
        rb = go.GetComponent<Rigidbody>();
        mono.StartCoroutine(resetRotationAfterTime());
        GameManager.Instance.PlayerChargeMeter.localScale = new Vector3(0, 1, 1);

    }

    public void exit()
    {
        rb.angularDrag = 0;
    }

    public void update(GameObject go)
    {
        
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
    Rigidbody rb;
    GameObject go;
    Transform transform;
    float charge = 0;
    public void enter(GameObject go, MonoBehaviour mono)
    {
        mono.StopAllCoroutines();
        rb = go.GetComponent<Rigidbody>();
        rb.angularDrag = 0;
        this.go = go;
        transform = go.transform;
      
    }

    public void exit()
    {
        rb.angularVelocity = Vector3.zero;
        rb.AddTorque(-go.transform.forward * charge * 200, ForceMode.Acceleration);
    }

    public State handleInput(GameObject go)
    {
        if(Input.GetMouseButtonUp(0))
        {
            return new defaultState();
        }
        else
        {
            return this;
        }
    }

    public void update(GameObject go)
    {
        charge += Time.deltaTime;
        charge = Mathf.Clamp(charge, 0, GameManager.Instance.MaxChargePower);
        transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, 120);

        float percent = charge*200 / GameManager.Instance.MaxChargePower;
        percent = Mathf.Clamp01(percent);
        GameManager.Instance.PlayerChargeMeter.localScale = new Vector3(percent, 1, 1);


    }
}

public class chargingRightState : State
{
    Rigidbody rb;
    GameObject go;
    Transform transform;
    float charge = 0;

    public void enter(GameObject go, MonoBehaviour mono)
    {
        mono.StopAllCoroutines();
        rb = go.GetComponent<Rigidbody>();
        rb.drag = 0;
        this.go = go;
        transform = go.transform;
        
    }

    public void exit()
    {
        rb.angularVelocity = Vector3.zero;
        rb.AddTorque(go.transform.forward * charge * 200, ForceMode.Acceleration);
    }

    public State handleInput(GameObject go)
    {
        if (Input.GetMouseButtonUp(1))
        {
            return new defaultState();
        }
        else
        {
            return this;
        }
    }

    public void update(GameObject go)
    {
        charge += Time.deltaTime;
        Mathf.Clamp(charge, 0, GameManager.Instance.MaxChargePower);
        transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, 60);

        float percent = charge * 200 / GameManager.Instance.MaxChargePower;
        percent = Mathf.Clamp01(percent);
        GameManager.Instance.PlayerChargeMeter.localScale = new Vector3(percent, 1, 1);
    }
}
