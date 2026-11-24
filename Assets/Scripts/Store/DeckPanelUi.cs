
using System.Collections.Generic;
using Core;
using TMPro;
using UnityEngine;

public class DeckPanelUI : MonoBehaviour
{
    [Header("Interface")]
    [SerializeField] private GameObject deckPanel;
    [SerializeField] private Transform cardsContainer;
    [SerializeField] private TMP_Text removalPriceText;
    [SerializeField] private TMP_Text warningText;

    [Header("Prefab das cartas")]
    [SerializeField] private ShopCardView cardPrefab;

    [Header("Configuração")]
    [SerializeField] private int precoRemocao = 75;

    private readonly List<ShopCardView> cartasVisuais = new();

    private void Start()
    {
        deckPanel.SetActive(false);
    }

    public void AbrirPainel()
    {
        deckPanel.SetActive(true);

        EsconderAviso();
        AtualizarPreco();
        MostrarCartas();
    }

    public void FecharPainel()
    {
        deckPanel.SetActive(false);
    }

    private void AtualizarPreco()
    {
        removalPriceText.text =
            $"Remover carta: {precoRemocao} ouro";
    }

    private void EsconderAviso()
    {
        if (warningText != null)
            warningText.gameObject.SetActive(false);
    }

    private void MostrarAviso(string mensagem)
    {
        Debug.Log(mensagem);

        if (warningText == null)
            return;

        warningText.text = mensagem;
        warningText.gameObject.SetActive(true);
    }

    private void MostrarCartas()
    {
        // Remove os objetos visuais anteriores
        foreach (Transform filho in cardsContainer)
        {
            Destroy(filho.gameObject);
        }

        cartasVisuais.Clear();

        // Obtém o baralho atual da run
        List<CardData> deck = GameManager.Gman.deckJogador;

        foreach (CardData carta in deck)
        {
            if (carta == null)
                continue;

            ShopCardView visual = Instantiate(
                cardPrefab,
                cardsContainer
            );

            // Associa o clique à remoção
            visual.Inicializar(carta, SelecionarCarta);

            cartasVisuais.Add(visual);
        }
    }

    private void SelecionarCarta(ShopCardView visual)
    {
        if (visual == null || visual.Carta == null)
            return;

        // Verifica se a carta está no painel
        if (!cartasVisuais.Contains(visual))
            return;

        // Verifica se o jogador tem ouro suficiente
        if (GameManager.Run.gold < precoRemocao)
        {
            MostrarAviso("Ouro insuficiente!");
            return;
        }

        // Remove uma ocorrência da carta do deck
        bool removida = GameManager.Gman.deckJogador.Remove(
            visual.Carta
        );

        if (!removida)
        {
            MostrarAviso("Não foi possível remover a carta.");
            return;
        }

        // Desconta o ouro
        GameManager.Run.gold -= precoRemocao;

        Debug.Log(
            $"Carta removida: {visual.Carta.name}. " +
            $"Custo: {precoRemocao} ouro."
        );

        // Impede outro clique na mesma carta
        visual.Desativar();

        // Remove a carta da interface
        cartasVisuais.Remove(visual);
        Destroy(visual.gameObject);

        EsconderAviso();

        // Atualiza o ouro exibido na loja
        ShopManager loja = FindFirstObjectByType<ShopManager>();

        if (loja != null)
            loja.AtualizarOuroLoja();
    }
}
