using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Effects/Damage")]
public class DamageEffect : CardEffect
{
    public int quantidade;

    public override void execute(
        BattleManager battle,
        BattleUnit usuario,
        BattleUnit alvo
    )
    {
        alvo.ReceberDano(quantidade);
    }
}