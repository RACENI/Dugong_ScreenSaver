using System;
using System.Windows;
using System.Windows.Input;

namespace Screen_Saver
{
    /// <summary>
    /// LockingSolve.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LockingSolve : Window
    {
        private AESEncryptDecrypt aes = new AESEncryptDecrypt();

        public LockingSolve()
        {
            InitializeComponent();
        }

        // 비번 확인 후 프로그램 종료 //
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string text = PPW.Password;

            if (text == aes.AESDecrypt(Convert.FromBase64String(RegistryKeySetting.GetValue("PW")), aes.GetKey(), aes.GetIV()))
            {
                if (MessageBox.Show("Screen Saver를 종료하시겠습니까?", fsetting.cap, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var a = Application.Current.Windows;
                    for (int i = 1; i < (a.Count - 1); i++)
                    {
                        a[i].Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("비밀번호를 확인해주세요.", fsetting.cap, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // 창닫기 //
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void PW_GotFocus(object sender, RoutedEventArgs e)
        {
            PW.Visibility = Visibility.Hidden;
            PPW.Visibility = Visibility.Visible;
            PPW.Focus();
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
