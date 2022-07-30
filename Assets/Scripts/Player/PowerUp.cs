using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [Header("Power Up Configuration")]
    [SerializeField] private float movementSpeed = 0.0f;
    private IMovement movement;
    private Vector3 initialUp;

    [Header("Power Up Type")]
    [SerializeField] public bool guns = false;
    [SerializeField] public bool shield = false;
    [SerializeField] public bool speed = false;
   

    void Start()
    {
        movement = new NormalMove();
        initialUp = transform.up;
    }

    void Update()
    {
        transform.position += movement.Move(transform.position, initialUp, transform.up, movementSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameObject.SetActive(false);
    }
}
