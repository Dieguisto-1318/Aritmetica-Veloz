using TMPro;
using UnityEngine;
using System.Collections;

/// <summary>
/// Gestiona la interfaz gráfica del juego.
/// Muestra preguntas, puntuación, y retroalimentación.
/// </summary>
public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI feedbackText;

    [Header("Feedback Settings")]
    [SerializeField] private Color correctColor = Color.green;
    [SerializeField] private Color incorrectColor = Color.red;
    [SerializeField] private float feedbackDuration = 1.5f;

    private CanvasGroup feedbackCanvasGroup;

    private void Start()
    {
        // Buscar CanvasGroup en el objeto de feedback si existe
        if (feedbackText != null)
        {
            feedbackCanvasGroup = feedbackText.GetComponent<CanvasGroup>();
            if (feedbackCanvasGroup == null)
            {
                feedbackCanvasGroup = feedbackText.gameObject.AddComponent<CanvasGroup>();
            }
            feedbackCanvasGroup.alpha = 0;
        }

        // Inicializar textos
        UpdateScore(0);
    }

    /// <summary>
    /// Actualiza el texto de la pregunta.
    /// </summary>
    public void UpdateQuestionText(string question)
    {
        if (questionText != null)
        {
            questionText.text = question;
            StartCoroutine(AnimateQuestionText());
        }
    }

    /// <summary>
    /// Actualiza el texto de puntuación.
    /// </summary>
    public void UpdateScore(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = $"Puntos: {score}";
        }
    }

    /// <summary>
    /// Muestra retroalimentación sobre la respuesta (correcta o incorrecta).
    /// </summary>
    public void ShowFeedback(string message, bool isCorrect)
    {
        if (feedbackText != null)
        {
            feedbackText.text = message;
            feedbackText.color = isCorrect ? correctColor : incorrectColor;
            StartCoroutine(FadeFeedback());
        }
    }

    /// <summary>
    /// Anima la aparición de la pregunta.
    /// </summary>
    private IEnumerator AnimateQuestionText()
    {
        // Pequeña escala al principio
        if (questionText != null)
        {
            Vector3 originalScale = questionText.transform.localScale;
            questionText.transform.localScale = originalScale * 0.8f;

            float elapsedTime = 0f;
            float duration = 0.3f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float progress = elapsedTime / duration;
                questionText.transform.localScale = Vector3.Lerp(originalScale * 0.8f, originalScale, progress);
                yield return null;
            }

            questionText.transform.localScale = originalScale;
        }
    }

    /// <summary>
    /// Desvanece el texto de retroalimentación.
    /// </summary>
    private IEnumerator FadeFeedback()
    {
        if (feedbackCanvasGroup == null)
            yield break;

        // Fade in
        feedbackCanvasGroup.alpha = 0;
        float elapsedTime = 0f;
        float fadeInDuration = 0.2f;

        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            feedbackCanvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeInDuration);
            yield return null;
        }

        feedbackCanvasGroup.alpha = 1;

        // Esperar
        yield return new WaitForSeconds(feedbackDuration - fadeInDuration);

        // Fade out
        elapsedTime = 0f;
        float fadeOutDuration = 0.3f;

        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            feedbackCanvasGroup.alpha = Mathf.Clamp01(1 - (elapsedTime / fadeOutDuration));
            yield return null;
        }

        feedbackCanvasGroup.alpha = 0;
    }
}
