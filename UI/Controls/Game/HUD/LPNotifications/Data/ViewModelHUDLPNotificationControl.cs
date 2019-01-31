namespace CryoFall.LPNotificator.UI.Controls.Game.HUD.LPNotifications.Data
{
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using System.Windows.Media;

    public class ViewModelHUDLPNotificationControl : BaseViewModel
    {
        public static readonly SolidColorBrush BrushBackgroundGreen
            = new SolidColorBrush(Color.FromArgb(0x70, 0x20, 0x90, 0x20));

        public static readonly SolidColorBrush BrushBackgroundRed
            = new SolidColorBrush(Color.FromArgb(0x70, 0xA0, 0x10, 0x10));

        public static readonly SolidColorBrush BrushTextWhite
            = new SolidColorBrush(Color.FromArgb(0xF3, 0xFF, 0xFF, 0xFF));

        private SolidColorBrush backgroundBrush;

        private int deltaCount;

        private string deltaCountText;

        private float requiredHeight;

        private SolidColorBrush textBrush;

        public ViewModelHUDLPNotificationControl(int deltaCount, int total)
        {
            this.DeltaCount = deltaCount;
            this.Total = total;
        }

        public SolidColorBrush BackgroundBrush => this.backgroundBrush;

        public int DeltaCount
        {
            get => this.deltaCount;
            set
            {
                this.deltaCount = value;
                var isPositive = this.deltaCount > 0;
                this.deltaCountText = isPositive ? '+' + this.deltaCount.ToString() : this.deltaCount.ToString();
                this.textBrush = BrushTextWhite; //isPositive ? BrushTextGreen : BrushTextRed;
                this.backgroundBrush = isPositive ? BrushBackgroundGreen : BrushBackgroundRed;
            }
        }

        public string DeltaCountText => this.deltaCountText;

        public int Total { get; }

        public float RequiredHeight
        {
            get => this.requiredHeight;
            set
            {
                this.requiredHeight = value;
                this.NotifyThisPropertyChanged();
            }
        }

        public SolidColorBrush TextBrush => this.textBrush;
    }
}