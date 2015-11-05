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
        //public override string ProfileName
        //{
        //    get
        //    {
        //        return "ViewModelToDomainMapping";
        //    }
        //}

        protected override void Configure()
        {
            //여기에 Object-To-Object 매핑정의를 생성(필요할때 계속 추가)

            //FAQ
            Mapper.CreateMap<FaqViewModel, ScFaq>()
                   .ForMember(d => d.FaqSn, map => map.MapFrom(s => s.FaqSn))
                   .ForMember(d => d.AnsTxt, map => map.MapFrom(s => s.AnsTxt))
                   .ForMember(d => d.QstTxt, map => map.MapFrom(s => s.QstTxt));
            //Notice
            Mapper.CreateMap<NoticeViewModel, ScNtc>();
            //Manual
            Mapper.CreateMap<ManualViewModel, ScForm>();
            


            //전문가 회원가입 to 회원
            Mapper.CreateMap<JoinExpertViewModel, ScUsr>()
                .ForMember(d => d.TelNo, map => map.MapFrom(s => s.TelNo1 + "-" + s.TelNo2 + "-" + s.TelNo3))
                .ForMember(d => d.MbNo, map => map.MapFrom(s => s.MbNo1 + "-" + s.MbNo2 + "-" + s.MbNo3))
                .ForMember(d => d.Email, map => map.MapFrom(s => s.Email1 + "@" + s.Email2));

            //전문가 회원가입모델 to 회사
            Mapper.CreateMap<JoinExpertViewModel, ScCompInfo>()
                .ForMember(d => d.TelNo, map => map.MapFrom(s => s.ComTelNo1 + "-" + s.ComTelNo2 + "-" + s.ComTelNo3))
                .ForMember(d => d.Addr1, map => map.MapFrom(s => s.ComAddr1))
                .ForMember(d => d.Addr2, map => map.MapFrom(s => s.ComAddr2))
                .ForMember(d => d.PostNo, map => map.MapFrom(s => s.ComPostNo))
                .ForMember(d => d.OwnNm, map => map.MapFrom(s => s.ComOwnNm))
                .ForMember(d => d.RegistrationNo, map => map.MapFrom(s => s.ComRegistrationNo));

            //멘토 회원가입 to 회원
            Mapper.CreateMap<JoinMentorViewModel, ScUsr>()
                .ForMember(d => d.Email, map => map.MapFrom(s => s.Email1 + "@" + s.Email2));

            //멘토 회원가입모델 to 회사
            Mapper.CreateMap<JoinMentorViewModel, ScCompInfo>();


            //멘토 종합 보고서 등록 to 멘토종합보고서
            Mapper.CreateMap<MentoringTotalReportViewModel, ScMentoringTotalReport>();

            //멘토일지 등록 to 멘토일지
            Mapper.CreateMap<MentoringReportViewModel, ScMentoringReport>();

            //회원가입모델 to 회원
            Mapper.CreateMap<JoinCompanyViewModel, ScUsr>()
                .ForMember(d => d.TelNo, map => map.MapFrom(s => s.TelNo1 + "-" + s.TelNo2 + "-" + s.TelNo3))
                .ForMember(d => d.MbNo, map => map.MapFrom(s => s.MbNo1 + "-" + s.MbNo2 + "-" + s.MbNo3))
                .ForMember(d => d.FaxNo, map => map.MapFrom(s => s.FaxNo1 + "-" + s.FaxNo2 + "-" + s.FaxNo2))
                .ForMember(d => d.Email, map => map.MapFrom(s => s.Email1 + "@" + s.Email2));

            //회원가입모델 to 회사
            Mapper.CreateMap<JoinCompanyViewModel, ScCompInfo>()
                .ForMember(d => d.TelNo, map => map.MapFrom(s => s.ComTelNo1 + "-" + s.ComTelNo2 + "-" + s.ComTelNo3))
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

            //사업관리 담당자 to 회원
            Mapper.CreateMap<BizWorkViewModel, ScUsr>()
                .ForMember(d => d.TelNo, map => map.MapFrom(s => s.TelNo1 + "-" + s.TelNo2 + "-" + s.TelNo3))
                .ForMember(d => d.MbNo, map => map.MapFrom(s => s.MbNo1 + "-" + s.MbNo2 + "-" + s.MbNo3))
                .ForMember(d => d.FaxNo, map => map.MapFrom(s => s.FaxNo1 + "-" + s.FaxNo2 + "-" + s.FaxNo3))
                .ForMember(d => d.Email, map => map.MapFrom(s => s.Email1 + "@" + s.Email2));

            //사업관리 담당자 to 사업
            Mapper.CreateMap<BizWorkViewModel, ScBizWork>()
                .ForMember(d => d.ExecutorId, map => map.MapFrom(s => s.LoginId));

            Mapper.CreateMap<DataRequstViewModels, ScReqDoc>();
            Mapper.CreateMap<RegSendViewModels, ScReqDoc>();

            //문의등록
            Mapper.CreateMap<RegQaRequestViewModels, ScQa>();

            //문진표 맵퍼
            Mapper.CreateMap<QuesMasterViewModel, QuesMaster>();
            Mapper.CreateMap<QuesWriterViewModel, QuesWriter>();
            Mapper.CreateMap<QuesCompanyInfoViewModel, QuesCompInfo>();
            Mapper.CreateMap<QuesCompExtentionViewModel, QuesCompExtention>();
            Mapper.CreateMap<QuesYearListViewModel, QuesResult2>();
            Mapper.CreateMap<OrgCompositionViewModel, QuesOgranAnalysis>();


            //전문가 재무보고서 의견 등록
            Mapper.CreateMap<RegCommentViewModel, RptFinanceComment>();
            



        }
    }
}