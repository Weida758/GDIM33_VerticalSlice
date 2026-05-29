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
    private int facingDir = 1;

    public float Speed
    {
        get { return speed; }
    }

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
        if (moveInput.x < 0 &&  facingDir == 1)
        {
            Flip();
            Debug.Log("Flipped");
        }
        else if (moveInput.x > 0 && facingDir == -1)
        {
            Flip();
            Debug.Log("Flipped");
        }
    }

    private void Flip()
    {
        facingDir *= -1;
        transform.localScale = new Vector3(facingDir, transform.localScale.y, transform.localScale.z);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput * speed;
        
    }

    public void AddMoveSpeed(float amount)
    {
        if (amount <= 0f) return;
        speed += amount;
    }
}
