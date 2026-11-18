using UnityEngine;

public abstract class CardEffect : ScriptableObject
{
    public Color cor;
    public abstract void execute(BattleManager battle, BattleUnit usuario, BattleUnit alvo);

}
