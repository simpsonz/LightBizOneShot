using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using AutoMapper;
using BizOneShot.Light.Web.ViewModels;
using BizOneShot.Light.Models;

namespace BizOneShot.Light.Web.Mappings
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get
            {
                return "DomainToViewModelMapping";
            }
        }

        protected override void Configure()
        {
            //여기에 Object-To-Object 매핑정의를 생성(필요할때 계속 추가)
            Mapper.CreateMap<ScFaq, FaqViewModel>()
                   .ForMember(d => d.FAQ_SN, map => map.MapFrom(s => s.FaqSn))
                   .ForMember(d => d.ANS_TXT, map => map.MapFrom(s => s.AnsTxt))
                   .ForMember(d => d.QST_TXT, map => map.MapFrom(s => s.QstTxt));
        }
    }
}