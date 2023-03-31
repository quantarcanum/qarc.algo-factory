using MediatR;
using Qarc.AlgoFactory.Core.Application.SharedKernel;
using Qarc.Algos.SharedKernel.InputModel;

namespace Qarc.AlgoFactory.Core.Application.Bars.Queries
{
    public class GetBarsHandler : IRequestHandler<GetBarsQuery, IEnumerable<Bar>>
    {
        private readonly IRepository<Bar> _repository;

        public GetBarsHandler(IRepository<Bar> repository)
        {
            this._repository = repository;
        }

        public async Task<IEnumerable<Bar>> Handle(GetBarsQuery request, CancellationToken cancellationToken)
        {
            return (IEnumerable<Bar>)await this._repository.GetAllAsync();
        }
    }
}
