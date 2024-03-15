using System.Windows;

namespace Screen_Saver
{
    /// <summary>
    /// Configuration.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Configuration : Window
    {
        public Configuration()
        {
            InitializeComponent();
            LoadRegistrySettings();

        }

        private void LoadRegistrySettings()
        {
            tray.IsChecked = RegistryKeySetting.GetValue("tray") != null;
            clock.IsChecked = RegistryKeySetting.GetValue("clock") != null;
            transparent.IsChecked = RegistryKeySetting.GetValue("transparent") != null;
            maple.IsChecked = RegistryKeySetting.GetValue("maple") != null;
        }

        private void UpdateRegistrySetting(string key, bool isChecked)
        {
            if (isChecked)
                RegistryKeySetting.SetValue(key, "is");
            else
                RegistryKeySetting.DeleteValue(key);
        }

        // 트레이 is //
        private void Tray_Checked(object sender, RoutedEventArgs e)
        {
            UpdateRegistrySetting("tray", true);
        }

        // 트레이 False //
        private void Tray_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateRegistrySetting("tray", false);
        }

        // 시계 is //
        private void Clock_Checked(object sender, RoutedEventArgs e)
        {
            UpdateRegistrySetting("clock", true);
        }

        // 시계 False // 
        private void Clock_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateRegistrySetting("clock", false);
        }

        private void transparent_Checked(object sender, RoutedEventArgs e)
        {
            UpdateRegistrySetting("transparent", true);
        }

        private void transparent_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateRegistrySetting("transparent", false); // 레지스트리에 삭제
        }

        // 메이플 is //
        private void Maple_Checked(object sender, RoutedEventArgs e)
        {
            UpdateRegistrySetting("maple", true); // 레지스트리에 쓰기
        }

        // 메이플 False //
        private void Maple_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateRegistrySetting("maple", false); // 레지스트리에 삭제
        }
    }
}