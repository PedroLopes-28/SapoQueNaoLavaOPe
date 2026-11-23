using Core;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [Header("Tutorial")]
    [SerializeField] private GameObject tutorialPanel;

    private void Start()
    {
        tutorialPanel.SetActive(false);
    }

    public void Jogar()
    {
        // Inicia uma nova run e carrega o mapa
        GameManager.Gman.NovaRun();
    }

    public void AbrirTutorial()
    {
        tutorialPanel.SetActive(true);
    }

    public void FecharTutorial()
    {
        tutorialPanel.SetActive(false);
    }
    
    public void SairDoJogo()
    {
        Debug.Log("Saindo do jogo...");

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}