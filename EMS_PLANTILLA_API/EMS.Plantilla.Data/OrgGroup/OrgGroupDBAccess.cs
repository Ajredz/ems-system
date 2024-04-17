using EMS.Plantilla.Data.DBContexts;
using EMS.Plantilla.Transfer.OrgGroup;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace EMS.Plantilla.Data.OrgGroup
{
    public interface IOrgGroupDBAccess
    {
        Task<IEnumerable<TableVarOrgGroup>> GetList(GetListInput input, int rowStart);

        Task<OrgGroup> GetByID(int ID);

        Task<IEnumerable<OrgGroup>> GetByParentOrgID(int ID);

        Task<IEnumerable<OrgGroupPosition>> GetOrgGroupPosition(int ID);

        Task<IEnumerable<OrgGroup>> GetByOrgType(string OrgType);

        Task<IEnumerable<TableVarOrgGroupPosition>> GetOrgGroupPositionByOrgID(int ID);

        Task<IEnumerable<OrgGroup>> GetByCode(string Code);

        Task<IEnumerable<OrgGroup>> GetByCodes(List<string> Codes);

        Task<bool> Post(OrgGroup param, OrgGroupHistory paramHistory, List<int> orgChild,
            List<OrgGroupPosition> orgPosition, List<OrgGroupTag> orgGroupTag);

        Task<bool> Put(OrgGroup param, List<int> childToUpdate, List<int> childToRemove,
            List<OrgGroupPosition> orgPositionToAdd, List<OrgGroupPosition> orgPositionToUpdate, List<OrgGroupPosition> orgPositionToDelete,
            List<OrgGroupTag> orgTagToAdd, List<OrgGroupTag> orgTagToUpdate, List<OrgGroupTag> orgTagToDelete, List<OrgGroupNPRF> orgGroupNPRF);

        Task<bool> Put(OrgGroup param);

        Task<bool> Delete(int ID);

        Task<IEnumerable<OrgGroup>> GetAll();

        Task<IEnumerable<OrgGroup>> GetChart(GetChartInput param);

        Task<IEnumerable<TableVarOrgChartPosition>> GetChartPosition(GetChartInput param);

        Task<IEnumerable<OrgGroup>> GetLastModified(DateTime? From, DateTime? To);

        Task<IEnumerable<SyncOrgGroupToH2Pay>> GetLastModifiedH2Pay(DateTime? From, DateTime? To);

        Task<IEnumerable<OrgGroupPosition>> GetLastModifiedOrgGroupPosition(DateTime? From, DateTime? To);

        Task<IEnumerable<OrgGroupTag>> GetTagsByOrgGroupID(int OrgGroupID);

        Task<bool> UploadInsert(List<OrgGroupEntity> param);

        Task<bool> UploadEdit(List<OrgGroup> param, List<OrgGroupPosition> orgPositionToAdd,
            List<OrgGroupPosition> orgPositionToUpdate, /*List<OrgGroupPosition> orgPositionToDelete,*/
            List<OrgGroupTag> orgTagToAdd, List<OrgGroupTag> orgTagToUpdate/*, List<OrgGroupTag> orgTagToDelete*/);

        Task<IEnumerable<OrgGroupPosition>> GetOrgGroupPositionByOrgCodes(List<string> Codes);

        Task<IEnumerable<OrgGroupTag>> GetOrgGroupTagByOrgCodes(List<string> Codes);

        Task<IEnumerable<TableVarPlantillaCount>> GetExportCountByOrgTypeData(GetPlantillaCountInput input, int rowStart);

        Task<OrgGroupPosition> GetByOrgGroupAndPosition(int OrgGroupID, int PositionID);

        Task<IEnumerable<TableVarOrgGroupExportList>> GetExportList(GetListInput input);

        Task<IEnumerable<TableVarOrgGroupHierarchy>> GetOrgGroupHierarchy(int OrgGroupID);
        Task<IEnumerable<TableVarOrgGroupHierarchy>> GetOrgGroupHierarchyBomd(int OrgGroupID);

        Task<IEnumerable<TableVarOrgGroupEmployee>> GetOrgGroupEmployeeList(GetEmployeeListInput input, int rowStart);

        Task<IEnumerable<OrgGroup>> GetByOrgGroupAutoComplete(GetByOrgTypeAutoCompleteInput param);

        Task<IEnumerable<OrgGroup>> GetAutoComplete(GetAutoCompleteInput param);

        Task<IEnumerable<TableVarHierarchyLevel>> GetOrgGroupHierarchyLevels();

        Task<IEnumerable<OrgGroupNPRF>> GetOrgGroupNPRF(int ID);

        Task<IEnumerable<int>> GetDescendants(int OrgGroupID);
        Task<IEnumerable<int>> GetOrgGroupDescendantsList(List<int> OrgGroupIDs);
        Task<IEnumerable<int>> GetDescendantsBomd(int OrgGroupID);

        Task<bool> UpdatePlantillaCount(List<OrgGroupPosition> orgPositionToUpdate);

        Task<bool> AddNPRF(List<OrgGroupNPRF> param);

        Task<IEnumerable<OrgGroup>> GetByIDs(List<int> IDs);

        Task<IEnumerable<OrgGroupPosition>> GetHierarchy(int? OrgGroupID, int PositionID);

        Task<IEnumerable<TableVarOrgGroupRollupPositionDropdown>> GetOrgGroupRollupPositionDropdown(int OrgGroupID);

        Task<TableVarGetRegionByOrgGroupID> GetRegionByOrgGroupID(int OrgGroupID);

        Task<IEnumerable<TableVarPositionOrgGroupUpwardAutocomplete>> GetPositionUpwardAutoComplete(GetPositionOrgGroupUpwardAutoCompleteInput param);
        Task<IEnumerable<TableVarPositionOrgGroupUpwardAutocomplete>> GetOrgGroupUpwardAutoComplete(GetPositionOrgGroupUpwardAutoCompleteInput param);
        Task<IEnumerable<TableVarOrgGroupBranchInfo>> GetBranchInfoList(GetBranchInfoListInput input, int rowStart);
        Task<IEnumerable<TableVarOrgGroupHistoryList>> GetOrgGroupHistoryList(OrgGroupHistoryListInput param, int rowStart);
        Task<IEnumerable<TableVarOrgGroupHistoryByDate>> GetOrgGroupHistoryByDate(OrgGroupHistoryByDateInput param, int rowStart);
        Task<bool> AddOrgGroupHistory(List<Data.OrgGroup.OrgGroupHistory> OrgGroupHistory, bool IsLatest, List<Data.OrgGroup.OrgGroupHistory> UpdateOrgGroupHistory);
        Task<bool> AddOrgGroup(List<Data.OrgGroup.OrgGroup> OrgGroup); // Ginger - Separated this method due to prod error
        Task<IEnumerable<OrgGroupHistory>> GetAllHistory();
        Task<IEnumerable<TableVarOrgGroupFormat>> GetOrgGroupFormatByID(List<int> ID);
        Task<IEnumerable<TableVarOrgGroupFormat>> GetOrgGroupParent(GetOrgGroupParentInput param);
        Task<IEnumerable<TableVarOrgGroupSOMD>> GetOrgGroupSOMD(List<int> IDs);
    }

    public class OrgGroupDBAccess : IOrgGroupDBAccess
    {
       private readonly PlantillaContext _dbContext;

        public OrgGroupDBAccess(PlantillaContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TableVarOrgGroup>> GetList(GetListInput input, int rowStart)
        {
            return await _dbContext.TableVarOrgGroup
               .FromSqlRaw(@"CALL sp_org_group_get_list(
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
                        )",
                               input.ID ?? 0
                            , input.Code ?? ""
                            , input.Description ?? ""
                            , input.OrgTypeDelimited ?? ""
                            , input.ParentOrgDescription ?? ""
                            , input.IsBranchActive ?? ""
                            , input.ServiceBayCountMin ?? -1
                            , input.ServiceBayCountMax ?? -1
                            , input.sidx ?? ""
                            , input.sord ?? ""
                            , rowStart
                            , input.rows
                            , input.AdminAccess != null ? input.AdminAccess.OrgGroupDescendantsDelimited : "0"
                            , input.AdminAccess != null ? input.AdminAccess.IsAdminAccess : false
                        )
               .AsNoTracking()
               .ToListAsync();
        }

        public async Task<OrgGroup> GetByID(int ID)
        {
            return await _dbContext.OrgGroup.FindAsync(ID);
        }

        public async Task<IEnumerable<OrgGroup>> GetByParentOrgID(int ID)
        {
            return await _dbContext.OrgGroup.AsNoTracking()
                .Where(x => (x.ParentOrgID == ID || x.CSODAM == ID || x.HRBP == ID) & x.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<OrgGroup>> GetByOrgType(string OrgType)
        {
            return await _dbContext.OrgGroup.AsNoTracking().Where(x => x.OrgType.Equals(OrgType) & x.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<OrgGroupPosition>> GetOrgGroupPosition(int ID)
        {
            return await _dbContext.OrgGroupPosition.AsNoTracking()
                .Where(x => x.OrgGroupID == ID & x.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<TableVarOrgGroupPosition>> GetOrgGroupPositionByOrgID(int ID)
        {
            return await _dbContext.TableVarOrgGroupPosition.FromSqlRaw("CALL sp_org_group_position_get_list({0})", ID)
                .AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<OrgGroup>> GetByCode(string Code)
        {
            return await _dbContext.OrgGroup.AsNoTracking()
                .Where(x => x.Code.Equals(Code, StringComparison.CurrentCultureIgnoreCase) & x.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<OrgGroup>> GetByCodes(List<string> Codes)
        {
            return await _dbContext.OrgGroup.AsNoTracking()
                .Where(x => Codes.Contains(x.Code)).Where(x => x.IsActive)
                .ToListAsync();
        }

        public async Task<bool> Post(OrgGroup param, OrgGroupHistory paramHistory, List<int> orgChild,
            List<OrgGroupPosition> orgPosition, List<OrgGroupTag> orgGroupTag)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.OrgGroup.AddAsync(param);
                await _dbContext.OrgGroupHistory.AddAsync(paramHistory);
                await _dbContext.SaveChangesAsync();

                if (orgChild != null)
                {
                    IEnumerable<OrgGroup> UpdateChildOrgGroup = (await _dbContext.OrgGroup.Where(x => orgChild.Contains(x.ID))
                    .ToListAsync())
                    .Select(x =>
                    {
                        x.ParentOrgID = param.ID;
                        return x;
                    });

                    UpdateChildOrgGroup
                    .Select(x =>
                    {
                        _dbContext.Entry(x).State = EntityState.Modified;
                        return x;
                    }).ToList();
                }

                if (orgPosition != null)
                {
                    await _dbContext.OrgGroupPosition.AddRangeAsync(orgPosition.Select(x => { x.OrgGroupID = param.ID; return x; }));
                }

                if (orgGroupTag != null)
                {
                    await _dbContext.OrgGroupTag.AddRangeAsync(orgGroupTag.Select(x => { x.OrgGroupID = param.ID; return x; }));
                }

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<bool> Put(OrgGroup param, List<int> childToUpdate, List<int> childToRemove,
            List<OrgGroupPosition> orgPositionToAdd, List<OrgGroupPosition> orgPositionToUpdate, List<OrgGroupPosition> orgPositionToDelete,
            List<OrgGroupTag> orgTagToAdd, List<OrgGroupTag> orgTagToUpdate, List<OrgGroupTag> orgTagToDelete, List<OrgGroupNPRF> orgGroupNPRF)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Entry(param).State = EntityState.Modified;

                if (childToUpdate != null)
                {
                    var UpdateChildOrgGroup = (await _dbContext.OrgGroup.Where(x => childToUpdate.Contains(x.ID))
                    .ToListAsync())
                    .Select(x =>
                    {
                        x.ParentOrgID = param.ID;
                        x.ModifiedBy = param.ModifiedBy;
                        x.ModifiedDate = param.ModifiedDate;
                        return x;
                    });

                    UpdateChildOrgGroup
                    .Select(x =>
                    {
                        _dbContext.Entry(x).State = EntityState.Modified;
                        return x;
                    }).ToList();
                }

                if (childToRemove != null)
                {
                    var RemoveChildOrgGroup = (await _dbContext.OrgGroup.Where(x => childToRemove.Contains(x.ID))
                    .ToListAsync())
                    .Select(x =>
                    {
                        x.ParentOrgID = 0;
                        x.ModifiedBy = param.ModifiedBy;
                        x.ModifiedDate = param.ModifiedDate;
                        return x;
                    });

                    RemoveChildOrgGroup
                    .Select(x =>
                    {
                        _dbContext.Entry(x).State = EntityState.Modified;
                        return x;
                    }).ToList();
                }

                _dbContext.OrgGroupPosition.AddRange(orgPositionToAdd);
                //_dbContext.OrgGroupPosition.RemoveRange(orgPositionToDelete);
                orgPositionToDelete.Select(x =>
                {
                    _dbContext.Entry(x).State = EntityState.Modified;
                    return x;
                }).ToList();
                
                orgPositionToUpdate.Select(x =>
                {
                    _dbContext.Entry(x).State = EntityState.Modified;
                    return x;
                }).ToList();

                _dbContext.OrgGroupTag.AddRange(orgTagToAdd);
                _dbContext.OrgGroupTag.RemoveRange(orgTagToDelete);
                orgTagToUpdate.Select(x =>
                {
                    _dbContext.Entry(x).State = EntityState.Modified;
                    return x;
                }).ToList();

                if (orgGroupNPRF != null)
                {
                    _dbContext.OrgGroupNPRF.AddRange(orgGroupNPRF.Select(x => { x.OrgGroupID = param.ID; return x; }));
                }

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }

            return true;
        }

        public async Task<bool> Delete(int ID)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                int ParentID = _dbContext.OrgGroup.Where(x => x.ID == ID).Select(x => x.ParentOrgID).FirstOrDefault();

                IEnumerable<OrgGroup> UpdateChildOrgGroup = (await _dbContext.OrgGroup.Where(x => x.ParentOrgID == ID)
                   .ToListAsync())
                   .Select(x =>
                   {
                       x.ParentOrgID = ParentID;
                       return x;
                   });

                if (UpdateChildOrgGroup != null)
                {
                    UpdateChildOrgGroup
                        .Select(x =>
                        {
                            _dbContext.Entry(x).State = EntityState.Modified;
                            return x;
                        }).ToList();
                }

                IEnumerable<OrgGroupPosition> OrgGroupPositionList = (await _dbContext.OrgGroupPosition.AsNoTracking()
                    .Where(x => x.OrgGroupID == ID & x.IsActive)
                    .ToListAsync());

                if (OrgGroupPositionList != null)
                {
                    _dbContext.OrgGroupPosition.RemoveRange(OrgGroupPositionList);
                }

                _dbContext.OrgGroup.Remove(new OrgGroup() { ID = ID });

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<IEnumerable<OrgGroup>> GetAll()
        {
            return await _dbContext.OrgGroup.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<OrgGroup>> GetChart(GetChartInput param)
        {
            return await _dbContext.OrgGroup.FromSqlRaw("CALL sp_org_group_get_chart({0},{1},{2},{3},{4})", 
                param.OrgGroupID, 
                param.Depth, 
                param.ShowClosedBranches,
                param.AdminAccess != null ? param.AdminAccess.OrgGroupDescendantsDelimited : "0", 
                param.AdminAccess != null ? param.AdminAccess.IsAdminAccess : false)
                .AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<TableVarOrgChartPosition>> GetChartPosition(GetChartInput param)
        {
            return await _dbContext.TableVarOrgChartPosition.FromSqlRaw("CALL sp_org_group_position_get_chart({0})", param.OrgGroupID)
                .AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<OrgGroup>> GetLastModified(DateTime? From, DateTime? To)
        {
            return await _dbContext.OrgGroup.AsNoTracking().Where(x =>
                    (x.ModifiedDate ?? x.CreatedDate) >= (From ?? (x.ModifiedDate ?? x.CreatedDate))
                    &
                    (x.ModifiedDate ?? x.CreatedDate) <= (To ?? (x.ModifiedDate ?? x.CreatedDate))
            ).ToListAsync();
        }

        public async Task<IEnumerable<SyncOrgGroupToH2Pay>> GetLastModifiedH2Pay(DateTime? From, DateTime? To)
        {
            return await _dbContext.SyncOrgGroupToH2Pay.AsNoTracking().Where(x =>
                    (x.ModifiedDate ?? x.CreatedDate) >= (From ?? (x.ModifiedDate ?? x.CreatedDate))
                    &
                    (x.ModifiedDate ?? x.CreatedDate) <= (To ?? (x.ModifiedDate ?? x.CreatedDate))
            ).ToListAsync();
        }

        public async Task<IEnumerable<OrgGroupPosition>> GetLastModifiedOrgGroupPosition(DateTime? From, DateTime? To)
        {
            return await _dbContext.OrgGroupPosition.AsNoTracking().Where(x =>
                     (x.ModifiedDate ?? x.CreatedDate) >= (From ?? (x.ModifiedDate ?? x.CreatedDate))
                    &
                    (x.ModifiedDate ?? x.CreatedDate) <= (To ?? (x.ModifiedDate ?? x.CreatedDate))
            ).ToListAsync();
        }

        public async Task<IEnumerable<OrgGroupTag>> GetTagsByOrgGroupID(int OrgGroupID)
        {
            return await _dbContext.OrgGroupTag.AsNoTracking().Where(x => x.OrgGroupID == OrgGroupID).ToListAsync();
        }

        public async Task<bool> UploadInsert(List<OrgGroupEntity> param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                List<OrgGroup> orgGroupList = param.Select(x => new OrgGroup
                {
                    Code = x.OrgGroupCode,
                    Description = x.OrgGroupDescription,
                    OrgType = x.OrgType,
                    Address = x.Address,
                    IsBranchActive = x.IsBranchActive,
                    ServiceBayCount = x.ServiceBayCount,
                    IsActive = true,
                    CreatedBy = x.UploadedBy,
                }).ToList();

                await _dbContext.OrgGroup.AddRangeAsync(orgGroupList);
                await _dbContext.SaveChangesAsync();

                await _dbContext.OrgGroupPosition.AddRangeAsync(orgGroupList
                    .Join(param,
                    x => new { x.Code },
                    y => new { Code = y.OrgGroupCode },
                    (x, y) => new { orgGroupList = x, param = y })
                    .SelectMany(x => x.param.OrgGroupPositionList
                    .Select(y => new OrgGroupPosition
                    {
                        OrgGroupID = x.orgGroupList.ID,
                        PositionID = y.PositionID,
                        ReportingPositionID = y.ReportingPositionID,
                        PlannedCount = y.PlannedCount,
                        ActiveCount = y.ActiveCount,
                        InactiveCount = y.InactiveCount,
                        IsHead = y.IsHead,
                        IsActive = true,
                        CreatedBy = x.orgGroupList.CreatedBy
                    })).ToList());

                await _dbContext.OrgGroupTag.AddRangeAsync(orgGroupList
                    .Join(param,
                    x => new { x.Code },
                    y => new { Code = y.OrgGroupCode },
                    (x, y) => new { orgGroupList = x, param = y })
                    .Where(x => x.param.OrgGroupTagList != null)
                    .SelectMany(x => x.param.OrgGroupTagList
                    .Select(y => new OrgGroupTag
                    {
                        OrgGroupID = x.orgGroupList.ID,
                        TagRefCode = y.RefCode,
                        TagValue = y.Value
                    })).ToList());

                List<OrgGroup> parentOrgList = (await _dbContext.OrgGroup.AsNoTracking().Where(x => param.Select(y => y.ParentOrgCode).Contains(x.Code)).ToListAsync());

                if (parentOrgList != null)
                {
                    IEnumerable<OrgGroup> UpdateParentOrgGroup = parentOrgList
                    .Join(param,
                        x => new { x.Code },
                        y => new { Code = y.ParentOrgCode },
                        (x, y) => new { x, y })
                    .Join(orgGroupList,
                        x => new { x.y.OrgGroupCode },
                        y => new { OrgGroupCode = y.Code },
                        (x, y) => new { x, y })
                    .Select(x => new OrgGroup
                    {
                        ID = x.y.ID,
                        Code = x.y.Code,
                        Description = x.y.Description,
                        OrgType = x.y.OrgType,
                        Address = x.y.Address,
                        IsBranchActive = x.y.IsBranchActive,
                        ServiceBayCount = x.y.ServiceBayCount,
                        ParentOrgID = x.x.x.ID,
                        IsActive = true,
                        CreatedBy = x.y.CreatedBy,
                        CreatedDate = DateTime.Now
                    }).ToList();

                    orgGroupList.Select(x =>
                    {
                        _dbContext.Entry(x).State = EntityState.Detached;
                        return x;
                    }).ToList();

                    parentOrgList.Select(x =>
                    {
                        _dbContext.Entry(x).State = EntityState.Detached;
                        return x;
                    }).ToList();

                    UpdateParentOrgGroup
                    .Select(x =>
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

        public async Task<bool> UploadEdit(List<OrgGroup> param, List<OrgGroupPosition> orgPositionToAdd,
            List<OrgGroupPosition> orgPositionToUpdate, /*List<OrgGroupPosition> orgPositionToDelete,*/
            List<OrgGroupTag> orgTagToAdd, List<OrgGroupTag> orgTagToUpdate/*, List<OrgGroupTag> orgTagToDelete*/)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                param.Select(x =>
                {
                    _dbContext.Entry(x).State = EntityState.Modified;
                    return x;
                }).ToList();
                
                _dbContext.OrgGroupPosition.AddRange(orgPositionToAdd);

                //orgPositionToDelete.Select(x =>
                //{
                //    _dbContext.Entry(x).State = EntityState.Modified;
                //    return x;
                //}).ToList();

                orgPositionToUpdate.Select(x =>
                {
                    _dbContext.Entry(x).State = EntityState.Modified;
                    return x;
                }).ToList();
                _dbContext.OrgGroupTag.AddRange(orgTagToAdd);

                //_dbContext.OrgGroupTag.RemoveRange(orgTagToDelete);

                orgTagToUpdate.Select(x =>
                {
                    _dbContext.Entry(x).State = EntityState.Modified;
                    return x;
                }).ToList();

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }

            return true;
        }

        public async Task<IEnumerable<OrgGroupPosition>> GetOrgGroupPositionByOrgCodes(List<string> Codes)
        {
            return await _dbContext.OrgGroupPosition.AsNoTracking()
                .Join(_dbContext.OrgGroup.AsNoTracking(),
                x => new { x.OrgGroupID },
                y => new { OrgGroupID = y.ID },
                (x, y) => new { OrgGroupPosition = x, OrgGroup = y })
                .Where(x => Codes.Contains(x.OrgGroup.Code))
                .Select(y => new OrgGroupPosition
                {
                    ID = y.OrgGroupPosition.ID,
                    OrgGroupID = y.OrgGroupPosition.OrgGroupID,
                    PositionID = y.OrgGroupPosition.PositionID,
                    ReportingPositionID = y.OrgGroupPosition.ReportingPositionID,
                    PlannedCount = y.OrgGroupPosition.PlannedCount,
                    ActiveCount = y.OrgGroupPosition.ActiveCount,
                    InactiveCount = y.OrgGroupPosition.InactiveCount,
                    IsHead = y.OrgGroupPosition.IsHead,
                    IsActive = y.OrgGroupPosition.IsActive,
                    CreatedBy = y.OrgGroupPosition.CreatedBy,
                    CreatedDate = y.OrgGroupPosition.CreatedDate
                }).ToListAsync();
        }

        public async Task<IEnumerable<OrgGroupTag>> GetOrgGroupTagByOrgCodes(List<string> Codes)
        {
            return await _dbContext.OrgGroupTag.AsNoTracking()
                .Join(_dbContext.OrgGroup.AsNoTracking(),
                x => new { x.OrgGroupID },
                y => new { OrgGroupID = y.ID },
                (x, y) => new { OrgGroupTag = x, OrgGroup = y })
                .Where(x => Codes.Contains(x.OrgGroup.Code))
                .Select(y => new OrgGroupTag
                {
                    ID = y.OrgGroupTag.ID,
                    OrgGroupID = y.OrgGroupTag.OrgGroupID,
                    TagRefCode = y.OrgGroupTag.TagRefCode,
                    TagValue = y.OrgGroupTag.TagValue
                }).ToListAsync();
        }

        public async Task<OrgGroupPosition> GetByOrgGroupAndPosition(int OrgGroupID, int PositionID)
        {
            return await _dbContext.OrgGroupPosition.AsNoTracking()
                .Where(x => x.OrgGroupID == OrgGroupID & x.PositionID == PositionID).FirstOrDefaultAsync();
        }

        public async Task<bool> Put(OrgGroup param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.Entry(param).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return true;
        }

        public async Task<IEnumerable<TableVarOrgGroupExportList>> GetExportList(GetListInput input)
        {
            return await _dbContext.TableVarOrgGroupExportList
               .FromSqlRaw(@"CALL sp_org_group_export_list(
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
                        )",
                              input.ID ?? 0
                            , input.Code ?? ""
                            , input.Description ?? ""
                            , input.OrgTypeDelimited ?? ""
                            , input.ParentOrgDescription ?? ""
                            , input.IsBranchActive ?? ""
                            , input.ServiceBayCountMin ?? -1
                            , input.ServiceBayCountMax ?? -1
                            , input.sidx ?? ""
                            , input.sord ?? ""
                            , input.AdminAccess != null ? input.AdminAccess.OrgGroupDescendantsDelimited : "0"
                            , input.AdminAccess != null ? input.AdminAccess.IsAdminAccess : false
                        )
               .AsNoTracking()
               .ToListAsync();
        }

        public async Task<IEnumerable<TableVarOrgGroupHierarchy>> GetOrgGroupHierarchy(int OrgGroupID)
        { 
            return await _dbContext.TableVarOrgGroupHierarchy
               .FromSqlRaw(@"CALL sp_org_group_get_chart_upward({0})", OrgGroupID)
               .AsNoTracking()
               .ToListAsync();
        }
        public async Task<IEnumerable<TableVarOrgGroupHierarchy>> GetOrgGroupHierarchyBomd(int OrgGroupID)
        {
            return await _dbContext.TableVarOrgGroupHierarchy
               .FromSqlRaw(@"CALL sp_org_group_get_chart_upward_bomd({0})", OrgGroupID)
               .AsNoTracking()
               .ToListAsync();
        }

        public async Task<IEnumerable<TableVarOrgGroupEmployee>> GetOrgGroupEmployeeList(GetEmployeeListInput input, int rowStart)
        {
            return await _dbContext.TableVarOrgGroupEmployee
                .FromSqlRaw(@"CALL sp_org_group_employee_list(
                              {0}
                            , {1}
                            , {2}
                            , {3}
                            , {4}
                            , {5}
                            , {6}
                        )", input.ID ?? 0
                            , input.PositionDelimited ?? ""
                            , input.EmployeeName ?? ""
                            , input.sidx ?? ""
                            , input.sord ?? ""
                            , rowStart
                            , input.rows
                        )
               .AsNoTracking()
               .ToListAsync();
		}

        public async Task<IEnumerable<TableVarPlantillaCount>> GetExportCountByOrgTypeData(GetPlantillaCountInput input, int rowStart)
        {
            return await _dbContext.TableVarPlantillaCount
             .FromSqlRaw(@"CALL sp_org_group_export_count_by_org_type(
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
                        )",
                             input.OrgType ?? ""
                          , input.OrgGroupDelimited ?? ""
                          , input.ScopeOrgType ?? ""
                          , input.ScopeOrgGroupDelimited ?? ""
                          , input.PositionDelimited ?? ""
                          , input.PlannedMin
                          , input.PlannedMax
                          , input.ActiveMin
                          , input.ActiveMax
                          , input.InactiveMin
                          , input.InactiveMax
                          , input.VarianceMin
                          , input.VarianceMax
                          , input.sidx ?? ""
                          , input.sord ?? ""
                          , rowStart
                          , input.rows
                          , input.IsExport
                          , input.AdminAccess != null ? input.AdminAccess.OrgGroupDescendantsDelimited : "0"
                          , input.AdminAccess != null ? input.AdminAccess.IsAdminAccess : false
                      )
             .AsNoTracking()
             .ToListAsync();
        }

        public async Task<IEnumerable<OrgGroup>> GetByOrgGroupAutoComplete(GetByOrgTypeAutoCompleteInput param)
        {
            return await _dbContext.OrgGroup
                .FromSqlRaw("CALL sp_org_group_by_org_type_autocomplete({0},{1},{2})"
                , (param.Term ?? "")
                , param.TopResults
                , param.OrgType)
                .AsNoTracking()
                .ToListAsync();
        }
        
        public async Task<IEnumerable<TableVarHierarchyLevel>> GetOrgGroupHierarchyLevels()
        {
            return await _dbContext.TableVarHierarchyLevel
                .FromSqlRaw("CALL sp_org_group_get_hierarchy_levels()").AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<OrgGroup>> GetAutoComplete(GetAutoCompleteInput param)
        {
            return await _dbContext.OrgGroup
                .FromSqlRaw("CALL sp_org_group_autocomplete({0},{1})", (param.Term ?? ""), param.TopResults)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<OrgGroupNPRF>> GetOrgGroupNPRF(int ID)
        {
            return await _dbContext.OrgGroupNPRF.AsNoTracking()
                            .Where(x => x.OrgGroupID == ID)
                            .ToListAsync();
		}
		
        public async Task<IEnumerable<int>> GetDescendants(int OrgGroupID)
        {
            TableVarOrgGroupDescendants result = (await _dbContext.TableVarOrgGroupDescendants
                .FromSqlRaw("CALL sp_org_group_get_descendants({0})", OrgGroupID)
                .AsNoTracking().ToListAsync()).First();

            List<int> convertedResult = result.Descendants.Split(",").Select(int.Parse).ToList();

            return convertedResult;
        }
        public async Task<IEnumerable<int>> GetOrgGroupDescendantsList(List<int> OrgGroupIDs)
        {
            TableVarOrgGroupDescendants result = (await _dbContext.TableVarOrgGroupDescendants
                .FromSqlRaw("CALL sp_org_group_get_descendants_list({0})", string.Join(",", OrgGroupIDs))
                .AsNoTracking().ToListAsync()).First();

            List<int> convertedResult = result.Descendants.Split(",").Select(int.Parse).ToList();

            return convertedResult;
        }
        public async Task<IEnumerable<int>> GetDescendantsBomd(int OrgGroupID)
        {
            TableVarOrgGroupDescendants result = (await _dbContext.TableVarOrgGroupDescendants
                .FromSqlRaw("CALL sp_org_group_get_descendants_bomd({0})", OrgGroupID)
                .AsNoTracking().ToListAsync()).First();

            List<int> convertedResult = result.Descendants.Split(",").Select(int.Parse).ToList();

            return convertedResult;
        }

        public async Task<bool> UpdatePlantillaCount(List<OrgGroupPosition> orgPositionToUpdate)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                orgPositionToUpdate.Select(x =>
                {
                    _dbContext.Entry(x).State = EntityState.Modified;
                    return x;
                }).ToList();

                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }

            return true;
        }

        public async Task<bool> AddNPRF(List<OrgGroupNPRF> param)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                await _dbContext.AddRangeAsync(param);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }

            return true;
        }

        public async Task<IEnumerable<OrgGroup>> GetByIDs(List<int> IDs) 
        {
            return await _dbContext.OrgGroup.AsNoTracking()
                .Where(x => IDs.Contains(x.ID)).Where(x => x.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<OrgGroupPosition>> GetHierarchy(int? OrgGroupID, int PositionID)
        {
            return await _dbContext.OrgGroupPosition
                .FromSqlRaw("CALL sp_get_hierarchy({0},{1})", OrgGroupID, PositionID)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<TableVarOrgGroupRollupPositionDropdown>> GetOrgGroupRollupPositionDropdown(int OrgGroupID)
        {
            return await _dbContext.TableVarOrgGroupRollupPositionDropdown
                .FromSqlRaw("CALL sp_org_group_rollup_position_dropdown({0})", OrgGroupID)
                .AsNoTracking()
                .ToListAsync();
        } 
        
        public async Task<TableVarGetRegionByOrgGroupID> GetRegionByOrgGroupID(int OrgGroupID)
        {
            return (await _dbContext.TableVarGetRegionByOrgGroupID
                .FromSqlRaw("CALL sp_get_region_by_org_group_id({0})", OrgGroupID)
                .AsNoTracking()
                .ToListAsync()).ToList().FirstOrDefault();
        }

        public async Task<IEnumerable<TableVarPositionOrgGroupUpwardAutocomplete>> GetPositionUpwardAutoComplete(GetPositionOrgGroupUpwardAutoCompleteInput param)
        {
            return await _dbContext.TableVarPositionOrgGroupUpwardAutocomplete
                .FromSqlRaw("CALL sp_position_upward_autocomplete({0},{1},{2})"
                , (param.Term ?? "")
                , param.TopResults
                , param.OrgGroupID)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<TableVarPositionOrgGroupUpwardAutocomplete>> GetOrgGroupUpwardAutoComplete(GetPositionOrgGroupUpwardAutoCompleteInput param)
        {
            return await _dbContext.TableVarPositionOrgGroupUpwardAutocomplete
                .FromSqlRaw("CALL sp_org_group_upward_autocomplete({0},{1},{2})"
                , (param.Term ?? "")
                , param.TopResults
                , param.OrgGroupID)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<TableVarOrgGroupBranchInfo>> GetBranchInfoList(GetBranchInfoListInput input, int rowStart)
        {
            return await _dbContext.TableVarOrgGroupBranchInfo
               .FromSqlRaw(@"CALL sp_org_group_get_list_branch_info(
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
                        )",
                               input.ID ?? 0
                            , input.Code ?? ""
                            , input.Description ?? ""
                            , input.Category ?? ""
                            , input.Email ?? ""
                            , input.Number ?? ""
                            , input.Address ?? ""
                            , input.OrgTypeDelimited ?? ""
                            , input.ParentOrgDescription ?? ""
                            , input.IsBranchActive ?? ""
                            , input.ServiceBayCountMin ?? -1
                            , input.ServiceBayCountMax ?? -1
                            , input.sidx ?? ""
                            , input.sord ?? ""
                            , rowStart
                            , input.rows
                            , input.AdminAccess != null ? input.AdminAccess.OrgGroupDescendantsDelimited : "0"
                            , input.AdminAccess != null ? input.AdminAccess.IsAdminAccess : false
                        )
               .AsNoTracking()
               .ToListAsync();
        }

        public async Task<IEnumerable<TableVarOrgGroupHistoryList>> GetOrgGroupHistoryList(OrgGroupHistoryListInput param, int rowStart)
        {
            return await _dbContext.TableVarOrgGroupHistoryList
                .FromSqlRaw("CALL sp_org_group_history_list({0},{1},{2},{3},{4},{5},{6})"
                , param.TDateFrom ?? ""
                , param.TDateTo ?? ""
                , param.IsLatest ?? ""
                , param.sidx ?? ""
                , param.sord ?? ""
                , rowStart
                , param.rows)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<TableVarOrgGroupHistoryByDate>> GetOrgGroupHistoryByDate(OrgGroupHistoryByDateInput param, int rowStart)
        {
            return await _dbContext.TableVarOrgGroupHistoryByDate
                .FromSqlRaw("CALL sp_org_group_history_by_date({0},{1},{2},{3},{4},{5})"
                , param.TDate ?? ""
                , param.IsLatest ?? ""
                , param.sidx ?? ""
                , param.sord ?? ""
                , rowStart
                , param.rows)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<bool> AddOrgGroupHistory(List<Data.OrgGroup.OrgGroupHistory> OrgGroupHistory,  bool IsLatest, List<Data.OrgGroup.OrgGroupHistory> UpdateOrgGroupHistory)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                // Change the code to another way due to prod error, both code works in staging
                // - Ginger
                //if (OrgGroup.Count > 0)
                //    _dbContext.UpdateRange(OrgGroup);

                 var trackedEntities = _dbContext.ChangeTracker.Entries().ToList(); // checker of entity tracking

                //Detach all entities from the context
                foreach (var entry in _dbContext.ChangeTracker.Entries().ToList())
                {
                    entry.State = EntityState.Detached;
                }

                if (IsLatest && OrgGroupHistory.Count == 0)
                {
                    _dbContext.UpdateRange(UpdateOrgGroupHistory);
                }
                else if (IsLatest && OrgGroupHistory.Count > 0)
                {
                    await _dbContext.AddRangeAsync(OrgGroupHistory);
                    _dbContext.UpdateRange(UpdateOrgGroupHistory);
                }
                else
                {
                    _dbContext.UpdateRange(UpdateOrgGroupHistory);
                }

                await _dbContext.SaveChangesAsync();
                transaction.Commit();

                foreach (var entry in _dbContext.ChangeTracker.Entries().ToList())
                {
                    entry.State = EntityState.Detached;
                }

            }
            return true;
        }
        public  async Task<bool> AddOrgGroup(List<Data.OrgGroup.OrgGroup> OrgGroup)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                 var trackedEntities = _dbContext.ChangeTracker.Entries().ToList(); // checker of entity tracking

                //Detach all entities from the context
                foreach (var entry in _dbContext.ChangeTracker.Entries().ToList())
                {
                    entry.State = EntityState.Detached;
                }

                if (OrgGroup.Count > 0)
                {
                    _dbContext.UpdateRange(OrgGroup);
                }
                await _dbContext.SaveChangesAsync();

                //Detach all entities from the context
                foreach (var entry in _dbContext.ChangeTracker.Entries().ToList())
                {
                    entry.State = EntityState.Detached;
                }

                transaction.Commit();
               
            }
                
            return true;
        }
        public async Task<IEnumerable<OrgGroupHistory>> GetAllHistory()
        {
            return await _dbContext.OrgGroupHistory.AsNoTracking().Where(x => x.IsActive).ToListAsync();
        }
        public async Task<IEnumerable<TableVarOrgGroupFormat>> GetOrgGroupFormatByID(List<int> ID)
        {
            return await _dbContext.TableVarOrgGroupFormat.FromSqlRaw("CALL sp_org_group_format_by_id({0})", string.Join(',',ID))
                .AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<TableVarOrgGroupFormat>> GetOrgGroupParent(GetOrgGroupParentInput param)
        {
            return await _dbContext.TableVarOrgGroupFormat
               .FromSqlRaw("CALL sp_get_org_group_parent({0},{1})", string.Join(",", param.OrgGroupIDs),param.OrgType)
               .AsNoTracking()
               .ToListAsync();
        }
        public async Task<IEnumerable<TableVarOrgGroupSOMD>> GetOrgGroupSOMD(List<int> IDs)
        {
            return await _dbContext.TableVarOrgGroupSOMD.FromSqlRaw("CALL sp_org_group_somd({0})", string.Join(',', IDs))
                .AsNoTracking().ToListAsync();
        }
    }
}