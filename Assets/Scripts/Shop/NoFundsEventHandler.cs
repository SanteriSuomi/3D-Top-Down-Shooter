namespace Shooter.Shop
{
    public static class NoFundsEventHandler
    {
        //
        // Class that handles the events between shop items and the funds out text popup.
        //
        public delegate void OnFundsOut(string text);
        public static event OnFundsOut OnFundsOutEvent;

        public static void TriggerFundsOutPopUp(string text)
        {
            // Activate the funds out text when invoked.
            OnFundsOutEvent.Invoke(text);
        }
    }
}