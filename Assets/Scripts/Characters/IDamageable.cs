namespace Shooter.Utility
{
    public interface IDamageable
    {
        float HitPoints { get; set; }
        void TakeDamage(float damage);
        void CheckHitpoints();
    }
}