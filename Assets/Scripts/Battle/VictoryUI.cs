using System.Collections.Generic;
using Core;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class VictoryUI : MonoBehaviour



{
    [Header("Canvas")]
    [SerializeField] private GameObject victoryCanvas;

    [Header("Recompensa de cartas")]
    [SerializeField] private CardDatabase cartasRecompensa;
    [SerializeField] private RewardCardView cardPrefab;
    [SerializeField] private Transform cardsContainer;
    
    [Header("Painéis de recompensa")]
    [SerializeField] private GameObject cardRewardPanel;
    
    [Header("Recompensa em ouro")]
    [SerializeField] private int quantidadeOuro = 50;
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private Button claimGoldButton;

    private readonly List<RewardCardView> cartasVisuais = new();

    private bool cartaEscolhida;
    private bool ouroColetado;
    private bool retornandoMapa;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (victoryCanvas != null)
            victoryCanvas.SetActive(false);
    }
    

    // Update is called once per frame
    public void MostrarVictory()
    {
        cartaEscolhida = false;
        ouroColetado = false;
        retornandoMapa = false;
        
        goldText.text = $"+{quantidadeOuro} de ouro";
        
        GerarCartasRecompensa();
        
        victoryCanvas.SetActive(true);
    }
    
    public void EscondeVictory()
    {
        victoryCanvas.SetActive(false);
    }

    private void GerarCartasRecompensa()
    {
        foreach (RewardCardView visual in cartasVisuais)
        {
            if (visual != null)
            {
                Destroy(visual.gameObject);
            }
        }
        cartasVisuais.Clear();
        
        if (cartasRecompensa == null)
        {
            Debug.LogError("CardDatabase de recompensa não configurado!");
            return;
        }
        
        List<CardData> cartas = new List<CardData>(
            cartasRecompensa.buscaTodas()
        );

        cartas.RemoveAll(c => c == null);

        //pega so cartas distintas
        cartas = new List<CardData>(
            new HashSet<CardData>(cartas)
        );

        for (int i = 0; i < 2; i++)
        {
            int indice = Random.Range(0, cartas.Count);
            CardData carta = cartas[indice];
            
            cartas.RemoveAt(indice);
            RewardCardView visual = Instantiate(
                cardPrefab, cardsContainer
                );
            
            visual.Inicializar(carta,EscolherCarta);
            cartasVisuais.Add(visual);
        }


    }
    
        private void EscolherCarta(CardData carta)
        {
            if (cartaEscolhida || retornandoMapa || carta == null)
                return;

            GameManager.Gman.deckJogador.Add(carta);

            cartaEscolhida = true;

            Debug.Log("Carta recebida: " + carta.name);

            // Esconde todo o painel de seleção de cartas
            cardRewardPanel.SetActive(false);
        }

    
    
    
        public void ReceberOuro()
        {
            if (ouroColetado || retornandoMapa)
                return;

            GameManager.Run.gold += quantidadeOuro;

            ouroColetado = true;

            // Esconde o botão, mas mantém o painel de ouro
            claimGoldButton.gameObject.SetActive(false);

            goldText.text = "Ouro coletado!";

            Debug.Log($"Recebeu {quantidadeOuro} de ouro");
        }


}
