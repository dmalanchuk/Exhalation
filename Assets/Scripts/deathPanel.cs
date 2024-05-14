using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class deathPanel : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI scoreText;
   private void Start()
   {
      // WaveSpawner WsP = FindObjectOfType<WaveSpawner>();
      // scoreText.text = "current wave: " + WsP.currentwWaveIndex.ToString();
   }

   public void Restart()
   {
      SceneManager.LoadScene(SceneManager.GetActiveScene().name);
   }
}
