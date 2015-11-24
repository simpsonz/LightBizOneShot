using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

using System.Threading.Tasks;
using BizOneShot.Light.Dao.WebConfiguration;
using BizOneShot.Light.Models.WebModels;

using AutoMapper;
using System.Data.SqlClient;

namespace EntityTestConsole
{

    class Program
    {
        static  void Main(string[] args)
        {
            //insertScCompInfo();
            //insertScCompInfoAsync();
            //UpdateScCompInfo();
            //UpdateScCompInfoAsync();
            // UpdateScCompInfoWithoutSelect();
            //UpdateScCompInfoWithQuery();
            //UpdateScCompInfoWithQuery1();
            //UpdateCompInfoMulti();

            //DeleteCompInfo();
            //DeleteScCompInfoWithoutSelect();



            //Mapper.CreateMap<ScFaq, FaqViewModel>()
            //       .ForMember(d => d.FAQ_SN, map => map.MapFrom(s => s.FaqSn))
            //       .ForMember(d => d.ANS_TXT, map => map.MapFrom(s => s.AnsTxt))
            //       .ForMember(d => d.QST_TXT, map => map.MapFrom(s => s.QstTxt));



            //ScFaq faq = new ScFaq
            //{
            //    FaqSn = 1,
            //    AnsTxt = "답변",
            //    QstTxt = "질문"
            //};

            //var vm =  Mapper.Map<FaqViewModel>(faq);

            //Console.WriteLine(vm.FAQ_SN + "//" + vm.QST_TXT + "//" + vm.ANS_TXT);

            //Console.ReadLine();


            //    using (var db = new WebDbContext())
            //    {

            //        //    //var scNtc = db.ScNtcs.Where(ntc => ntc.NoticeSn > 3).OrderBy(ntc => ntc.NoticeSn).Take(1).FirstOrDefault();

            //        //    //var scNtc = db.ScNtcs.Where(ntc => ntc.NoticeSn > 3).OrderBy(ntc => ntc.NoticeSn).FirstOrDefault();
            //        //    //Console.WriteLine(scNtc.NoticeSn);

            //        //    db.Configuration.LazyLoadingEnabled = false;



            //        var usr = new ScUsr
            //    {
            //        LoginId = "shinkoooooo",


            //        RegId = "shinkoooooo",


            //    };

            //    usr.ScCompInfo = new ScCompInfo
            //    {
            //        CompNm = "테스트"
            //    };

            //    db.ScUsrs.Add(usr);

            //    //db.SaveChanges();


            //    }



            //using (var context = new WebDbContext())
            //{
            //    //var clientIdParameter = new SqlParameter("@ClientId", 4);

            //    var result = context.Database
            //        .SqlQuery<UspSelectSidoForWebListReturnModel>("USP_SelectSidoForWebList")
            //        .ToList();

            //    foreach(var sido in result)
            //    {
            //        Console.WriteLine(sido.SIDO);
            //    }

            //    Console.ReadLine();
            //}

            //using (var context = new WebDbContext())
            //{
            //    var listRptMatsers = context.ScExpertMappings.Where(em => em.ExpertId == "simpsonz23").Select(sm => sm.ScBizWork).Select(tt => tt.RptMasters).ToList();

            //    IList<RptMaster> rptMaterView = new List<RptMaster>();
            //    foreach (var rptMasters in listRptMatsers)
            //    {
            //        foreach(var rptMaster in rptMasters)
            //        {
            //            rptMaterView.Add(rptMaster);
            //        }
            //    }


            //}

            using (var context = new WebDbContext())
            {
                var listRptMatsers = context.ScCompMappings.Where(cm => cm.CompSn == 94).Select(sm => sm.ScCompInfo).Select(tt => tt.RptMasters).ToList();

                IList<RptMaster> rptMaterView = new List<RptMaster>();
                foreach (var rptMasters in listRptMatsers)
                {
                    foreach (var rptMaster in rptMasters)
                    {
                        rptMaterView.Add(rptMaster);
                    }
                }


            }

            Console.ReadLine();
                
        }







        public static void insertScCompInfo()
        {
            var comp = new ScCompInfo
            {
                Addr1 = "테스트주소",
                CompNm = "테스트회사",
                CompType = "A"
            };

            using (var db = new WebDbContext())
            {

                db.ScCompInfoes.Add(comp);

                //이것도 가능
                //db.Entry<ScCompInfo>(comp).State = System.Data.Entity.EntityState.Added;

                db.SaveChanges();
            }
        }


        public  static async void insertScCompInfoAsync()
        {
            var comp = new ScCompInfo
            {
                Addr1 = "테스트주소",
                CompNm = "테스트회사",
                CompType = "A"
            };

            using (var db = new WebDbContext())
            {

                db.ScCompInfoes.Add(comp);

                //이것도 가능
                //db.Entry<ScCompInfo>(comp).State = System.Data.Entity.EntityState.Added;

                await db.SaveChangesAsync();
            }
        }

        public static void UpdateScCompInfo()
        {
            using (var db = new WebDbContext())
            {
                var compinfo = db.ScCompInfoes.Find(6);
                if (compinfo != null)
                {
                    compinfo.CompNm = "업데이트111";
                }
                else
                {
                    var comp = new ScCompInfo
                    {
                        Addr1 = "테스트업데이트주소",
                        CompNm = "업데이트당ㅇ앙아앙아",
                        CompType = "A"
                    };

                    db.ScCompInfoes.Add(comp);
                }

                db.SaveChanges();

            }
        }

        public static async Task UpdateScCompInfoAsync()
        {
            using (var db = new WebDbContext())
            {

                var compinfo = await db.ScCompInfoes.FindAsync(6);
                if (compinfo != null)
                {
                    compinfo.CompNm = "업데이트111";
                }
                else
                {
                    var comp = new ScCompInfo
                    {
                        Addr1 = "테스트업데이트주소",
                        CompNm = "업데이트당ㅇ앙아앙아",
                        CompType = "A"
                    };

                    db.ScCompInfoes.Add(comp);
                }

                await db.SaveChangesAsync();

            }
        }

        public static void UpdateScCompInfoWithoutSelect()
        {
            var comp = new ScCompInfo
            {
                CompSn = 85
            };

            using (var db = new WebDbContext())
            {
                db.ScCompInfoes.Attach(comp);

                comp.CompNm = "bizon188";

                db.SaveChanges();

            }
        }

        public static void UpdateScCompInfoWithQuery()
        {
            string sql = @"update SC_COMP_INFO set COMP_NM={0} where COMP_TYPE={1}";
            using (var db = new WebDbContext())
            {
                List<Object> sqlParamsList = new List<object>();
                sqlParamsList.Add("test117");
                sqlParamsList.Add("A");

                db.Database.ExecuteSqlCommand(sql, sqlParamsList.ToArray());

               // var entry = db.Entry(new ScCompInfo { CompSn = 10 });

            }

        }

        public static void UpdateScCompInfoWithQuery1()
        {
            string sql = @"update SC_COMP_INFO set COMP_NM=@compNm where COMP_TYPE=@compType";
            using (var db = new WebDbContext())
            {
                System.Data.SqlClient.SqlParameter p1 = new System.Data.SqlClient.SqlParameter("@compNm", "test114");
                System.Data.SqlClient.SqlParameter p2 = new System.Data.SqlClient.SqlParameter("@compType", "A");

              
                db.Database.ExecuteSqlCommand(sql,p1,p2);


  
            }

            

        }



        public static void UpdateCompInfoMulti()
        {
            using (var db = new WebDbContext())
            {
                var comps = db.ScCompInfoes.Where(ci => ci.CompType == "A" && ci.CompNm == "test113").ToList();

                comps.ForEach(cis =>
                {
                    cis.Email = "test@test.com";
                });

                db.SaveChanges();
            }
        }


        public static async void UpdateCompInfoMultiAsync()
        {
            using (var db = new WebDbContext())
            {
                var comps = db.ScCompInfoes.Where(ci => ci.CompType == "A" && ci.CompNm == "test113").ToList();

                comps.ForEach(cis =>
                {
                    cis.Email = "test@test111.com";
                });

                await db.SaveChangesAsync();
            }
        }

        public static  void DeleteCompInfo()
        {
            using (var db = new WebDbContext())
            {
                var compinfo = db.ScCompInfoes.Find(30);

                db.ScCompInfoes.Remove(compinfo);

                db.SaveChanges();
            }
        }

        public static void DeleteScCompInfoWithoutSelect()
        {
            var comp = new ScCompInfo
            {
                CompSn = 30
            };

         
            using (var db = new WebDbContext())
            {
                //db.ScCompInfoes.Attach(comp);

                //db.ScCompInfoes.Remove(comp);

                db.Entry(comp).State = System.Data.Entity.EntityState.Deleted;

                try
                {
                    db.SaveChanges();
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException ex)
                {
                    ex.Entries.Single().Reload();
                }

            }
           
        }


        public static string pwdEncrypt(string password)
        {
            using (var db = new WebDbContext())
            {
                //string sql = @"select PWDENCRYPT({0})";
               // string commandString = @"UPDATE SC_USR set LOGIN_PW = PWDENCRYPT({0}) where LOGIN_ID = {1}";

                string commandString = string.Format(@"UPDATE SC_USR set LOGIN_PW = PWDENCRYPT('{0}') where LOGIN_ID = '{1}'",
                                                      password, "shinkoooooo");

                var result = db.Database.ExecuteSqlCommand(commandString);



                //var result = db.Database.SqlQuery<>(commandString, sqlParamsList.ToArray()).FirstOrDefault();

                //var comps = db.ScUsrs.Where(ci => ci.LoginId == "shinkoooooo").ToList();

                //comps.ForEach(cis =>
                //{
                //    cis.LoginPw = result.;+
                //});

                db.SaveChanges();

                //return System.Text.Encoding.ASCII.GetString(result);
                return "TRUE";
            }


            
        }


        
    }
}
