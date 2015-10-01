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
using BizOneShot.Light.Models.ViewModels;


namespace BizOneShot.Light.Util.Helper
{
    public class FileHelper
    {
        #region 업로드

        public bool hasImageFile(HttpPostedFileBase file)
        {
            string fileExtension = Path.GetExtension(file.FileName);

            return fileExtension.EndsWith("jpg") || fileExtension.EndsWith("jpeg") || fileExtension.EndsWith("gif")
                || fileExtension.EndsWith("png") || fileExtension.EndsWith("bmp");
        }

        public string  GetUploadFileName(HttpPostedFileBase file)
        {
          
            //FileContent savedFiles = new FileContent();
            string newFileName = string.Empty;
            try
            {
                string fileName, fileExtension;

             
                if (file != null && file.ContentLength > 0 && !string.IsNullOrEmpty(file.FileName))
                {
                    fileName = Path.GetFileName(file.FileName);
                    fileExtension = Path.GetExtension(fileName);
                    newFileName = string.Format("{0}-{1}{2}", Path.GetFileNameWithoutExtension(fileName), Guid.NewGuid().ToString(), fileExtension);
                }

                return newFileName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UploadFile(HttpPostedFileBase file, string subDirectoryPath, string savedFileName)
        {
            string rootFilePath = ConfigurationManager.AppSettings["RootFilePath"];
            string directoryPath = Path.Combine(rootFilePath, subDirectoryPath);

            try
            {
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);

                if (file != null && file.ContentLength > 0 && !string.IsNullOrEmpty(file.FileName))
                { 
                    await Task.Run(() => file.SaveAs(Path.Combine(directoryPath, savedFileName)));
                }
     
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<IList<FileContent>> UploadFile(IEnumerable<HttpPostedFileBase> files, FileType fileType)
        {
            string rootFilePath = ConfigurationManager.AppSettings["RootFilePath"];
            string newSubPath = Path.Combine(fileType.ToString(), DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());
            string directoryPath = Path.Combine(rootFilePath, newSubPath);

            IList<FileContent> savedFiles = new List<FileContent>();
            try
            {
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);

                string fileName, fileExtension, newFileName;

                foreach (var file in files)
                {
                    if (file != null && file.ContentLength > 0 && !string.IsNullOrEmpty(file.FileName))
                    {
                        fileName = Path.GetFileName(file.FileName);
                        fileExtension = Path.GetExtension(fileName);
                        newFileName = string.Format("{0}-{1}{2}", Path.GetFileNameWithoutExtension(fileName), Guid.NewGuid().ToString(), fileExtension);

                        savedFiles.Add(new FileContent { FileNm = fileName, FilePath = Path.Combine(newSubPath, newFileName) });

                        await Task.Run(() => file.SaveAs(Path.Combine(directoryPath, newFileName)));

                    }
                }

                return savedFiles;
            }
            catch (Exception ex)
            {
                throw ex;
            }
          
        }

        #endregion

        #region 다운로드
        public void DownloadFile(IList<FileContent> files, string archiveName)
        {
            string rootFilePath = ConfigurationManager.AppSettings["RootFilePath"];

            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;


            response.Clear();

            response.BufferOutput = false;

            //response.ContentType = GetContentType(archiveName);
            response.ContentType = "application / octet - stream";
            string encodedFileName = HttpContext.Current.Server.UrlEncode(archiveName).Replace("+", "%20");
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

        #endregion


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
}
