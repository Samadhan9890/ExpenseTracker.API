using AutoMapper;
using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Models.DTOs.ClientMaster;
using ExpenseTracker.Services.Models.EntityModels;
using ExpenseTracker.Services.Repository;
using ExpenseTracker.Services.Repository.Interface;
using ExpenseTracker.Services.Services.IServices;
using NC.StorageProcessor.Interfaces;

namespace ExpenseTracker.Services.Services
{
    public class ClientMasterService : IClientMasterService
	{
		private readonly IClientMasterRepository _clientMasterRepository;
        private readonly IInvestmentRepository _investmentRepository;
        private readonly IMapper _mapper;
		private readonly IDocumentProcessor _documentProcessor;
		private readonly IConfiguration _configuration;
		private ResponseDto _responseDto;
        private readonly ILogger<ClientMasterService> _logger;
		private readonly IBusinessDevTeamRepository _bdtRepo;

        public ClientMasterService(IClientMasterRepository clientMasterRepository, 
            IMapper mapper,
            IDocumentProcessor documentProcessor,
            IConfiguration configuration,
            ResponseDto responseDto,
            ILogger<ClientMasterService> logger
			,IBusinessDevTeamRepository bdtRepo,
            IInvestmentRepository investmentRepository)
        {
            _clientMasterRepository = clientMasterRepository;
            _mapper = mapper;
            _documentProcessor = documentProcessor;
            _configuration = configuration;
            _responseDto = responseDto;
            _logger = logger;
            _bdtRepo = bdtRepo;
			_investmentRepository = investmentRepository;
        }
        public async Task<ResponseDto> AddClientMasterAsync(ClientMasterRequestDto clientMasterDto)
		{
			try
			{
				// Map DTO to Entity
				var clientEntity = _mapper.Map<ClientMaster>(clientMasterDto);


				// Add Client Master
				var client = await _clientMasterRepository.AddClientMasterAsync(clientEntity);

                _logger.LogInformation("Added new client master with ID: {ClientId}", client.ClientId);

                string documentContainer = _configuration.GetValue<string>("AzStorageConfig:documentContainer")??"documents";
                _logger.LogInformation("Document container: {DocumentContainer}", documentContainer);

                string path = string.Empty;
				//upload profile pic
				if(clientMasterDto.ProfileImageAttachment != null)
				{					
					path =await _documentProcessor.UploadDocument
						(documentContainer
						,$"{client.ClientId}{client.Name.Replace(" ", "")}/{_configuration.GetValue<string>("AttachmentOptions:ProfileImage:fileName")}{Path.GetExtension(clientMasterDto.ProfileImageAttachment.FileName)}"
						,clientMasterDto.ProfileImageAttachment.OpenReadStream());

					client.ProfileImageAttachmentPath = $"{path}";
                    _logger.LogInformation("Uploaded profile image and set path: {ProfileImagePath}", path);
                }               

                //upload aadhar pic
                if (clientMasterDto.AadharAttachment != null)
				{
					path = await _documentProcessor.UploadDocument
						(documentContainer
						, $"{client.ClientId}{client.Name.Replace(" ", "")}/{_configuration.GetValue<string>("AttachmentOptions:Aadhar:fileName")}{Path.GetExtension(clientMasterDto.AadharAttachment.FileName)}"
						, clientMasterDto.AadharAttachment.OpenReadStream());

					client.AadharAttachmentPath = $"{path}";
                    _logger.LogInformation("Uploaded Aadhar image and set path: {AadharAttachmentPath}", path);

                }

                //upload aadhar pic
                if (clientMasterDto.PanAttachment != null)
				{
					path = await _documentProcessor.UploadDocument
						(documentContainer
						, $"{client.ClientId}{client.Name.Replace(" ", "")}/{_configuration.GetValue<string>("AttachmentOptions:Pan:fileName")}{Path.GetExtension(clientMasterDto.PanAttachment.FileName)}"
						, clientMasterDto.PanAttachment.OpenReadStream());

					client.PanAttachmentPath = $"{path}";
                    _logger.LogInformation("Uploaded PAN image and set path: {PanAttachmentPath}", path);

                }



				//Update the paths
				await _clientMasterRepository.SaveChangesAsync();
				//await _clientMasterRepository.UpdateClientMasterAsync(client);	

				//add banking details

				if (clientMasterDto.LstClientBankingDetails != null && clientMasterDto.LstClientBankingDetails.Count > 0) {
                    List<ClientBankingDetail> bankingDetails = new List<ClientBankingDetail>();

                    foreach (var bank in clientMasterDto.LstClientBankingDetails)
                    {
                        bankingDetails.Add(new ClientBankingDetail
                        {
                            ClientId = client.ClientId,
                            InvestmentId = 0,
                            InvestmentGuid = Guid.Empty,
                            Mode = bank.Mode,
                            AccountHolderName = bank.AccountHolderName,
                            AccountNoOrUpiId = bank.AccountNoOrUpiId,
                            BankName = bank.BankName,
                            IFSCCode = bank.IFSCCode,
                            Status = true,
                            CreatedDate = DateTime.Now,
                            CreatedBy = client.CreatedBy,
                            BankingType = "Client",
                            BusinessDevTeamId = 0
                        });
                    }

                    //save banking details
                    await _clientMasterRepository.CreateBankingDetails(bankingDetails);

                }
				


    //            try
				//{
    //                //Create Entry in BD

    //                BusinessDevTeam bd = new BusinessDevTeam()
    //                {
				//		ClientId = client.ClientId,
    //                    PanNo = client.PanNo,
    //                    AadharNo = client.AadharNo,
    //                    Name = client.Name,
    //                    Address = $"{client.PerAddressLine1} {client.PerAddressLine2} {client.PerCity} {client.PerState} {client.PerPinCode}",
				//		IsClient = true,
				//		JoiningDate = DateTime.Now
    //                };
    //                await _bdtRepo.CreateBusinessDevTeamAsync(bd);
				//}
				//catch (Exception ex)
				//{
				//	//dont throw even if exception
				//	_logger.LogError($"Unable to add client as BD in system - {ex}");
				//	_responseDto.Message = $"Client created, But not added in BD team. {ex.Message}";
				//}
				
			}
			catch (Exception ex)
			{
				_responseDto.IsSuccess = false;
				_responseDto.Message = ex.Message;
                _logger.LogError(ex, "An error occurred while adding client master.");

            }

            return _responseDto;
		}

		public Task<ResponseDto> DeleteClientMasterAsync(int id)
		{
			throw new NotImplementedException();
		}

		public async Task<ResponseDto> GetAllClientMastersAsync()
		{
			try
			{
				List<ClientMaster> clients = await _clientMasterRepository.GetAllClientMastersAsync();
				List<ClientMasterResponseDto> clientsDto = _mapper.Map<List<ClientMasterResponseDto>>(clients);
				_responseDto.Result = clientsDto;
			}
			catch (Exception ex)
			{
				_responseDto.IsSuccess = false;
				_responseDto.Message = ex.Message;
                _logger.LogError(ex, "An error occurred while fetching all client masters.");

            }
            return _responseDto;
			
		}

		public async Task<ResponseDto> GetClientMasterByGuidAsync(Guid id)
		{
			try
			{
				ClientMaster client = await _clientMasterRepository.GetClientMasterByGuidAsync(id);
				ClientMasterResponseDto clientMasterDto = _mapper.Map<ClientMasterResponseDto>(client);

				string documentContainer = _configuration.GetValue<string>("AzStorageConfig:documentContainer") ?? "documents";
				if (!string.IsNullOrWhiteSpace(clientMasterDto.ProfileImageAttachmentPath))
				{
                    _logger.LogInformation("Fetching profile picture from path: {ProfileImagePath}", clientMasterDto.ProfileImageAttachmentPath);
                    clientMasterDto.ProfilePicDoc = await _documentProcessor.GetDocument(documentContainer, clientMasterDto.ProfileImageAttachmentPath);
				}

				_responseDto.Result = clientMasterDto;
			}
			catch (Exception ex)
			{
				_responseDto.IsSuccess = false;
				_responseDto.Message = ex.Message;
                _logger.LogError(ex, "An error occurred while fetching client master by GUID: {ClientGuid}", id);
            }
            return _responseDto;
		}

		public async Task<ResponseDto> GetClientMasterByIdAsync(int id)
		{
			try
			{
				ClientMaster client = await _clientMasterRepository.GetClientMasterByIdAsync(id);
				ClientMasterResponseDto clientMasterDto = _mapper.Map<ClientMasterResponseDto>(client);
				
				string documentContainer = _configuration.GetValue<string>("AzStorageConfig:documentContainer")?? "documents";
				if (! string.IsNullOrWhiteSpace(clientMasterDto.ProfileImageAttachmentPath))
				{
					clientMasterDto.ProfilePicDoc = await _documentProcessor.GetDocument(documentContainer, clientMasterDto.ProfileImageAttachmentPath);
				}

                if (!string.IsNullOrWhiteSpace(clientMasterDto.PanAttachmentPath))
                {
                    clientMasterDto.PanCardDoc = await _documentProcessor.GetDocument(documentContainer, clientMasterDto.PanAttachmentPath);
                }

                if (!string.IsNullOrWhiteSpace(clientMasterDto.AadharAttachmentPath))
                {
                    clientMasterDto.AadharCardDoc = await _documentProcessor.GetDocument(documentContainer, clientMasterDto.AadharAttachmentPath);
                }

                _responseDto.Result = clientMasterDto;
                _logger.LogInformation("Successfully retrieved client master by ID: {ClientId}", id);
            }
            catch (Exception ex)
			{
				_responseDto.IsSuccess = false;
				_responseDto.Message = ex.Message;
                _logger.LogError(ex, "An error occurred while fetching client master by ID: {ClientId}", id);
            }
            return _responseDto;
		}

		public async Task<ResponseDto> UpdateClientMasterAsync(ClientMasterRequestDto clientMasterDto)
		{
			try
			{
				string documentContainer = _configuration.GetValue<string>("AzStorageConfig:documentContainer") ?? "documents";                
				if (clientMasterDto.ProfileImageAttachment != null)
                {
                    //_logger.LogInformation("Replacing profile image at path: {ProfileImagePath}", clientMasterDto.ProfileImageAttachmentPath);

                    clientMasterDto.ProfileImageAttachmentPath = clientMasterDto.ProfileImageAttachmentPath ?? $"{clientMasterDto.ClientId}{clientMasterDto.Name.Replace(" ", "")}/{_configuration.GetValue<string>("AttachmentOptions:ProfileImage:fileName")}{Path.GetExtension(clientMasterDto.ProfileImageAttachment.FileName)}";
                    bool res = await _documentProcessor.ReplaceDocument(documentContainer, clientMasterDto.ProfileImageAttachmentPath, clientMasterDto.ProfileImageAttachment.OpenReadStream());
                    if (!res)
                    {
                        clientMasterDto.ProfileImageAttachmentPath = null;
                    }

                }

                if (clientMasterDto.AadharAttachment != null)
				{
                    clientMasterDto.AadharAttachmentPath = clientMasterDto.AadharAttachmentPath ?? $"{clientMasterDto.ClientId}{clientMasterDto.Name.Replace(" ", "")}/{_configuration.GetValue<string>("AttachmentOptions:Aadhar:fileName")}{Path.GetExtension(clientMasterDto.AadharAttachment.FileName)}";

                   // _logger.LogInformation("Replacing Aadhar image at path: {AadharAttachmentPath}", clientMasterDto.AadharAttachmentPath);
                    bool res = await _documentProcessor.ReplaceDocument(documentContainer, clientMasterDto.AadharAttachmentPath, clientMasterDto.AadharAttachment.OpenReadStream());
                    if (!res)
                    {
                        clientMasterDto.AadharAttachment = null;
                    }
                }

				if (clientMasterDto.PanAttachment != null)
				{
                    clientMasterDto.PanAttachmentPath = clientMasterDto.PanAttachmentPath ?? $"{clientMasterDto.ClientId}{clientMasterDto.Name.Replace(" ", "")}/{_configuration.GetValue<string>("AttachmentOptions:Pan:fileName")}{Path.GetExtension(clientMasterDto.PanAttachment.FileName)}";

                    //_logger.LogInformation("Replacing PAN image at path: {PanAttachmentPath}", clientMasterDto.PanAttachmentPath);
                    bool res = await _documentProcessor.ReplaceDocument(documentContainer, clientMasterDto.PanAttachmentPath, clientMasterDto.PanAttachment.OpenReadStream());
                    if (!res)
                    {
                        clientMasterDto.PanAttachmentPath = null;
                    }
                }

				ClientMaster client = _mapper.Map<ClientMaster>(clientMasterDto);

                client.ProfileImageAttachmentPath = clientMasterDto.ProfileImageAttachmentPath;
                client.AadharAttachmentPath = clientMasterDto.AadharAttachmentPath;
                client.PanAttachmentPath = clientMasterDto.PanAttachmentPath;

				await _clientMasterRepository.UpdateClientMasterAsync(client);
			}
			catch (Exception ex)
			{
                _logger.LogError(ex, "An error occurred while updating client master.");
                _responseDto.IsSuccess = false;
				_responseDto.Message = ex.Message;
			}
			return _responseDto;

			
		}
        public async Task<ResponseDto> GetBankingDetailsByClientIdAsync(int clientId)
        {
            try
            {
                if (clientId <= 0)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "Invalid Client ID provided.";
                    return _responseDto;
                }

                var bankingDetails = await _clientMasterRepository.GetBankingDetailsByClientIdAsync(clientId);

                if (bankingDetails == null || !bankingDetails.Any())
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.Message = "No banking details found for the given Client ID.";
                    return _responseDto;
                }

                // Map the banking details to DTO
                var bankingDetailsDto = _mapper.Map<List<ClientBankingDetailsDto>>(bankingDetails);

                // Set success response
                _responseDto.IsSuccess = true;
                _responseDto.Message = "Banking details retrieved successfully.";
                _responseDto.Result = bankingDetailsDto;

                return _responseDto;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = $"An error occurred while retrieving banking details: {ex.Message}";
                _logger.LogError(ex, "An error occurred while fetching banking details for Client ID: {ClientId}", clientId);
                return _responseDto;
            }
        }
    }
}
