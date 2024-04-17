using EMS.Plantilla.Core.OrgGroup;
using EMS.Plantilla.Transfer.OrgGroup;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.API;

namespace EMS.Plantilla.API.Controllers
{
    [Route("plantilla/[controller]")]
    [ApiController]
    public class OrgGroupController : ControllerBase
    {
        private readonly IOrgGroupService _service;

        /// <summary>
        /// Depency Injected Method
        /// </summary>
        /// <param name="service"></param>
        public OrgGroupController(IOrgGroupService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("get-dropdown")]
        public async Task<IActionResult> GetDropDown([FromQuery] APICredentials credentials, [FromQuery] GetDropDownInput param)
        {
            return await _service.GetDropDown(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-dropdown-detailed")]
        public async Task<IActionResult> GetDropDownDetailed([FromQuery] APICredentials credentials)
        {
            return await _service.GetDropDownDetailed(credentials).ConfigureAwait(true);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="credentials"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-chart")]
        public async Task<IActionResult> GetChart([FromQuery] APICredentials credentials, [FromQuery] GetChartInput param)
        {
            return await _service.GetChart(credentials, param).ConfigureAwait(true);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="credentials"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-chart-position")]
        public async Task<IActionResult> GetChartPosition([FromQuery] APICredentials credentials, [FromQuery] GetChartInput param)
        {
            return await _service.GetChartPosition(credentials, param).ConfigureAwait(true);
        }

        /// <summary>
        /// Get records to be displayed on JQGrid
        /// </summary>
        /// <param name="credentials">API Credentials</param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-list")]
        public async Task<IActionResult> GetList([FromQuery] APICredentials credentials, [FromQuery] GetListInput param)
        {
            return await _service.GetList(credentials, param).ConfigureAwait(true);
        }

        /// <summary>
        /// Adding of new records
        /// </summary>
        /// <param name="credentials">API Credentials</param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Post([FromQuery] APICredentials credentials, [FromBody] Form param)
        {
            return await _service.Post(credentials, param).ConfigureAwait(true);
        }

        /// <summary>
        /// Updating of records
        /// </summary>
        /// <param name="credentials">API Credentials</param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("edit")]
        public async Task<IActionResult> Put([FromQuery] APICredentials credentials, [FromBody] Form param)
        {
            return await _service.Put(credentials, param).ConfigureAwait(true);
        }

        /// <summary>
        /// Deleting of records
        /// </summary>
        /// <param name="credentials">API Credentials</param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> Delete([FromQuery] APICredentials credentials, [FromQuery] int ID)
        {
            return await _service.Delete(credentials, ID).ConfigureAwait(true);
        }

        /// <summary>
        /// Get records by ID
        /// </summary>
        /// <param name="credentials">API Credentials</param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-by-id")]
        public async Task<IActionResult> GetByID([FromQuery] APICredentials credentials, [FromQuery] GetByIDInput param)
        {
            return await _service.GetByID(credentials, param).ConfigureAwait(true);
        }

        /// <summary>
        /// Get records by Parent Org ID
        /// </summary>
        /// <param name="credentials">API Credentials</param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-children-org-dropdown")]
        public async Task<IActionResult> GetChildrenOrgDropDown([FromQuery] APICredentials credentials, [FromQuery] GetByIDInput param)
        {
            return await _service.GetChildrenOrgDropDown(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-org-group-position")]
        public async Task<IActionResult> GetOrgGroupPosition([FromQuery] APICredentials credentials, [FromQuery] GetByIDInput param)
        {
            return await _service.GetOrgGroupPosition(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-last-modified")]
        public async Task<IActionResult> GetLastModified([FromQuery] SharedUtilities.UNIT_OF_TIME unit, [FromQuery] int value)
        {
            return await _service.GetLastModified(unit, value).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-last-modified-position")]
        public async Task<IActionResult> GetLastModifiedOrgGroupPosition([FromQuery] SharedUtilities.UNIT_OF_TIME unit, [FromQuery] int value)
        {
            return await _service.GetLastModifiedOrgGroupPosition(unit, value).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-export-count-by-org-type-data")]
        public async Task<IActionResult> GetExportCountByOrgTypeData([FromQuery] APICredentials credentials, [FromQuery] GetPlantillaCountInput param)
        {
            return await _service.GetExportCountByOrgTypeData(credentials, param).ConfigureAwait(true);
        }

        /// <summary>
        /// Uploading of new records
        /// </summary>
        /// <param name="credentials">API Credentials</param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("upload-insert")]
        public async Task<IActionResult> UploadInsert([FromQuery] APICredentials credentials, [FromBody] List<UploadFileEntity> param)
        {
            return await _service.UploadInsert(credentials, param).ConfigureAwait(true);
        }

        /// <summary>
        /// Batch edit of records via uploading
        /// </summary>
        /// <param name="credentials">API Credentials</param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("upload-edit")]
        public async Task<IActionResult> UploadEdit([FromQuery] APICredentials credentials, [FromBody] List<UploadFileEntity> param)
        {
            return await _service.UploadEdit(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-dropdown-by-org-type")]
        public async Task<IActionResult> GetDropDownByOrgType([FromQuery] APICredentials credentials, [FromQuery] string OrgType)
        {
            return await _service.GetDropDownByOrgType(credentials, OrgType).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-by-org-group-and-position")]
        public async Task<IActionResult> GetByOrgGroupAndPosition([FromQuery] APICredentials credentials, [FromQuery] GetByOrgGroupIDAndPositionIDInput param)
        {
            return await _service.GetByOrgGroupAndPosition(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-export-list")]
        public async Task<IActionResult> GetExportList([FromQuery] APICredentials credentials, [FromQuery] GetListInput param)
        {
            return await _service.GetExportList(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-org-group-hierarchy")]
        public async Task<IActionResult> GetOrgGroupHierarchy([FromQuery] APICredentials credentials, [FromQuery] int ID)
        {
            return await _service.GetOrgGroupHierarchy(credentials, ID).ConfigureAwait(true);
        }
        [HttpGet]
        [Route("get-org-group-hierarchy-bomd")]
        public async Task<IActionResult> GetOrgGroupHierarchyBomd([FromQuery] APICredentials credentials, [FromQuery] int ID)
        {
            return await _service.GetOrgGroupHierarchyBomd(credentials, ID).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-org-group-employee-list")]
        public async Task<IActionResult> GetOrgGroupEmployeeList([FromQuery] APICredentials credentials, [FromQuery] GetEmployeeListInput input)
        {
            return await _service.GetOrgGroupEmployeeList(credentials, input).ConfigureAwait(true);
		}

        [HttpGet]
        [Route("get-id-by-org-type-autocomplete")]
        public async Task<IActionResult> GetIDByOrgTypeAutoComplete([FromQuery] APICredentials credentials, [FromQuery] GetByOrgTypeAutoCompleteInput param)
        {
            return await _service.GetIDByOrgTypeAutoComplete(credentials, param).ConfigureAwait(true);
        }

        /// <summary>
        /// Get ID and description of records to be displayed for auto complete elements
        /// </summary>
        /// <param name="credentials">API Credentials</param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-id-by-autocomplete")]
        public async Task<IActionResult> GetIDByAutoComplete([FromQuery] APICredentials credentials, [FromQuery] GetAutoCompleteInput param)
        {
            return await _service.GetIDByAutoComplete(credentials, param).ConfigureAwait(true);
		}
		
        [HttpGet]
        [Route("get-org-hierarchy-levels-dropdown")]
        public async Task<IActionResult> GetOrgGroupHierarchyLevelsDropDown([FromQuery] APICredentials credentials)
        {
            return await _service.GetOrgGroupHierarchyLevelsDropDown(credentials).ConfigureAwait(true);
        } 
        
        [HttpGet]
        [Route("get-org-group-descendants")]
        public async Task<IActionResult> GetOrgGroupDescendants([FromQuery] APICredentials credentials, int OrgGroupID)
        {
            return await _service.GetOrgGroupDescendants(credentials, OrgGroupID).ConfigureAwait(true);
        }
        [HttpPost]
        [Route("get-org-group-descendants-list")]
        public async Task<IActionResult> GetOrgGroupDescendantsList([FromQuery] APICredentials credentials,[FromBody] List<int> OrgGroupIDs)
        {
            return await _service.GetOrgGroupDescendantsList(credentials, OrgGroupIDs).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-org-group-descendants-bomd")]
        public async Task<IActionResult> GetOrgGroupDescendantsBomd([FromQuery] APICredentials credentials, int OrgGroupID)
        {
            return await _service.GetOrgGroupDescendantsBomd(credentials, OrgGroupID).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-org-group-position-with-description")]
        public async Task<IActionResult> GetPositionWithDescription([FromQuery] APICredentials credentials, [FromQuery] int OrgGroupID)
        {
            return await _service.GetPositionWithDescription(credentials, OrgGroupID).ConfigureAwait(true);
        }
          
        [HttpGet]
        [Route("get-org-group-nprf")]
        public async Task<IActionResult> GetOrgGroupNPRF([FromQuery] APICredentials credentials, [FromQuery] int OrgGroupID)
        {
            return await _service.GetOrgGroupNPRF(credentials, OrgGroupID).ConfigureAwait(true);
        }

        [HttpPut]
        [Route("update-plantilla-count")]
        public async Task<IActionResult> UpdatePlantillaCount([FromQuery] APICredentials credentials, [FromBody] List<PlantillaCountUpdateForm> param)
        {
            return await _service.UpdatePlantillaCount(credentials, param).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("add-nprf")]
        public async Task<IActionResult> AddNPRF([FromQuery] APICredentials credentials, [FromBody] List<OrgGroupNPRFForm> param)
        {
            return await _service.AddNPRF(credentials, param).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("get-by-ids")]
        public async Task<IActionResult> GetByIDs([FromQuery] APICredentials credentials, [FromBody] List<int> IDs)
        {
            return await _service.GetByIDs(credentials, IDs).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-code-dropdown")]
        public async Task<IActionResult> GetCodeDropDown([FromQuery] APICredentials credentials, [FromQuery] GetDropDownInput param)
        {
            return await _service.GetCodeDropDown(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-org-group-rollup-position-dropdown")]
        public async Task<IActionResult> GetOrgGroupRollupPositionDropdown([FromQuery] APICredentials credentials, [FromQuery] int OrgGroupID)
        {
            return await _service.GetOrgGroupRollupPositionDropdown(credentials, OrgGroupID).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-region-by-org-group-id")]
        public async Task<IActionResult> GetRegionByOrgGroupID([FromQuery] APICredentials credentials, [FromQuery] int OrgGroupID)
        {
            return await _service.GetRegionByOrgGroupID(credentials, OrgGroupID).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-by-codes")]
        public async Task<IActionResult> GetByCodes([FromQuery] APICredentials credentials, [FromQuery] string CodesDelimited)
        {
            return await _service.GetByCodes(credentials, CodesDelimited).ConfigureAwait(true);
		}
		
        [HttpGet]
        [Route("get-position-upward-autocomplete")]
        public async Task<IActionResult> GetPositionUpwardAutoComplete([FromQuery] APICredentials credentials, [FromQuery] GetPositionOrgGroupUpwardAutoCompleteInput param)
        {
            return await _service.GetPositionUpwardAutoComplete(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-org-group-upward-autocomplete")]
        public async Task<IActionResult> GetOrgGroupUpwardAutoComplete([FromQuery] APICredentials credentials, [FromQuery] GetPositionOrgGroupUpwardAutoCompleteInput param)
        {
            return await _service.GetOrgGroupUpwardAutoComplete(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-branch-info-list")]
        public async Task<IActionResult> GetBranchInfoList([FromQuery] APICredentials credentials, [FromQuery] GetBranchInfoListInput param)
        {
            return await _service.GetBranchInfoList(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-org-group-history-list")]
        public async Task<IActionResult> GetOrgGroupHistoryList([FromQuery] APICredentials credentials, [FromQuery] OrgGroupHistoryListInput param)
        {
            return await _service.GetOrgGroupHistoryList(credentials, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-org-group-history-by-date")]
        public async Task<IActionResult> GetOrgGroupHistoryByDate([FromQuery] APICredentials credentials, [FromQuery] OrgGroupHistoryByDateInput param)
        {
            return await _service.GetOrgGroupHistoryByDate(credentials, param).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("add-org-group-history")]
        public async Task<IActionResult> AddOrgGroupHistory([FromQuery] APICredentials credentials,[FromQuery] string TDate,[FromQuery] bool IsLatest, [FromBody] List<AddOrgGroupHistoryInput> param)
        {
            return await _service.AddOrgGroupHistory(credentials, TDate, IsLatest, param).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-all-org-group-history")]
        public async Task<IActionResult> GetAllOrgGroupHistory([FromQuery] APICredentials credentials)
        {
            return await _service.GetAllOrgGroupHistory(credentials).ConfigureAwait(true);
        }

        [HttpGet]
        [Route("get-employee-email-by-org-id")]
        public async Task<IActionResult> GetEmployeeEmailByOrgId(int OrgGroupID)
        {
            return await _service.GetEmployeeEmailByOrgId(OrgGroupID).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("get-org-group-format-by-id")]
        public async Task<IActionResult> GetOrgGroupFormatByID(List<int> ID)
        {
            return await _service.GetOrgGroupFormatByID(ID).ConfigureAwait(true);
        }
        [HttpPost]
        [Route("get-org-group-parent")]
        public async Task<IActionResult> GetOrgGroupParent(GetOrgGroupParentInput param)
        {
            return await _service.GetOrgGroupParent(param).ConfigureAwait(true);
        }
        [HttpPost]
        [Route("get-org-group-somd")]
        public async Task<IActionResult> GetOrgGroupSOMD(List<int> IDs)
        {
            return await _service.GetOrgGroupSOMD(IDs).ConfigureAwait(true);
        }
    }
}