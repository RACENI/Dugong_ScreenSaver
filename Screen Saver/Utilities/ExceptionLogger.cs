using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen_Saver.Utilities
{
    internal class ExceptionLogger
    {
        // 예외를 로그 파일에 기록하는 메서드
        public static void LogException(Exception ex)
        {
            try
            {
                // 현재 날짜와 시간을 포함한 로그 파일 이름 생성
                string logFileName = $"ExceptionLog_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

                // 프로그램 실행 파일이 있는 디렉토리에 로그 파일 경로 생성
                string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, logFileName);

                // 로그 파일에 예외 정보 기록
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine($"[DateTime]: {DateTime.Now}");
                    writer.WriteLine($"[Exception Type]: {ex.GetType().FullName}");
                    writer.WriteLine($"[Message]: {ex.Message}");
                    writer.WriteLine($"[StackTrace]: {ex.StackTrace}");
                    writer.WriteLine();
                }
            }
            catch (Exception)
            {
                // 예외가 발생한 경우 여기서는 아무것도 하지 않음
                // 예외를 기록하는 과정에서 문제가 발생할 수 있으므로 이를 방지하기 위해 추가
            }
        }
    }
}
