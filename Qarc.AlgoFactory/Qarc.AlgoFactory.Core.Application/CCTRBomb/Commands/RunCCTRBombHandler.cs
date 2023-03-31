using MediatR;
using Qarc.AlgoFactory.Core.Application.SharedKernel;
using Qarc.Algos.CCTRBomb;
using Qarc.Algos.CCTRBomb.Constants;
using Qarc.Algos.CCTRBomb.Models;
using Qarc.Algos.SharedKernel;
using Qarc.Algos.SharedKernel.InputModel;
using Qarc.Algos.SharedKernel.OutputModel;

namespace Qarc.AlgoFactory.Core.Application.CCTRBomb.Commands
{
    public class RunCCTRBombHandler : IRequestHandler<RunCCTRBombCommand, Unit>
    {
        private readonly IRepository<Bar> _barRepository;
        private readonly IRepository<GuerrillaBar> _bombRepository;

        public RunCCTRBombHandler(IRepository<Bar> barRepository, IRepository<GuerrillaBar> bombRepository)
        {
            this._barRepository = barRepository;
            this._bombRepository = bombRepository;
        }

        /// <summary>
        /// Get all bars filtering by Instrument, Exchange and aggregation 
        /// Get last bomb filtering by Instrument, Exchange and aggregation
        /// Filter out all bars before last bomb
        /// Apply algo on filtered bars and find latest bomb bars
        /// Write Bomb bars
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Unit> Handle(RunCCTRBombCommand request, CancellationToken cancellationToken)
        {
            var bars = (IEnumerable<Bar>)await this._barRepository.GetAllAsync(i => i.Instrument.Name == request.instrumentName && i.Instrument.Exchange == request.instrumentExchange && i.Aggregation.Type == request.aggregation.Type && i.Aggregation.Value == request.aggregation.Value);

            var bombBars = await this._bombRepository.GetAllAsync(i => i.Bar.Instrument.Name == request.instrumentName && i.Bar.Instrument.Exchange == request.instrumentExchange && i.Bar.Aggregation.Type == request.aggregation.Type && i.Bar.Aggregation.Value == request.aggregation.Value);
            if (bombBars.Any())
            {
                var lastBombBar = bombBars.OrderBy(i => i.Bar.BarInfo.Time).Last();
                if (lastBombBar != null && lastBombBar.Bar != null && lastBombBar.Bar.BarInfo != null)
                {
                    bars = bars.Where(i => i.BarInfo.Time > lastBombBar.Bar.BarInfo.Time);
                }
            }

            // problem! Ia ultimul broken pivot sau ceva de genul! altfel poti rata bombs (de ex daca pleaca de la pivot 1 - ratezi bomb!) si NU stoca duplicate! fa upsert! 
            if (bars.Any())
            {
                IAlgo<AlgoBar, IInputFilter> guerrillaAlgo = new CCTRBombAlgo();
                var guerrillaBarsChronoOrder = (IEnumerable<GuerrillaBar>)guerrillaAlgo.Run(bars, null);

                IEnumerable<GuerrillaBar> fullSeries = this.JoinBarsWithGuerrillaBars(bars, guerrillaBarsChronoOrder);

                await this._bombRepository.CreateManyAsync(fullSeries);
            }


            return Unit.Value;
        }

        private IEnumerable<GuerrillaBar> JoinBarsWithGuerrillaBars(IEnumerable<Bar> bars, IEnumerable<GuerrillaBar> guerrillaBarsChronoOrder)
        {
            var result = new List<GuerrillaBar>();
            if (guerrillaBarsChronoOrder.Any())
            {
                foreach (var bar in bars)
                {
                    var gBar = guerrillaBarsChronoOrder.SingleOrDefault(i => i.Bar.Id == bar.Id);
                    if (gBar != null)
                    {
                        result.Add(gBar);
                    }
                    else
                    {
                        var temp = new GuerrillaBar(bar, 0, PivotBias.High);
                        result.Add(temp);
                    }
                }
            }

            return result;
        }
    }
}
