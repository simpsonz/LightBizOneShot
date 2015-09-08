using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using AutoMapper;
using BizOneShot.Light.Models.ViewModels;
using BizOneShot.Light.Models.WebModels;

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
                   .ForMember(d => d.QclNm, map => map.MapFrom(s => s.ScQcl.QclNm));

            //공지사항 Notice 매핑
            Mapper.CreateMap<ScNtc, NoticeViewModel>();

            Mapper.CreateMap<ScNtc, NoticeDetailViewModel>()
                .ForMember(d => d.PreNoticeSn, map => map.UseValue(0))
                .ForMember(d => d.NextNoticeSn, map => map.UseValue(0));

            //매뉴얼 Manual 매핑
            Mapper.CreateMap<ScForm, ManualViewModel>();

            //Company MyInfo 매핑
            Mapper.CreateMap<ScUsr, CompanyMyInfoViewModel>()
                .ForMember(d => d.ResumeName, map => map.MapFrom(s => s.ScUsrResume.ScFileInfo.FileNm))
                .ForMember(d => d.ResumeName, map => map.MapFrom(s => s.ScUsrResume.ScFileInfo.FilePath))
                .ForMember(d => d.ComAddr, map => map.MapFrom(s => s.ScCompInfo.PostNo + " " + s.ScCompInfo.Addr1 + " " + s.ScCompInfo.Addr2))
                .ForMember(d => d.ComOwnNm, map => map.MapFrom(s => s.ScCompInfo.OwnNm))
                .ForMember(d => d.CompNm, map => map.MapFrom(s => s.ScCompInfo.CompNm))
                .ForMember(d => d.ComRegistrationNo, map => map.MapFrom(s => s.ScCompInfo.RegistrationNo))
                .ForMember(d => d.ComTelNo, map => map.MapFrom(s => s.ScCompInfo.TelNo));

            //회원가입 매핑
            Mapper.CreateMap<ScUsr, JoinCompanyViewModel>()
                .ForMember(d => d.TelNo1, map => map.MapFrom(s => s.TelNo.Split('-').GetValue(0).ToString()))
                .ForMember(d => d.TelNo2, map => map.MapFrom(s => s.TelNo.Split('-').GetValue(1).ToString()))
                .ForMember(d => d.TelNo3, map => map.MapFrom(s => s.TelNo.Split('-').GetValue(2).ToString()))
                .ForMember(d => d.FaxNo1, map => map.MapFrom(s => s.FaxNo.Split('-').GetValue(0).ToString()))
                .ForMember(d => d.FaxNo2, map => map.MapFrom(s => s.FaxNo.Split('-').GetValue(1).ToString()))
                .ForMember(d => d.FaxNo3, map => map.MapFrom(s => s.FaxNo.Split('-').GetValue(2).ToString()))
                .ForMember(d => d.MbNo1, map => map.MapFrom(s => s.MbNo.Split('-').GetValue(0).ToString()))
                .ForMember(d => d.MbNo2, map => map.MapFrom(s => s.MbNo.Split('-').GetValue(1).ToString()))
                .ForMember(d => d.MbNo3, map => map.MapFrom(s => s.MbNo.Split('-').GetValue(2).ToString()))
                .ForMember(d => d.Email1, map => map.MapFrom(s => s.Email.Split('@').GetValue(0).ToString()))
                .ForMember(d => d.Email2, map => map.MapFrom(s => s.Email.Split('@').GetValue(1).ToString()))
                .ForMember(d => d.CompNm, map => map.MapFrom(s => s.ScCompInfo.CompNm));

            //사업관리자 뷰 매핑
            Mapper.CreateMap<ScUsr, BizMngDropDownModel>()
                .ForMember(d => d.CompSn, map => map.MapFrom(s => s.ScCompInfo.CompSn))
                .ForMember(d => d.CompNm, map => map.MapFrom(s => s.ScCompInfo.CompNm));

            //사업 뷰 매핑
            Mapper.CreateMap<ScBizWork, BizWorkDropDownModel>();

            //전문가 회원 뷰 매핑
            Mapper.CreateMap<ScUsr, JoinExpertViewModel>()
                .ForMember(d => d.BizMagComName, map => map.MapFrom(s => s.ScExpertMappings.ElementAt(0).ScBizWork.ScCompInfo.CompNm))
                .ForMember(d => d.BizMngCompSn, map => map.MapFrom(s => s.ScExpertMappings.ElementAt(0).ScBizWork.ScCompInfo.CompSn))
                .ForMember(d => d.ResumeName, map => map.MapFrom(s => s.ScUsrResume.ScFileInfo.FileNm))
                .ForMember(d => d.ResumePath, map => map.MapFrom(s => s.ScUsrResume.ScFileInfo.FilePath))
                .ForMember(d => d.ComPostNo, map => map.MapFrom(s => s.ScCompInfo.PostNo))
                .ForMember(d => d.ComAddr1, map => map.MapFrom(s => s.ScCompInfo.Addr1))
                .ForMember(d => d.ComAddr2, map => map.MapFrom(s => s.ScCompInfo.Addr2))
                .ForMember(d => d.ComOwnNm, map => map.MapFrom(s => s.ScCompInfo.OwnNm))
                .ForMember(d => d.CompNm, map => map.MapFrom(s => s.ScCompInfo.CompNm))
                .ForMember(d => d.ComRegistrationNo, map => map.MapFrom(s => s.ScCompInfo.RegistrationNo))
                .ForMember(d => d.TelNo1, map => map.MapFrom(s => s.TelNo.Split('-').GetValue(0).ToString()))
                .ForMember(d => d.TelNo2, map => map.MapFrom(s => s.TelNo.Split('-').GetValue(1).ToString()))
                .ForMember(d => d.TelNo3, map => map.MapFrom(s => s.TelNo.Split('-').GetValue(2).ToString()))
                .ForMember(d => d.MbNo1, map => map.MapFrom(s => s.MbNo.Split('-').GetValue(0).ToString()))
                .ForMember(d => d.MbNo2, map => map.MapFrom(s => s.MbNo.Split('-').GetValue(1).ToString()))
                .ForMember(d => d.MbNo3, map => map.MapFrom(s => s.MbNo.Split('-').GetValue(2).ToString()))
                .ForMember(d => d.Email1, map => map.MapFrom(s => s.Email.Split('@').GetValue(0).ToString()))
                .ForMember(d => d.Email2, map => map.MapFrom(s => s.Email.Split('@').GetValue(1).ToString()));

            Mapper.CreateMap<ScExpertMapping, JoinExpertViewModel>()
                .ForMember(d => d.BizMagComName, map => map.MapFrom(s => s.ScBizWork.ScCompInfo.CompNm))
                .ForMember(d => d.BizMngCompSn, map => map.MapFrom(s => s.ScBizWork.ScCompInfo.CompSn))
                .ForMember(d => d.ResumeName, map => map.MapFrom(s => s.ScUsr.ScUsrResume.ScFileInfo.FileNm))
                .ForMember(d => d.ResumePath, map => map.MapFrom(s => s.ScUsr.ScUsrResume.ScFileInfo.FilePath))
                .ForMember(d => d.ComPostNo, map => map.MapFrom(s => s.ScBizWork.ScCompInfo.PostNo))
                .ForMember(d => d.ComAddr1, map => map.MapFrom(s => s.ScBizWork.ScCompInfo.Addr1))
                .ForMember(d => d.ComAddr2, map => map.MapFrom(s => s.ScBizWork.ScCompInfo.Addr2))
                .ForMember(d => d.ComOwnNm, map => map.MapFrom(s => s.ScBizWork.ScCompInfo.OwnNm))
                .ForMember(d => d.CompNm, map => map.MapFrom(s => s.ScBizWork.ScCompInfo.CompNm))
                .ForMember(d => d.ComRegistrationNo, map => map.MapFrom(s => s.ScBizWork.ScCompInfo.RegistrationNo))
                .ForMember(d => d.TelNo1, map => map.MapFrom(s => s.ScUsr.TelNo.Split('-').GetValue(0).ToString()))
                .ForMember(d => d.TelNo2, map => map.MapFrom(s => s.ScUsr.TelNo.Split('-').GetValue(1).ToString()))
                .ForMember(d => d.TelNo3, map => map.MapFrom(s => s.ScUsr.TelNo.Split('-').GetValue(2).ToString()))
                .ForMember(d => d.MbNo1, map => map.MapFrom(s => s.ScUsr.MbNo.Split('-').GetValue(0).ToString()))
                .ForMember(d => d.MbNo2, map => map.MapFrom(s => s.ScUsr.MbNo.Split('-').GetValue(1).ToString()))
                .ForMember(d => d.MbNo3, map => map.MapFrom(s => s.ScUsr.MbNo.Split('-').GetValue(2).ToString()))
                .ForMember(d => d.Email1, map => map.MapFrom(s => s.ScUsr.Email.Split('@').GetValue(0).ToString()))
                .ForMember(d => d.Email2, map => map.MapFrom(s => s.ScUsr.Email.Split('@').GetValue(1).ToString()))
                .ForMember(d => d.Name, map => map.MapFrom(s => s.ScUsr.Name))
                .ForMember(d => d.LoginId, map => map.MapFrom(s => s.ScUsr.LoginId))
                .ForMember(d => d.UsrTypeDetail, map => map.MapFrom(s => s.ScUsr.UsrTypeDetail));

            //공지사항 Notice 매핑
            Mapper.CreateMap<ScBizWork, BizWorkViewModel>()
                .ForMember(d => d.ComCount, map => map.MapFrom(s => s.ScCompMappings.Count))
                .ForMember(d => d.Name, map => map.MapFrom(s => s.ScUsr.Name))
                .ForMember(d => d.TelNo1, map => map.MapFrom(s => s.ScUsr.TelNo.Split('-').GetValue(0).ToString()))
                .ForMember(d => d.TelNo2, map => map.MapFrom(s => s.ScUsr.TelNo.Split('-').GetValue(1).ToString()))
                .ForMember(d => d.TelNo3, map => map.MapFrom(s => s.ScUsr.TelNo.Split('-').GetValue(2).ToString()))
                .ForMember(d => d.MbNo1, map => map.MapFrom(s => s.ScUsr.MbNo.Split('-').GetValue(0).ToString()))
                .ForMember(d => d.MbNo2, map => map.MapFrom(s => s.ScUsr.MbNo.Split('-').GetValue(1).ToString()))
                .ForMember(d => d.MbNo3, map => map.MapFrom(s => s.ScUsr.MbNo.Split('-').GetValue(2).ToString()))
                .ForMember(d => d.FaxNo1, map => map.MapFrom(s => s.ScUsr.FaxNo.Split('-').GetValue(0).ToString()))
                .ForMember(d => d.FaxNo2, map => map.MapFrom(s => s.ScUsr.FaxNo.Split('-').GetValue(1).ToString()))
                .ForMember(d => d.FaxNo3, map => map.MapFrom(s => s.ScUsr.FaxNo.Split('-').GetValue(2).ToString()))
                .ForMember(d => d.Email1, map => map.MapFrom(s => s.ScUsr.Email.Split('@').GetValue(0).ToString()))
                .ForMember(d => d.Email2, map => map.MapFrom(s => s.ScUsr.Email.Split('@').GetValue(1).ToString()))
                .ForMember(d => d.LoginId, map => map.MapFrom(s => s.ScUsr.LoginId))
                .ForMember(d => d.DeptNm, map => map.MapFrom(s => s.ScUsr.DeptNm));


            Mapper.CreateMap<ScCompInfo, JoinCompanyViewModel>()
                .ForMember(d => d.TelNo1, map => map.MapFrom(s => s.ScUsrs.ElementAt(0).TelNo.Split('-').GetValue(0).ToString()))
                .ForMember(d => d.TelNo2, map => map.MapFrom(s => s.ScUsrs.ElementAt(0).TelNo.Split('-').GetValue(1).ToString()))
                .ForMember(d => d.TelNo3, map => map.MapFrom(s => s.ScUsrs.ElementAt(0).TelNo.Split('-').GetValue(2).ToString()))
                //.ForMember(d => d.FaxNo1, map => map.MapFrom(s => s.ScUsrs.ElementAt(0).FaxNo.Split('-').GetValue(0).ToString()))
                //.ForMember(d => d.FaxNo2, map => map.MapFrom(s => s.ScUsrs.ElementAt(0).FaxNo.Split('-').GetValue(1).ToString()))
                //.ForMember(d => d.FaxNo3, map => map.MapFrom(s => s.ScUsrs.ElementAt(0).FaxNo.Split('-').GetValue(2).ToString()))
                //.ForMember(d => d.MbNo1, map => map.MapFrom(s => s.ScUsrs.ElementAt(0).MbNo.Split('-').GetValue(0).ToString()))
                //.ForMember(d => d.MbNo2, map => map.MapFrom(s => s.ScUsrs.ElementAt(0).MbNo.Split('-').GetValue(1).ToString()))
                //.ForMember(d => d.MbNo3, map => map.MapFrom(s => s.ScUsrs.ElementAt(0).MbNo.Split('-').GetValue(2).ToString()))
                .ForMember(d => d.Email1, map => map.MapFrom(s => s.ScUsrs.ElementAt(0).Email.Split('@').GetValue(0).ToString()))
                .ForMember(d => d.Email2, map => map.MapFrom(s => s.ScUsrs.ElementAt(0).Email.Split('@').GetValue(1).ToString()))
                .ForMember(d => d.Name, map => map.MapFrom(s => s.ScUsrs.ElementAt(0).Name))
                .ForMember(d => d.CompNm, map => map.MapFrom(s => s.CompNm))
                .ForMember(d => d.ComOwnNm, map => map.MapFrom(s => s.OwnNm))
                .ForMember(d => d.ComRegistrationNo, map => map.MapFrom(s => s.RegistrationNo));

            //멘토 회원 뷰 매핑
            Mapper.CreateMap<ScMentorMappiing, JoinMentorViewModel>()
                .ForMember(d => d.BizWorkNm, map => map.MapFrom(s => s.ScBizWork.BizWorkNm))
                .ForMember(d => d.LoginId, map => map.MapFrom(s => s.ScUsr.LoginId))
                .ForMember(d => d.Email, map => map.MapFrom(s => s.ScUsr.Email))
                .ForMember(d => d.TelNo, map => map.MapFrom(s => s.ScUsr.TelNo))
                .ForMember(d => d.Name, map => map.MapFrom(s => s.ScUsr.Name))
                .ForMember(d => d.UsrTypeDetail, map => map.MapFrom(s => s.ScUsr.UsrTypeDetail))
                .ForMember(d => d.ResumeName, map => map.MapFrom(s => s.ScUsr.ScUsrResume.ScFileInfo.FileNm));

        }
    }
}