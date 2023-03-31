using MediatR;
using Qarc.AlgoFactory.Core.Application.SharedKernel;
using Qarc.Algos.SharedKernel.InputModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qarc.AlgoFactory.Core.Application.Bars.Queries
{
    public class GetBarByIdHandler : IRequestHandler<GetBarByIdQuery, Bar>
    {
        private readonly IRepository<Bar> _repository;

        public GetBarByIdHandler(IRepository<Bar> repository)
        {
            this._repository = repository;
        }

        public async Task<Bar> Handle(GetBarByIdQuery request, CancellationToken cancellationToken)
        {
            return await this._repository.GetAsync(request.id);
        }
    }
}
