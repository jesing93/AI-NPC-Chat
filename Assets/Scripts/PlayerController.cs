using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    private Rigidbody2D rig;
    private Vector2 moveInput;

    void Awake ()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    void Update ()
    {
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }

    void FixedUpdate ()
    {
        rig.velocity = moveInput * moveSpeed;
    }
}