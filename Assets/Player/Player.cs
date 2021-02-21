using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.Serialization;
using UnityEngine.UI;
using TouchPhase = UnityEngine.TouchPhase;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    public Camera mainCamera;
    [Space]
    public GameObject ui;
    public GameObject uiInput;
    public Button uiButton;
    public GameObject uiQuiz;
    public GameObject interactionIconsParent;
    public GameObject interactIcon;
    public Image panel;
    [Header("Character")] 
    public Transform character;
    public Animator animator;
    [Header("Audio")] 
    public AudioClip footstep;
    [Header("inputSystem")]
    public InputSystemUIInputModule mainAction;
    public InputSystemUIInputModule uiAction;

    private Rigidbody _rigidbody;
    private AudioSource _audioSource;

    private Vector3 _move;
    [HideInInspector] public PlayerInput input;

    private Interactable _interactable;
    private GameObject _icon;

    [HideInInspector] public QuizLock quizLock;
    
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
        input = GetComponent<PlayerInput>();

        _audioSource.clip = footstep;
    }

    // Update is called once per frame
    void Update()
    {
        _rigidbody.MovePosition(transform.position + _move);

        if (_move != Vector3.zero)
        {
            animator.SetBool(IsWalking, true);
            character.rotation = Quaternion.LookRotation(_move);
            if (!_audioSource.isPlaying) _audioSource.Play();
        }
        else
        {
            animator.SetBool(IsWalking, false);
            _audioSource.Stop();
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            input.SwitchCurrentControlScheme("Touch");

#if !UNITY_EDITOR
        if (input.currentControlScheme != "Touch") uiInput.SetActive(false);
        else uiInput.SetActive(true);
#endif
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

        _interactable.OnInteract(this);
        
        Debug.Log("[Interactable] Interact with " + _interactable.name);
        UnInteract();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_interactable) return;
        ForceInteract(other.gameObject);
    }

    public void ForceInteract(GameObject other)
    {
        if (!other.TryGetComponent(out _interactable)) return;

        uiButton.interactable = true;

        _icon = Instantiate(interactIcon, Vector3.zero, Quaternion.identity, interactionIconsParent.transform);
        _icon.transform.SetSiblingIndex(0);
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

    private void UnInteract()
    {
        _interactable = null;
        
        uiButton.interactable = false;

        DestroyIcon();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!_interactable) return;
        if (other.gameObject != _interactable.gameObject) return;

        UnInteract();
    }

    public void OnUiClose(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        OnUiClose();
    }

    public void OnUiClose()
    {
        Debug.Log("a");
        if (quizLock) quizLock.Close(this);
    }
}