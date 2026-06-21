using AutoMapper;
using ExpenseTracker.Services.Contracts.IContracts;
using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Models.EntityModels;
using ExpenseTracker.Services.Repository.Interface;

namespace ExpenseTracker.Services.Contracts
{
	public class BankMasterServices : IBankMasterService
	{
		private readonly IMasterRepository _masterRepository;
		private ResponseDto _responseDto;
		private readonly ILogger<BankMasterServices> _logger;
		private readonly IMapper _mapper;
		public BankMasterServices(IMasterRepository masterRepository, ResponseDto responseDto,ILogger<BankMasterServices> logger,IMapper mapper)
        {
            _masterRepository = masterRepository;
			_responseDto = responseDto;
			_logger = logger;
			_mapper = mapper;
        }
        public async Task<ResponseDto> AddBankMasterAsync(BankMasterDto bankMaster)
		{
			try
			{
				var bank = _mapper.Map<TblBankMaster>(bankMaster);
				await _masterRepository.AddBankMasterAsync(bank);
				_responseDto.Message = "Bank master created successfully";
			}
			catch (Exception ex)
			{

				_responseDto.IsSuccess = false;
				_responseDto.Message = $"Unable to add bank master. {ex.Message}";
			}

			return _responseDto;
		}

		public async Task<ResponseDto> DeleteBankMasterAsync(int id)
		{
			try
			{
				await _masterRepository.DeleteBankMasterAsync(id);				
			}
			catch (Exception ex)
			{
				_responseDto.IsSuccess = false;
				_responseDto.Message = ex.Message;
				_logger.LogError($"Unable to delete bank master - {ex.Message} ");
			}
			return _responseDto;
		}

		public async Task<ResponseDto> GetAllBankMastersAsync()
		{
			try
			{
				var banks = await _masterRepository.GetAllBankMastersAsync();
				List<BankMasterDto> bankMasterDto = _mapper.Map<List<BankMasterDto>>(banks);
				_responseDto.Result = bankMasterDto;
				
			}
			catch (Exception ex)
			{
				_responseDto.IsSuccess = false;
				_responseDto.Message = "Unable to get Bank Masters";				
			}
			return _responseDto;
		}

		public async Task<ResponseDto> GetBankMasterByIdAsync(int id)
		{
			try
			{
				var bankMaster = await _masterRepository.GetBankMasterByIdAsync(id) ?? throw new Exception($"Bank Master not found for ID {id}");
				_responseDto.Result = bankMaster;
			}
			catch (Exception ex)
			{
				_responseDto.IsSuccess=false;
				_responseDto.Message = ex.Message;
				_logger.LogError($"Unable to get the bank master - {ex}");
			}

			return _responseDto;

		}

		public async Task<ResponseDto> UpdateBankMasterAsync(BankMasterDto bank)
		{
			try
			{				
				await _masterRepository.UpdateBankMaster(bank);
			}
			catch (Exception ex)
			{

				_responseDto.IsSuccess = false;
				_responseDto.Message = ex.Message;
			}

			return _responseDto;
			

		}
	}
}
