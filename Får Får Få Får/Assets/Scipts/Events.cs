using System;
using UnityEngine;

public static class Events
{
    public static event Action onBattleLost;
    public static void BattleLost() => onBattleLost?.Invoke();

    public static event Action onBattleWon;
    public static void BattleWon() => onBattleWon?.Invoke();
}
