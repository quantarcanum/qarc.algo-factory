using MediatR;
using Qarc.Algos.MarketMakerFootprint.Models;
using Qarc.Algos.SharedKernel.InputModel;

namespace Qarc.AlgoFactory.Core.Application.MMFootprint
{
    public record class RunMMFootprintCommand(string instrumentName, string instrumentExchange, Aggregation aggregation, FootprintPatternsFilter filter) : IRequest<Unit>;
}
