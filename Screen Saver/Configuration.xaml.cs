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
        }

        // 트레이 is //
        private void Tray_Checked(object sender, RoutedEventArgs e)
        {
            RegistryKeySetting.SetValue("tray", "is"); // 레지스트리에 쓰기
        }

        // 트레이 False //
        private void Tray_Unchecked(object sender, RoutedEventArgs e)
        {
            RegistryKeySetting.DeleteValue("tray"); // 레지스트리에 삭제
        }

        // 시계 is //
        private void Clock_Checked(object sender, RoutedEventArgs e)
        {
            RegistryKeySetting.SetValue("clock", "is"); // 레지스트리에 쓰기
        }

        // 시계 False // 
        private void Clock_Unchecked(object sender, RoutedEventArgs e)
        {
            RegistryKeySetting.DeleteValue("clock"); // 레지스트리에 삭제
        }

        private void transparent_Checked(object sender, RoutedEventArgs e)
        {
            RegistryKeySetting.SetValue("transparent", "is"); // 레지스트리에 쓰기
        }

        private void transparent_Unchecked(object sender, RoutedEventArgs e)
        {
            RegistryKeySetting.DeleteValue("transparent"); // 레지스트리에 삭제
        }

        // 메이플 is //
        private void Maple_Checked(object sender, RoutedEventArgs e)
        {
            RegistryKeySetting.SetValue("maple", "is"); // 레지스트리에 쓰기
        }

        // 메이플 False //
        private void Maple_Unchecked(object sender, RoutedEventArgs e)
        {
            RegistryKeySetting.DeleteValue("maple"); // 레지스트리에 삭제
        }

        // 폼 로드 //
        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {

            if (RegistryKeySetting.GetValue("tray") == null)
                tray.IsChecked = false;
            else
                tray.IsChecked = true; // 트레이창 체크

            if (RegistryKeySetting.GetValue("clock") == null)
                clock.IsChecked = false;
            else
                clock.IsChecked = true; // 시계 체크

            if (RegistryKeySetting.GetValue("transparent") == null)
                transparent.IsChecked = false;
            else
                transparent.IsChecked = true; // 투명잠금화면 체크

            if (RegistryKeySetting.GetValue("maple") == null)
                maple.IsChecked = false;
            else
                maple.IsChecked = true; // 메이플 체크
        }
    }
}