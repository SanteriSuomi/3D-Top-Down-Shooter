namespace Shooter.Shop
{
    public static class NoFundsEventHandler
    {
        public delegate void OnFundsOut(string text);
        public static event OnFundsOut OnFundsOutEvent;

        public static void TriggerFundsOutPopUp(string text)
        {
            // Activate the funds out text when invoked.
            OnFundsOutEvent.Invoke(text);
        }
    }
}