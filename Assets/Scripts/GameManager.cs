using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public UIManager uiManager;
    public int money = 0;

    // notify if other systems want to listen
    public Action<int> onMoneyChanged;

    void Awake()
    {
        if (Instance == null) Instance = this; else Destroy(gameObject);
    }

    public void AddMoney(int amount, Vector3 fromWorldPos)
    {
        if (amount <= 0) return;
        money += amount;
        uiManager?.UpdateScore(money);
        uiManager?.SpawnCoinFly(fromWorldPos);
        onMoneyChanged?.Invoke(money);
    }
}
