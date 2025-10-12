using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    void Start()
    {
        int lastScore = PlayerPrefs.GetInt("score", 0);
        scoreText.text = "Puntaje: " + lastScore;
    }
}
