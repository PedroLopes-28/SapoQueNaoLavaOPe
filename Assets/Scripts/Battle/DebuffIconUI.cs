
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebuffIconUI : MonoBehaviour
{
    [SerializeField] private Image imagem;
    [SerializeField] private TMP_Text duracaoText;

    public void Inicializar(Sprite sprite, int duracao)
    {
        imagem.sprite = sprite;
        imagem.preserveAspect = true;

        duracaoText.text = duracao.ToString();
    }
}
