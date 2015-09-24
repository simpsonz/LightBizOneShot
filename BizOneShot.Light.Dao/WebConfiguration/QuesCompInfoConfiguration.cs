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
using BizOneShot.Light.Models.WebModels;
using System.Threading;
using DatabaseGeneratedOption = System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption;

namespace BizOneShot.Light.Dao.WebConfiguration
{
    // QUES_COMP_INFO
    internal partial class QuesCompInfoConfiguration : EntityTypeConfiguration<QuesCompInfo>
    {
        public QuesCompInfoConfiguration()
            : this("dbo")
        {
        }
 
        public QuesCompInfoConfiguration(string schema)
        {
            ToTable(schema + ".QUES_COMP_INFO");
            HasKey(x => x.QuestionSn);

            Property(x => x.QuestionSn).HasColumnName("QUESTION_SN").IsRequired().HasColumnType("int").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.CompNm).HasColumnName("COMP_NM").IsOptional().HasColumnType("nvarchar").HasMaxLength(70);
            Property(x => x.EngCompNm).HasColumnName("ENG_COMP_NM").IsOptional().HasColumnType("nvarchar").HasMaxLength(70);
            Property(x => x.TelNo).HasColumnName("TEL_NO").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(30);
            Property(x => x.FaxNo).HasColumnName("FAX_NO").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(20);
            Property(x => x.Name).HasColumnName("NAME").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(50);
            Property(x => x.Email).HasColumnName("EMAIL").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(40);
            Property(x => x.RegistrationNo).HasColumnName("REGISTRATION_NO").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(10);
            Property(x => x.CoRegistrationNo).HasColumnName("CO_REGISTRATION_NO").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(13);
            Property(x => x.PublishDt).HasColumnName("PUBLISH_DT").IsOptional().HasColumnType("datetime");
            Property(x => x.HomeUrl).HasColumnName("HOME_URL").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(200);
            Property(x => x.CompAddr).HasColumnName("COMP_ADDR").IsOptional().HasColumnType("nvarchar").HasMaxLength(255);
            Property(x => x.FactoryAddr).HasColumnName("FACTORY_ADDR").IsOptional().HasColumnType("nvarchar").HasMaxLength(255);
            Property(x => x.LabAddr).HasColumnName("LAB_ADDR").IsOptional().HasColumnType("nvarchar").HasMaxLength(255);
            Property(x => x.FacPossessYn).HasColumnName("FAC_POSSESS_YN").IsOptional().IsFixedLength().IsUnicode(false).HasColumnType("char").HasMaxLength(1);
            Property(x => x.RndYn).HasColumnName("RND_YN").IsOptional().IsFixedLength().IsUnicode(false).HasColumnType("char").HasMaxLength(1);
            Property(x => x.ProductNm1).HasColumnName("PRODUCT_NM1").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.ProductNm2).HasColumnName("PRODUCT_NM2").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.ProductNm3).HasColumnName("PRODUCT_NM3").IsOptional().HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.MarketList).HasColumnName("MARKET_LIST").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(20);
            Property(x => x.CompType).HasColumnName("COMP_TYPE").IsOptional().IsFixedLength().IsUnicode(false).HasColumnType("char").HasMaxLength(1);
            Property(x => x.ResidentType).HasColumnName("RESIDENT_TYPE").IsOptional().IsFixedLength().IsUnicode(false).HasColumnType("char").HasMaxLength(1);
            Property(x => x.CertiType).HasColumnName("CERTI_TYPE").IsOptional().IsFixedLength().IsUnicode(false).HasColumnType("char").HasMaxLength(1);
            Property(x => x.RegId).HasColumnName("REG_ID").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(25);
            Property(x => x.RegDt).HasColumnName("REG_DT").IsOptional().HasColumnType("datetime");
            Property(x => x.UpdId).HasColumnName("UPD_ID").IsOptional().IsUnicode(false).HasColumnType("varchar").HasMaxLength(25);
            Property(x => x.UpdDt).HasColumnName("UPD_DT").IsOptional().HasColumnType("datetime");

            // Foreign keys
            HasRequired(a => a.QuesMaster).WithOptional(b => b.QuesCompInfo); // FK_QUES_MASTER_TO_QUES_COMP_INFO
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
