using System;
using System.Windows;
using System.Windows.Input;

namespace Screen_Saver
{
    /// <summary>
    /// APW.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class APW : Window
    {
        public APW()
        {
            InitializeComponent();
        }

        private AESEncryptDecrypt aes = new AESEncryptDecrypt();

        // 비밀번호 변경 //
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string currentPassword = AAPW.Password;
            string newPassword = NNPW.Password;
            string newPasswordCheck = NNPW_Check.Password;


            if (newPassword.Length >= 3 && newPassword.Length <= 30)
            {
                string decryptedPassword = aes.AESDecrypt(Convert.FromBase64String(RegistryKeySetting.GetValue("PW")), aes.GetKey(), aes.GetIV());

                if (decryptedPassword == currentPassword)
                {
                    if (newPassword == newPasswordCheck)
                    {
                        aes.SetKey();
                        aes.SetIV();
                        RegistryKeySetting.SetValue("PW", aes.AESEncrypt(newPassword, aes.GetKey(), aes.GetIV()));
                        MessageBox.Show("비밀번호 변경이 완료되었습니다." +
                            "\r\n\r\n변경된 비밀변호 : " + newPassword, fsetting.cap, MessageBoxButton.OK);
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("변경할 비밀번호를 다시 입력해 주십시오.", fsetting.cap, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("현재 비밀번호가 틀렸습니다.", fsetting.cap, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("비밀번호는 3자 이상 30자이하 여야 합니다.", fsetting.cap, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void APW__GotFocus(object sender, RoutedEventArgs e)
        {
            APW_.Visibility = Visibility.Hidden;
            AAPW.Visibility = Visibility.Visible;
            AAPW.Focus();
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
