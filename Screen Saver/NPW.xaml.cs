using Screen_Saver.Utilities;
using System;
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
            string password = NNPW.Password;
            string confirmPassword = NNPW_Check.Password;

            if (isPasswordValid(password, confirmPassword))
            {
                try
                {
                    AESEncryptDecrypt aes = new AESEncryptDecrypt();
                    string encryptedPassword = aes.encryptAES(password);

                    if (encryptedPassword != null)
                    {
                        RegistryKeySetting.SetValue("PW", encryptedPassword);

                        MessageBox.Show($"초기 비밀번호 설정이 완료되었습니다.\n\n설정된 비밀번호: {password}", Setting.cap);
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("비밀번호 설정에 실패했습니다.", Setting.cap, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch(Exception ex)
                {
                    ExceptionLogger.LogException(ex);
                    MessageBox.Show("비밀번호 설정에 실패했습니다.", Setting.cap, MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }
        }

        // 비밀번호 유효성 검사
        private bool isPasswordValid(string password, string confirmPassword)
        {
            if (password.Length < 3 || password.Length > 30)
            {
                MessageBox.Show("비밀번호는 3자 이상 30자 이하이어야 합니다.", Setting.cap, MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("비밀번호가 일치하지 않습니다.", Setting.cap, MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
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
