using System.Windows;
using System.Windows.Input;

namespace Screen_Saver
{
    /// <summary>
    /// NPW.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class NPW : Window
    {
        public NPW()
        {
            InitializeComponent();
        }

        // 초기 비밀번호 설정 //
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (NNPW.Password.Length >= 3 && NNPW_Check.Password.Length <= 30)
            {
                if (NNPW.Password == NNPW_Check.Password)
                {
                    AESEncryptDecrypt aes = new AESEncryptDecrypt();

                    string pw = NNPW.Password;

                    RegistryKeySetting.SetValue("PW", aes.AESEncrypt(pw, aes.GetKey(), aes.GetIV()));

                    MessageBox.Show("초기 비밀번호 설정이 완료되었습니다." +
                        "\r\n\r\n설정된 비밀번호 : " + pw, fsetting.cap);

                    Close();
                }
                else
                {
                    MessageBox.Show("비밀번호가 다릅니다.", fsetting.cap, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("비밀번호는 3자 이상 30자이하 여야 합니다.", fsetting.cap, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void NPW__GotFocus(object sender, RoutedEventArgs e)
        {
            NPW_.Visibility = Visibility.Hidden;
            NNPW.Visibility = Visibility.Visible;
            NNPW.Focus();
        }

        private void NPW_Check_GotFocus(object sender, RoutedEventArgs e)
        {
            NPW_Check.Visibility = Visibility.Hidden;
            NNPW_Check.Visibility = Visibility.Visible;
            NNPW_Check.Focus();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                Button_Click(sender, e);
            }
        }
    }
}
