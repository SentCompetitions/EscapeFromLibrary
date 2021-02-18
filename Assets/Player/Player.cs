using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TouchPhase = UnityEngine.TouchPhase;

public class Player : MonoBehaviour
{
    public GameObject mobileInput;
    
    private Rigidbody _rigidbody;

    private Vector3 _move;
    private PlayerInput _input;
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _input = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        _rigidbody.MovePosition(transform.position + _move);;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            _input.SwitchCurrentControlScheme("Touch");
        
        if (_input.currentControlScheme != "Touch") mobileInput.SetActive(false);
        else mobileInput.SetActive(true);
    }

    public void Move(InputAction.CallbackContext context)
    {
        Vector2 move = context.ReadValue<Vector2>();
        _move = new Vector3(move.y, 0f, move.x) * 0.1f;
    }

    public void Interact(InputAction.CallbackContext context)
    {
        
    } 
}
