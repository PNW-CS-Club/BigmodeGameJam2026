using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private InputAction leftAction;
    private InputAction rightAction;
    private InputAction upAction;
    private InputAction downAction;
    private InputAction spaceAction;

    /** -1 for left, 0 for none, 1 for right */
    private float xInput;

    private Rigidbody2D rb;
    
    
    private static readonly int End = Animator.StringToHash("End");
    private static readonly int Start = Animator.StringToHash("Start");
    [SerializeField] Animator playerAnimator;

    [SerializeField] private float startYPos;
    [SerializeField, Min(0.01f)] private float moveForce;
    [SerializeField, Min(0.01f)] private float maxSpeed;
    [SerializeField, Min(1f)] private float decelerationBonus;
    
    private bool canControl = true;
    

    void Awake() {
        leftAction = CreateInputAction(Key.A, Key.LeftArrow); // accepts any number of key arguments
        rightAction = CreateInputAction(Key.D, Key.RightArrow);
        upAction = CreateInputAction(Key.W, Key.UpArrow);
        downAction = CreateInputAction(Key.S, Key.DownArrow);
        spaceAction = CreateInputAction(Key.Space);
        
        rb = GetComponent<Rigidbody2D>();
    }
    
    /**
     * Creates a new "button press" input action
     * that returns 1.0f when any of the given keys are pressed and 0.0f otherwise.
     */
    private static InputAction CreateInputAction(params Key[] keys) {
        InputAction action = new();
        foreach (Key key in keys) {
            action.AddBinding(Keyboard.current[key].path);
        }
        action.Enable();
        return action;
    }

    public void ResetPlayer() {
        transform.position = new Vector3(0f, startYPos, 0f);
        canControl = true;
        rb.linearVelocity = Vector2.zero;
        playerAnimator.ResetTrigger(End);
        playerAnimator.SetTrigger(Start);
    }

    void Update() {
        // Get all input in Update, but don't modify physics in Update 
        var left = leftAction.ReadValue<float>();
        var right = rightAction.ReadValue<float>();
        xInput = right - left;
    }

    void FixedUpdate() {
        // Only modify physics in FixedUpdate
        
        if (!canControl) return;

        var force = xInput * moveForce * Time.fixedDeltaTime;
        if (rb.linearVelocityX * xInput < 0) {
            // the player should decelerate faster than they accelerate
            force *= decelerationBonus;
        }

        rb.AddForce(Vector2.right * force);

        // clamp speed to be at most maxSpeed
        if (rb.linearVelocityX > maxSpeed) {
            rb.linearVelocityX = maxSpeed;
        }
        else if (rb.linearVelocityX < -maxSpeed) {
            rb.linearVelocityX = -maxSpeed;
        }

        rb.linearVelocityY = 0f;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (!collision.gameObject.CompareTag("Obstacle")) return;
        SoundManager.Instance.PlaySound3D("Hurt", transform.position);
        canControl = false;
        
        playerAnimator.ResetTrigger(Start);
        playerAnimator.SetTrigger(End);
        
        SendMessageUpwards("EndRun");
    }
}
