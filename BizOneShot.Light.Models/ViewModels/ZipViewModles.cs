namespace BizOneShot.Light.Models.ViewModels
{
    public class SelectGunguListViewModel
    {
        public string GUNGU { get; set; }
    }

    public class SelectAddressByStreetSearchParamModel
    {
        public string SIDO { get; set; }
        public string GUNGU { get; set; }
        public string RD_NM { get; set; }
        public string MN_NO { get; set; }
        public string SUB_NO { get; set; }
    }

    public class SelectAddressByDongSearchParamModel
    {
        public string SIDO { get; set; }
        public string GUNGU { get; set; }
        public string DONG { get; set; }
        public string MN_NO { get; set; }
        public string SUB_NO { get; set; }
    }

    public class SelectAddressByBuildingSearchParamModel
    {
        public string SIDO { get; set; }
        public string GUNGU { get; set; }
        public string BLD_NM { get; set; }
    }

    public class SelectAddressListViewModel
    {
        public string SIDO { get; set; }
        public string ZIP_CD { get; set; }
        public string ROAD_NM_ADDR { get; set; }
        public string JIBUN_ADDR { get; set; }
    }
}