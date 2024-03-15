using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WpfAnimatedGif;

namespace Screen_Saver
{
    public partial class LockingWindow : Window
    {
        private DispatcherTimer clock_timer = new DispatcherTimer();
        private DispatcherTimer maple_timer;

        private TcpServer tcpServer;


        public LockingWindow(DispatcherTimer timer)
        {
            InitializeComponent();

            maple_timer = timer; // 메이플 타이머 인수로 받아와서 값 넣기

            clock.Content = "";

            //시계 표시 부분//

            if (RegistryKeySetting.GetValue("clock") != null)
            {
                clock_timer.Interval = TimeSpan.FromSeconds(1);
                clock_timer.Tick += new EventHandler(timer_Tick);
                clock_timer.Start();
                // 시계 추가 가즈앗!
            }

            if (RegistryKeySetting.GetValue("transparent") == null)
            {
                imageLoad(); // 이미지 나오게!
            }
            else
            {
                title.Opacity = 0.01;
                title.AllowsTransparency = true;
            }

            if(RegistryKeySetting.GetValue("maple") != null)
            {
                tcpServer = new TcpServer();
                tcpServer.start();
            }
        }

        // 로그인창 뜨게 끔 //
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window LockingSolve = new LockingSolve(this);
            LockingSolve.ShowDialog();
        }

        private void title_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            Window LockingSolve = new LockingSolve(this);
            LockingSolve.ShowDialog();
        }

        // Alt + F4 disabled //
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            tcpServer.Stop();
            clock_timer.Stop();
            maple_timer.Stop();
            // e.Cancel = true; //본 프로그램 배포시 주석 제거 요망
        }
        private void title_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.System && e.SystemKey == System.Windows.Input.Key.F4)
            {
                e.Handled = true;
            }
        }

        // 이미지를 불러오는 부분 //
        private void imageLoad()
        {
            try
            {
                string path = Path.GetFullPath("Image\\UI." + RegistryKeySetting.GetValue("extension"));
                Stream imageStreamSource = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);

                string extension = RegistryKeySetting.GetValue("extension");

                switch (extension)
                {
                    case "jpg":
                        JpegBitmapDecoder decoder = new JpegBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                        BitmapSource bitmapSource = decoder.Frames[0];

                        image.Source = bitmapSource;
                        break;

                    case "bmp":
                        BmpBitmapDecoder decoder2 = new BmpBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                        bitmapSource = decoder2.Frames[0];

                        image.Source = bitmapSource;

                        break;

                    case "png":
                        PngBitmapDecoder decoder3 = new PngBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                        bitmapSource = decoder3.Frames[0];

                        image.Source = bitmapSource;
                        break;

                    case "wdp":
                        WmpBitmapDecoder decoder4 = new WmpBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                        bitmapSource = decoder4.Frames[0];

                        image.Source = bitmapSource;
                        break;

                    case "gif":
                        var image2 = new BitmapImage();
                        image2.BeginInit();
                        image2.UriSource = new Uri(path);
                        image2.EndInit();

                        ImageBehavior.SetAnimatedSource(image, image2);
                        ImageBehavior.SetRepeatBehavior(image, RepeatBehavior.Forever);
                        break;

                    case "tif":
                        var decoder6 = new TiffBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                        bitmapSource = decoder6.Frames[0];

                        image.Source = bitmapSource;
                        break;
                }
            }
            catch
            {
                MessageBox.Show("이미지 불러오는 것에 실패했습니다.", fsetting.cap, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // 후킹 //
        protected override void OnSourceInitialized(EventArgs e)
        {
            InterceptKeys interceptKeys = new InterceptKeys();
            base.OnSourceInitialized(e);
            interceptKeys.hookID = _InterceptKeys.SetHook(interceptKeys.proc);
        }

        // 시각 //
        private void timer_Tick(object sender, EventArgs e)
        {
            clock.Content = DateTime.Now.ToString("yyyy년MM월dd일\r\nHH시mm분ss초");
        }


    }
}