using Screen_Saver.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Screen_Saver.Managers
{
    internal class VersionManager
    {
        public enum CheckFlag { NEW, OLD, ERROR }
        public CheckFlag VersionChecker()
        {
            try
            {
                var req = WebRequest.CreateHttp(Setting.versionURL);

                using (var res = req.GetResponse())
                {
                    using (var stream = res.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            var remoteVersion = reader.ReadLine().TrimEnd();

                            return (Setting.version == remoteVersion) ? CheckFlag.NEW : CheckFlag.OLD;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex);
                return CheckFlag.ERROR;
            }
        }
    }
}
