using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Rigidbody2D rb;

    [SerializeField] bool isGrounded;

    [SerializeField] float jumpForce = 10f;

    [SerializeField] Transform startLocation;

    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float turnSpeed = 10f;

    [SerializeField] float thrustForce = 5f;
    [SerializeField] float rotationSpeed = 2f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //float horizontalInput = Input.GetAxis("Horizontal");
        //transform.Rotate(Vector3.forward * -horizontalInput * rotationSpeed);
        //transform.Rotate(Vector3.forward * rb.velocity * rotationSpeed);

        Vector2 velocity = rb.velocity;
        if (velocity != Vector2.zero)
        {
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        //Movement();
        //Movement2();
        Movement3();
    }

    void PlayerInput()
    {

    }

    //Rocket Move Style 1
    void Movement()
    {
        rb.velocity = new Vector2(moveSpeed, Input.GetAxisRaw("Vertical") * turnSpeed) * Time.deltaTime * 100;
    }

    //Rocket Move Style 2

    void Movement2()
    {
        Vector2 movement = new Vector2(moveSpeed, rb.velocity.y);
        rb.velocity = movement;

        if (Input.GetKey(KeyCode.Space))
        {
            Debug.Log("Jump");
            if (rb.velocity.y <= 10)
            {
                rb.AddForce(transform.up * thrustForce);
            }
        }
    }

    void Movement3()
    {
        isGrounded = Physics2D.OverlapCircle(transform.position, 0.5f, LayerMask.GetMask("Ground"));

        //float horizontalInput = Input.GetAxis("Horizontal");
        //Vector2 movement = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        //rb.velocity = movement;

        Vector2 movement = new Vector2(moveSpeed, rb.velocity.y);
        rb.velocity = movement;

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            Debug.Log("Hit wall");
            transform.position = startLocation.position;
            rb.velocity = Vector2.zero;
        }

        if(collision.tag == "EndPoint")
        {
            collision.GetComponentInParent<EndPoint>().NextLevel();
        }
    }


}
