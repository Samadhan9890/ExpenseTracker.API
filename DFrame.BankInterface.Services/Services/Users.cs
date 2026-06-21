using AutoMapper;
using ExpenseTracker.Services.Contracts.IContracts;
using ExpenseTracker.Services.Contracts.RequestResponseDto;
using ExpenseTracker.Services.Data;
using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Models.EntityModels;
using Microsoft.EntityFrameworkCore;
using System;

namespace ExpenseTracker.Services.Contracts
{
    public class Users : IUserFactory
    {
        private readonly ILogger<Users> _logger;
        private readonly AppDBContext _dbContext;
        private readonly ResponseDto _responseDto;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public Users(ResponseDto responseDto, AppDBContext appDBContext,
                            ILogger<Users> logger, IMapper mapper, IConfiguration configuration)
        {
            _dbContext = appDBContext;
            _responseDto = responseDto;
            _logger = logger;
            _mapper = mapper;
            _configuration = configuration;
        }



        public async Task<ResponseDto>CreateUser(UserDto userDto)
        {
            try {
                TblUser user = _mapper.Map<TblUser>(userDto);
                user.UserPassword = BCrypt.Net.BCrypt.HashPassword(_configuration.GetValue<string>("DEFAULT_USER_PASSWORD"));
                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();
                _responseDto.Result = userDto;
                _logger.LogInformation($"User Created Successfully.");
            }
            catch (Exception ex)
            {

                _responseDto.IsSuccess = false;
                _responseDto.Message = $"Unable to create user master";
                _logger.LogError(ex, "An error occurred while creating user.");
            }
            return _responseDto;
        }

      public async Task<ResponseDto> DeleteUser(int userId)
        {
            try {
                await _dbContext.Users.Where(x => x.InternalUserId == userId)
                                                           .ExecuteDeleteAsync();
                await _dbContext.SaveChangesAsync();
                _responseDto.Result = userId;
                _logger.LogInformation($"Record Deleted for User Id {userId}");
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Unable to Delete User";
                _logger.LogError(ex, "An error occurred while deleting user with Id: {UserId}", userId);
            }
            return _responseDto;
        }

      public async Task<ResponseDto> GetUserAsync(int userId)
        {
            try
            {
                _responseDto.Result = await _dbContext.Users.AsNoTracking().Where(x => x.InternalUserId == userId).FirstOrDefaultAsync();
                _logger.LogInformation($"Retunrining Business Unit Details for ID {userId}");
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Unable to get User";
                _logger.LogError(ex, "An error occurred while retrieving user with Id: {UserId}", userId);
            }
            return _responseDto;
        }

      public async  Task<ResponseDto> GetUsersAsync()
        {
            try
            {
                _responseDto.Result = await _dbContext.Users.AsNoTracking().ToListAsync();
                _logger.LogInformation($"Retunrining All Business Unit");
            }
			catch (Exception ex)
			{
				_responseDto.IsSuccess = false;
				_responseDto.Message = "Unable to get Users";
                _logger.LogError(ex, "An error occurred while retrieving all users.");
            }
            return _responseDto;
}

       public async Task<ResponseDto> UpdateUser(UserDto userDto)
        {
            try
            {
                TblUser existingUserMaster = await _dbContext.Users.Where(b => b.InternalUserId == userDto.InternalUserId)
                                      .FirstAsync();
                _mapper.Map(userDto, existingUserMaster);
                _dbContext.Users.Update(existingUserMaster).Property(p => p.InternalUserId).IsModified = false;
                await _dbContext.SaveChangesAsync();
                _responseDto.Result = userDto;
                _logger.LogInformation($"User Updated Successfully.");
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Unable to update user";
                _logger.LogError(ex, "An error occurred while updating user with Id: {UserId}", userDto.InternalUserId);
            }
            return _responseDto;
        }

        public async Task<ResponseDto> UpdatePassword(int id, ChangePasswordModel userPassword, string fields)
        {
            try
            {
                var _user = GetUserAsync(id).Result;
                TblUser user = _mapper.Map<TblUser>(_user.Result);

                if (BCrypt.Net.BCrypt.Verify(userPassword.PrevPassword, user.UserPassword))
                {

                    if (fields.Contains("userPassword"))
                    {
                        user.UserPassword = BCrypt.Net.BCrypt.HashPassword(userPassword.UserPassword);
                    }

                    if (fields.Contains("prevPassword"))
                    {
                        user.PrevPassword = userPassword.PrevPassword;
                    }

                    _dbContext.Entry(user).State = EntityState.Modified;
                    await _dbContext.SaveChangesAsync();
                    _responseDto.Result = user;
                    _logger.LogInformation($"User password Updated Successfully.");
                }
                else
                {
                    _responseDto.IsSuccess = true;
                    _responseDto.Result = null;
                    _responseDto.Message = "Unable to update password";
                }
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Unable to update password";
                _logger.LogError(ex, "An error occurred while updating password for user with Id: {Id}", id);
            }
            return _responseDto;
        }
    }
}
