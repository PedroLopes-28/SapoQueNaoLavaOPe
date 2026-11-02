using UnityEngine;

public class VictoryUI : MonoBehaviour


{
    [SerializeField] private GameObject victoryCanvas;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        victoryCanvas.SetActive(false);
    }

    // Update is called once per frame
    public void MostrarVictory()
    {
        victoryCanvas.SetActive(true);
    }
    
    public void EscondeVictory()
    {
        victoryCanvas.SetActive(false);
    }
    
}
