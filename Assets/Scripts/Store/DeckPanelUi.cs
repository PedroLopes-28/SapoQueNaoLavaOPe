using System.Collections.Generic;
using Core;
using UnityEngine;

public class DeckPanelUI : MonoBehaviour
{
    [Header("Interface")]
    [SerializeField] private GameObject deckPanel;
    [SerializeField] private Transform cardsContainer;

    [Header("Prefab das cartas")]
    [SerializeField] private ShopCardView cardPrefab;

    private readonly List<ShopCardView> cartasVisuais = new();

    private void Start()
    {
        deckPanel.SetActive(false);
    }

    public void AbrirPainel()
    {
        deckPanel.SetActive(true);

        MostrarCartas();
    }

    public void FecharPainel()
    {
        deckPanel.SetActive(false);
    }

    private void MostrarCartas()
    {
        // Remove as cartas visuais anteriores
        foreach (ShopCardView visual in cartasVisuais)
        {
            if (visual != null)
                Destroy(visual.gameObject);
        }

        cartasVisuais.Clear();

        // Busca as cartas atuais da run
        List<CardData> deck =
            GameManager.Gman.deckJogador;

        foreach (CardData carta in deck)
        {
            if (carta == null)
                continue;

            ShopCardView visual = Instantiate(
                cardPrefab,
                cardsContainer
            );

            visual.Inicializar(carta, null);
            visual.Desativar();
            cartasVisuais.Add(visual);
        }
    }
}