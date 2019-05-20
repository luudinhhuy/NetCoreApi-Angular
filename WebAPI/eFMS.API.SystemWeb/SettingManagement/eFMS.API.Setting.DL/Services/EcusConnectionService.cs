﻿using eFMS.API.Setting.DL.IService;
using eFMS.API.Setting.DL.Models;
using eFMS.API.Setting.Service.Contexts;
using eFMS.API.Setting.Service.Models;
using ITL.NetCore.Connection.EF;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using ITL.NetCore.Connection.BL;
using AutoMapper;
using eFMS.API.Setting.DL.Models.Criteria;
using System.Data;
using eFMS.API.Setting.DL.Models.Ecus;
using eFMS.API.Setting.DL.Helpers;
using eFMS.API.Provider.Services.IService;

namespace eFMS.API.Setting.DL.Services
{
    public class EcusConnectionService : RepositoryBase<SetEcusconnection, SetEcusConnectionModel>, IEcusConnectionService
    {
        private ICatAreaApiService catAreaApi;
        public EcusConnectionService(IContextBase<SetEcusconnection> repository, IMapper mapper, ICatAreaApiService apiService) : base(repository, mapper)
        {
            catAreaApi = apiService;
        }

        public SetEcusConnectionModel GetConnectionDetails(int id)
        {
            eFMSDataContext dc = (eFMSDataContext)DataContext.DC;
            SetEcusConnectionModel EcusConnection = new SetEcusConnectionModel();
            var con = dc.SetEcusconnection.Where(x => x.Id == id).FirstOrDefault();
            if (con == null)
            {
                return null;
            }
            else
            {
                EcusConnection = mapper.Map<SetEcusConnectionModel>(con);
                var user = dc.SysUser.Where(x => x.Id == con.UserId).FirstOrDefault();
                var user_created = dc.SysUser.Where(x => x.Id == con.UserCreated).FirstOrDefault();
                var user_modified = dc.SysUser.Where(x => x.Id == con.UserModified).FirstOrDefault();
                EcusConnection.Username = user?.Username;
                EcusConnection.UserCreatedName = user_created?.Username;
                EcusConnection.UserModifiedName = user_modified?.Username;
                
            }
            return EcusConnection;
        }

        public List<SetEcusConnectionModel> GetConnections()
        {
            List<SetEcusConnectionModel> returnList = new List<SetEcusConnectionModel>();
            eFMSDataContext dc = (eFMSDataContext)DataContext.DC;
            var cons = dc.SetEcusconnection.ToList();
            var query = (from con in cons
                         join user in dc.SysUser on con.UserId equals user.Id into users
                         from u in users
                         join em in dc.SysEmployee on u.EmployeeId equals em.Id

                         select new {con,u,em}
                );
            if (query == null)
            {
                return returnList;
            }
            foreach(var item in query)
            {
                SetEcusConnectionModel ecus = mapper.Map<SetEcusConnectionModel>(item.con);
                ecus.Username = item.u.Username;
                ecus.Fullname = item.em.EmployeeNameEn;
                returnList.Add(ecus);
            }
            return returnList;
        }

        public List<SetEcusConnectionModel> Paging(SetEcusConnectionCriteria criteria, int pageNumber, int pageSize, out int totalItems)
        {
            var list = Query(criteria);
            totalItems = list.Count;
            if (pageSize > 1)
            {
                if (pageNumber < 1)
                {
                    pageNumber = 1;
                }
                list = list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }
            return list;
        }

        private List<SetEcusConnectionModel> Query(SetEcusConnectionCriteria criteria)
        {
            List<SetEcusConnectionModel> list = GetConnections();
            if (criteria.All == null)
            {
                list = list.Where(x => ((x.Username ?? "").Contains(criteria.Username ?? "")
                && ((x.Name??"").Contains(criteria.Name??"")
                && ((x.ServerName??"").Contains(criteria.ServerName??""))
                && ((x.Dbname??"").Contains(criteria.Dbname??"")))
                )).ToList();
            }
            else
            {
                list = list.Where(x => ((x.Username ?? "").Contains(criteria.All ?? "")
                 || ((x.Name ?? "").Contains(criteria.All ?? "")
                 || ((x.ServerName ?? "").Contains(criteria.All ?? ""))
                 || ((x.Dbname ?? "").Contains(criteria.All ?? "")))
               )).ToList();
            }

            return list;
        }

        public List<DTOKHAIMD> GetDataEcusByUser(string userId, string serverName, string dbusername, string dbpassword, string database)
        {
            List<DTOKHAIMD> results = null;
            if (DataContext.Any(x => x.UserId == userId))
            {
                results = GetDataFromEcus(serverName, dbusername, dbpassword, database);
            }
            return results;
        }

        public object Test(string token)
        {
            var results = catAreaApi.GetAreas(token).Result.ToList();
            return results;
        }

        private List<DTOKHAIMD> GetDataFromEcus(string serverName, string dbusername, string dbpassword, string database)
        {
            string queryString = "SELECT * FROM [ECUS5VNACCS].[dbo].[DTOKHAIMD]";
            string connectionString = @"Server=" + serverName + ",1433; Database=" + database + "; User ID=" + dbusername + "; Password=" + dbpassword;
            var data = Helpers.Helper.ExecuteDataSet(connectionString, queryString);
            if (data != null)
            {
                DataTable dt = data.Tables[0];
                return dt.DataTableToList<DTOKHAIMD>();
            }
            return null;
        }
    }
}
