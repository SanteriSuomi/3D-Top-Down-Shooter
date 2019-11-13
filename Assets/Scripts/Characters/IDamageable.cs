namespace Shooter.Utility
{
    public interface IDamageable
    {
        float Hitpoints { get; set; }
        void TakeDamage(float damage);
        void CheckHitpoints();
    }
}