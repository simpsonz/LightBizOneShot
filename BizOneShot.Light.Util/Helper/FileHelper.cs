﻿using System;
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
        #region 업로드

        public async Task<IList<FileContent>> UploadFile(IEnumerable<HttpPostedFileBase> files, FileType fileType, string subPath)
        {
            string rootFilePath = ConfigurationManager.AppSettings["RootFilePath"];
            string newSubPath = Path.Combine(fileType.ToString(), subPath ?? "");
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
                        newFileName = string.Format("{0}-{1}{2}", Path.GetFileNameWithoutExtension(fileName), Guid.NewGuid().ToByteArray(), fileExtension);

                        savedFiles.Add(new FileContent { FileNm = fileName, FilePath = Path.Combine(newSubPath, newFileName) });

                        await Task.Run(() => file.SaveAs(Path.Combine(directoryPath, newFileName)));

                    }
                }

                //await Task.Run(
                //    () =>
                //    {
                //        foreach (var file in files)
                //        {
                //            if (file != null && file.ContentLength > 0 && !string.IsNullOrEmpty(file.FileName))
                //            {
                //                fileName = Path.GetFileName(file.FileName);
                //                fileExtension = Path.GetExtension(fileName);
                //                newFileName = string.Format("{0}-{1}{2}", Path.GetFileNameWithoutExtension(fileName), Guid.NewGuid().ToByteArray(), fileExtension);

                //                savedFiles.Add(new FileContent { FileNm = fileName, FilePath = Path.Combine(newSubPath, newFileName) });

                //                file.SaveAs(Path.Combine(directoryPath, newFileName));
                //            }
                //        }
                //    });


                return savedFiles;
            }
            catch (Exception ex)
            {
                throw ex;
            }
          
        }

        #endregion

        #region 다운로드
        //public async Task DownloadFileAsync(IList<FileContent> files, string archiveName)
        //{
        //    await new TaskFactory().StartNew(
        //        () =>
        //        {
        //            DownloadFile(files, archiveName);
        //        });
        //}

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

    public class FileContent
    {
        public int FileSn { get; set; }
        public string FileNm { get; set; }
        public string FilePath { get; set; }
        public string FileUrl { get; set; }
        public string FileType { get; set; }
        public string FileExtension { get; set; }
        public long FileSizeInbytes { get; set; }
        public long FileSizeInKb { get { return (long)Math.Ceiling((double)FileSizeInbytes / 1024);  } }
    }

    public enum FileType
    {
        Document,   //자료(요청)
        Resume,     //이력서
        Manual      //매뉴얼
    }

}
