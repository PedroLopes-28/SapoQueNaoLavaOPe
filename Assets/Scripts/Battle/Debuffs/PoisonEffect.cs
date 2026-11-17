using UnityEngine;

[CreateAssetMenu(menuName = "Debuffs/Veneno")]
public class PoisonEffect : DebuffEffect
{
    public int danoPorTurno;

    public override void Aplicar(BattleUnit unidade)
    {
        
        unidade.ReceberDano(danoPorTurno);
        this.duracao--;
        this.danoPorTurno = duracao;
    }
}