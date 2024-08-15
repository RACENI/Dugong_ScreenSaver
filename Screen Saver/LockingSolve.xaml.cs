using Screen_Saver.Utilities;
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
        private LockingWindow lockingWindow;

        public LockingSolve(LockingWindow lockingWindow)
        {
            InitializeComponent();

            this.lockingWindow = lockingWindow;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AttemptProgramExit();
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
                AttemptProgramExit();
            }
        }

        private void AttemptProgramExit()
        {
            string enteredPassword = PPW.Password;
            string storedPassword = RegistryKeySetting.GetValue("PW");

            try
            {
                string plainPassword = aes.decryptAES(storedPassword);
                if (enteredPassword == plainPassword)
                {
                    if (MessageBox.Show("잠금해제 하시겠습니까?", Setting.cap, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        lockingWindow.Close();
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("비밀번호를 확인해주세요.", Setting.cap, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex);
                MessageBox.Show("알수없는 오류가 발생하였습니다.", Setting.cap, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
