using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Gestiona la lógica principal del juego.
/// Controla preguntas, puntuación y cambio de niveles.
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private int minNumber = 1;
    [SerializeField] private int maxNumber = 20;
    [SerializeField] private int wrongAnswersCount = 3;

    [Header("References")]
    [SerializeField] private TargetObject targetPrefab;
    [SerializeField] private Transform targetsContainer;
    [SerializeField] private UIManager uiManager;

    private QuestionGenerator questionGenerator;
    private QuestionData currentQuestion;
    private int score = 0;
    private int questionsAnswered = 0;
    private List<TargetObject> currentTargets = new List<TargetObject>();

    private void Start()
    {
        questionGenerator = new QuestionGenerator(minNumber, maxNumber);
        GenerateNewQuestion();
    }

    /// <summary>
    /// Genera una nueva pregunta y crea los objetivos con respuestas.
    /// </summary>
    public void GenerateNewQuestion()
    {
        // Limpiar objetivos anteriores
        foreach (TargetObject target in currentTargets)
        {
            Destroy(target.gameObject);
        }
        currentTargets.Clear();

        // Generar nueva pregunta
        currentQuestion = questionGenerator.GenerateRandomQuestion();
        Debug.Log($"Nueva pregunta: {currentQuestion.GetQuestionText()} = {currentQuestion.CorrectAnswer}");

        // Actualizar UI
        if (uiManager != null)
        {
            uiManager.UpdateQuestionText(currentQuestion.GetQuestionText());
        }

        // Generar respuestas incorrectas
        List<int> wrongAnswers = questionGenerator.GenerateWrongAnswers(currentQuestion.CorrectAnswer, wrongAnswersCount);
        
        // Crear lista de todas las respuestas
        List<int> allAnswers = new List<int> { currentQuestion.CorrectAnswer };
        allAnswers.AddRange(wrongAnswers);
        
        // Mezclar las respuestas
        ShuffleList(allAnswers);

        // Crear objetivos en pantalla
        CreateTargets(allAnswers);
    }

    /// <summary>
    /// Crea los objetivos en la pantalla.
    /// </summary>
    private void CreateTargets(List<int> answers)
    {
        float spacing = 2f;
        float startY = 0f;

        for (int i = 0; i < answers.Count; i++)
        {
            Vector3 spawnPosition = new Vector3(7f, startY + (i * spacing), 0f);
            TargetObject target = Instantiate(targetPrefab, spawnPosition, Quaternion.identity, targetsContainer);
            target.SetValue(answers[i]);
            target.SetGameManager(this);
            currentTargets.Add(target);
        }
    }

    /// <summary>
    /// Se llama cuando la flecha impacta un objetivo.
    /// </summary>
    public void OnTargetHit(int targetValue)
    {
        if (targetValue == currentQuestion.CorrectAnswer)
        {
            OnCorrectAnswer();
        }
        else
        {
            OnWrongAnswer();
        }
    }

    private void OnCorrectAnswer()
    {
        score += 10;
        questionsAnswered++;
        Debug.Log($"¡Correcto! Puntuación: {score}");
        
        if (uiManager != null)
        {
            uiManager.UpdateScore(score);
            uiManager.ShowFeedback("¡Correcto!", true);
        }

        Invoke("GenerateNewQuestion", 2f);
    }

    private void OnWrongAnswer()
    {
        Debug.Log("Respuesta incorrecta. Intenta de nuevo.");
        
        if (uiManager != null)
        {
            uiManager.ShowFeedback("¡Incorrecto!", false);
        }

        Invoke("GenerateNewQuestion", 2f);
    }

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public QuestionData GetCurrentQuestion()
    {
        return currentQuestion;
    }

    public int GetScore()
    {
        return score;
    }

    public int GetQuestionsAnswered()
    {
        return questionsAnswered;
    }
}
