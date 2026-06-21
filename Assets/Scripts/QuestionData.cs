using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Almacena y gestiona los datos de las preguntas matemáticas.
/// Genera operaciones simples para niños de 5 a 12 años.
/// </summary>
public class QuestionData
{
    public int Number1 { get; set; }
    public int Number2 { get; set; }
    public OperationType Operation { get; set; }
    public int CorrectAnswer { get; set; }

    public enum OperationType
    {
        Addition,      // Suma
        Subtraction    // Resta
    }

    public QuestionData(int num1, int num2, OperationType op)
    {
        Number1 = num1;
        Number2 = num2;
        Operation = op;
        CalculateCorrectAnswer();
    }

    private void CalculateCorrectAnswer()
    {
        switch (Operation)
        {
            case OperationType.Addition:
                CorrectAnswer = Number1 + Number2;
                break;
            case OperationType.Subtraction:
                CorrectAnswer = Number1 - Number2;
                break;
        }
    }

    public string GetQuestionText()
    {
        string operationSymbol = Operation == OperationType.Addition ? "+" : "-";
        return $"{Number1} {operationSymbol} {Number2} = ?";
    }
}

/// <summary>
/// Generador de preguntas aleatorias.
/// </summary>
public class QuestionGenerator
{
    private int minNumber = 1;
    private int maxNumber = 20;

    public QuestionGenerator(int min = 1, int max = 20)
    {
        minNumber = min;
        maxNumber = max;
    }

    public QuestionData GenerateRandomQuestion()
    {
        int num1 = Random.Range(minNumber, maxNumber + 1);
        int num2 = Random.Range(minNumber, maxNumber + 1);
        
        // Para restas, asegurar que num1 >= num2
        if (num1 < num2)
        {
            int temp = num1;
            num1 = num2;
            num2 = temp;
        }

        QuestionData.OperationType operation = Random.value > 0.5f 
            ? QuestionData.OperationType.Addition 
            : QuestionData.OperationType.Subtraction;

        return new QuestionData(num1, num2, operation);
    }

    public List<int> GenerateWrongAnswers(int correctAnswer, int count = 3)
    {
        List<int> wrongAnswers = new List<int>();
        
        for (int i = 0; i < count; i++)
        {
            int wrongAnswer;
            do
            {
                wrongAnswer = correctAnswer + Random.Range(-5, 6);
            } while (wrongAnswer == correctAnswer || wrongAnswers.Contains(wrongAnswer) || wrongAnswer < 0);
            
            wrongAnswers.Add(wrongAnswer);
        }

        return wrongAnswers;
    }
}
