using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D rb;
    private PlayerAction inputs;
    private InputActionMap inputMap;
    private InputAction moveAction;
    private Vector2 moveInput;
    private Animator anim;

    private void Awake()
    {
        inputs = new PlayerAction();
        inputMap = inputs.Player;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
        moveAction = inputMap.FindAction("Move");
    }

    private void OnEnable()
    {
        moveAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
    }
    private void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();
        anim.SetFloat("moveInput", Mathf.Abs(moveInput.x + moveInput.y));
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput * speed;
        
    }
}
