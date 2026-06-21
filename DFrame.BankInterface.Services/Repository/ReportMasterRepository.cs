using AutoMapper;
using ExpenseTracker.Services.Data;
using ExpenseTracker.Services.Models.EntityModels;
using ExpenseTracker.Services.Models.ServiceModels;
using ExpenseTracker.Services.Repository.Interface;
using ExpenseTracker.Services.Utilities.ConstantHelper;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ExpenseTracker.Services.Repository
{
    public class ReportMasterRepository:IReportMasterRepository
    {
        private readonly AppDBContext _appDBContext;
        private readonly IMapper _mapper;
        public ReportMasterRepository(AppDBContext appDBContext, IMapper mapper)
        {
            _appDBContext = appDBContext;
            _mapper = mapper;
        }

        public async Task<List<TblCustomReportMaster>> GetReportManagerList()
        {
            try
            {
                var result = await _appDBContext.CustomReportMaster.OrderByDescending(x => x.ModifyDate).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<TblCustomReportMaster> GetReportManagerDataById(int id)
        {
            try
            {
                var result = await _appDBContext.CustomReportMaster.Where(x => id == x.CustomReportId).FirstOrDefaultAsync();
                if (result == null)
                {
                    throw new Exception("Report not found");
                }
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<TblCustomReportMaster> AddUpdateReport(TblCustomReportMaster tblCustomReport, TblUser user)
        {
            try
            {
                var isReportWithSameNameExist = _appDBContext.CustomReportMaster.Any(x => x.CustomReportId != tblCustomReport.CustomReportId && x.CustomReportName == tblCustomReport.CustomReportName);
                if (isReportWithSameNameExist)
                {
                    throw new Exception("Report with this name" + tblCustomReport.CustomReportName + " is already exist");
                }

                if (tblCustomReport.CustomReportId > 0)
                {
                    var customReport = await _appDBContext.CustomReportMaster.FirstOrDefaultAsync(x => x.CustomReportId == tblCustomReport.CustomReportId);
                    if (customReport == null)
                    {
                        throw new Exception("Report not found");
                    }
                    else
                    {
                        customReport.CustomReportName = tblCustomReport.CustomReportName;
                        customReport.CustomReportDesc = tblCustomReport.CustomReportDesc;
                        customReport.CustomReportQuery = tblCustomReport.CustomReportQuery;
                        customReport.CustomReportCode = tblCustomReport.CustomReportCode;
                        customReport.ColumnFilter = tblCustomReport.ColumnFilter;
                        customReport.DateFilter = tblCustomReport.DateFilter;
                        customReport.OrderFilter = tblCustomReport.OrderFilter;
                        customReport.IsDag = tblCustomReport.IsDag;
                        customReport.Status = tblCustomReport.Status;
                        customReport.RoleAccess = tblCustomReport.RoleAccess;
                        customReport.ModifyId = user.InternalUserId;
                        customReport.ModifyDate = DateTime.Today;

                        _appDBContext.CustomReportMaster.Update(customReport);
                        await _appDBContext.SaveChangesAsync();
                    }
                }
                else
                {
                    var maxReportId = _appDBContext.CustomReportMaster.Count() > 0 ? Convert.ToInt32(_appDBContext.CustomReportMaster.Max(x => x.CustomReportId)) : 0;
                    tblCustomReport.CustomReportCode = "RPT/" + (maxReportId + 1).ToString("#00000");
                    tblCustomReport.EntryDate = DateTime.Today;
                    tblCustomReport.EntryId = user.InternalUserId;

                    await _appDBContext.AddAsync(tblCustomReport);
                    await _appDBContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return tblCustomReport;
        }

        public async Task DeleteReportMaster(int id)
        {
            await _appDBContext.CustomReportMaster.Where(x => x.CustomReportId == id).ExecuteDeleteAsync();
        }


        public async Task<List<TblCustomReportMaster>> GetReportViewerList(TblUser user)
        {

            List<int?> userRoleAccess = new List<int?>();

            if (!String.IsNullOrEmpty(user.RoleAccess))
            {
                var USER_ROLE_ACCESS1 = user.RoleAccess.Split(',');
                foreach (var B in USER_ROLE_ACCESS1)
                {
                    if (!String.IsNullOrEmpty(B.Trim()))
                        userRoleAccess.Add(Convert.ToInt32(B.Trim()));
                }
            }

            var reportMasterList = await _appDBContext.CustomReportMaster.Where(x => x.Status == 1).OrderByDescending(x => x.ModifyDate).ToListAsync();
            List<int> reportIds = new List<int>();

            foreach (var p in reportMasterList)
            {
                if (!String.IsNullOrEmpty(p.RoleAccess))
                {
                    var reportRoleAccess = p.RoleAccess.Split(',');
                    foreach (var B in reportRoleAccess)
                    {
                        if (userRoleAccess.Contains(Convert.ToInt32(B)))
                        {
                            reportIds.Add(p.CustomReportId);
                        }

                    }
                }
            }

            return reportMasterList.Where(x => reportIds.Contains(x.CustomReportId)).ToList();
        }

        public async Task<ReportViewerModel> GetReportViewerDataById(int id, TblUser user)
        {
            try
            {
                List<int?> userRoleAccess = new List<int?>();

                if (!String.IsNullOrEmpty(user.RoleAccess))
                {
                    var USER_ROLE_ACCESS1 = user.RoleAccess.Split(',');
                    foreach (var B in USER_ROLE_ACCESS1)
                    {
                        if (!String.IsNullOrEmpty(B.Trim()))
                            userRoleAccess.Add(Convert.ToInt32(B.Trim()));
                    }
                }

                ReportViewerModel reportView = new ReportViewerModel();
                var reportData = await _appDBContext.CustomReportMaster.FirstOrDefaultAsync(x => x.CustomReportId == id);
                if (reportData == null)
                {
                    throw new Exception("Report not found");
                }

                bool isUserAbleToAccessReport = false;
                if (!String.IsNullOrEmpty(reportData.RoleAccess))
                {
                    var reportRoleAccess = reportData.RoleAccess.Split(',');
                    foreach (var B in reportRoleAccess)
                    {
                        if (userRoleAccess.Contains(Convert.ToInt32(B)))
                        {
                            isUserAbleToAccessReport = true;
                        }

                    }
                }

                if (!isUserAbleToAccessReport)
                {
                    throw new Exception("User cannot access the report");
                }

                if (!String.IsNullOrEmpty(reportData.DateFilter))
                {
                    string[] dateFilter = reportData.DateFilter.Split(',');
                    int index = 1;
                    foreach (var d in dateFilter)
                    {
                        reportView.ListSearchCriteria1.Add(new FilterSearchCriteria
                        {
                            Id = index++,
                            Label = d,
                            Value = d
                        });
                    }
                }

                if (!String.IsNullOrWhiteSpace(reportData.ColumnFilter))
                {
                    string[] roleAccess = reportData.ColumnFilter.Split(',');

                    int index = 1;
                    foreach (var search in roleAccess)
                    {
                        reportView.ListSearchCriteria2.Add(new FilterSearchCriteria
                        {
                            Id = index,
                            Label = search,
                            Value = search
                        });

                        reportView.ListSearchCriteria3.Add(new FilterSearchCriteria
                        {
                            Id = index,
                            Label = search,
                            Value = search
                        });

                        index++;
                    }

                }

                return reportView;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ReportExcelModel> GenerateExcelForReport(int id, ReportViewerFilter reportView, TblUser user)
        {
            try
            {
                DataSet ds = new DataSet();
                var BU = "";
                var reportData = await _appDBContext.CustomReportMaster.FirstOrDefaultAsync(x => x.CustomReportId == id) ?? throw new Exception("Report not found");
				
                string strQuery = reportData.CustomReportQuery;

                if (reportData.IsDag == 1)
                {
                    strQuery = "SELECT * FROM ( " + strQuery + ") T WHERE 1=1 ";
                }

                else if (reportData.IsDag == 2)
                {
                    strQuery = "SELECT * FROM ( " + strQuery + ") T WHERE 1=1 "; // T WHERE T.BU IN ( " + BU + ")";
                }

                else if (reportData.IsDag == 3)
                {
                    strQuery = "SELECT * FROM ( " + strQuery + ") T WHERE T.ENTRY_ID = ( " + user.InternalUserId.ToString() + ")";
                }

                if (reportView.SearchText1 != "-1")
                {
                    strQuery = strQuery + " AND " + reportView.SearchText1.Trim() + " LIKE '%" + reportView.Code1.Trim() + "%'";
                }
                if (reportView.SearchText2 != "-1")
                {
                    strQuery = strQuery + " AND " + reportView.SearchText1.Trim() + " LIKE '%" + reportView.Code2.Trim() + "%'";
                }


                if (reportView.SearchDate != "-1")
                {
                    strQuery = strQuery + " AND (" + reportView.SearchDate + " BETWEEN Convert(date,'" + reportView.Date1 + "',120) AND  Convert(date,'" + reportView.Date2 + "',120))";
                }

                if (reportData.OrderFilter != null && reportData.OrderFilter != "")
                {
                    strQuery = strQuery + " ORDER BY " + reportData.OrderFilter;
                }

                SqlAccess sqlAccess = new SqlAccess(_appDBContext);
                DataTable TB = sqlAccess.ExceuteQuery(strQuery);
                ds.Tables.Add(TB);

                var applicationParameter = await _appDBContext.ApplicationParameter.Where(x => x.Status == 1).ToListAsync();

                return new ReportExcelModel() { Data = ds.Tables[0], ReportName = reportData.CustomReportName, ReportPath = applicationParameter.FirstOrDefault(x => x.Parameter == "REPORT_MANAGER_PARKING_PATH").Value, SystemTempPath = applicationParameter.FirstOrDefault(x => x.Parameter == "REPORT_MANAGER_SYS_TEMP_PATH").Value };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
