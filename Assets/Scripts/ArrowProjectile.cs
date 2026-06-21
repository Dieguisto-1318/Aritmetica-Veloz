using UnityEngine;

/// <summary>
/// Representa la flecha que dispara el arco.
/// Maneja física, colisiones y validación de respuestas.
/// </summary>
public class ArrowProjectile : MonoBehaviour
{
    [Header("Physics")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float drag = 0.01f;

    [Header("Detection")]
    [SerializeField] private CircleCollider2D circleCollider;
    [SerializeField] private float destroyDelay = 3f;

    private GameManager gameManager;
    private Vector2 velocity;
    private bool isLaunched = false;
    private float timeInAir = 0f;

    private void Start()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        if (circleCollider == null)
            circleCollider = GetComponent<CircleCollider2D>();
    }

    private void FixedUpdate()
    {
        if (!isLaunched)
            return;

        // Aplicar gravedad manualmente
        velocity.y -= gravity * Time.fixedDeltaTime;
        
        // Aplicar drag (resistencia del aire)
        velocity *= (1f - drag);

        // Actualizar posición
        rb.velocity = velocity;

        timeInAir += Time.fixedDeltaTime;

        // Rotar la flecha según la dirección
        if (velocity.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        // Destruir si pasa mucho tiempo en el aire
        if (timeInAir > destroyDelay)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Lanza la flecha con dirección y fuerza especificada.
    /// </summary>
    public void Launch(Vector2 direction, float force)
    {
        isLaunched = true;
        velocity = direction.normalized * force;
        rb.velocity = velocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isLaunched)
            return;

        TargetObject target = collision.GetComponent<TargetObject>();
        if (target != null)
        {
            HandleTargetHit(target);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isLaunched)
            return;

        TargetObject target = collision.gameObject.GetComponent<TargetObject>();
        if (target != null)
        {
            HandleTargetHit(target);
        }
    }

    /// <summary>
    /// Procesa el impacto con un objetivo.
    /// </summary>
    private void HandleTargetHit(TargetObject target)
    {
        if (gameManager != null)
        {
            gameManager.OnTargetHit(target.GetValue());
        }

        // Destruir la flecha al impactar
        Destroy(gameObject);
    }

    public void SetGameManager(GameManager manager)
    {
        gameManager = manager;
    }

    public bool IsLaunched()
    {
        return isLaunched;
    }
}
