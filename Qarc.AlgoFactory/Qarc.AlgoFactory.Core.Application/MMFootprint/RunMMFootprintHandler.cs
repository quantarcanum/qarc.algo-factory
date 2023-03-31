using MediatR;
using Qarc.AlgoFactory.Core.Application.CCTRBomb.Commands;
using Qarc.AlgoFactory.Core.Application.SharedKernel;
using Qarc.Algos.CCTRBomb;
using Qarc.Algos.CCTRBomb.Models;
using Qarc.Algos.MarketMakerFootprint;
using Qarc.Algos.MarketMakerFootprint.Models;
using Qarc.Algos.SharedKernel;
using Qarc.Algos.SharedKernel.InputModel;
using Qarc.Algos.SharedKernel.OutputModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qarc.AlgoFactory.Core.Application.MMFootprint
{
    public class RunMMFootprintHandler : IRequestHandler<RunMMFootprintCommand, Unit>
    {
        private readonly IRepository<Bar> _barRepository;
        private readonly IRepository<MarketMakerFootprintBar> _mmfootprintRepository;

        public RunMMFootprintHandler(IRepository<Bar> barRepository, IRepository<MarketMakerFootprintBar> mmfootprintRepository)
        {
            this._barRepository = barRepository;
            this._mmfootprintRepository = mmfootprintRepository;
        }

        public async Task<Unit> Handle(RunMMFootprintCommand request, CancellationToken cancellationToken)
        {
            var bars = (IEnumerable<Bar>)await this._barRepository.GetAllAsync(i => i.Instrument.Name == request.instrumentName && i.Instrument.Exchange == request.instrumentExchange && i.Aggregation.Type == request.aggregation.Type && i.Aggregation.Value == request.aggregation.Value);

            var mmfBars = await this._mmfootprintRepository.GetAllAsync(i => i.Bar.Instrument.Name == request.instrumentName && i.Bar.Instrument.Exchange == request.instrumentExchange && i.Bar.Aggregation.Type == request.aggregation.Type && i.Bar.Aggregation.Value == request.aggregation.Value);
            if (mmfBars.Any())
            {
                var lastMMFBar = mmfBars.OrderBy(i => i.Bar.BarInfo.Time).Last();
                if (lastMMFBar != null && lastMMFBar.Bar != null && lastMMFBar.Bar.BarInfo != null)
                {
                    bars = bars.Where(i => i.BarInfo.Time > lastMMFBar.Bar.BarInfo.Time);
                }
            }
 
            if (bars.Any())
            {
                var tickSize = bars.First().Instrument.TickSize;
                var timeDiff = bars.First().Instrument.TimezoneDifference;
                IAlgo<AlgoBar, IInputFilter> mmfootprintsAlgo = new MarketMakerFootprintAlgo(tickSize, timeDiff);
                var mmfBarsChronoOrder = (IEnumerable<MarketMakerFootprintBar>)mmfootprintsAlgo.Run(bars, request.filter);

                await this._mmfootprintRepository.CreateManyAsync(mmfBarsChronoOrder);
            }

            return Unit.Value;
        }
    }
}
