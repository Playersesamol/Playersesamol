using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public float delay = 2f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponent<SpriteRenderer>().enabled = false;
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            Destroy(gameObject, 0.5f);

            GetComponent<AudioSource>().Play();

            Invoke("LoadNextScene", delay);
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene("Nivel2");
    }
}

