using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace jAPS.API.Controllers
{
    public class ApiControllerBase : ControllerBase
    {
        private IMediator mediator;
        protected IMediator Mediator => mediator ??= HttpContext.RequestServices.GetService<IMediator>();
    }
}
