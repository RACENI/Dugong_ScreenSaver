using Microsoft.Win32;
using Screen_Saver.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Screen_Saver.Managers
{
    internal class ImageManager
    {
        public enum ImageFlag { ERROR, COMPLETE, FORMAT_ERROR, CHANGE_ERROR, DELETE_ERROR, NONE_FILE }

        // 파일 대화상자를 통해 이미지 열기
        public ImageFlag OpenImageFileDialog(OpenFileDialog openFileDialog)
        {
            if (File.Exists(openFileDialog.FileName))
            {
                string selectedFile = openFileDialog.FileName;
                string extension = Path.GetExtension(selectedFile).ToLower(new System.Globalization.CultureInfo("en-US", false)); // 확장자 소문자로 변경

                // 파일 확장자에 따라 처리
                switch (extension)
                {
                    case ".jpg":
                    case ".bmp":
                    case ".png":
                    case ".wdp":
                    case ".gif":
                    case ".tif":
                        string destinationPath = $"image\\UI{extension}";
                        try
                        {
                            RegistryKeySetting.SetValue("extension", extension);
                            File.Copy(selectedFile, destinationPath, true);
                            return ImageFlag.COMPLETE;
                        }
                        catch (Exception ex)
                        {
                            ExceptionLogger.LogException(ex);
                            return ImageFlag.CHANGE_ERROR;
                        }
                    default:
                        return ImageFlag.FORMAT_ERROR;
                }

            }
            return ImageFlag.ERROR;
        }

        // 기본 이미지로 복원
        public ImageFlag SetDefaultImage()
        {
            try
            {
                File.Copy(@"image\UI_default.png", @"image\UI.png", true);
                RegistryKeySetting.SetValue("extension", ".png");
                return ImageFlag.COMPLETE;
            }
            catch (FileNotFoundException ex)
            {
                ExceptionLogger.LogException(ex);
                return ImageFlag.CHANGE_ERROR;
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex);
                return ImageFlag.ERROR;
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
    }
}
