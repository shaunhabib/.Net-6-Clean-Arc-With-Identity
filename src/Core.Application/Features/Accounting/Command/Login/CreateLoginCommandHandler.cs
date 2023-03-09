using Core.Application.Contracts.Features.Accounting.Command.Login;
using Core.Application.Contracts.Features.Accounting.Command.Register;
using Core.Application.Extensions;
using Core.Application.Features.Accounting.Command.Register;
using Core.Application.Features.Brand.Commands.Create;
using Core.Domain.Shared.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;
using static LinqToDB.SqlQuery.SqlFromClause;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Core.Application.Features.Accounting.Command.Login
{
    public class CreateLoginCommandHandler : IRequestHandler<CreateLoginCommand, Response<bool>>
    {
        #region ctor and services
        private readonly ILogger<CreateLoginCommandHandler> _logger;
        private readonly ResourceManager _resourceManager;
        private List<String> _validationError;
        private readonly UserManager<IdentityUser> _userManager;

        public CreateLoginCommandHandler(ILogger<CreateLoginCommandHandler> logger, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _resourceManager = new ResourceManager(typeof(CreateLoginCommandHandlerResource));
            _validationError = new List<string>();
            _userManager = userManager;
        }
        #endregion
        public async Task<Response<bool>> Handle(CreateLoginCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(command.UserName);

                if (user is null)
                    return Response<bool>.Fail(_resourceManager.GetString("No_User_Found"));

                var result = await _userManager.CheckPasswordAsync(user, command.Password);

                if (!result)
                    return Response<bool>.Fail(_resourceManager.GetString("Incorrect_Password"));
                
                return Response<bool>.Success(true, _resourceManager.GetString("Success"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.GetFullMessage());
                _validationError.Add(ex.GetFullMessage());
                return Response<bool>.Fail(_validationError);
            }
        }
    }
}
