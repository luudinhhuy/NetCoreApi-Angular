﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Mobile.Common;
using API.Mobile.Models;
using API.Mobile.ViewModel;

namespace API.Mobile.Repository
{
    public class JobRepositoryImpl : IJobRepository
    {
        private List<Job> jobs = FakeData.jobs;
        private List<Stage> stages = FakeData.stages;

        public JobViewModel Get(JobCriteria criteria, string userId, int? offset, int limit = 15)
        {
            var data = Search(criteria, userId);
            var totalItems = data.Count;
            var numberJobFinishs = data.Count(x => x.CurrentStageStatus == StatusEnum.JobStatus.Finish);
            if (offset != null)
            {
                int skip = (int)offset;
                int take = (int)limit;
                data = data.Skip(skip).Take(take).ToList();
            }

            //data.ForEach(x => {
            //    x.NumberStageFinish = stages.Count(y => y.JobId == x.Id && y.Status == StatusEnum.StageStatus.Done);
            //    x.NumberStage = stages.Count(y => y.JobId == x.Id);
            //});
            data.ForEach(x =>
            {
                x.PercentFinish = (decimal)stages.Count(y => y.JobId == x.Id && y.Status == StatusEnum.StageStatus.Done) / (decimal)stages.Count(y => y.JobId == x.Id);
            });
            var result = new JobViewModel { Jobs = data, TotalItems = totalItems, NumberJobFinishs = numberJobFinishs, Offset = offset, Limit = limit };
            return result;
        }

        public Job Get(string id)
        {
            var job = jobs.Find(x => x.Id == id);
            return job;
        }

        public List<Job> GetBy(JobCriteria criteria)
        {
            return Search(criteria, null);
        }

        private List<Job> Search(JobCriteria criteria, string userId)
        {
            var listJob = jobs;
            listJob = listJob.Where(x => ((x.Id ?? "").Contains(criteria.SearchText ?? "") 
                                       || (x.CustomerName ?? "").Contains(criteria.SearchText ?? "")
                                       || (x.PO_NO ?? "").Contains(criteria.SearchText ?? "")
                                       || (x.MBL ?? "").Contains(criteria.SearchText ?? ""))
                                       && (x.UserId == userId || string.IsNullOrEmpty(userId))
                                       && (x.AssignTime >= criteria.FromDate || criteria.FromDate == null)
                                       && (x.AssignTime <= criteria.ToDate || criteria.ToDate == null)
            ).ToList();
            var results = listJob;
            switch (criteria.SearchStatus)
            {
                case StatusEnum.JobStatusSearch.Finish:
                    results = results.Where(x => x.CurrentStageStatus == StatusEnum.JobStatus.Finish).OrderByDescending(x => x.ServiceDate).ToList();
                    break;
                case StatusEnum.JobStatusSearch.InProgess:
                    results = results.Where(x => x.CurrentStageStatus != StatusEnum.JobStatus.Finish).OrderBy(x => x.CurrentStageStatus).ToList();
                    break;
                default:
                    results = results.OrderByDescending(x => x.ServiceDate).ToList();
                    break;
            }

            return results;
        }
    }
}
