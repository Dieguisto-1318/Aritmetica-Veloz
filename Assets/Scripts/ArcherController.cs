using UnityEngine;

/// <summary>
/// Controla el arco y la mecánica de apuntamiento y disparo.
/// Basado en la mecánica de Angry Birds: drag para apuntar, suelta para disparar.
/// </summary>
public class ArcherController : MonoBehaviour
{
    [Header("Arrow Settings")]
    [SerializeField] private ArrowProjectile arrowPrefab;
    [SerializeField] private Transform arrowSpawnPoint;
    [SerializeField] private float minForce = 10f;
    [SerializeField] private float maxForce = 50f;

    [Header("Visual Feedback")]
    [SerializeField] private LineRenderer trajectoryLine;
    [SerializeField] private int trajectoryPoints = 50;

    private Vector2 dragStartPosition;
    private bool isDragging = false;
    private ArrowProjectile currentArrow;

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        // Detectar inicio del drag
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            dragStartPosition = Input.mousePosition;
            CreateArrow();
        }

        // Durante el drag
        if (Input.GetMouseButton(0) && isDragging)
        {
            UpdateAiming();
        }

        // Fin del drag - disparar
        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;
            ShootArrow();
            HideTrajectory();
        }
    }

    /// <summary>
    /// Crea una nueva flecha en la posición de disparo.
    /// </summary>
    private void CreateArrow()
    {
        if (currentArrow != null)
            Destroy(currentArrow.gameObject);

        currentArrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, Quaternion.identity);
        currentArrow.SetGameManager(GetComponent<GameManager>());
    }

    /// <summary>
    /// Actualiza el aiming basado en el drag del mouse.
    /// </summary>
    private void UpdateAiming()
    {
        Vector2 currentMousePosition = Input.mousePosition;
        Vector2 dragDelta = dragStartPosition - currentMousePosition;

        // Convertir píxeles a unidades del mundo
        Vector3 worldDragDelta = Camera.main.ScreenToWorldPoint(new Vector3(dragDelta.x, dragDelta.y, 0))
                                 - Camera.main.ScreenToWorldPoint(Vector3.zero);

        // Calcular ángulo
        float angle = Mathf.Atan2(worldDragDelta.y, worldDragDelta.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Mostrar trayectoria
        ShowTrajectory(worldDragDelta.magnitude);
    }

    /// <summary>
    /// Dispara la flecha con la fuerza calculada.
    /// </summary>
    private void ShootArrow()
    {
        if (currentArrow == null)
            return;

        Vector2 dragDelta = dragStartPosition - (Vector2)Input.mousePosition;
        Vector3 worldDragDelta = Camera.main.ScreenToWorldPoint(new Vector3(dragDelta.x, dragDelta.y, 0))
                                 - Camera.main.ScreenToWorldPoint(Vector3.zero);

        float force = Mathf.Clamp(worldDragDelta.magnitude, minForce, maxForce);
        Vector2 direction = transform.right;

        currentArrow.Launch(direction, force);
    }

    /// <summary>
    /// Muestra una línea de trayectoria predicha.
    /// </summary>
    private void ShowTrajectory(float dragDistance)
    {
        if (trajectoryLine == null)
            return;

        float force = Mathf.Clamp(dragDistance, minForce, maxForce);
        Vector2 direction = transform.right;

        trajectoryLine.positionCount = trajectoryPoints;
        Vector3[] positions = new Vector3[trajectoryPoints];

        for (int i = 0; i < trajectoryPoints; i++)
        {
            float time = (float)i / (trajectoryPoints - 1) * 2f;
            Vector3 position = arrowSpawnPoint.position 
                + (Vector3)(direction * force * time)
                + Vector3.down * 0.5f * 9.81f * time * time;
            positions[i] = position;
        }

        trajectoryLine.SetPositions(positions);
    }

    /// <summary>
    /// Oculta la línea de trayectoria.
    /// </summary>
    private void HideTrajectory()
    {
        if (trajectoryLine != null)
            trajectoryLine.positionCount = 0;
    }
}
