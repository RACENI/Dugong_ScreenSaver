using Screen_Saver.Utilities;
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

            try
            {
                //비밀번호 변경 실패 조건들
                if (newPassword.Length < 3 || newPassword.Length > 30)
                {
                    MessageBox.Show("비밀번호는 3자 이상 30자이하 여야 합니다.", Setting.cap, MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                string decryptedPassword = aes.decryptAES(RegistryKeySetting.GetValue("PW"));
                if (decryptedPassword != null && decryptedPassword != currentPassword)
                {
                    MessageBox.Show("현재 비밀번호가 틀렸습니다.", Setting.cap, MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (newPassword != newPasswordCheck)
                {
                    MessageBox.Show("변경할 비밀번호를 다시 입력해 주십시오.", Setting.cap, MessageBoxButton.OK, MessageBoxImage.Error);
                }

                string encryptPass = aes.encryptAES(newPassword);
                if (encryptPass != null) // 빈 문자열은 비밀번호로 설정 불가능(double check)
                {
                    RegistryKeySetting.SetValue("PW", encryptPass);
                    MessageBox.Show("비밀번호 변경이 완료되었습니다." +
                        "\r\n\r\n변경된 비밀변호 : " + newPassword, Setting.cap, MessageBoxButton.OK);
                    Close();
                }
                else
                {
                    MessageBox.Show("비밀번호 암호화 중 오류가 발생했습니다.", Setting.cap, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch(Exception ex)
            {
                ExceptionLogger.LogException(ex);
                MessageBox.Show("비밀번호 암호화 중 오류가 발생했습니다.", Setting.cap, MessageBoxButton.OK, MessageBoxImage.Error);
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
