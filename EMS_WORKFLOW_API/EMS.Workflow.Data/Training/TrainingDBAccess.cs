using EMS.Workflow.Data.Accountability;
using EMS.Workflow.Data.DBContexts;
using EMS.Workflow.Data.LogActivity;
using EMS.Workflow.Transfer.Accountability;
using EMS.Workflow.Transfer.Training;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Workflow.Data.Training
{
    public interface ITrainingDBAccess
    {
        Task<IEnumerable<TableVarTableTemplate>> GetList(GetListInput param, int rowStart);
        Task<bool> Add(TrainingTemplate param,List<TrainingTemplateDetails> paramAdd);

        Task<bool> Edit(TrainingTemplate param, List<TrainingTemplateDetails> paramAdd, 
            List<TrainingTemplateDetails> paramEdit, List<TrainingTemplateDetails> paramDelete);
        Task<TrainingTemplate> GetByID(int ID);
        Task<IEnumerable<TrainingTemplate>> GetAllTrainingTemplate();
        Task<IEnumerable<TrainingTemplateDetails>> GetDetailsByTrainingTemplateID(int ID);
        Task<IEnumerable<TableVarEmployeeTraining>> GetEmployeeTrainingList(GetEmployeeTrainingListInput param, int rowStart);
        Task<IEnumerable<EmployeeTrainingStatusHistory>> AddEmployeeTrainingTemplate(AddEmployeeTrainingInput param, int CreatedBy);
        Task<bool> AddEmployeeTraining(EmployeeTraining param, EmployeeTrainingStatusHistory paramStatusHistory);
        Task<bool> EditEmployeeTraining(EmployeeTraining param);
        Task<EmployeeTraining> GetEmployeeTrainingByID(long ID);
        Task<bool> UploadInsert(List<TrainingUploadFile> param, APICredentials credentials);
        Task<List<EmployeeTraining>> GetEmployeeTrainingByIDs(List<long> ID);
        Task<bool> ChangeStatus(List<EmployeeTraining> param, List<EmployeeTrainingStatusHistory> paramStatusHistory);
        Task<IEnumerable<TableVarEmployeeTrainingStatusHistory>> GetEmployeeTrainingStatusHistory(int EmployeeTrainingID);
        Task<IEnumerable<TableVarEmployeeTrainingScore>> GetEmployeeTrainingScore(int EmployeeTrainingID);
        Task<IEnumerable<EmployeeTraining>> GetEmployeeByEmployeeTrainingIDs(List<int> EmployeeTrainingID);
        Task<IEnumerable<EmployeeTraining>> GetEmployeeTrainingByEmployeeIDTitle(int EmployeeID,string Title);
    }

    public class TrainingDBAccess : ITrainingDBAccess
    {
        private readonly WorkflowContext _dbContext;

        public TrainingDBAccess(WorkflowContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TableVarTableTemplate>> GetList(GetListInput param, int rowStart)
        {
            return await _dbContext.TableVarTableTemplate
                .FromSqlRaw(@"CALL sp_training_template_get_list(
                              {0}
                            , {1}
                            , {2}
                            , {3}
                            , {4}
                            , {5}
                            , {6}
                            , {7}
                            , {8}
                    )", param.ID ?? 0
                      , param.TemplateName ?? ""
                      , param.CreatedDateFrom ?? ""
                      , param.CreatedDateTo ?? ""
                      , param.IsExport
                      , param.sidx ?? ""
                      , param.sord ?? ""
                      , rowStart
                      , param.rows)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<bool> Add(TrainingTemplate param, List<TrainingTemplateDetails> paramAdd)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.TrainingTemplate.AddAsync(param);
                await _dbContext.SaveChangesAsync();

                if (paramAdd != null)
                {
                    await _dbContext.TrainingTemplateDetails.AddRangeAsync(paramAdd
                        .Select(x => { x.TrainingTemplateID = param.ID; return x; }));
                }

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }
        public async Task<bool> Edit(TrainingTemplate param, List<TrainingTemplateDetails> paramAdd,
                         List<TrainingTemplateDetails> paramEdit, List<TrainingTemplateDetails> paramDelete)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Entry(param).State = EntityState.Modified;

                if (paramAdd != null)
                {
                    _dbContext.TrainingTemplateDetails.AddRange(paramAdd);
                }

                if (paramEdit != null)
                {
                    paramEdit.Select(x =>
                    {
                        _dbContext.Entry(x).State = EntityState.Modified;
                        return x;
                    }).ToList();
                }

                if (paramDelete != null)
                {
                    paramDelete.Select(x =>
                    {
                        _dbContext.Entry(x).State = EntityState.Modified;
                        return x;
                    }).ToList();
                }

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }

            return true;
        }
        public async Task<IEnumerable<TrainingTemplate>> GetAllTrainingTemplate()
        {
            return await _dbContext.TrainingTemplate.AsNoTracking()
                .Where(x => x.IsActive).ToListAsync();
        }
        public async Task<TrainingTemplate> GetByID(int ID)
        {
            return await _dbContext.TrainingTemplate.FindAsync(ID);
        }
        public async Task<IEnumerable<TrainingTemplateDetails>> GetDetailsByTrainingTemplateID(int ID)
        {
            return await _dbContext.TrainingTemplateDetails.AsNoTracking()
                .Where(x => x.IsActive && x.TrainingTemplateID == ID)
                .ToListAsync();
        }
        public async Task<IEnumerable<TableVarEmployeeTraining>> GetEmployeeTrainingList(GetEmployeeTrainingListInput param, int rowStart)
        {
            return await _dbContext.TableVarEmployeeTraining
                .FromSqlRaw(@"CALL sp_employee_training_get_list(
                              {0}
                            , {1}
                            , {2}
                            , {3}
                            , {4}
                            , {5}
                            , {6}
                            , {7}
                            , {8}
                            , {9}
                            , {10}
                            , {11}
                            , {12}
                            , {13}
                            , {14}
                            , {15}
                            , {16}
                            , {17}
                            , {18}
                            , {19}
                            , {20}
                            , {21}
                            , {22}
                            , {23}
                    )", param.ID ?? 0
                      , param.EmployeeID ?? ""
                      , param.StatusDelimited ?? ""
                      , param.StatusUpdateDateFrom ?? ""
                      , param.StatusUpdateDateTo ?? ""
                      , param.DateScheduleFrom ?? ""
                      , param.DateScheduleTo ?? ""
                      , param.TypeDelimited ?? ""
                      , param.Title ?? ""
                      , param.Description ?? ""
                      , param.IsAdminAccess
                      , param.IsResolver
                      , param.IsExport
                      , param.CreatedBy ?? ""
                      , param.CreatedDateFrom ?? ""
                      , param.CreatedDateTo ?? ""
                      , param.ModifiedBy ?? ""
                      , param.ModifiedDateFrom ?? ""
                      , param.ModifiedDateTo ?? ""
                      , param.sidx ?? ""
                      , param.sord ?? ""
                      , rowStart
                      , param.rows
                      , param.UnderAccess ?? "")
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<IEnumerable<EmployeeTrainingStatusHistory>> AddEmployeeTrainingTemplate(AddEmployeeTrainingInput param, int CreatedBy)
        {
            return await _dbContext.EmployeeTrainingStatusHistory
                .FromSqlRaw(@"CALL sp_add_employee_training_template(
                      {0}
                    , {1}
                    , {2}
                    )"
                    , param.EmployeeID
                    , string.Join(",",param.TrainingEmployeeID)
                    , CreatedBy)
                .AsNoTracking().ToListAsync();
        }

        public async Task<bool> AddEmployeeTraining(EmployeeTraining param, EmployeeTrainingStatusHistory paramStatusHistory)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.EmployeeTraining.AddAsync(param);
                await _dbContext.SaveChangesAsync();

                paramStatusHistory.EmployeeTrainingID = param.ID;
                await _dbContext.EmployeeTrainingStatusHistory.AddAsync(paramStatusHistory);
                await _dbContext.SaveChangesAsync();

                transaction.Commit();
            }
            return true;
        }
        public async Task<bool> UploadInsert(List<TrainingUploadFile> param, APICredentials credentials)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                DateTime dateTime = DateTime.Now;

                List<EmployeeTraining> training = param.Select(x => new EmployeeTraining
                {
                    EmployeeID = x.EmployeeID,
                    Status = x.Status,
                    StatusUpdateDate = dateTime,
                    Type = x.Type,
                    Title = x.Title,
                    Description = x.Description,
                    DateSchedule = x.DateScheduleConvert,
                    ClassroomID = x.ClassroomID,
                    ClassroomName = x.ClassroomName,
                    IsActive = true,
                    CreatedBy = credentials.UserID,
                    CreatedDate = dateTime
                }).ToList();

                await _dbContext.EmployeeTraining.AddRangeAsync(training);
                await _dbContext.SaveChangesAsync();

                await _dbContext.EmployeeTrainingStatusHistory.AddRangeAsync(training
                    .Join(param,
                    x => new { x.EmployeeID, x.Type, x.Title },
                    y => new { y.EmployeeID, y.Type, y.Title },
                    (x, y) => new { training = x, param = y })
                    .Select(y => new EmployeeTrainingStatusHistory
                    {
                        EmployeeTrainingID = y.training.ID,
                        Status = y.param.Status,
                        IsActive = true,
                        CreatedBy = credentials.UserID,
                        CreatedDate = dateTime
                    }).ToList());

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<bool> EditEmployeeTraining(EmployeeTraining param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.EmployeeTraining.Update(param);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<EmployeeTraining> GetEmployeeTrainingByID(long ID)
        {
            return await _dbContext.EmployeeTraining.AsNoTracking().Where(x => x.ID == ID).FirstOrDefaultAsync();
        }

        public async Task<List<EmployeeTraining>> GetEmployeeTrainingByIDs(List<long> ID)
        {
            return await _dbContext.EmployeeTraining.AsNoTracking()
                .Where(x => ID.Contains(x.ID))
                .ToListAsync();
        }

        public async Task<bool> ChangeStatus(List<EmployeeTraining> param, List<EmployeeTrainingStatusHistory> paramStatusHistory)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.EmployeeTraining.UpdateRange(param);
                await _dbContext.EmployeeTrainingStatusHistory.AddRangeAsync(paramStatusHistory);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }
        public async Task<IEnumerable<TableVarEmployeeTrainingStatusHistory>> GetEmployeeTrainingStatusHistory(int EmployeeTrainingID)
        {
            return await _dbContext.TableVarEmployeeTrainingStatusHistory
               .FromSqlRaw("CALL sp_employee_training_status_history_get({0})", EmployeeTrainingID)
               .AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<TableVarEmployeeTrainingScore>> GetEmployeeTrainingScore(int EmployeeTrainingID)
        {
            return await _dbContext.TableVarEmployeeTrainingScore
               .FromSqlRaw("CALL sp_employee_training_score_get({0})", EmployeeTrainingID)
               .AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<EmployeeTraining>> GetEmployeeByEmployeeTrainingIDs(List<int> EmployeeTrainingID)
        {
            return await _dbContext.EmployeeTraining.AsNoTracking()
                .Where(x => x.IsActive && EmployeeTrainingID.Contains(x.ID)).ToListAsync();
        }
        public async Task<IEnumerable<EmployeeTraining>> GetEmployeeTrainingByEmployeeIDTitle(int EmployeeID, string Title)
        {
            return await _dbContext.EmployeeTraining.AsNoTracking()
                .Where(x => x.IsActive && x.EmployeeID.Equals(EmployeeID) && x.Title.Equals(Title)).ToListAsync();
        }
    }
}
