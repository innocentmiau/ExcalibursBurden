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

    public void TakeDamage(float value) => Health = Math.Max(Health - value, 0);

    public void AddHealth(float value) => Health = Math.Min(Health + value, MaxHealth);
    
}