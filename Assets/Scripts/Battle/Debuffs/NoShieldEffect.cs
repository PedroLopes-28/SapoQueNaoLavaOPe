using UnityEngine;

[CreateAssetMenu(menuName = "Debuffs/SemEscudo")]
public class NoShieldEffect : DebuffEffect
{

    public override void Aplicar(BattleUnit unidade)
    {
        unidade.bloqueio = 0;
    }
}