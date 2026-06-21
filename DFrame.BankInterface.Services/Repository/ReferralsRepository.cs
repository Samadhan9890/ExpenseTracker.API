using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Services.Data;
using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Models.DTOs.Referrals;
using ExpenseTracker.Services.Repository.Interface;
using ExpenseTracker.Services.Utilities;
using NPOI.POIFS.Properties;
using System.Data;

namespace ExpenseTracker.Services.Repository
{
    public class ReferralsRepository : IReferralsRepository
    {
        private readonly AppDBContext _context;
        public ReferralsRepository(AppDBContext appDBContext)
        {
            _context = appDBContext;
        }

        

        public async Task<List<ReferralsPerformanceDto>> GetAllBdPerformance()
        {
            var clientMastersWithReferrals = await _context.ClientMasters
                    .Where(cm => cm.Status) // Filter for active clients, adjust as needed
                    .Select(cm => new ReferralsPerformanceDto
                    {
                        ClientId = cm.ClientId,
                        Name = cm.Name,
                        PanNo = cm.PanNo,
                        Address = cm.PerAddressLine1,
                        JoiningDate = cm.CreatedDate,
                        IsClient = true,
                        Status = cm.Status,
                        // Self-join to get referred clients
                        ClientsDetails = _context.ClientMasters
                            .Where(referred => referred.ReferredBy == cm.ClientId.ToString()) // Self-join using ReferredBy
                            .Select(referred => new ReferalsClientsDetails
                            {
                                ClientId = referred.ClientId,
                                Guid = referred.Guid,
                                Name = referred.Name,
                                BloodRelation = referred.BloodRelation,
                                RefferedByName = cm.Name, // Referrer name
                                InvestmentsDetails = _context.Investments
                                    .Where(inv => inv.ClientId == referred.ClientId)
                                    .Select(inv => new ReferalsClientsInvestments
                                    {
                                        InvestmentId = inv.InvestmentId,
                                        PlanId = inv.PlanId,
                                        PlanName = inv.PlanName,
                                        InvestmentAmount = inv.InvestmentAmount,
                                        InvestmentStartDate = inv.InvestmentStartDate,
                                        InvestmentEndDate = inv.InvestmentEndDate,
                                        Status = inv.Status,
                                        ClosingDate = inv.ClosingDate,
                                        ClosedBy = inv.ClosedBy,
                                        ClosingComment = inv.ClosingComment
                                    }).ToList()
                            }).ToList()
                    })
                    .Where(dto => dto.ClientsDetails.Any()) // Exclude clients without referrals
                    .ToListAsync();


            return clientMastersWithReferrals;

        }

        public async Task<ClientHierarchyDto> GetAllBusinessDevTeamHierarchyByClientId(int clientId)
        {
           
            var res = await GetClientHierarchyAsync(clientId);
            return res;
        }

        public Task<List<ClientHierarchyDto>> BusinessDevTeamHierarchies()
        {
            throw new NotImplementedException();
        }


        //public async Task<ClientHierarchyDto> GetClientHierarchyAsync(int clientId)
        //{
        //    var clientHierarchy = new ClientHierarchyDto();

        //    Dictionary<string, string> spParams = new Dictionary<string, string>();
        //    spParams.Add("@ClientId", clientId.ToString());           

        //    SqlAccess sqlAccess = new SqlAccess(_context);
        //    var dsReult = sqlAccess.ExecuteMultiScalarSp("USP_GET_CLIENT_HIERARCHY", spParams);

        //    List<InvestmentMonthwiseSummary> investmentMonthwise = DatatableHelper.BindList<InvestmentMonthwiseSummary>(result.Tables[1]);


        //    return clientHierarchy;
        //}


        public async Task<ClientHierarchyDto> GetClientHierarchyAsync(int clientId)
        {
            var clientHierarchy = new ClientHierarchyDto();

            try
            {
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "USP_GET_CLIENT_HIERARCHY"; // Name of your stored procedure
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@ClientId", clientId));

                    await _context.Database.OpenConnectionAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        // Map main client details
                        if (await reader.ReadAsync())
                        {
                            clientHierarchy.Client = new ClientDto
                            {
                                ClientId = reader.GetInt32(0),
                                Name = reader.IsDBNull(1) ? null : reader.GetString(1),
                                Investments = new List<InvestmentDto>()
                            };

                            // Loop through investments for the main client
                            do
                            {
                                if (!reader.IsDBNull(2)) // Check if InvestmentId is not null
                                {
                                    clientHierarchy.Client.Investments.Add(new InvestmentDto
                                    {
                                        InvestmentId = reader.GetInt32(2),
                                        Amount = reader.IsDBNull(3) ? 0 : reader.GetDecimal(3), // Default to 0 if null
                                        InvestmentDate = reader.IsDBNull(4) ? DateTime.MinValue : reader.GetDateTime(4) // Default to MinValue if null
                                    });
                                }
                            } while (await reader.ReadAsync());
                        }

                        // Move to next result set for parent client
                        if (await reader.NextResultAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                var parentClientId = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                                var parentClientName = reader.IsDBNull(1) ? null : reader.GetString(1);

                                // Create the parent client entry
                                clientHierarchy.ParentClient = new ClientDto
                                {
                                    ClientId = parentClientId,
                                    Name = parentClientName,
                                    Investments = new List<InvestmentDto>()
                                };

                                // Loop through investments for the parent client
                                while (await reader.ReadAsync())
                                {
                                    if (!reader.IsDBNull(2)) // Check if InvestmentId is not null
                                    {
                                        clientHierarchy.ParentClient.Investments.Add(new InvestmentDto
                                        {
                                            InvestmentId = reader.GetInt32(2),
                                            Amount = reader.IsDBNull(3) ? 0 : reader.GetDecimal(3), // Default to 0 if null
                                            InvestmentDate = reader.IsDBNull(4) ? DateTime.MinValue : reader.GetDateTime(4) // Default to MinValue if null
                                        });
                                    }
                                }
                            }
                        }

                        // Move to next result set for child clients
                        if (await reader.NextResultAsync())
                        {
                            clientHierarchy.ChildClients = new List<ClientDto>();

                            while (await reader.ReadAsync())
                            {
                                var childClientId = reader.GetInt32(0);
                                var childClientName = reader.IsDBNull(1) ? null : reader.GetString(1);

                                // Check if the child client already exists in the list
                                var existingChildClient = clientHierarchy.ChildClients
                                    .FirstOrDefault(c => c.ClientId == childClientId);

                                if (existingChildClient == null)
                                {
                                    existingChildClient = new ClientDto
                                    {
                                        ClientId = childClientId,
                                        Name = childClientName,
                                        Investments = new List<InvestmentDto>()
                                    };
                                    clientHierarchy.ChildClients.Add(existingChildClient);
                                }

                                // Add investments for the child client
                                if (!reader.IsDBNull(2)) // Check if InvestmentId is not null
                                {
                                    existingChildClient.Investments.Add(new InvestmentDto
                                    {
                                        InvestmentId = reader.GetInt32(2),
                                        Amount = reader.IsDBNull(3) ? 0 : reader.GetDecimal(3), // Default to 0 if null
                                        InvestmentDate = reader.IsDBNull(4) ? DateTime.MinValue : reader.GetDateTime(4) // Default to MinValue if null
                                    });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log it)
                throw new Exception("An error occurred while retrieving the client hierarchy.", ex);
            }
            finally
            {
                // Ensure that the database connection is closed
                if (_context.Database.GetDbConnection().State == ConnectionState.Open)
                {
                    await _context.Database.GetDbConnection().CloseAsync();
                }
            }

            return clientHierarchy;
        }






    }
}
