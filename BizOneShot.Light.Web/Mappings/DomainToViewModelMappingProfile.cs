﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using AutoMapper;
using BizOneShot.Light.Models.ViewModels;
using BizOneShot.Light.Models.WebModels;
using BizOneShot.Light.Util.Helper;

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

            Mapper.CreateMap<ScForm, ManualDetailViewModel>()
                .ForMember(d => d.Manual, map => map.MapFrom(s => s))
                .ForMember(d => d.PreFormSn, map => map.UseValue(0))
                .ForMember(d => d.NextFormSn, map => map.UseValue(0));


            //파일 FileInfo 매핑
            Mapper.CreateMap<ScFileInfo, FileInfoViewModel>();

            Mapper.CreateMap<ScFileInfo, FileContent>();


            //Company MyInfo 매핑;
            Mapper.CreateMap<ScUsr, CompanyMyInfoViewModel>()
                .ForMember(d => d.ResumeName, map => map.MapFrom(s => s.ScUsrResume.ScFileInfo.FileNm))
                .ForMember(d => d.ResumePath, map => map.MapFrom(s => s.ScUsrResume.ScFileInfo.FilePath))
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

            Mapper.CreateMap<ScExpertMapping, BizWorkDropDownModel>()
                .ForMember(d => d.BizWorkSn, map => map.MapFrom(s => s.ScBizWork.BizWorkSn))
                .ForMember(d => d.BizWorkNm, map => map.MapFrom(s => s.ScBizWork.BizWorkNm));


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
                .ForMember(d => d.BizWorkNm, map => map.MapFrom(s => s.ScBizWork.BizWorkNm))
                .ForMember(d => d.ResumeName, map => map.MapFrom(s => s.ScUsr.ScUsrResume.ScFileInfo.FileNm))
                .ForMember(d => d.ResumePath, map => map.MapFrom(s => s.ScUsr.ScUsrResume.ScFileInfo.FilePath))
                .ForMember(d => d.ComPostNo, map => map.MapFrom(s => s.ScUsr.ScCompInfo.PostNo))
                .ForMember(d => d.ComAddr1, map => map.MapFrom(s => s.ScUsr.ScCompInfo.Addr1))
                .ForMember(d => d.ComAddr2, map => map.MapFrom(s => s.ScUsr.ScCompInfo.Addr2))
                .ForMember(d => d.ComOwnNm, map => map.MapFrom(s => s.ScUsr.ScCompInfo.OwnNm))
                .ForMember(d => d.CompNm, map => map.MapFrom(s => s.ScUsr.ScCompInfo.CompNm))
                .ForMember(d => d.ComTelNo, map => map.MapFrom(s => s.ScUsr.ScCompInfo.TelNo))
                .ForMember(d => d.ComRegistrationNo, map => map.MapFrom(s => s.ScUsr.ScCompInfo.RegistrationNo))
                .ForMember(d => d.TelNo1, map => map.MapFrom(s => s.ScUsr.TelNo.Split('-').GetValue(0).ToString()))
                .ForMember(d => d.TelNo2, map => map.MapFrom(s => s.ScUsr.TelNo.Split('-').GetValue(1).ToString()))
                .ForMember(d => d.TelNo3, map => map.MapFrom(s => s.ScUsr.TelNo.Split('-').GetValue(2).ToString()))
                .ForMember(d => d.TelNo, map => map.MapFrom(s => s.ScUsr.TelNo))
                .ForMember(d => d.MbNo1, map => map.MapFrom(s => s.ScUsr.MbNo.Split('-').GetValue(0).ToString()))
                .ForMember(d => d.MbNo2, map => map.MapFrom(s => s.ScUsr.MbNo.Split('-').GetValue(1).ToString()))
                .ForMember(d => d.MbNo3, map => map.MapFrom(s => s.ScUsr.MbNo.Split('-').GetValue(2).ToString()))
                .ForMember(d => d.MbNo, map => map.MapFrom(s => s.ScUsr.MbNo))
                .ForMember(d => d.Email1, map => map.MapFrom(s => s.ScUsr.Email.Split('@').GetValue(0).ToString()))
                .ForMember(d => d.Email2, map => map.MapFrom(s => s.ScUsr.Email.Split('@').GetValue(1).ToString()))
                .ForMember(d => d.Email, map => map.MapFrom(s => s.ScUsr.Email))
                .ForMember(d => d.Name, map => map.MapFrom(s => s.ScUsr.Name))
                .ForMember(d => d.LoginId, map => map.MapFrom(s => s.ScUsr.LoginId))
                .ForMember(d => d.UsrTypeDetail, map => map.MapFrom(s => s.ScUsr.UsrTypeDetail));


            Mapper.CreateMap<ScBizWork, BizWorkViewModel>()
                .ForMember(d => d.ComCount, map => map.MapFrom(s => s.ScCompMappings.Count))
                .ForMember(d => d.Name, map => map.MapFrom(s => s.ScUsr.Name))
                .ForMember(d => d.TelNo1, map => map.MapFrom(s => s.ScUsr.TelNo.Split('-').GetValue(0).ToString()))
                .ForMember(d => d.TelNo2, map => map.MapFrom(s => s.ScUsr.TelNo.Split('-').GetValue(1).ToString()))
                .ForMember(d => d.TelNo3, map => map.MapFrom(s => s.ScUsr.TelNo.Split('-').GetValue(2).ToString()))
                .ForMember(d => d.TelNo, map => map.MapFrom(s => s.ScUsr.TelNo))
                .ForMember(d => d.MbNo1, map => map.MapFrom(s => s.ScUsr.MbNo.Split('-').GetValue(0).ToString()))
                .ForMember(d => d.MbNo2, map => map.MapFrom(s => s.ScUsr.MbNo.Split('-').GetValue(1).ToString()))
                .ForMember(d => d.MbNo3, map => map.MapFrom(s => s.ScUsr.MbNo.Split('-').GetValue(2).ToString()))
                .ForMember(d => d.MbNo, map => map.MapFrom(s => s.ScUsr.MbNo))
                .ForMember(d => d.FaxNo1, map => map.MapFrom(s => s.ScUsr.FaxNo.Split('-').GetValue(0).ToString()))
                .ForMember(d => d.FaxNo2, map => map.MapFrom(s => s.ScUsr.FaxNo.Split('-').GetValue(1).ToString()))
                .ForMember(d => d.FaxNo3, map => map.MapFrom(s => s.ScUsr.FaxNo.Split('-').GetValue(2).ToString()))
                .ForMember(d => d.FaxNo, map => map.MapFrom(s => s.ScUsr.FaxNo))
                .ForMember(d => d.Email1, map => map.MapFrom(s => s.ScUsr.Email.Split('@').GetValue(0).ToString()))
                .ForMember(d => d.Email2, map => map.MapFrom(s => s.ScUsr.Email.Split('@').GetValue(1).ToString()))
                .ForMember(d => d.Email, map => map.MapFrom(s => s.ScUsr.Email))
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

            //멘토 회원 등록 매핑
            Mapper.CreateMap<ScMentorMappiing, JoinMentorViewModel>()
                .ForMember(d => d.BizWorkNm, map => map.MapFrom(s => s.ScBizWork.BizWorkNm))
                .ForMember(d => d.LoginId, map => map.MapFrom(s => s.ScUsr.LoginId))
                .ForMember(d => d.Email, map => map.MapFrom(s => s.ScUsr.Email))
                .ForMember(d => d.TelNo, map => map.MapFrom(s => s.ScUsr.TelNo))
                .ForMember(d => d.Name, map => map.MapFrom(s => s.ScUsr.Name))
                .ForMember(d => d.UsrTypeDetail, map => map.MapFrom(s => s.ScUsr.UsrTypeDetail))
                .ForMember(d => d.ResumeName, map => map.MapFrom(s => s.ScUsr.ScUsrResume.ScFileInfo.FileNm));

            //맨토 회원 뷰 매핑
            Mapper.CreateMap<ScUsr, MentorMyInfoViewModel>()
                .ForMember(d => d.TelNo1, map => map.MapFrom(s => s.TelNo.Split('-').GetValue(0).ToString()))
                .ForMember(d => d.TelNo2, map => map.MapFrom(s => s.TelNo.Split('-').GetValue(1).ToString()))
                .ForMember(d => d.TelNo3, map => map.MapFrom(s => s.TelNo.Split('-').GetValue(2).ToString()))
                .ForMember(d => d.FaxNo1, map => map.MapFrom(s => s.FaxNo.Split('-').GetValue(0).ToString()))
                .ForMember(d => d.FaxNo2, map => map.MapFrom(s => s.FaxNo.Split('-').GetValue(1).ToString()))
                .ForMember(d => d.FaxNo3, map => map.MapFrom(s => s.MbNo.Split('-').GetValue(2).ToString()))
                .ForMember(d => d.MbNo1, map => map.MapFrom(s => s.MbNo.Split('-').GetValue(0).ToString()))
                .ForMember(d => d.MbNo2, map => map.MapFrom(s => s.MbNo.Split('-').GetValue(1).ToString()))
                .ForMember(d => d.MbNo3, map => map.MapFrom(s => s.MbNo.Split('-').GetValue(2).ToString()))
                .ForMember(d => d.Email1, map => map.MapFrom(s => s.Email.Split('@').GetValue(0).ToString()))
                .ForMember(d => d.Email2, map => map.MapFrom(s => s.Email.Split('@').GetValue(1).ToString()))
                .ForMember(d => d.FileSn, map => map.MapFrom(s => s.ScUsrResume.ScFileInfo.FileSn))
                .ForMember(d => d.ResumeName, map => map.MapFrom(s => s.ScUsrResume.ScFileInfo.Status == "N" ? s.ScUsrResume.ScFileInfo.FileNm : ""))
                .ForMember(d => d.ResumePath, map => map.MapFrom(s => s.ScUsrResume.ScFileInfo.Status == "N" ? s.ScUsrResume.ScFileInfo.FilePath : ""))
                .ForMember(d => d.Addr, map => map.MapFrom(s => "(" + s.PostNo + ") " + s.Addr1 + " " + s.Addr2))
                .ForMember(d => d.BizWorkNm, map => map.MapFrom(s => s.ScMentorMappiings.FirstOrDefault().ScBizWork.BizWorkNm ?? ""));

            //맨토 토탈 레포트 뷰
            Mapper.CreateMap<ScMentoringTotalReport, MentoringTotalReportViewModel>()
                .ForMember(d => d.BizWorkNm, map => map.MapFrom(s => s.ScBizWork.BizWorkNm))
                .ForMember(d => d.CompNm, map => map.MapFrom(s => s.ScCompInfo.CompNm))
                .ForMember(d => d.FileContents, map => map.MapFrom(s => s.ScMentoringTrFileInfoes.Select(mtfi => mtfi.ScFileInfo)));

            //기업 뷰 매핑
            Mapper.CreateMap<ScCompInfo, CompInfoDropDownModel>();

            //기업 회원 관리(사업관리자) 뷰 매핑
            Mapper.CreateMap<ScCompMapping, CompanyMngViewModel>()
                .ForMember(d => d.ApproveRequestDate, map => map.MapFrom(s => s.RegDt))
                .ForMember(d => d.CompNm, map => map.MapFrom(s => s.ScCompInfo.CompNm))
                .ForMember(d => d.RegistrationNo, map => map.MapFrom(s => s.ScCompInfo.RegistrationNo))
                .ForMember(d => d.OwnNm, map => map.MapFrom(s => s.ScCompInfo.OwnNm))
                .ForMember(d => d.BizWorkNm, map => map.MapFrom(s => s.ScBizWork.BizWorkNm))
                .ForMember(d => d.MngCompSn, map => map.MapFrom(s => s.ScBizWork.ScCompInfo.CompSn))
                .ForMember(d => d.PostNo, map => map.MapFrom(s => s.ScCompInfo.PostNo))
                .ForMember(d => d.Addr1, map => map.MapFrom(s => s.ScCompInfo.Addr1))
                .ForMember(d => d.Addr2, map => map.MapFrom(s => s.ScCompInfo.Addr2))
                //.ForMember(d => d.Name, map => map.MapFrom(s => s.ScCompInfo.ScUsrs.ElementAt(0).Name))
                .ForMember(d => d.Name, map => map.MapFrom(s => s.ScBizWork.ScUsr.Name))
                .ForMember(d => d.TelNo, map => map.MapFrom(s => s.ScBizWork.ScUsr.TelNo))
                .ForMember(d => d.MbNo, map => map.MapFrom(s => s.ScBizWork.ScUsr.MbNo))
                .ForMember(d => d.Email, map => map.MapFrom(s => s.ScBizWork.ScUsr.Email))
                .ForMember(d => d.MentorName, map => map.MapFrom(s => s.ScUsr.Name))
                .ForMember(d => d.MentorTelNo, map => map.MapFrom(s => s.ScUsr.TelNo))
                .ForMember(d => d.MentorMbNo, map => map.MapFrom(s => s.ScUsr.MbNo))
                .ForMember(d => d.MentorEmail, map => map.MapFrom(s => s.ScUsr.Email));

            Mapper.CreateMap<ScMentorMappiing, MentorDropDownModel>()
                .ForMember(d => d.LoginId, map => map.MapFrom(s => s.ScUsr.LoginId))
                .ForMember(d => d.Name , map => map.MapFrom(s => s.ScUsr.Name));


            //기업 회원 관리(사업관리자) 뷰 매핑
            Mapper.CreateMap<ScCompMapping, ExpertCompanyViewModel>()
                .ForMember(d => d.CompNm, map => map.MapFrom(s => s.ScCompInfo.CompNm))
                .ForMember(d => d.ComRegistrationNo, map => map.MapFrom(s => s.ScCompInfo.RegistrationNo))
                .ForMember(d => d.ComOwnNm, map => map.MapFrom(s => s.ScCompInfo.OwnNm))
                .ForMember(d => d.CompSn, map => map.MapFrom(s => s.ScCompInfo.CompSn))
                .ForMember(d => d.Name, map => map.MapFrom(s => s.ScCompInfo.ScUsrs.ElementAt(0).Name))
                .ForMember(d => d.TelNo, map => map.MapFrom(s => s.ScCompInfo.ScUsrs.ElementAt(0).TelNo))
                .ForMember(d => d.Email, map => map.MapFrom(s => s.ScCompInfo.ScUsrs.ElementAt(0).Email))
                .ForMember(d => d.LoginId, map => map.MapFrom(s => s.ScCompInfo.ScUsrs.ElementAt(0).LoginId));

            Mapper.CreateMap<ScReqDoc, DataRequstViewModels>()
                .ForMember(d => d.SenderComNm, map => map.MapFrom(s => s.ScUsr_SenderId.ScCompInfo.CompNm))
                .ForMember(d => d.SenderRegistrationNo, map => map.MapFrom(s => s.ScUsr_SenderId.ScCompInfo.RegistrationNo))
                .ForMember(d => d.ReceiverName, map => map.MapFrom(s => s.ScUsr_ReceiverId.Name))
                .ForMember(d => d.ReceiverComNm, map => map.MapFrom(s => s.ScUsr_ReceiverId.ScCompInfo.CompNm))
                .ForMember(d => d.ReceiverRegistrationNo, map => map.MapFrom(s => s.ScUsr_ReceiverId.ScCompInfo.RegistrationNo));

            Mapper.CreateMap<ScQa, QaRequstViewModels>()
                .ForMember(d => d.QuestionComNm, map => map.MapFrom(s => s.ScUsr_QuestionId.ScCompInfo.CompNm))
                .ForMember(d => d.QuestionRegistrationNo, map => map.MapFrom(s => s.ScUsr_QuestionId.ScCompInfo.RegistrationNo))
                .ForMember(d => d.AnswerComNm, map => map.MapFrom(s => s.ScUsr_AnswerId.ScCompInfo.CompNm))
                .ForMember(d => d.AnswerRegistrationNo, map => map.MapFrom(s => s.ScUsr_AnswerId.ScCompInfo.RegistrationNo));

            Mapper.CreateMap<UspSelectSidoForWebListReturnModel, SelectAddressListViewModel>();
            Mapper.CreateMap<UspSelectGunguForWebListReturnModel, SelectGunguListViewModel>();
            Mapper.CreateMap<UspSelectAddressByBuildingSearchForWebListReturnModel, SelectAddressListViewModel>();
            Mapper.CreateMap<UspSelectAddressByDongSearchForWebListReturnModel, SelectAddressListViewModel>();
            Mapper.CreateMap<UspSelectAddressByStreetSearchForWebListReturnModel, SelectAddressListViewModel>();


            Mapper.CreateMap<ScUsr, LoginViewModel>()
                .ForMember(d => d.ID, map => map.MapFrom(s => s.LoginId));








        }
    }
}