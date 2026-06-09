using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Движение")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 360f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private int maxJumps = 1;

    [Header("Зависимости")]
    [SerializeField] private Rigidbody rb;

    private Vector3 moveDirection;
    private float targetAngle;
    private bool jumpRequested;
    private int jumpsRemaining;

    private void Reset()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
            rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void Start()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        targetAngle = transform.eulerAngles.y;
        jumpsRemaining = maxJumps;
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

        // Поворот
        if (Mathf.Abs(horizontal) > 0.1f)
        {
            targetAngle = horizontal > 0 ? 90f : -90f;
        }
        else if (Mathf.Abs(vertical) > 0.1f)
        {
            targetAngle = vertical > 0 ? 0f : 180f;
        }

        // Прыжок — только если есть оставшиеся прыжки
        if (Input.GetButtonDown("Jump") && jumpsRemaining > 0)
        {
            jumpRequested = true;
        }
    }

    private void FixedUpdate()
    {
        // Поворот
        Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
        rb.rotation = Quaternion.RotateTowards(rb.rotation, 
            targetRotation, rotationSpeed * Time.fixedDeltaTime);

        // Движение
        Vector3 velocity = moveDirection * moveSpeed;
        velocity.y = rb.linearVelocity.y;
        rb.linearVelocity = velocity;

        // Прыжок
        if (jumpRequested)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            jumpsRemaining--;
            jumpRequested = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Восстанавливаем прыжки при касании земли
        if (collision.contacts[0].normal.y > 0.5f)
        {
            jumpsRemaining = maxJumps;
        }
    }
}