using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{

    public void LoadScene(string scene)
    {
        // Debug.Log("Cargando escena" + scene);
        SceneManager.LoadScene(scene);
    }

    public void Quit()
    {
        // Debug.Log("Te has salido del juego");
        Application.Quit();
    }
}