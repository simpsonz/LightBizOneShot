namespace BizOneShot.Light.Util.Helper
{
    public class BizHelper
    {
        public string getMentorAreaNm(string mentorAreaCd)
        {
            var mentorAreaNm = string.Empty;
            switch (mentorAreaCd.Trim())
            {
                case "F":
                    mentorAreaNm = "자금";
                    break;
                case "D":
                    mentorAreaNm = "기술개발";
                    break;
                case "P":
                    mentorAreaNm = "특허";
                    break;
                case "M":
                    mentorAreaNm = "마케팅";
                    break;
                case "L":
                    mentorAreaNm = "법무";
                    break;
                case "T":
                    mentorAreaNm = "세무/회계";
                    break;
                case "W":
                    mentorAreaNm = "노무";
                    break;
                case "E":
                    mentorAreaNm = "기타";
                    break;
            }
            return mentorAreaNm;
        }
    }
}