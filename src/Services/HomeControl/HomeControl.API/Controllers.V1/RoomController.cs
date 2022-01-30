using HomeControl.API.Application.Commands;
using HomeControl.API.Controllers.v1;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Net;
using System.Threading.Tasks;

namespace HomeControl.API.Controllers.V1
{
    public class RoomController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public RoomController(IMediator mediator, ILogger logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        [Route("create")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CancelOrderAsync([FromBody] CreateRoomCommand command, [FromHeader(Name = "x-requestid")] string requestId)
        {
            bool commandResult = false;

            if (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty)
            {
                var requestCreateRoom = new IdentifierCommand<CreateRoomCommand, bool>(command, guid);



                commandResult = await _mediator.Send(requestCreateRoom);
            }

            if (!commandResult)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
