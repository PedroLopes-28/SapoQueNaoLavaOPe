using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopCardView : MonoBehaviour
{
    [SerializeField] private Image cardImage;
    [SerializeField] private TMP_Text custoText;
    [SerializeField] private TMP_Text descricaoText;
    [SerializeField] private Button botao;

    public CardData Carta { get; private set; }

    private Action<ShopCardView> aoComprar;

    public void Inicializar(
        CardData carta,
        Action<ShopCardView> callback)
    {
        Carta = carta;
        aoComprar = callback;

        cardImage.sprite = carta.sprite;
        cardImage.preserveAspect = true;

        custoText.text = carta.custo.ToString();
        descricaoText.text = carta.desc;

        botao.interactable = true;
        botao.onClick.RemoveAllListeners();
        botao.onClick.AddListener(TentarComprar);
    }

    private void TentarComprar()
    {
        aoComprar?.Invoke(this);
    }

    public void Desativar()
    {
        botao.interactable = false;
    }
}