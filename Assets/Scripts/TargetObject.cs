using TMPro;
using UnityEngine;

/// <summary>
/// Representa un objetivo (globo, caja, etc.) que contiene un número.
/// Detecta colisiones con flechas y notifica al GameManager.
/// </summary>
public class TargetObject : MonoBehaviour
{
    [Header("Display")]
    [SerializeField] private TextMeshPro numberText;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Physics")]
    [SerializeField] private CircleCollider2D circleCollider;
    [SerializeField] private float destroyDelay = 0.5f;

    [Header("Visual Feedback")]
    [SerializeField] private Color hitColor = Color.red;
    [SerializeField] private float flashDuration = 0.2f;

    private int value;
    private GameManager gameManager;
    private Color originalColor;
    private bool isDestroyed = false;

    private void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        if (circleCollider == null)
            circleCollider = GetComponent<CircleCollider2D>();
        if (numberText == null)
            numberText = GetComponentInChildren<TextMeshPro>();

        originalColor = spriteRenderer.color;
    }

    /// <summary>
    /// Establece el valor del objetivo.
    /// </summary>
    public void SetValue(int newValue)
    {
        value = newValue;
        if (numberText != null)
        {
            numberText.text = value.ToString();
        }
    }

    /// <summary>
    /// Establece la referencia al GameManager.
    /// </summary>
    public void SetGameManager(GameManager manager)
    {
        gameManager = manager;
    }

    public int GetValue()
    {
        return value;
    }

    /// <summary>
    /// Reproduce una animación de destrucción visual.
    /// </summary>
    public void PlayDestroyAnimation()
    {
        if (isDestroyed)
            return;

        isDestroyed = true;

        // Flash de color
        if (spriteRenderer != null)
        {
            StartCoroutine(FlashColor());
        }

        // Destruir después de un tiempo
        Destroy(gameObject, destroyDelay);
    }

    private System.Collections.IEnumerator FlashColor()
    {
        for (int i = 0; i < 2; i++)
        {
            spriteRenderer.color = hitColor;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(flashDuration);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ArrowProjectile arrow = collision.GetComponent<ArrowProjectile>();
        if (arrow != null && arrow.IsLaunched())
        {
            PlayDestroyAnimation();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ArrowProjectile arrow = collision.gameObject.GetComponent<ArrowProjectile>();
        if (arrow != null && arrow.IsLaunched())
        {
            PlayDestroyAnimation();
        }
    }
}
