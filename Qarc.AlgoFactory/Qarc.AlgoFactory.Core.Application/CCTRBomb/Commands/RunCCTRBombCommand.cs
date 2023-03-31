using MediatR;
using Qarc.Algos.SharedKernel.InputModel;

namespace Qarc.AlgoFactory.Core.Application.CCTRBomb.Commands
{
    public record class RunCCTRBombCommand(string instrumentName, string instrumentExchange, Aggregation aggregation) : IRequest<Unit>;
}
