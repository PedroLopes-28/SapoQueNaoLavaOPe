using UnityEngine;

[CreateAssetMenu(fileName = "BlockEffect", menuName = "Cards/Effects/BlockEffect")]
public class BlockEffect : CardEffect
{
    public int quantidade;

    public override void execute(
        BattleManager battle,
        BattleUnit usuario,
        BattleUnit alvo
    )
    {
        alvo.GanharBloqueio(quantidade);
    }

}
