using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfColorView
{
    public enum ColorOrder
    {
        Default = 1,
        Name,
        Hex,
        Brightness,
        HSL,
        Saturation,
    }

    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private readonly IEnumerable<ColorInfo> allColors;
        private List<ColorInfo> _colorInfos = new();
        private ColorInfo _selectedBackground;
        private ColorOrder _selectedColorOrder;
        private ColorInfo _selectedForeground;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            SizeChanged += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine($"W: {ActualWidth} | H: {ActualHeight}");
            };

            allColors = typeof(Colors).GetProperties().Select(pInfo => new ColorInfo(pInfo.Name, (Color)pInfo.GetValue(null, null)));

            foreach (var element in Enum.GetValues<ColorOrder>())
            {
                OrderedColors.Add(element, new List<ColorInfo>(GetOrderedColors(element)));
            }

            ColorInfos = OrderedColors[ColorOrder.Default];
            SelectedBackground = ColorInfos.First(x => x.ColorName == nameof(Brushes.White));
            SelectedForeground = ColorInfos.First(x => x.ColorName == nameof(Brushes.Black));
            SelectedColorOrder = ColorOrder.Default;
            CopyCommand = new RelayCommand(new Action<object>(Copy));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public static string AssemblyVersion => Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public List<ColorInfo> ColorInfos { get => _colorInfos; set => SetProperty(ref _colorInfos, value); }

        public ICommand CopyCommand { get; }

        public ColorInfo SelectedBackground { get => _selectedBackground; set => SetProperty(ref _selectedBackground, value); }

        public ColorOrder SelectedColorOrder
        {
            get => _selectedColorOrder;
            set
            {
                if (SetProperty(ref _selectedColorOrder, value))
                {
                    string tempBackgroundName = SelectedBackground?.ColorName;
                    string tempForegroundName = SelectedForeground?.ColorName;

                    ColorInfos = OrderedColors[_selectedColorOrder];

                    // necessary because the SelectedXxxxground is set to null because of DataBinding and new assignment
                    SelectedBackground = ColorInfos.FirstOrDefault(x => x.ColorName == tempBackgroundName);
                    SelectedForeground = ColorInfos.FirstOrDefault(x => x.ColorName == tempForegroundName);
                    UpdateDatagridSelectionView();
                }
            }
        }

        public ColorInfo SelectedForeground { get => _selectedForeground; set => SetProperty(ref _selectedForeground, value); }

        private Dictionary<ColorOrder, List<ColorInfo>> OrderedColors { get; } = new();

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (propertyName is null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            if (!EqualityComparer<T>.Default.Equals(storage, value))
            {
                storage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }

            return false;
        }

        private void Copy(object input)
        {
            Clipboard.SetText(input.ToString());
        }

        private List<ColorInfo> GetOrderedColors(ColorOrder orderBy)
        {
            return new List<ColorInfo>(orderBy switch
            {
                ColorOrder.Default => allColors.OrderBy(x => x.HSL).ThenBy(x => x.Brigthness).ThenBy(x => x.Saturation),
                ColorOrder.Name => allColors.OrderBy(x => x.ColorName),
                ColorOrder.Hex => allColors.OrderBy(x => x.HexValue),
                ColorOrder.Brightness => allColors.OrderBy(x => x.Brigthness),
                ColorOrder.HSL => allColors.OrderBy(x => x.HSL),
                ColorOrder.Saturation => allColors.OrderBy(x => x.Saturation),
                _ => null, //does never happen
            });
        }

        private void UpdateDatagridSelectionView()
        {
            DgBackground.UpdateLayout();
            DgBackground.ScrollIntoView(SelectedBackground);
            DgForeground.UpdateLayout();
            DgForeground.ScrollIntoView(SelectedForeground);
        }
    }
}