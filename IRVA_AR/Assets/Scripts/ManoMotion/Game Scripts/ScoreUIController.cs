using System;
using AR_ManoMotion;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace ManoMotion.Game_Scripts
{
    public class ScoreUIController : MonoBehaviour
    {
        [SerializeField] private ARInitController arInitController;
        [SerializeField] private TMP_Text scoreText;

        private FruitSpawner _fruitSpawner;
        private int _score;
        
        private void Awake()
        {
            arInitController.OnGameStarted += SubscribeToFruitSpawner;
        }

        private void SubscribeToFruitSpawner()
        {
            _fruitSpawner = FindObjectOfType<FruitSpawner>();
            _fruitSpawner.OnFruitSpawned += HandleOnFruitSpawned;
        }

        private void HandleOnFruitSpawned(FruitController fruit)
        {
            fruit.OnPlayerCut += () => scoreText.text = $"Score: {++_score}";
        }
    }
}
