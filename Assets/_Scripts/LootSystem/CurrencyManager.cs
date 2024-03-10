using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    int _coinCount;
    int _gemCount;

    // Call this method when a Coin object is picked up
    public void AddCoins(int amount)
    {
        _coinCount += amount;
        // update the UI here
    }

    // Call this method when a Gem object is picked up
    public void AddGems(int amount)
    {
        _gemCount += amount;
        // update the UI here
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
}
