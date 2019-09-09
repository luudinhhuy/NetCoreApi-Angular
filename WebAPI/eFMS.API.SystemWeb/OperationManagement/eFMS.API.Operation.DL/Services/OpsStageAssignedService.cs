﻿using AutoMapper;
using eFMS.API.Operation.DL.IService;
using eFMS.API.Operation.DL.Models;
using eFMS.API.Operation.Service.Models;
using eFMS.API.Provider.Services.IService;
using ITL.NetCore.Connection.BL;
using ITL.NetCore.Connection.EF;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using eFMS.API.Provider.Models;
using ITL.NetCore.Common;
using eFMS.API.Operation.Service.Contexts;
using eFMS.API.Operation.DL.Common;
using eFMS.IdentityServer.DL.UserManager;
using eFMS.API.Common.NoSql;
using System.Linq.Expressions;

namespace eFMS.API.Operation.DL.Services
{
    public class OpsStageAssignedService : RepositoryBase<OpsStageAssigned, OpsStageAssignedModel>, IOpsStageAssignedService
    {
        private readonly ICatStageApiService catStageApi;
        private readonly ICurrentUser currentUser;
        private readonly ICatDepartmentApiService catDepartmentApi;
        public OpsStageAssignedService(IContextBase<OpsStageAssigned> repository, IMapper mapper,
            ICatStageApiService stageApi,
            ICurrentUser user,
            ICatDepartmentApiService departmentApi) : base(repository, mapper)
        {
            catStageApi = stageApi;
            currentUser = user;
            catDepartmentApi = departmentApi;
        }

        public HandleState AddMultipleStage(List<OpsStageAssignedEditModel> models, Guid jobId)
        {
            ChangeTrackerHelper.currentUser = currentUser.UserID;
            var result = new HandleState();
            eFMSDataContext dc = (eFMSDataContext)DataContext.DC;
            var stagesByJob = DataContext.Get(x => x.JobId == jobId);
            var listToDelete = stagesByJob.Where(x => !models.Any(model => model.Id == x.Id));
            foreach(var item in models)
            {
                if (item.Id == Guid.Empty)
                {
                    var assignedItem = mapper.Map<OpsStageAssigned>(item);
                    assignedItem.Id = Guid.NewGuid();
                    assignedItem.JobId = jobId;
                    assignedItem.Deadline = item.Deadline ?? null;
                    assignedItem.Status = Constants.InSchedule;
                    assignedItem.CreatedDate = assignedItem.ModifiedDate = DateTime.Now;
                    assignedItem.UserCreated = currentUser.UserID;
                    dc.Add(assignedItem);
                }
                else
                {
                    var assignedToUpdate = dc.OpsStageAssigned.Find(item.Id);
                    assignedToUpdate.UserModified = currentUser.UserID;
                    assignedToUpdate.ModifiedDate = DateTime.Now;
                    assignedToUpdate.OrderNumberProcessed = item.OrderNumberProcessed;
                    dc.Update(assignedToUpdate);
                }
            }
            if(listToDelete.Count() > 0)
            {
                var list = mapper.Map<List<OpsStageAssigned>>(listToDelete);
                dc.RemoveRange(list);
            }
            try
            {
                dc.SaveChanges();
            }
            catch (Exception ex)
            {
                result = new HandleState(ex.Message);
            }
            return result;
        }

        public OpsStageAssignedModel GetBy(Guid id)
        {
            var data = DataContext.First(x => x.Id == id);
            if (data == null) return null;
            var stages = catStageApi.GetAll().Result;
            var departments = catDepartmentApi.GetAll().Result;
            var result = mapper.Map<OpsStageAssignedModel>(data);
            if(stages != null)
            {
                var stage = stages?.FirstOrDefault(x => x.Id == result.StageId);
                result.StageCode = stages.FirstOrDefault(x => x.Id == result.StageId).Code;
                result.StageNameEN = stages.FirstOrDefault(x => x.Id == result.StageId).StageNameEn;
                result.DepartmentName = departments?.FirstOrDefault(x => x.Id == stage.DepartmentId)?.DeptName;
                result.Description = result.Description != null ? result.Description : stages.FirstOrDefault(x => x.Id == result.StageId).DescriptionEn;
            }
            return result;
        }

        public List<OpsStageAssignedModel> GetByJob(Guid jobId)
        {
            List<OpsStageAssignedModel> results = null;
            var data = DataContext.Get(x => x.JobId == jobId);
            if (data == null) return null;
            results = MapListToModel(data);
            return results;
        }

        public List<OpsStageAssignedModel> GetNotAssigned(Guid jobId)
        {
            var data = DataContext.Get(x => x.JobId == jobId);
            var stages = catStageApi.GetAll().Result;
            if (stages == null) return null;
            var results = stages.Where(x => !data.Any(assigned => assigned.StageId == x.Id))
                .Select(x => new OpsStageAssignedModel {
                    Id = Guid.Empty,
                    StageId = x.Id,
                    Name = string.Empty,
                    StageCode = x.Code,
                    StageNameEN = x.StageNameEn
                }).ToList();
            return results;
        }

        public HandleState Update(OpsStageAssignedEditModel model)
        {
            var assigned = mapper.Map<OpsStageAssigned>(model);
            assigned.UserModified = currentUser.UserID;
            assigned.ModifiedDate = DateTime.Now;
            eFMSDataContext dc = (eFMSDataContext)DataContext.DC;
            var stageAssigneds = DataContext.Get(x => x.JobId == model.JobId);
            var job = dc.OpsTransaction.Find(model.JobId);
            if (job.CurrentStatus != Constants.Deleted && job.CurrentStatus != Constants.Finish)
            {
                if (assigned.Status?.Trim() == DataTypeEx.GetStageStatus(StageEnum.Overdue))
                {
                    job.CurrentStatus = Constants.Overdue;
                }
                if (assigned.Status.Contains(DataTypeEx.GetStageStatus(StageEnum.Done)))
                {
                    var others = stageAssigneds.Where(x => x.Id != model.Id);
                    if(others.All(x => x.Status.Contains(Constants.Done)))
                    {
                        job.CurrentStatus = Constants.Finish;
                    }
                }
                if(job.CurrentStatus?.Trim() == Constants.InSchedule && assigned.Status.Trim() == Constants.Processing)
                {
                    job.CurrentStatus = Constants.Processing;
                }
            }
            var result = new HandleState();
            try
            {
                result = DataContext.Update(assigned, x => x.Id == assigned.Id, false);
                if (result.Success)
                {
                    dc.OpsTransaction.Update(job);
                }
                dc.SaveChanges();
            }
            catch (Exception ex)
            {
                result = new HandleState(ex.Message);
            }
            return result;
        }

        private List<OpsStageAssignedModel> MapListToModel(IQueryable<OpsStageAssigned> data)
        {
            var stages = catStageApi.GetAll().Result;
            var departments = catDepartmentApi.GetAll().Result;
            var results = new List<OpsStageAssignedModel>();
            foreach (var item in data)
            {
                var stage = stages?.FirstOrDefault(x => x.Id == item.StageId);
                var assignedItem = mapper.Map<OpsStageAssignedModel>(item);
                assignedItem.StageCode = stage?.Code;
                assignedItem.StageNameEN = stage?.StageNameEn;
                assignedItem.Status = assignedItem.Status?.Trim();
                assignedItem.DepartmentName = stage == null? null: departments?.FirstOrDefault(x => x.Id == stage.DepartmentId)?.DeptName;
                assignedItem.DoneDate = item.Status?.Trim() == DataTypeEx.GetStageStatus(StageEnum.Done) ? item.ModifiedDate : null;
                assignedItem.Description = assignedItem.Description != null ? assignedItem.Description : stages.FirstOrDefault(x => x.Id == item.StageId).DescriptionEn;
                results.Add(assignedItem);
            }
            return results;
        }
    }
}
