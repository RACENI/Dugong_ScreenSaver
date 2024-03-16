using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;
using static Screen_Saver.HotKeyClass;
using System.Windows.Input;
using System.Diagnostics;
using System.Net;
using Screen_Saver.Utilities;


namespace Screen_Saver.Managers
{
    internal class ScreenSaverManager
    {
        ExceptionLogger ExceptionLogger = new ExceptionLogger();

        #region 잠금메서드
        public void LockScreen()
        {
            if (RegistryKeySetting.GetValue("PW") == null)
            {
                MessageBox.Show("비밀번호를 설정해주십시오.", fsetting.cap, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {

                string path_ui = Path.GetFullPath("Image\\UI" + RegistryKeySetting.GetValue("extension"));
                FileInfo fi = new FileInfo(path_ui); // UI
                if (fi.Exists)
                {
                    DispatcherTimer timer = null;

                    if (RegistryKeySetting.GetValue("maple") == "is")
                    {
                        timer = startTimer();
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
        #endregion

        #region 비밀번호 초기화
        public bool resetPassword()
        {
            try
            {
                RegistryKeySetting.DeleteValue("PW");
                RegistryKeySetting.DeleteValue("ddkl");
                RegistryKeySetting.DeleteValue("dgkah");
                return true;
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex);
                return false;
            }
        }
        #endregion

        #region 핫키 설정
        //핫키 세팅 
        public void windowHotKey(Window window, AccessModifierKeys keyA, Key keyB)
        {
            try
            {
                HotKeyClass hot = new HotKeyClass();

                hot.handler = new HotKeyHandeler(window);
                hot.handler.RegisterHotKey(keyA, keyB);
                hot.handler.HotKeyPressed += new EventHandler<HotKeyEventArgs>(handler_HotKeyPressed);
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex);
            }
        }

        // 핫키 이벤트핸들러
        private void handler_HotKeyPressed(object sender, HotKeyEventArgs e)
        {
            LockScreen();
        }
        #endregion

        #region 버전체크
        public int CheckVersion()
        {
            try
            {
                var req = WebRequest.CreateHttp(fsetting.versionURL);

                using (var res = req.GetResponse())
                {
                    using (var stream = res.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            var remoteVersion = reader.ReadLine().TrimEnd();

                            return (fsetting.version == remoteVersion) ? 1 : 2;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex);
                return 3;
            }
        }
        #endregion

        public void OpenPasswordSettiong()
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

        #region 이미지 처리
        public int RestoreImage(MessageBoxResult result)
        {
            if (result == MessageBoxResult.No)
            {
                return OpenImageFileDialog();
            }
            else if (result == MessageBoxResult.Yes)
            {
                return RestoreDefaultImage();
            }
            else
            {
                return 0;
            }
        }

        // 파일 대화상자를 통해 이미지 열기
        private int OpenImageFileDialog()
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "사진 파일 (*.jpg, *.bmp, *.png, *wdp, *gif, *tif) | *.jpg; *.bmp; *.png; *.wdp; *.gif; *.tif;";

            if (openDialog.ShowDialog() == true)
            {
                if (File.Exists(openDialog.FileName))
                {
                    string selectedFile = openDialog.FileName;
                    string extension = Path.GetExtension(selectedFile).ToLower();

                    // 파일 확장자에 따라 처리
                    switch (extension)
                    {
                        case ".jpg":
                        case ".bmp":
                        case ".png":
                        case ".wdp":
                        case ".gif":
                        case ".tif":
                            return ChangeImageExtension(selectedFile, extension);
                        default:
                            return 2;
                    }
                }
            }
            return 0;
        }

        // 선택된 이미지로 변경
        private int ChangeImageExtension(string filePath, string extension)
        {
            string destinationPath = $"image\\UI{extension}";

            // 기존 이미지 파일 제거
            bool checkDelete = DeleteExistingImageFiles();
            if (checkDelete)
            {
                // 이미지 확장자 및 복사
                try
                {
                    RegistryKeySetting.SetValue("extension", extension);
                    File.Copy(filePath, destinationPath, true);
                    return 1;
                }
                catch (Exception ex)
                {
                    ExceptionLogger.LogException(ex);
                    return 3;
                }
            }
            else
            {
                return 4;
            }
        }

        // 기존 이미지 파일 제거
        private bool DeleteExistingImageFiles()
        {
            try
            {
                string[] extensions = { ".jpg", ".bmp", ".png", ".wdp", ".gif", ".tif" };

                foreach (string extension in extensions)
                {
                    string filePath = $"image\\UI{extension}";
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex);
                return false;
            }
        }

        // 기본 이미지로 복원
        private int RestoreDefaultImage()
        {
            try
            {
                File.Copy(@"image\\UI_default.png", @"image\\UI.png", true);
                RegistryKeySetting.SetValue("extension", ".png");
                return 1;
            }
            catch (FileNotFoundException ex)
            {
                ExceptionLogger.LogException(ex);
                return 5;
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex);
                return 0;
            }
        }
        #endregion

        #region 타이머
        //여기도 새로 클래스 만들어야되긴 할듯 타이머 클래스
        private DispatcherTimer startTimer()
        {
            DispatcherTimer timer = new DispatcherTimer(); //객체생성
            timer.Interval = TimeSpan.FromMilliseconds(100); //시간간격 설정
            timer.Tick += new EventHandler(maple_timer); //이벤트 추가
            timer.Start(); //타이머 시작. 종료는 timer.Stop();
            return timer;
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
        #endregion






        //좌표지정 메서드
        /*        private int[] off_xy()
                {
                    return [10, 20];
                }*/
    }
}
