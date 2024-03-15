using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using static Screen_Saver.HotKeyClass;

namespace Screen_Saver
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


            width = window.Width;
            height = window.Height;

            if (RegistryKeySetting.GetValue("extension") == null)
            {
                RegistryKeySetting.SetValue("extension", "png");
            }

            /*            DispatcherTimer timer = new DispatcherTimer();    //객체생성
                        timer.Interval = TimeSpan.FromMilliseconds(1000 * 5);    //시간간격 설정
                        timer.Tick += new EventHandler(timer_Tick);          //이벤트 추가
                        timer.Start();                                       //타이머 시작. 종료는 timer.Stop(); 으로 한다*/
        }




        // 화면 잠그기 버튼 //
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Locking();
        }

        //Menu Item/
        // 사진 변경(jpg, png, bmp, gif 지원) //
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult a = MessageBox.Show("기본 이미지로 변경하시겠습니까?\r\n※아니오를 선택하면 원하는 사진을 고를 수 있는 창이 나옵니다.", fsetting.cap, MessageBoxButton.YesNoCancel);
            if (a == MessageBoxResult.No)
            {
                OpenFileDialog openDialog = new OpenFileDialog();
                openDialog.Filter = "사진 파일 (*.jpg, *.bmp, *.png, *wdp, *gif, *tif) | *.jpg; *.bmp; *.png; *.wdp; *.gif; *.tif;";
                if (openDialog.ShowDialog() == true)
                {
                    if (File.Exists(openDialog.FileName))
                    {
                        string First = @openDialog.FileName;
                        string check = Path.GetExtension(First);

                        try
                        {
                            // 파일 확장자 체크 //
                            switch (check.ToLower()) //check 모두 소문자로 변경
                            {
                                case ".jpg":
                                    string Second = @"image\\UI.jpg";

                                    usingpicturedel(); // 기존 사진 파일 제거

                                    RegistryKeySetting.SetValue("extension", "jpg");
                                    File.Copy(First, Second, true);
                                    MessageBox.Show("사진 변경이 완료 되었습니다.", fsetting.cap);
                                    break;

                                case ".bmp":
                                    Second = @"image\\UI.bmp";

                                    usingpicturedel(); // 기존 사진 파일 제거

                                    RegistryKeySetting.SetValue("extension", "bmp");
                                    File.Copy(First, Second, true);
                                    MessageBox.Show("사진 변경이 완료 되었습니다.", fsetting.cap);
                                    break;

                                case ".png":

                                    Second = @"image\\UI.png";

                                    usingpicturedel(); // 기존 사진 파일 제거

                                    RegistryKeySetting.SetValue("extension", "png");
                                    File.Copy(First, Second, true);
                                    MessageBox.Show("사진 변경이 완료 되었습니다.", fsetting.cap);
                                    break;


                                case ".wdp":

                                    Second = @"image\\UI.wdp";

                                    usingpicturedel(); // 기존 사진 파일 제거

                                    RegistryKeySetting.SetValue("extension", "wdp");
                                    File.Copy(First, Second, true);
                                    MessageBox.Show("사진 변경이 완료 되었습니다.", fsetting.cap);
                                    break;

                                case ".gif":
                                    Second = @"image\\UI.gif";

                                    usingpicturedel(); // 기존 사진 파일 제거

                                    RegistryKeySetting.SetValue("extension", "gif");
                                    File.Copy(First, Second, true);
                                    MessageBox.Show("사진 변경이 완료 되었습니다.", fsetting.cap);
                                    break;


                                case ".tif":
                                    Second = @"image\\UI.tif";

                                    usingpicturedel(); // 기존 사진 파일 제거

                                    RegistryKeySetting.SetValue("extension", "tif");
                                    File.Copy(First, Second, true);
                                    MessageBox.Show("사진 변경이 완료 되었습니다.", fsetting.cap);
                                    break;

                                default:
                                    MessageBox.Show("이미지를 변경하는 데 오류가 발생했습니다." +
                                        "\r\n파일 확장자를 소문자로 변경해주십시오.", fsetting.cap, MessageBoxButton.OK, MessageBoxImage.Error);
                                    break;
                            }
                        }
                        catch
                        {
                            MessageBox.Show("창이 열려 있지 않은지 확인하십시오.", fsetting.cap, MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            else if (a == MessageBoxResult.Yes)
            {
                try
                {
                    File.Copy(@"image\\UI_default.png", @"image\\UI.png", true);
                    RegistryKeySetting.SetValue("extension", "png");
                    MessageBox.Show("사진 변경이 완료 되었습니다.", fsetting.cap);
                }
                catch (FileNotFoundException)
                {
                    MessageBox.Show("파일을 찾을 수 없습니다!", fsetting.cap, MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch
                {
                    MessageBox.Show("알 수 없는 오류 발생!", fsetting.cap, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // 비밀번호 설정창 오픈 //
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            if (RegistryKeySetting.GetValue("PW") == null)
            {
                Window NPW = new NPW();
                NPW.ShowDialog();
            }
            else
            {
                Window APW = new APW();
                APW.ShowDialog();
            }
        }

        // 제작자 //
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("//제작자 : SARACEN//" +
                "\r\n블로그 - https://saracenletter.tistory.com" +
                "\r\n이메일 - saracen_dev@naver.com" +
                "\r\n\r\n//그림 : 문영민//" +
                "\r\n이메일 - dnwnskfk33@naver.com" +
                "\r\n\r\n본 프로그램은 저작권법과 국제 협약의 보호를 받습니다. 본 프로그램의 전부 또는 일부를 무단으로 복제, 배포하는 행위는 민사 및 형사법에 의해 엄격히 규제되어 있으며, 기소 사유가 됩니다." +
                "\r\nCopyright 2019. SARACEN All rights reserved.", fsetting.cap, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // 버전 체크 //
        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                string NowVersion = "2.0";

                var req = WebRequest.CreateHttp("http://screen.uy.to/");

                using (var res = req.GetResponse())
                {
                    using (var stream = res.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            var remoteVersion = reader.ReadLine().TrimEnd();

                            if (NowVersion == remoteVersion)
                            {
                                MessageBox.Show("최신 버전 입니다."
                                    + "\r\n현재버전 : " + NowVersion
                                    + "\r\n최신버전 : " + remoteVersion, fsetting.cap);
                            }
                            else
                            {
                                if (MessageBox.Show("구 버전 입니다."
                                     + "\r\n현재버전 : " + NowVersion
                                     + "\r\n최신버전 : " + remoteVersion
                                     + "\r\n\r\n확인을 누르면 다운로드 페이지로 이동합니다.", fsetting.cap, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                                {
                                    Process.Start("https://saracenletter.tistory.com/214");
                                }
                            }

                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("인터넷 연결 상태를 확인하십시오.", fsetting.cap, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // 사용법 //
        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("사용법을 확인하시겠습니까?", fsetting.cap, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    Process.Start("https://saracenletter.tistory.com/214");
                }
                catch
                {
                    MessageBox.Show("인터넷 연결 상태를 확인하십시오.", fsetting.cap, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // 환경 설정창 오픈 //
        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            Window Con = new Configuration();
            Con.ShowDialog();
        }

        //비밀번호 초기화//
        private void MenuItem_Click_6(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("비밀번호를 초기화 하시겠습니까?\r\n비밀번호와 관련된 모든 정보는 삭제됩니다.", fsetting.cap, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    RegistryKeySetting.DeleteValue("PW");
                    RegistryKeySetting.DeleteValue("ddkl");
                    RegistryKeySetting.DeleteValue("dgkah");
                    MessageBox.Show("비밀번호 초기화 완료!", fsetting.cap);
                }
                catch
                {
                    MessageBox.Show("알 수 없는 오류 발생!", fsetting.cap, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // 폼 로드시 트레이 체크 및 단축키 설정 //
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            HotKeyClass hot = new HotKeyClass();

            hot.handler = new HotKeyHandeler(this);
            hot.handler.RegisterHotKey(AccessModifierKeys.Control, Key.Q);
            hot.handler.HotKeyPressed += new EventHandler<HotKeyEventArgs>(handler_HotKeyPressed);

            if (RegistryKeySetting.GetValue("tray") == "is")
            {
                try
                {
                    tray_setting();
                    Close(); // 시작시 창 닫음 (아이콘만 띄우기 위함)
                    MessageBox.Show("트레이 모드로 실행됩니다.", fsetting.cap);
                }
                catch (Exception ee)
                {
                    MessageBox.Show("트레이 이동중 에러 발생\n" + ee, fsetting.cap, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // 폼 종료시 트레이에 남도록 설정! //
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (RegistryKeySetting.GetValue("tray") == "is")
            {
                try
                {
                    if (notify == null) tray_setting();
                    notify.Visible = true;   // 트레이아이콘 보이기
                    e.Cancel = true;
                    Hide();
                    base.OnClosing(e);
                }
                catch
                {
                    MessageBox.Show("트레이 오류!\n" + e, fsetting.cap, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        //MainWindow 관련 메서드들//
        // 화면 잠그기 설정 //
        private void Locking()
        {
            if (RegistryKeySetting.GetValue("PW") == null)
            {
                MessageBox.Show("비밀번호를 설정해주십시오.", fsetting.cap, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {

                string path_ui = Path.GetFullPath("Image\\UI." + RegistryKeySetting.GetValue("extension"));
                FileInfo fi = new FileInfo(path_ui); // UI
                if (fi.Exists)
                {
                    DispatcherTimer timer = new DispatcherTimer();    //객체생성
                    timer.Interval = TimeSpan.FromMilliseconds(100);    //시간간격 설정
                    timer.Tick += new EventHandler(maple_timer);          //이벤트 추가
                    if (RegistryKeySetting.GetValue("maple") == "is")
                    {
                        timer.Start();                                       //타이머 시작. 종료는 timer.Stop();
                    }

                    Window LW = new LockingWindow(timer); // 폼 종료시 타이머 멈출 수 있게 객체로 던짐
                    LW.ShowDialog();

                }
                else
                {
                    MessageBox.Show("이미지 파일이 없습니다.\r\n사진 변경을 통해 추가해주십시오.", fsetting.cap, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        //게임 확인 타이머 메서드
        private void maple_timer(object sender, EventArgs e)
        {

            bool mapleon = false;
            Process[] ProcessList = Process.GetProcessesByName("MapleStory");
            if (mapleon == false && ProcessList.Length > 0)
            {
                mapleon = true;
            }
            else if (mapleon == true && ProcessList.Length < 1)
            {
                //Process.Start("shutdown.exe", "-s -t 5");

            }
        }

        //컴퓨터 종료 타이머 메서드
        private void off_timer(object sender, EventArgs e)
        {
            
        }

        //좌표지정 메서드
/*        private int[] off_xy()
        {
            return [10, 20];
        }*/

        // 트레이 선언 //
        public System.Windows.Forms.NotifyIcon notify;
        // 트레이 세팅 //
        private void tray_setting()
        {
            System.Windows.Forms.ContextMenu menu = new System.Windows.Forms.ContextMenu();
            // 아이콘 설정부분
            notify = new System.Windows.Forms.NotifyIcon();
            notify.Icon = new System.Drawing.Icon(@"icon.ico");   // Resources 아이콘 사용 시 // 외부아이콘 사용 시
                                                                  //new System.Drawing.Icon(@"icon.ico");
                                                                  //notify.Icon = Properties.Resources.TiimeAlram;   // Resources 아이콘 사용 시
            notify.ContextMenu = menu;
            notify.Text = fsetting.cap;

            // 아이콘 더블클릭 이벤트 설정
            notify.DoubleClick += Notify_DoubleClick;

            System.Windows.Forms.MenuItem item1 = new System.Windows.Forms.MenuItem();
            menu.MenuItems.Add(item1);
            item1.Index = 0;
            item1.Text = "프로그램 종료";
            item1.Click += delegate (object click, EventArgs eClick)
            {
                Application.Current.Shutdown();
                notify.Dispose();
            };

            System.Windows.Forms.MenuItem item2 = new System.Windows.Forms.MenuItem();
            menu.MenuItems.Add(item2);
            item2.Index = 0;
            item2.Text = "화면 잠금";
            item2.Click += delegate (object click, EventArgs eClick)
            {
                Locking();
            };

            System.Windows.Forms.MenuItem item3 = new System.Windows.Forms.MenuItem();
            menu.MenuItems.Add(item3);
            item3.Index = 0;
            item3.Text = "열기";
            item3.Click += delegate (object click, EventArgs eClick)
            {
                Show();
                WindowState = WindowState.Normal;
                Visibility = Visibility.Visible;
                notify.Visible = false;   // 트레이아이콘 숨기기
            };
        }
        // 트레이 더블클릭 //
        private void Notify_DoubleClick(object sender, EventArgs e)
        {
            Show();
            WindowState = WindowState.Normal;
            Visibility = Visibility.Visible;
            notify.Visible = false; // 트레이아이콘 숨기기
        }

        //핫키 세팅부(ctrl+L)// 
        private void handler_HotKeyPressed(object sender, HotKeyEventArgs e)
        {
            if (e.Key == Key.Q)
                Locking();
        }

        //기존 사진 파일 제거//
        private void usingpicturedel()
        {
            try
            {
                FileInfo fileDel = new FileInfo(@"image\\UI.jpg");
                if (fileDel.Exists) //삭제할 파일이 있는지
                {
                    fileDel.Delete();
                }
                fileDel = new FileInfo(@"image\\UI.bmp");
                if (fileDel.Exists) //삭제할 파일이 있는지
                {
                    fileDel.Delete();
                }
                fileDel = new FileInfo(@"image\\UI.png");
                if (fileDel.Exists) //삭제할 파일이 있는지
                {
                    fileDel.Delete();
                }
                fileDel = new FileInfo(@"image\\UI.wdp");
                if (fileDel.Exists) //삭제할 파일이 있는지
                {
                    fileDel.Delete();
                }
                fileDel = new FileInfo(@"image\\UI.gif");
                if (fileDel.Exists) //삭제할 파일이 있는지
                {
                    fileDel.Delete();
                }
                fileDel = new FileInfo(@"image\\UI.tif");
                if (fileDel.Exists) //삭제할 파일이 있는지
                {
                    fileDel.Delete();
                }
            }
            catch
            {
                MessageBox.Show("기존 사진 파일 제거 과정에서 오류 발생!", fsetting.cap, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private double width, height; // 크기조절을 위한 초기값
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //윈도우가 줄어드는 비율로 컨텐츠 비율 수정

            button.Margin = new Thickness(e.NewSize.Width / width * button.Margin.Left, e.NewSize.Height / height * button.Margin.Top, 0, 0);
            button.Width *= e.NewSize.Width / width;
            button.Height *= e.NewSize.Height / height;

            image.Width *= e.NewSize.Width / width;
            image.Height *= e.NewSize.Height / height;

            menu.Width *= e.NewSize.Width / width;
            menu.Height *= e.NewSize.Height / height;

            fmenuitem.Width *= e.NewSize.Width / width;
            fmenuitem.Height *= e.NewSize.Height / height;

            smenuitem.Width *= e.NewSize.Width / width;
            smenuitem.Height *= e.NewSize.Height / height;

            width = e.NewSize.Width;
            height = e.NewSize.Height;
        }
    }
}