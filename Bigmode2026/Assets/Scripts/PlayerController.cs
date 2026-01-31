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

    [SerializeField, Min(0.01f)] private float moveForce;
    [SerializeField, Min(0.01f)] private float maxVelocity;
    

    void Awake() {
        leftAction = CreateInputAction(Key.A, Key.LeftArrow);
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

    void Update() {
        // Get all input in Update, but don't modify physics in Update 
        var left = leftAction.ReadValue<float>();
        var right = rightAction.ReadValue<float>();
        xInput = right - left;
    }

    void FixedUpdate() {
        // Only modify physics in FixedUpdate

        rb.AddForce(Vector2.right * (xInput * moveForce * Time.fixedDeltaTime));

        if (rb.linearVelocityX > maxVelocity) {
            rb.linearVelocityX = maxVelocity;
        }
        else if (rb.linearVelocityX < -maxVelocity) {
            rb.linearVelocityX = -maxVelocity;
        }
    }
}
