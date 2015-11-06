using System;

namespace BizOneShot.Light.Models.ViewModels
{
    internal class CommonViewModel
    {
    }

    public class FileContent
    {
        public int FileSn { get; set; }
        public string FileNm { get; set; }
        public string FilePath { get; set; }
        public string FileUrl { get; set; }
        public string FileBase64String { get; set; }
        public string FileType { get; set; }
        public string FileExtension { get; set; }
        public long FileSizeInbytes { get; set; }

        public long FileSizeInKb
        {
            get { return (long) Math.Ceiling((double) FileSizeInbytes/1024); }
        }
    }

    public enum FileType
    {
        Document, //자료(요청)
        Resume, //이력서
        Manual, //매뉴얼
        Mentoring_Report, //맨토링 일지
        Mentoring_Total //맨토링 종합일지
    }
}