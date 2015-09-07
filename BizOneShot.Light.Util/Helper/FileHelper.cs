using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using Microsoft.Win32;
using System.Web;
using Ionic.Zip;

namespace BizOneShot.Light.Util.Helper
{
    public class FileHelper
    {
        public async Task DownloadFileAsync(IList<FileContent> files, string archiveName)
        {
            await new TaskFactory().StartNew(
                () =>
                {
                    DownloadFile(files, archiveName);
                });
        }

        public void DownloadFile(IList<FileContent> files, string archiveName)
        {
            string rootFilePath = ConfigurationManager.AppSettings["RootFilePath"];

            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;

            response.Clear();

            response.BufferOutput = false;

            //response.ContentType = GetContentType(archiveName);
            response.ContentType = "application / octet - stream";
            string encodedFileName = HttpContext.Current.Server.UrlDecode(archiveName).Replace("+", "%20");
            response.AddHeader("Content-Disposition", "attachment; filename=" + encodedFileName);

            if (files.Count > 1)
            {
                using (ZipFile zipFile = new ZipFile())
                {
                    foreach (FileContent file in files)
                    {
                        if (File.Exists(Path.Combine(rootFilePath, file.FilePath)))
                        {
                            byte[] temps = File.ReadAllBytes(Path.Combine(rootFilePath, file.FilePath));
                            // 시스템의 기본 인코딩 타입으로 읽어서


                            byte[] b = System.Text.Encoding.Default.GetBytes(file.FileNm);
                            // IBM437로 변환해 준다.
                            string fileName = System.Text.Encoding.GetEncoding("IBM437").GetString(b);
                            zipFile.AddEntry(fileName, temps);
                        }
                    }
                    zipFile.Save(response.OutputStream);
                    response.Flush();
                }
            }
            else if (files.Count == 1)
            {
                if (File.Exists(Path.Combine(rootFilePath, files[0].FilePath)))
                {
                    response.TransmitFile(Path.Combine(rootFilePath, files[0].FilePath));
                }
                response.Flush();
            }

            response.End();

        }


        public string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName);

            if (String.IsNullOrWhiteSpace(extension))
            {
                return null;
            }

            var registryKey = Registry.ClassesRoot.OpenSubKey(extension);

            if (registryKey == null)
            {
                return null;
            }

            var value = registryKey.GetValue("Content Type") as string;

            return String.IsNullOrWhiteSpace(value) ? null : value;
        }

    }

    public class FileContent
    {
        public int FileSn { get; set; }
        public string FileNm { get; set; }
        public string FilePath { get; set; }
        public string FileUrl { get; set; }
        public string FileType { get; set; }
        public string FileExtension { get; set; }
    }
}
