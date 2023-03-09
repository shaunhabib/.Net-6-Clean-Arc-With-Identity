using Core.Application.Contracts.Features.Accounting.Command.Register;
using Core.Application.Contracts.Features.Brand.Commands.Create;
using Core.Application.Extensions;
using Core.Application.Features.Brand.Commands.Create;
using Core.Domain.Persistence.Contracts;
using Core.Domain.Shared.Contacts;
using Core.Domain.Shared.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;
using System.Transactions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Core.Application.Features.Accounting.Command.Register
{
    public class CreateRegisterCommandHandler : IRequestHandler<CreateRegisterCommand, Response<bool>>
    {
        #region ctor and services
        private readonly ILogger<CreateRegisterCommandHandler> _logger;
        private readonly ResourceManager _resourceManager;
        private List<String> _validationError;
        private readonly UserManager<IdentityUser> _userManager;

        public CreateRegisterCommandHandler(ILogger<CreateRegisterCommandHandler> logger, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _resourceManager = new ResourceManager(typeof(CreateRegisterCommandHandlerResource));
            _validationError = new List<string>();
            _userManager = userManager;
        }
        #endregion
        public async Task<Response<bool>> Handle(CreateRegisterCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var user = new IdentityUser
                {
                    UserName = command.UserName,
                    Email = command.Email
                };
                var result = await _userManager.CreateAsync(user, command.Password);

                if (!result.Succeeded)
                    return Response<bool>.Fail(_resourceManager.GetString("Failed"));

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
