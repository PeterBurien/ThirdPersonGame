using System;

public static class EventBus
{
    public static event Action OnAllCoinsCollected;
    
    public static void TriggerOnAllCoinsCollected()
    {
        OnAllCoinsCollected?.Invoke();
    }
}