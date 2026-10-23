using System;
using TMPro;
using UnityEngine;

public class BattleUI : MonoBehaviour
{
    [SerializeField] private BattleManager battleManager;

    [SerializeField] private TMP_Text deckText;
    [SerializeField] private TMP_Text descartesText;
    [SerializeField] private TMP_Text velocidadeText;

    [SerializeField] private Transform handContainer;
    [SerializeField] private CardView cardPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Start()
    {
       AtualizarUI();
    }
    
    private void OnEnable()
    {
        battleManager.OnCartasAtualizadas += AtualizarUI;
    }

    private void AtualizarUI()
    {
        deckText.text = battleManager.QtdDeck.ToString();
        descartesText.text = battleManager.QtdDescartes.ToString();
        velocidadeText.text = battleManager.VelocidadeAtual.ToString();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
