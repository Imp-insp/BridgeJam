using UnityEngine;
using UnityEngine.SceneManagement;
// using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private string gameSceneName; // Precisa atualizar o nome da cena do level no editor

    //  Funções dos botões
    public void OnPlayButtonPressed()
    {
        Debug.Log("Iniciando o jogo...");
        SceneManager.LoadScene(gameSceneName);
    }

    public void OnQuitButtonPressed()
    {
        Application.Quit();

    }
}