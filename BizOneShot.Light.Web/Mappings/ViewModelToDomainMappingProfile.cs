using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using AutoMapper;
using BizOneShot.Light.Models.ViewModels;
using BizOneShot.Light.Models.WebModels;
using BizOneShot.Light.Models.DareModels;

namespace BizOneShot.Light.Web.Mappings
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public override string ProfileName
        {
            get
            {
                return "ViewModelToDomainMapping";
            }
        }

        protected override void Configure()
        {
            //여기에 Object-To-Object 매핑정의를 생성(필요할때 계속 추가)
            Mapper.CreateMap<FaqViewModel,ScFaq>()
                   .ForMember(d => d.FaqSn, map => map.MapFrom(s => s.FaqSn))
                   .ForMember(d => d.AnsTxt, map => map.MapFrom(s => s.AnsTxt))
                   .ForMember(d => d.QstTxt, map => map.MapFrom(s => s.QstTxt));

            //회원가입모델 to 회원
            Mapper.CreateMap<JoinCompanyViewModel, ScUsr>()
                .ForMember(d => d.TelNo, map => map.MapFrom(s => s.TelNo1 + s.TelNo2 + s.TelNo3))
                .ForMember(d => d.MbNo, map => map.MapFrom(s => s.MbNo1 + s.MbNo2 + s.MbNo3))
                .ForMember(d => d.Email, map => map.MapFrom(s => s.Email1 + "@"+ s.Email2));

            //회원가입모델 to 회사
            Mapper.CreateMap<JoinCompanyViewModel, ScCompInfo>()
                .ForMember(d => d.TelNo, map => map.MapFrom(s => s.ComTelNo1 + s.ComTelNo2 + s.ComTelNo3))
                .ForMember(d => d.Addr1, map => map.MapFrom(s => s.ComAddr1))
                .ForMember(d => d.Addr2, map => map.MapFrom(s => s.ComAddr2))
                .ForMember(d => d.PostNo, map => map.MapFrom(s => s.ComPostNo))
                .ForMember(d => d.OwnNm, map => map.MapFrom(s => s.ComOwnNm))
                .ForMember(d => d.RegistrationNo, map => map.MapFrom(s => s.ComRegistrationNo));

            //회원가입모델 to 다래회원
            Mapper.CreateMap<JoinCompanyViewModel, SHUSER_SyUser>()
                .ForMember(d => d.IdUser, map => map.MapFrom(s => s.LoginId))
                .ForMember(d => d.MembBusnpersNo, map => map.MapFrom(s => s.ComRegistrationNo))
                .ForMember(d => d.NmUser, map => map.MapFrom(s => s.Name))
                .ForMember(d => d.Pwd, map => map.MapFrom(s => s.LoginPw));
        }
    }
}