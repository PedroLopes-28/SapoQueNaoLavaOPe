using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardCardView : MonoBehaviour
{
    [SerializeField] private Image cardImage;
    [SerializeField] private TMP_Text custoText;
    [SerializeField] private TMP_Text descricaoText;
    [SerializeField] private Button botao;

    private CardData carta;
    private Action<CardData> aoEscolher;

    public void Inicializar(
        CardData dados,
        Action<CardData> callback)
    {
        carta = dados;
        aoEscolher = callback;

        custoText.text = carta.custo.ToString();
        descricaoText.text = carta.desc;
        cardImage.sprite = carta.sprite;

        botao.onClick.RemoveAllListeners();
        botao.onClick.AddListener(Escolher);
    }

    private void Escolher()
    {
        if (carta == null)
            return;

        aoEscolher?.Invoke(carta);// ação verificavel no victoryUI ai eu adiciono tudo por lá
    }

    public void Desativar()
    {
        botao.interactable = false;
    }
}