using System;
using System.Collections.Generic;
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
    
    private readonly List<CardView> cartasVisuais = new();
    private readonly List<CardData> cartasExibidas = new();
    
    [SerializeField] private HandFanLayout handFan;


    private void Start()
    {
       AtualizarUI();
    }
    
    private void OnEnable()
    { if(battleManager != null)
        battleManager.OnCartasAtualizadas += AtualizarUI;
        
    }

    private void OnDisable()
    {
        if (battleManager != null)
        {
            battleManager.OnCartasAtualizadas -= AtualizarUI;
        }
    }

    private void AtualizarUI()
    {
        AtualizarContadores();
        AtualizarMao();
    }

    private void AtualizarContadores()
    {
        deckText.text = battleManager.QtdDeck.ToString();
        descartesText.text = battleManager.QtdDescartes.ToString();
        velocidadeText.text = battleManager.VelocidadeAtual.ToString();

    }

    private void AtualizarMao()
    {
        List<CardData> maoAtual =
            new List<CardData>(battleManager.CartasNaMao);

        //tira só as cartas que estão na mão
        for (int i = cartasExibidas.Count - 1; i >= 0; i--)
        {
            CardData carta = cartasExibidas[i];

            
            int ocorrenciasNaMao = 0;
            int ocorrenciasExibidas = 0;

            foreach (CardData c in maoAtual)
                if (c == carta)
                    ocorrenciasNaMao++;

            foreach (CardData c in cartasExibidas)
                if (c == carta)
                    ocorrenciasExibidas++;

            if (ocorrenciasExibidas > ocorrenciasNaMao)
            {
                Destroy(cartasVisuais[i].gameObject);

                cartasVisuais.RemoveAt(i);
                cartasExibidas.RemoveAt(i);
            }
        }

        // Instancia somente as cartas novas
        foreach (CardData carta in maoAtual)
        {
            int quantidadeAtual = 0;
            int quantidadeNecessaria = 0;

            foreach (CardData c in cartasExibidas)
                if (c == carta)
                    quantidadeAtual++;

            foreach (CardData c in maoAtual)
                if (c == carta)
                    quantidadeNecessaria++;

            if (quantidadeAtual >= quantidadeNecessaria)
                continue;

            CardView visual = Instantiate(
                cardPrefab,
                handContainer
            );

            visual.Inicializar(carta, battleManager);

            cartasVisuais.Add(visual);
            cartasExibidas.Add(carta);
            handFan.DefinirCartas(cartasVisuais);
        }
    }

    public void atualizaFan()
    {
        handFan.DefinirCartas(cartasVisuais);
    }

}
