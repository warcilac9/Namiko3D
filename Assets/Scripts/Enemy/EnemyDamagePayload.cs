public class EnemyDamagePayload
{
    public float Amount { get; }
    public float HurtDuration { get; }
    public bool IsLethal { get; }

    public EnemyDamagePayload(float amount, float hurtDuration, bool isLethal)
    {
        Amount = amount;
        HurtDuration = hurtDuration;
        IsLethal = isLethal;
    }
}
