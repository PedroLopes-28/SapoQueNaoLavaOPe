using UnityEngine;

public class BattleUnit : MonoBehaviour
{
    public int vidaAtual;
    public int bloqueio = 0;

    public void ReceberDano(int dano)
    {
        int danoRestante = Mathf.Max(0, dano - bloqueio);

        bloqueio = Mathf.Max(0, bloqueio - dano);

        vidaAtual = Mathf.Max(0, vidaAtual - danoRestante);
    }

    public void GanharBloqueio(int quantidade)
    {
        bloqueio += quantidade;
    }
}
