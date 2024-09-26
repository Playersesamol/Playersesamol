using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SonidoEntreE : MonoBehaviour
{
    private static SonidoEntreE instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        // Verificar si se llegó a la escena final y detener el sonido
        if (SceneManager.GetActiveScene().name == "GameOver")
        {
            DetenerSonido();
        }
    }

    private void DetenerSonido()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}

