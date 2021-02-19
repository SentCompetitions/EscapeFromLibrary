using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
using TouchPhase = UnityEngine.TouchPhase;

public class Player : MonoBehaviour
{
    [FormerlySerializedAs("camera")] public Camera mainCamera;
    [Space]
    public GameObject ui;
    public GameObject uiInput;
    public Button uiButton;
    public GameObject interactIcon;

    private Rigidbody _rigidbody;

    private Vector3 _move;
    private PlayerInput _input;

    private Interactable _interactable;
    private GameObject _icon;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _input = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        _rigidbody.MovePosition(transform.position + _move);

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            _input.SwitchCurrentControlScheme("Touch");

        if (_input.currentControlScheme != "Touch") uiInput.SetActive(false);
        else uiInput.SetActive(true);
    }

    public void Move(InputAction.CallbackContext context)
    {
        Vector2 move = context.ReadValue<Vector2>();
        _move = new Vector3(move.y, 0f, move.x) * 0.1f;
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (!_interactable) return;
        if (!context.performed) return;

        _interactable.OnInteract();
        
        Debug.Log("Interact with " + _interactable.name);
        DestroyIcon();
        _interactable = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_interactable) return;
        if (!other.TryGetComponent(out _interactable)) return;
        
        uiButton.interactable = true;

        _icon = Instantiate(interactIcon, Vector3.zero, Quaternion.identity, ui.transform);
        _icon.GetComponent<InteractIcon>().targetCamera = mainCamera;
        _icon.GetComponent<InteractIcon>().target = other.transform;
    }

    private void DestroyIcon()
    {
        if (!_icon) return;
        _icon.GetComponent<Animator>().SetTrigger("Destroy");
        Destroy(_icon, 1f);
        _icon = null;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!_interactable) return;
        if (other.gameObject != _interactable.gameObject) return;
        
        _interactable = null;
        
        uiButton.interactable = false;

        DestroyIcon();
    }
}