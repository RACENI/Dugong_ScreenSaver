using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WpfAnimatedGif;

namespace Screen_Saver
{
    public partial class LockingWindow : Window
    {
        private DispatcherTimer clockTimer = new DispatcherTimer();
        private DispatcherTimer mapleTimer;
        //private TcpServer tcpServer;

        public LockingWindow(DispatcherTimer mapleTimer)
        {
            InitializeComponent();

            this.mapleTimer = mapleTimer; // 메이플 타이머 인수로 받아와서 값 넣기

            InitializeClock();
            InitializeImage();
            //InitializeTcpServer();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenLockingSolveWindow();
        }

        private void title_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            OpenLockingSolveWindow();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //tcpServer?.Stop();
            clockTimer?.Stop();
            mapleTimer?.Stop();
        }
        private void title_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.System && e.SystemKey == Key.F4)
            {
                e.Handled = true;
            }
        }

        private void InitializeClock()
        {
            if (RegistryKeySetting.GetValue("clock") != null)
            {
                clockTimer.Interval = TimeSpan.FromSeconds(1);
                clockTimer.Tick += Timer_Tick;
                clockTimer.Start();
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            clock.Content = DateTime.Now.ToString("yyyy년MM월dd일\r\nHH시mm분ss초");
        }

        private void InitializeImage()
        {
            if (RegistryKeySetting.GetValue("transparent") == null)
            {
                LoadImage();
            }
            else
            {
                title.Opacity = 0.01;
                title.AllowsTransparency = true;
            }
        }

        private void InitializeTcpServer()
        {
/*            if (RegistryKeySetting.GetValue("maple") != null)
            {
                tcpServer = new TcpServer();
                tcpServer.Start();
            }*/
        }

        // 이미지를 불러오는 부분 //
        private void LoadImage()
        {
            try
            {
                string extension = RegistryKeySetting.GetValue("extension");
                string path = Path.GetFullPath($"Image\\UI{extension}");

                using (new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {

                    switch (extension)
                    {
                        case ".jpg":
                        case ".bmp":
                        case ".png":
                        case ".wdp":
                        case ".tif":
                            BitmapDecoder decoder = BitmapDecoder.Create(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read), BitmapCreateOptions.None, BitmapCacheOption.Default);
                            image.Source = decoder.Frames[0];
                            break;


                        case ".gif":
                            var imageSource = new BitmapImage();
                            imageSource.BeginInit();
                            imageSource.UriSource = new Uri(path);
                            imageSource.EndInit();
                            ImageBehavior.SetAnimatedSource(image, imageSource);
                            ImageBehavior.SetRepeatBehavior(image, RepeatBehavior.Forever);
                            break;

                        default:
                            throw new NotSupportedException($"Extension '{extension}' is not supported.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"이미지 불러오는 것에 실패했습니다. \r\n지원되지 않는 형식 : {ex.Message}", Setting.cap, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenLockingSolveWindow()
        {
            LockingSolve lockingSolve = new LockingSolve(this);
            lockingSolve.ShowDialog();
        }

        // 후킹 //
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            InterceptKeys interceptKeys = new InterceptKeys();
            interceptKeys.hookID = _InterceptKeys.SetHook(interceptKeys.proc);
        }
    }
}