using MediatR;
using Qarc.Algos.SharedKernel.InputModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qarc.AlgoFactory.Core.Application.Bars.Queries
{
    public record class GetBarByIdQuery(string id) : IRequest<Bar>;
}
