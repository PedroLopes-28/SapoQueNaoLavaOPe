
using System.Collections.Generic;
using UnityEngine;

public class HandFanLayout : MonoBehaviour
{
    [Header("Organização da mão")]
    [SerializeField] private float espacamento = 115f;
    [SerializeField] private float angulo = 9f;
    [SerializeField] private float curvatura = 12f;

    [Header("Efeito Hover")]
    [SerializeField] private float escalaHover = 1.3f;
    [SerializeField] private float elevacaoHover = 90f;
    [SerializeField] private float velocidadeAnimacao = 12f;

    private readonly List<CardHover> cartas = new();

    private CardHover cartaDestacada;

    public void DefinirCartas(List<CardView> cartasVisuais)
    {
        cartas.Clear();
        cartaDestacada = null;

        foreach (CardView visual in cartasVisuais)
        {
            if (visual == null)
                continue;

            CardHover hover = visual.GetComponent<CardHover>();

            if (hover != null)
                cartas.Add(hover);
        }

        RestaurarOrdem();
    }

    public void Destacar(CardHover carta)
    {
        if (!cartas.Contains(carta))
            return;

        cartaDestacada = carta;

        // Coloca a carta na frente das outras
        carta.transform.SetAsLastSibling();
    }

    public void RemoverDestaque(CardHover carta)
    {
        if (cartaDestacada != carta)
            return;

        cartaDestacada = null;
        RestaurarOrdem();
    }

    private void RestaurarOrdem()
    {
        for (int i = 0; i < cartas.Count; i++)
        {
            if (cartas[i] != null)
                cartas[i].transform.SetSiblingIndex(i);
        }
    }

    private void Update()
    {
        int quantidade = cartas.Count;

        if (quantidade == 0)
            return;

        float centro = (quantidade - 1) / 2f;
        float t = 1f - Mathf.Exp(
            -velocidadeAnimacao * Time.deltaTime
        );

        for (int i = 0; i < quantidade; i++)
        {
            CardHover carta = cartas[i];

            if (carta == null)
                continue;

            RectTransform rect = carta.GetComponent<RectTransform>();

            float distancia = i - centro;

            // Distribui horizontalmente
            float x = distancia * espacamento;

            // Cartas laterais ficam mais baixas
            float y = -distancia * distancia * curvatura;

            float rotacao = -distancia * angulo;

            bool destacada = carta == cartaDestacada;

            Vector3 escala = carta.EscalaBase;

            if (destacada)
            {
                y += elevacaoHover;
                rotacao = 0f;
                escala *= escalaHover;
            }

            Vector2 destino = new Vector2(x, y);

            rect.anchoredPosition = Vector2.Lerp(
                rect.anchoredPosition,
                destino,
                t
            );

            rect.localRotation = Quaternion.Slerp(
                rect.localRotation,
                Quaternion.Euler(0, 0, rotacao),
                t
            );

            rect.localScale = Vector3.Lerp(
                rect.localScale,
                escala,
                t
            );
        }
    }
}
