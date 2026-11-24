
using System.Collections.Generic;
using Core;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [Header("Cartas disponíveis")]
    [SerializeField] private CardDatabase cartasLoja;

    [Header("Prefab da carta")]
    [SerializeField] private ShopCardView cardPrefab;

    [Header("Posições das cartas")]
    [SerializeField] private Transform[] cardSpawnPoints;

    [Header("Preço")]
    [SerializeField] private int precoCarta = 50;
    [SerializeField] private TMP_Text[] priceTexts;

    [Header("Interface")]
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private TMP_Text warningText;

    private readonly List<CardData> cartasSorteadas = new();
    private readonly List<ShopCardView> cartasVisuais = new();

    private void Start()
    {
        AtualizarOuro();

        if (warningText != null)
            warningText.gameObject.SetActive(false);

        GerarCartasLoja();
    }

    private void AtualizarOuro()
    {
        goldText.text = GameManager.Run.gold.ToString();
    }

    private void GerarCartasLoja()
    {
        if (cartasLoja == null || cardPrefab == null)
        {
            Debug.LogError(
                "Configure o CardDatabase e o Prefab da loja!"
            );
            return;
        }

        if (cardSpawnPoints == null ||
            priceTexts == null ||
            cardSpawnPoints.Length != 3 ||
            priceTexts.Length != 3)
        {
            Debug.LogError(
                "A loja precisa de 3 SpawnPoints e 3 textos de preço!"
            );
            return;
        }

        List<CardData> cartasDisponiveis =
            new List<CardData>(cartasLoja.buscaTodas());

        // Remove cartas nulas
        cartasDisponiveis.RemoveAll(c => c == null);

        // Remove cartas duplicadas
        cartasDisponiveis = new List<CardData>(
            new HashSet<CardData>(cartasDisponiveis)
        );

        if (cartasDisponiveis.Count < 3)
        {
            Debug.LogError(
                "O CardDatabase precisa ter pelo menos 3 cartas diferentes!"
            );
            return;
        }

        cartasSorteadas.Clear();
        cartasVisuais.Clear();

        for (int i = 0; i < 3; i++)
        {
            // Sorteia uma carta aleatória
            int indice = Random.Range(
                0,
                cartasDisponiveis.Count
            );

            CardData carta = cartasDisponiveis[indice];

            // Evita repetir a mesma carta
            cartasDisponiveis.RemoveAt(indice);

            cartasSorteadas.Add(carta);

            // Instancia a carta
            ShopCardView visual = Instantiate(
                cardPrefab,
                cardSpawnPoints[i]
            );

            RectTransform rect =
                visual.GetComponent<RectTransform>();

            rect.anchoredPosition = Vector2.zero;
            rect.localRotation = Quaternion.identity;

            // Associa a compra ao clique
            visual.Inicializar(carta, ComprarCarta);

            cartasVisuais.Add(visual);

            // Exibe o preço
            priceTexts[i].text = $"{precoCarta} ouro";
            priceTexts[i].gameObject.SetActive(true);
        }
    }

    private void ComprarCarta(ShopCardView visual)
    {
        // Verifica se a carta é válida
        if (visual == null || visual.Carta == null)
            return;

        // Obtém o índice original do slot
        int indice = cartasVisuais.IndexOf(visual);

        // Impede comprar uma carta já comprada
        if (indice < 0)
            return;

        // Verifica se o jogador possui ouro suficiente
        if (GameManager.Run.gold < precoCarta)
        {
            MostrarAviso("Ouro insuficiente!");
            return;
        }

        // Desconta o ouro
        GameManager.Run.gold -= precoCarta;

        // Adiciona a carta ao deck da run
        GameManager.Gman.deckJogador.Add(visual.Carta);

        Debug.Log(
            $"Carta comprada: {visual.Carta.name} " +
            $"por {precoCarta} de ouro."
        );

        // Atualiza o saldo exibido
        AtualizarOuro();

        // Desativa a interação da carta
        visual.Desativar();

        // Esconde o preço da carta comprada
        priceTexts[indice].gameObject.SetActive(false);

        // Mantém o slot na mesma posição
        cartasVisuais[indice] = null;

        // Esconde a carta comprada
        visual.gameObject.SetActive(false);

        // Esconde qualquer aviso anterior
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
}
