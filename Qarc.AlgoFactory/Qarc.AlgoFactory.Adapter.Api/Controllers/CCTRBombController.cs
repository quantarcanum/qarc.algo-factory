using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Qarc.AlgoFactory.Adapter.Api.InputModels;
using Qarc.AlgoFactory.Core.Application.CCTRBomb.Commands;

namespace Qarc.AlgoFactory.Adapter.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CCTRBombController : ControllerBase
    {
        private readonly ILogger<CCTRBombController> _logger;
        private readonly ISender _senderMediator;
        private readonly IMapper _mapper;

        public CCTRBombController(ILogger<CCTRBombController> logger, ISender senderMediator, IMapper mapper)
        {
            _logger = logger;
            _senderMediator = senderMediator;
            _mapper = mapper;
        }

        [HttpPost(Name = "RunCCTRBomb")]
        public async Task<IActionResult> Post([FromBody] RunAlgoInputModel filter)
        {
            try
            {
                var result = await this._senderMediator.Send(new RunCCTRBombCommand(filter.InstrumentName, filter.InstrumentExchange, filter.Aggregation));

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + " " + ex.StackTrace);
            }
        }
    }
}
