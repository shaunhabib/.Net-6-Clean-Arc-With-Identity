using Core.Application.Contracts.Features.Accounting.Command.Login;
using Core.Application.Contracts.Features.Accounting.Command.Register;
using Core.Domain.Shared.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Web.Api.Controllers
{
    public class AccountingController : BaseApiController
    {

        [HttpPost]
        [ProducesResponseType(typeof(Response<bool>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Register(CreateRegisterCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(Response<bool>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Login(CreateLoginCommand command)
        {
            var response = await Mediator.Send(command);
            return Ok(response);
        }
    }
}
