namespace CryoFall.LPNotificator.Bootstrappers
{
    using AtomicTorch.CBND.CoreMod.Bootstrappers;
    using AtomicTorch.CBND.CoreMod.Characters;
    using AtomicTorch.CBND.CoreMod.Characters.Player;
    using AtomicTorch.CBND.CoreMod.Systems;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Game.HUD;
    using AtomicTorch.CBND.GameApi.Data.Characters;
    using AtomicTorch.CBND.GameApi.Data.State;
    using AtomicTorch.CBND.GameApi.Scripting;
    using CryoFall.LPNotificator.UI.Controls.Game.HUD.LPNotifications;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public class BootstrapperClientLPNotification : BaseBootstrapper
    {
        private static HUDLayoutControl hudLayoutControl;

        private static StateSubscriptionStorage stateSubscriptionStorage;

        private static PlayerCharacterPrivateState privateState;

        private static IActionState lastActionState;

        private static ushort lp = 0;

        public override void ClientInitialize()
        {
            BootstrapperClientGame.InitEndCallback += GameInit;

            BootstrapperClientGame.ResetCallback += GameReset;
        }

        private static void GameInit(ICharacter character)
        {
            foreach (var child in Api.Client.UI.LayoutRootChildren)
            {
                if (child is HUDLayoutControl layoutControl)
                {
                    hudLayoutControl = layoutControl;
                }
            }
            if (hudLayoutControl != null)
            {
                hudLayoutControl.Loaded += LayoutControl_Loaded;
            }
            else
            {
                Api.Logger.Error("LPNotifications: HUDLayoutControl not found.");
            }

            stateSubscriptionStorage = new StateSubscriptionStorage();
            privateState = PlayerCharacter.GetPrivateState(character);
            lp = privateState.Technologies.LearningPoints;
            privateState.Technologies.ClientSubscribe(
                t => t.LearningPoints,
                OnLPChanged,
                stateSubscriptionStorage);
        }

        private static void LayoutControl_Loaded(object sender, RoutedEventArgs e)
        {
            DependencyObject child = VisualTreeHelper.GetChild(hudLayoutControl, 0);
            child = VisualTreeHelper.GetChild(child, 0);
            var rootGrid = VisualTreeHelper.GetChild(child, 0);
            if (rootGrid is Grid grid)
            {
                var scaleBox = new Scalebox() { Scale = 1 };
                Grid.SetRow(scaleBox, 2);
                Grid.SetColumn(scaleBox, 2);
                var border = new Border()
                {
                    VerticalAlignment = VerticalAlignment.Bottom,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Margin = new Thickness(5, 5, 120, 100)
                };
                var lpNotificationPanel = new HUDLPNotificationsPanelControl();
                border.Child = lpNotificationPanel;
                scaleBox.Content = border;
                grid.Children.Add(scaleBox);
            }
        }

        private static void OnLPChanged()
        {
            var lpChange = privateState.Technologies.LearningPoints - lp;
            lp = privateState.Technologies.LearningPoints;
            HUDLPNotificationsPanelControl.Show(lpChange, lp);
        }

        private static void GameReset()
        {
            stateSubscriptionStorage?.Dispose();
            stateSubscriptionStorage = null;
        }
    }
}