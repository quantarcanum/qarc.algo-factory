using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Qarc.AlgoFactory.Adapter.Api.InputModels;
using Qarc.AlgoFactory.Adapter.Api.ViewModels;
using Qarc.AlgoFactory.Core.Application.Bars.Queries;
using Qarc.AlgoFactory.Core.Application.CCTRBomb.Commands;
using Qarc.AlgoFactory.Core.Application.MMFootprint;

namespace Qarc.AlgoFactory.Adapter.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MMFootprintController : ControllerBase
    {
        private readonly ILogger<MMFootprintController> _logger;
        private readonly ISender _senderMediator;
        private readonly IMapper _mapper;

        public MMFootprintController(ILogger<MMFootprintController> logger, ISender senderMediator, IMapper mapper)
        {
            _logger = logger;
            _senderMediator = senderMediator;
            _mapper = mapper;
        }

        [HttpPost(Name = "RunMMFootprints")]
        public async Task<IActionResult> Post([FromBody] RunMMFAlgoInputModel filter)
        {
            try
            {
                var result = await this._senderMediator.Send(new RunMMFootprintCommand(filter.RunAlgoInputModel.InstrumentName, filter.RunAlgoInputModel.InstrumentExchange, filter.RunAlgoInputModel.Aggregation, filter.FootprintPatternsFilter));

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + " " + ex.StackTrace);
            }
        }

        [HttpGet(Name = "GetBars")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await this._senderMediator.Send(new GetBarsQuery());

                if (result == null)
                {
                    return NotFound();
                }

                var viewModel = this._mapper.Map<IEnumerable<GuerrilaBarViewModel>>(result);

                return Ok(viewModel);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
