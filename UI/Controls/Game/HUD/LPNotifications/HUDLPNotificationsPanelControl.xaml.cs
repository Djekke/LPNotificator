namespace CryoFall.LPNotificator.UI.Controls.Game.HUD.LPNotifications
{
    using AtomicTorch.CBND.CoreMod.ClientComponents.Timer;
    using AtomicTorch.GameEngine.Common.Client.MonoGame.UI;
    using System;
    using System.Windows.Controls;

    public partial class HUDLPNotificationsPanelControl : BaseUserControl
    {
        private const int MaxNotificationsToDisplay = 7;

        private const double NotificationHideDelaySeconds = 7.5;

        private static HUDLPNotificationsPanelControl instance;

        private UIElementCollection stackPanelChildren;

        public HUDLPNotificationsPanelControl()
        {
        }

        public static void Show(int deltaCount, int total)
        {
            instance.ShowInternal(deltaCount, total);
        }

        protected override void InitControl()
        {
            instance = this;
            this.stackPanelChildren = this.GetByName<StackPanel>("StackPanel").Children;

            if (!IsDesignTime)
            {
                this.stackPanelChildren.Clear();
            }
        }

        private void HideOldNotificationsIfTooManyDisplayed()
        {
            var countToHide = this.stackPanelChildren.Count - MaxNotificationsToDisplay;
            for (var index = 0; index < countToHide; index++)
            {
                var control = (HUDLPNotificationControl)this.stackPanelChildren[index];
                control.Hide(quick: true);
            }
        }

        private void ShowInternal(int deltaCount, int total)
        {
            if (total < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(total));
            }

            if (deltaCount == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(deltaCount));
            }

            var notificationControl = HUDLPNotificationControl.Create(deltaCount, total);
            this.stackPanelChildren.Add(notificationControl);

            // hide after delay
            ClientComponentTimersManager.AddAction(
                NotificationHideDelaySeconds,
                () => notificationControl.Hide(quick: false));

            this.HideOldNotificationsIfTooManyDisplayed();
        }
    }
}