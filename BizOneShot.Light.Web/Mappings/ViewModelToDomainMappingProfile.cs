﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using AutoMapper;
using BizOneShot.Light.Web.ViewModels;
using BizOneShot.Light.Models;

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
                   .ForMember(d => d.FaqSn, map => map.MapFrom(s => s.FAQ_SN))
                   .ForMember(d => d.AnsTxt, map => map.MapFrom(s => s.ANS_TXT))
                   .ForMember(d => d.QstTxt, map => map.MapFrom(s => s.QST_TXT));
        }
    }
}