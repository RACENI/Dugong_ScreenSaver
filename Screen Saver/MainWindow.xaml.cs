using Microsoft.Win32;
using Screen_Saver.Managers;
using Screen_Saver.Utilities;
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
        private readonly ScreenSaverManager ScreenSaverManager;
        private readonly TrayManager TrayManager;
        private double width, height; // 크기조절을 위한 초기값

        #region Initialization Region
        public MainWindow()
        {
            InitializeComponent();
            // 폼 크기 초기화
            InitializeWindowSize();
            // 이미지 확장자 기본값 설정
            SetDefaultExtension();
            //객체 초기화
            ScreenSaverManager = new ScreenSaverManager();
            TrayManager = new TrayManager(this);
            HotKeyManager hotKeyManager = new HotKeyManager();

            //단축키 등록
            hotKeyManager.CreatHotKey(this, AccessModifierKeys.Control, Key.Q);

            //트레이 체크
            if (RegistryKeySetting.GetValue("tray") == "is")
            {
                TrayManager.TrayOpen();
                MessageBox.Show("트레이 모드로 실행됩니다.", Setting.cap);
            }
        }

        private void InitializeWindowSize()
        {
            width = window.Width;
            height = window.Height;
        }
        private void SetDefaultExtension()
        {
            if (RegistryKeySetting.GetValue("extension") == null)
            {
                RegistryKeySetting.SetValue("extension", "png");
            }
        }
        #endregion


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LockManager lockManager = new LockManager();
            switch(lockManager.LockScreen())
            {
                case LockManager.LockFlag.PASSWORD_ERROR:
                    MessageBox.Show("비밀번호를 설정해주십시오.", Setting.cap, MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }
        }

        #region MenuItem Click Method
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ImageManager imageManager = new ImageManager();
            ImageManager.ImageFlag imageFlag = ImageManager.ImageFlag.ERROR;

            MessageBoxResult messageResult = MessageBox.Show("기본 이미지로 변경하시겠습니까?\r\n※아니오를 선택하면 원하는 사진을 고를 수 있는 창이 나옵니다.", Setting.cap, MessageBoxButton.YesNoCancel);
            
            if (messageResult == MessageBoxResult.Yes)
                imageFlag = imageManager.SetDefaultImage();
            else if (messageResult == MessageBoxResult.No)
            {
                OpenFileDialog openDialog = new OpenFileDialog();
                openDialog.Filter = "사진 파일 (*.jpg, *.bmp, *.png, *wdp, *gif, *tif) | *.jpg; *.bmp; *.png; *.wdp; *.gif; *.tif;";

                if (openDialog.ShowDialog() == true)
                {
                    imageFlag = imageManager.OpenImageFileDialog(openDialog);
                }
            }
            else if (messageResult == MessageBoxResult.Cancel)
                return;


            switch (imageFlag)
            {
                case ImageManager.ImageFlag.ERROR:
                    MessageBox.Show("알 수 없는 오류.", Setting.cap);
                    break;
                case ImageManager.ImageFlag.COMPLETE:
                    MessageBox.Show("사진 변경이 완료되었습니다.", Setting.cap);
                    break;
                case ImageManager.ImageFlag.FORMAT_ERROR:
                    MessageBox.Show("지원되지 않는 형식입니다.\r\n파일 확장자를 소문자로 변경해주세요.", Setting.cap, MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case ImageManager.ImageFlag.CHANGE_ERROR:
                    MessageBox.Show($"사진 변경 중 오류가 발생했습니다.", Setting.cap, MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                case ImageManager.ImageFlag.DELETE_ERROR:
                    MessageBox.Show($"기존 이미지 파일 제거 중 오류가 발생했습니다.", Setting.cap, MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }
        }

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

        // 제작자
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("//제작자 : SARACEN//" +
                "\r\n블로그 - https://saracenletter.tistory.com" +
                "\r\n이메일 - saracen_dev@naver.com" +
                "\r\n\r\n//그림 : 문영민//" +
                "\r\n이메일 - dnwnskfk33@naver.com" +
                "\r\n\r\n본 프로그램은 저작권법과 국제 협약의 보호를 받습니다. 본 프로그램의 전부 또는 일부를 무단으로 복제, 배포하는 행위는 민사 및 형사법에 의해 엄격히 규제되어 있으며, 기소 사유가 됩니다." +
                "\r\nCopyright 2019. SARACEN All rights reserved.", Setting.cap, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // 버전 체크
        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            VersionManager versionManager = new VersionManager();

            switch (versionManager.VersionChecker())
            {
                case VersionManager.CheckFlag.NEW:
                    MessageBox.Show("최신 버전 입니다.", Setting.cap);
                    break;

                case VersionManager.CheckFlag.OLD:
                    if (MessageBox.Show("구 버전 입니다.\r\n\r\n확인을 누르면 다운로드 페이지로 이동합니다.", Setting.cap, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        Process.Start(Setting.downloadURL);
                    }
                    break;
                case VersionManager.CheckFlag.ERROR:
                    MessageBox.Show("인터넷 연결 상태를 확인하십시오.", Setting.cap, MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                default:
                    MessageBox.Show("알 수 없는 오류 발생.", Setting.cap, MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }
        }

        // 사용법 //
        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("사용법을 확인하시겠습니까?", Setting.cap, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    Process.Start(Setting.manualURL);
                }
                catch (Exception ex)
                {
                    ExceptionLogger.LogException(ex);
                    MessageBox.Show("인터넷 연결 상태를 확인하십시오.", Setting.cap, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // 환경 설정창 오픈 //
        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            Window configurationWindow = new Configuration();
            configurationWindow.ShowDialog();
        }

        //비밀번호 초기화//
        private void MenuItem_Click_6(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("비밀번호를 초기화 하시겠습니까?\r\n비밀번호와 관련된 모든 정보는 삭제됩니다.", Setting.cap, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    RegistryKeySetting.DeleteValue("PW");
                    RegistryKeySetting.DeleteValue("ddkl");
                    RegistryKeySetting.DeleteValue("dgkah");
                    MessageBox.Show("비밀번호 초기화 완료!", Setting.cap);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("오류가 발생했습니다!", Setting.cap, MessageBoxButton.OK, MessageBoxImage.Error);
                    ExceptionLogger.LogException(ex);
                }
            }
        }
        #endregion

        #region FormEvent Method
        // 폼 종료시 트레이에 남도록 설정! //
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (RegistryKeySetting.GetValue("tray") == "is")
            {
                int result = TrayManager.TrayOpen();
                switch (result)
                {
                    case 1:
                        e.Cancel = true;
                        break;
                    case 2:
                        MessageBox.Show("트레이 오류!", Setting.cap, MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                }
            }
        }

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
        #endregion
    }
}