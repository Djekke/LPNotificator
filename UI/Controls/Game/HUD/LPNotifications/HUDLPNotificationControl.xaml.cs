namespace CryoFall.LPNotificator.UI.Controls.Game.HUD.LPNotifications
{
    using System;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media.Animation;
    using AtomicTorch.CBND.CoreMod.Systems.Cursor;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Game.Technologies;
    using AtomicTorch.GameEngine.Common.Client.MonoGame.UI;
    using CryoFall.LPNotificator.UI.Controls.Game.HUD.LPNotifications.Data;
    using Menu = AtomicTorch.CBND.CoreMod.UI.Controls.Core.Menu.Menu;

    public partial class HUDLPNotificationControl : BaseUserControl
    {
        private Storyboard storyboardHide;

        private Storyboard storyboardShow;

        private ViewModelHUDLPNotificationControl viewModel;

        public bool IsHiding { get; set; }

        public static HUDLPNotificationControl Create(int deltaCount, int total)
        {
            return new HUDLPNotificationControl()
            {
                viewModel = new ViewModelHUDLPNotificationControl(deltaCount, total)
            };
        }

        public void Hide(bool quick)
        {
            if (quick)
            {
                this.storyboardHide.SpeedRatio = 6.5;
            }

            if (this.IsHiding)
            {
                // already hiding
                return;
            }

            this.IsHiding = true;

            if (!this.isLoaded)
            {
                this.RemoveControl();
                return;
            }

            //Api.Logger.WriteDev($"Hiding notification: {this.viewModel.ProtoItem.Name}: {this.viewModel.DeltaCountText}");
            this.storyboardShow.Stop();
            this.storyboardHide.Begin();
        }

        protected override void InitControl()
        {
            this.storyboardShow = this.GetResource<Storyboard>("StoryboardShow");
            this.storyboardHide = this.GetResource<Storyboard>("StoryboardHide");
        }

        protected override void OnLoaded()
        {
            this.viewModel.RequiredHeight = (float)this.ActualHeight;
            this.DataContext = this.viewModel;

            if (IsDesignTime)
            {
                return;
            }

            this.storyboardShow.Begin();
            this.storyboardHide.Completed += this.StoryboardHideCompletedHandler;

            this.MouseEnter += this.MouseEnterHandler;
            this.MouseLeftButtonDown += this.MouseLeftButtonHandler;
            this.MouseRightButtonDown += this.MouseRightButtonHandler;
            this.MouseLeave += this.MouseLeaveHandler;
        }

        protected override void OnUnloaded()
        {
            if (IsDesignTime)
            {
                return;
            }

            this.storyboardHide.Completed -= this.StoryboardHideCompletedHandler;
            this.DataContext = null;
            this.viewModel.Dispose();
            this.viewModel = null;

            this.MouseEnter -= this.MouseEnterHandler;
            this.MouseLeftButtonDown -= this.MouseLeftButtonHandler;
            this.MouseRightButtonDown -= this.MouseRightButtonHandler;
            this.MouseLeave -= this.MouseLeaveHandler;
        }

        private void MouseEnterHandler(object sender, MouseEventArgs e)
        {
            ClientCursorSystem.CurrentCursorId = CursorId.InteractionPossible;
        }

        private void MouseLeaveHandler(object sender, MouseEventArgs e)
        {
            ClientCursorSystem.CurrentCursorId = CursorId.Default;
        }

        private void MouseLeftButtonHandler(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            this.Hide(quick: true);
            Menu.Open<WindowTechnologies>();
        }

        private void MouseRightButtonHandler(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            this.Hide(quick: true);
        }

        private void RemoveControl()
        {
            ((Panel)this.Parent).Children.Remove(this);
        }

        private void StoryboardHideCompletedHandler(object sender, EventArgs e)
        {
            this.RemoveControl();
        }
    }
}