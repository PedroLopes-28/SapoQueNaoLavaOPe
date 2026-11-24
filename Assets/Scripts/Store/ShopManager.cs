
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

    private readonly List<CardData> cartasSorteadas = new();

    private void Start()
    {
        AtualizarOuro();
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
            Debug.LogError("Configure o CardDatabase e o Prefab da loja!");
            return;
        }

        if (cardSpawnPoints.Length != 3 || priceTexts.Length != 3)
        {
            Debug.LogError(
                "A loja precisa de 3 SpawnPoints e 3 textos de preço!"
            );
            return;
        }

        // Cria uma lista independente do CardDatabase
        List<CardData> cartasDisponiveis =
            new List<CardData>(cartasLoja.buscaTodas());

        // Remove referências nulas e duplicadas
        cartasDisponiveis.RemoveAll(c => c == null);

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

        for (int i = 0; i < 3; i++)
        {
            // Sorteia uma carta
            int indice = Random.Range(0, cartasDisponiveis.Count);

            CardData carta = cartasDisponiveis[indice];

            // Impede sortear a mesma carta novamente
            cartasDisponiveis.RemoveAt(indice);

            cartasSorteadas.Add(carta);

            // Instancia a carta no espaço correspondente
            ShopCardView visual = Instantiate(
                cardPrefab,
                cardSpawnPoints[i]
            );

            RectTransform rect = visual.GetComponent<RectTransform>();

            rect.anchoredPosition = Vector2.zero;
            rect.localRotation = Quaternion.identity;

            visual.Inicializar(carta);

            // Exibe o preço da carta
            priceTexts[i].text = $"{precoCarta} ouro";
        }
    }
}
