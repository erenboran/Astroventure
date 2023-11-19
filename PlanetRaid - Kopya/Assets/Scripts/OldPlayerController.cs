using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OldPlayerController : MonoBehaviour
{
    public static OldPlayerController Instance;

    private float moveSpeed = 5f;
    [SerializeField] private float startMoveSpeed = 5f;

    private Vector2 movement;

    private Rigidbody rb;

    public Camera cam;

    private Vector3 mousePos;

    public float turnSpeed = 10f;

    public Quaternion characterSpineRotation;

    Vector3 direction;
    [SerializeField]
    Transform[] hitWeapons;

    Transform _weapon;

    public Transform collectPoint;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }

        rb = GetComponent<Rigidbody>();
    }

    private void OnMove(InputValue value)
    {
        movement = value.Get<Vector2>();

        direction = new Vector3(movement.x, 0f, movement.y);

        if (movement.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }

    }

    public void ChangeMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }



    public void OnAttackStart()
    {
        moveSpeed = 0f;
    }

    public void OnAttackRelease()
    {
        moveSpeed = startMoveSpeed;
    }



    private void FixedUpdate()
    {

        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);


    }



    public void PerformPunch(Enums.Weapons hitWeapon)
    {
        _weapon = hitWeapons[(int)hitWeapon];

        Collider[] hitDamagables = Physics.OverlapSphere(_weapon.position, 0.5f);

        foreach (Collider hit in hitDamagables)
        {
            if (hit.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(2);
            }

        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out ICollectable collectable))
        {
            collectable.Collect(collectPoint);
        }
    }

    // private void OnCollisionEnter(Collision other)
    // {
    //     if (other.gameObject.TryGetComponent(out ICollectable collectable))
    //     {
    //         collectable.Collect();
    //     }
    // }



}
