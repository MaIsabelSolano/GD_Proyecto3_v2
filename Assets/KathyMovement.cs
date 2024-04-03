using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KathyMovement : MonoBehaviour
{
    // Start is called before the first frame update
    

    private Rigidbody2D rigidbody2D_;
    [SerializeField] float speed_ = 5f; // Movement speed
    Vector2 movement;
    void Start()
    {
        rigidbody2D_ = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        // Get input from the player
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

    }

    private void FixedUpdate() {
        rigidbody2D_.MovePosition(rigidbody2D_.position + movement * speed_ * Time.deltaTime);
    }
}
