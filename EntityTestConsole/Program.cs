﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizOneShot.Light.Dao.Mappings;
using BizOneShot.Light.Models;

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
            //UpdateScCompInfoWithoutSelect();
            //UpdateScCompInfoWithQuery();
            //UpdateScCompInfoWithQuery1();
            //UpdateCompInfoMulti();

            //DeleteCompInfo();
            //DeleteScCompInfoWithoutSelect();
            SelectFaq();


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
                CompSn = 6
            };

            using (var db = new WebDbContext())
            {
                db.ScCompInfoes.Attach(comp);

                comp.CompNm = "test112";

                db.SaveChanges();

            }
        }

        public static void UpdateScCompInfoWithQuery()
        {
            string sql = @"update SC_COMP_INFO set COMP_NM={0} where COMP_TYPE={1}";
            using (var db = new WebDbContext())
            {
                List<Object> sqlParamsList = new List<object>();
                sqlParamsList.Add("test113");
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

        public static void SelectFaq()
        {
            using (var db = new WebDbContext())
            {
                //db.ScCompInfoes.Attach(comp);

                //db.ScCompInfoes.Remove(comp);

                var faqs = from m in db.ScFaqs select m;

                faqs = faqs.Where(s => s.QstTxt.Contains("2"));

                foreach(var od in faqs)
                {

                    Console.WriteLine(od.QstTxt);
                }

            }

        }




    }
}
