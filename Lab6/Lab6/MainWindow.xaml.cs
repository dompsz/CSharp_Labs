using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;

namespace lab6
{
    public sealed partial class MainWindow : Window
    {
        private ObservableCollection<GlobalPosition> positions = new ObservableCollection<GlobalPosition>();
        private GlobalPosition selectedPosition;

        public MainWindow()
        {
            this.InitializeComponent();
            PositionsListView.ItemsSource = positions;
        }

        private void OnAddClicked(object sender, RoutedEventArgs e)
        {
            if (float.TryParse(LatitudeInput.Text, out float latitude) &&
                float.TryParse(LongitudeInput.Text, out float longitude) &&
                float.TryParse(AltitudeInput.Text, out float altitude))
            {
                var position = new GlobalPosition
                {
                    Latitude = latitude,
                    Longtitude = longitude,
                    Altitude = altitude
                };
                positions.Add(position);
                PositionsListView.ItemsSource = null;
                PositionsListView.ItemsSource = positions;
                ClearInputs();
            }
        }

        private void OnUpdateClicked(object sender, RoutedEventArgs e)
        {
            if (selectedPosition != null &&
                float.TryParse(LatitudeInput.Text, out float latitude) &&
                float.TryParse(LongitudeInput.Text, out float longitude) &&
                float.TryParse(AltitudeInput.Text, out float altitude))
            {
                selectedPosition.Latitude = latitude;
                selectedPosition.Longtitude = longitude;
                selectedPosition.Altitude = altitude;
                PositionsListView.ItemsSource = null;
                PositionsListView.ItemsSource = positions;
                ClearInputs();
            }
        }

        private void OnDeleteClicked(object sender, RoutedEventArgs e)
        {
            if (selectedPosition != null)
            {
                positions.Remove(selectedPosition);
                ClearInputs();
            }
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedPosition = (GlobalPosition)PositionsListView.SelectedItem;
            if (selectedPosition != null)
            {
                LatitudeInput.Text = selectedPosition.Latitude.ToString();
                LongitudeInput.Text = selectedPosition.Longtitude.ToString();
                AltitudeInput.Text = selectedPosition.Altitude.ToString();
            }
        }

        private void ClearInputs()
        {
            LatitudeInput.Text = string.Empty;
            LongitudeInput.Text = string.Empty;
            AltitudeInput.Text = string.Empty;
        }
    }
}