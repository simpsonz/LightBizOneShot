// ReSharper disable RedundantUsingDirective
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable RedundantNameQualifier
// TargetFrameworkVersion = 4.51
#pragma warning disable 1591    //  Ignore "Missing XML Comment" warning

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using BizOneShot.Light.Models.DareModels;
using System.Threading;
using DatabaseGeneratedOption = System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption;

namespace BizOneShot.Light.Dao.DareConfiguration
{
    // SY_USER
    internal partial class SHUSER_SyUserConfiguration : EntityTypeConfiguration<SHUSER_SyUser>
    {
        public SHUSER_SyUserConfiguration()
            : this("SHUSER")
        {
        }
 
        public SHUSER_SyUserConfiguration(string schema)
        {
            ToTable(schema + ".SY_USER");
            HasKey(x => x.IdUser);

            Property(x => x.IdUser).HasColumnName("ID_USER").IsRequired().HasColumnType("nvarchar").HasMaxLength(20).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.MembBusnpersNo).HasColumnName("MEMB_BUSNPERS_NO").IsRequired().HasColumnType("nvarchar").HasMaxLength(13);
            Property(x => x.NmUser).HasColumnName("NM_USER").IsRequired().HasColumnType("nvarchar").HasMaxLength(20);
            Property(x => x.Pwd).HasColumnName("PWD").IsRequired().HasColumnType("nvarchar").HasMaxLength(300);
            Property(x => x.SmartPwd).HasColumnName("SMART_PWD").IsOptional().HasColumnType("nvarchar").HasMaxLength(300);
            Property(x => x.UsrGbn).HasColumnName("USR_GBN").IsRequired().HasColumnType("nvarchar").HasMaxLength(3);
            Property(x => x.Usercode).HasColumnName("USERCODE").IsOptional().HasColumnType("nvarchar").HasMaxLength(8);
            Property(x => x.LangCd).HasColumnName("LANG_CD").IsOptional().HasColumnType("nvarchar").HasMaxLength(3);
            Property(x => x.EmpCode).HasColumnName("EMP_CODE").IsOptional().HasColumnType("nvarchar").HasMaxLength(10);
            Property(x => x.StDocuapp).HasColumnName("ST_DOCUAPP").IsOptional().HasColumnType("nvarchar").HasMaxLength(3);
            Property(x => x.GrdUser).HasColumnName("GRD_USER").IsOptional().HasColumnType("nvarchar").HasMaxLength(3);
            Property(x => x.ShowModuleCd).HasColumnName("SHOW_MODULE_CD").IsOptional().HasColumnType("nvarchar").HasMaxLength(2);
            Property(x => x.ShowMenuCd).HasColumnName("SHOW_MENU_CD").IsOptional().HasColumnType("nvarchar").HasMaxLength(12);
            Property(x => x.HangilPwd).HasColumnName("HANGIL_PWD").IsOptional().HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.LastLoginDate).HasColumnName("LAST_LOGIN_DATE").IsOptional().HasColumnType("nvarchar").HasMaxLength(14);
            Property(x => x.LastLogoutDate).HasColumnName("LAST_LOGOUT_DATE").IsOptional().HasColumnType("nvarchar").HasMaxLength(14);
            Property(x => x.CheckingLogDt).HasColumnName("CHECKING_LOG_DT").IsOptional().HasColumnType("nvarchar").HasMaxLength(14);
            Property(x => x.PersData).HasColumnName("PERS_DATA").IsOptional().HasColumnType("nvarchar").HasMaxLength(300);
            Property(x => x.UserStatus).HasColumnName("USER_STATUS").IsOptional().HasColumnType("nvarchar").HasMaxLength(1);
            Property(x => x.SmsChargeCnt).HasColumnName("SMS_CHARGE_CNT").IsOptional().HasColumnType("int");
            Property(x => x.SmsSendCnt).HasColumnName("SMS_SEND_CNT").IsOptional().HasColumnType("nvarchar").HasMaxLength(5);
            Property(x => x.DbAccessLog).HasColumnName("DB_ACCESS_LOG").IsOptional().HasColumnType("nvarchar").HasMaxLength(1);
            Property(x => x.LastProductType).HasColumnName("LAST_PRODUCT_TYPE").IsOptional().HasColumnType("nvarchar").HasMaxLength(20);
            Property(x => x.CreateIdDate).HasColumnName("CREATE_ID_DATE").IsOptional().HasColumnType("nvarchar").HasMaxLength(8);
            Property(x => x.InsertId).HasColumnName("INSERT_ID").IsOptional().HasColumnType("nvarchar").HasMaxLength(20);
            Property(x => x.InsertIp).HasColumnName("INSERT_IP").IsOptional().HasColumnType("nvarchar").HasMaxLength(20);
            Property(x => x.InsertDt).HasColumnName("INSERT_DT").IsOptional().HasColumnType("nvarchar").HasMaxLength(14);
            Property(x => x.ModifyId).HasColumnName("MODIFY_ID").IsOptional().HasColumnType("nvarchar").HasMaxLength(20);
            Property(x => x.ModifyIp).HasColumnName("MODIFY_IP").IsOptional().HasColumnType("nvarchar").HasMaxLength(20);
            Property(x => x.ModifyDt).HasColumnName("MODIFY_DT").IsOptional().HasColumnType("nvarchar").HasMaxLength(14);
            Property(x => x.MenuAuthCd).HasColumnName("MENU_AUTH_CD").IsOptional().HasColumnType("nvarchar").HasMaxLength(1);
            Property(x => x.MainMenuView).HasColumnName("MAIN_MENU_VIEW").IsOptional().HasColumnType("nvarchar").HasMaxLength(1);
            Property(x => x.IsB2BUser).HasColumnName("IS_B2B_USER").IsOptional().HasColumnType("nvarchar").HasMaxLength(1);
            Property(x => x.Etc).HasColumnName("ETC").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.IsHrModuleYn).HasColumnName("IS_HR_MODULE_YN").IsOptional().HasColumnType("nvarchar").HasMaxLength(1);
            Property(x => x.MobileData).HasColumnName("MOBILE_DATA").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.WebUsrGbn).HasColumnName("WEB_USR_GBN").IsOptional().HasColumnType("nvarchar").HasMaxLength(1);
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
