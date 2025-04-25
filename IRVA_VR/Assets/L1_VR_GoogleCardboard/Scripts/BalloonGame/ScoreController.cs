using L1_VR_GoogleCardboard.Scripts.Helpers;
using TMPro;
using UnityEngine;

namespace L1_VR_GoogleCardboard.Scripts.BalloonGame
{
    /// <summary>
    /// Used to keep track of the game score & update a world space UI canvas.
    /// </summary>
    public class ScoreController : Singleton<ScoreController>
    {
       [SerializeField] private TextMeshProUGUI scoreText;

       private int _score;
       
       public void IncrementScore()
       {
           // TODO 6.3 : Keep track of the score & set the UI text.
           ++_score;
           
           scoreText.text = _score.ToString();
       }

       public void DecrementScore()
       {
           // TODO 6.4 : Keep track of the score & set the UI text.
           --_score;
           
           scoreText.text = _score.ToString();
       }
    }
}