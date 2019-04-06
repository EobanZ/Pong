using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {

    State _state = new defaultState();

    Quaternion startRotation;

    // Use this for initialization
    void Start () {
        startRotation = transform.localRotation;
        _state.enter(gameObject, this);
	}
	
	// Update is called once per frame
	void Update () {

        if (!GameManager.Instance.gameStarted || GameManager.Instance.GameOver)
            return;

        State state = _state.handleInput(gameObject);
        if(state != _state)
        {
            _state.exit();
            _state = state;
            _state.enter(gameObject, this);
        }
        _state.update(gameObject);

        //Standard Movement
        float movement = Input.GetAxis("Mouse Y");
        this.gameObject.transform.Translate(0, movement, 0, Space.World);
        if(transform.position.y > GameManager.Instance.MaxYMovement)
        {
            transform.position = new Vector3(transform.position.x, GameManager.Instance.MaxYMovement, transform.position.z);
        }
        if(transform.position.y < -GameManager.Instance.MaxYMovement)
        {
            transform.position = new Vector3(transform.position.x, -GameManager.Instance.MaxYMovement, transform.position.z);
        }

    }

}
