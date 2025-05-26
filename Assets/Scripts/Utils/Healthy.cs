using System;

public sealed class Healthy
{
    
    public float MaxHealth { get; }
    public float Health { get; private set; }
    
    public Healthy(float maxHealth = 0)
    {
        MaxHealth = maxHealth;
        Health = MaxHealth;
    }

    public bool TakeDamage(float value)
    {
        Health = Math.Max(Health - value, 0);
        if (Health <= 0) return true;
        return false;
    }

    public void AddHealth(float value) => Health = Math.Min(Health + value, MaxHealth);
    
}