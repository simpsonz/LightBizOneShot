using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using BizOneShot.Light.Models.ViewModels;
using Ionic.Zip;
using Microsoft.Win32;

namespace BizOneShot.Light.Util.Helper
{
    public class FileHelper
    {
        #region 다운로드

        public void DownloadFile(IList<FileContent> files, string archiveName)
        {
            var rootFilePath = ConfigurationManager.AppSettings["RootFilePath"];

            var response = HttpContext.Current.Response;


            response.Clear();

            response.BufferOutput = false;

            response.ContentType = GetContentType(archiveName);
            //response.ContentType = "application / octet - stream";
            var encodedFileName = HttpContext.Current.Server.UrlEncode(archiveName).Replace("+", "%20");
            response.AddHeader("Content-Disposition", "attachment; filename=" + encodedFileName);

            if (files.Count > 1)
            {
                using (var zipFile = new ZipFile(Encoding.UTF8))
                {
                    foreach (var file in files)
                    {
                        if (File.Exists(Path.Combine(rootFilePath, file.FilePath)))
                        {
                            var temps = File.ReadAllBytes(Path.Combine(rootFilePath, file.FilePath));
                         
                            zipFile.AddEntry(file.FileNm, temps);
                        }
                    }
                    if (zipFile.Count > 0)
                    {
                        zipFile.Save(response.OutputStream);
                    }
                   

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

            if (string.IsNullOrWhiteSpace(extension))
            {
                return null;
            }

            var registryKey = Registry.ClassesRoot.OpenSubKey(extension);

            if (registryKey == null)
            {
                return null;
            }

            var value = registryKey.GetValue("Content Type") as string;

            return string.IsNullOrWhiteSpace(value) ? null : value;
        }


        public async Task<string> GetPhotoString(string filePath)
        {
            var rootFilePath = ConfigurationManager.AppSettings["RootFilePath"];

            var fileFullPath = Path.Combine(rootFilePath, filePath);

            var img = new WebImage(File.ReadAllBytes(fileFullPath));
            if (img.Width > 300)
                img.Resize(300, 300);
            return await Task.Run(() => "data:image/png;base64," + Convert.ToBase64String(img.GetBytes()));
            //return await Task.Run(() => "data:image/png;base64," + Convert.ToBase64String(File.ReadAllBytes(fileFullPath)));
        }

        #region 업로드

        public bool HasImageFile(HttpPostedFileBase file)
        {
            var fileExtension = Path.GetExtension(file.FileName).ToUpper();

            return fileExtension.EndsWith("JPG") || fileExtension.EndsWith("JPEG") || fileExtension.EndsWith("GIF")
                   || fileExtension.EndsWith("PNG") || fileExtension.EndsWith("BMP");
        }

        public string GetUploadFileName(HttpPostedFileBase file)
        {
            //FileContent savedFiles = new FileContent();
            var newFileName = string.Empty;
            try
            {
                string fileName, fileExtension;


                if (file != null && file.ContentLength > 0 && !string.IsNullOrEmpty(file.FileName))
                {
                    fileName = Path.GetFileName(file.FileName);
                    fileExtension = Path.GetExtension(fileName);
                    newFileName = string.Format("{0}-{1}{2}", Path.GetFileNameWithoutExtension(fileName), Guid.NewGuid(),
                        fileExtension);
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
            var rootFilePath = ConfigurationManager.AppSettings["RootFilePath"];
            var directoryPath = Path.Combine(rootFilePath, subDirectoryPath);

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
            var rootFilePath = ConfigurationManager.AppSettings["RootFilePath"];
            var newSubPath = Path.Combine(fileType.ToString(), DateTime.Now.Year.ToString(),
                DateTime.Now.Month.ToString());
            var directoryPath = Path.Combine(rootFilePath, newSubPath);

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
                        newFileName = string.Format("{0}-{1}{2}", Path.GetFileNameWithoutExtension(fileName),
                            Guid.NewGuid(), fileExtension);

                        savedFiles.Add(new FileContent
                        {
                            FileNm = fileName,
                            FilePath = Path.Combine(newSubPath, newFileName)
                        });

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

        //    string rootFilePath = ConfigurationManager.AppSettings["RootFilePath"];
        //{

        //public string GetPhoteString(string filePath)

        //    string fileFullPath = Path.Combine(rootFilePath, filePath);

        //    return "data:image/png;base64," + Convert.ToBase64String(File.ReadAllBytes(fileFullPath));
        //}
    }
}