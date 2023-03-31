using AutoMapper;
using Qarc.AlgoFactory.Adapter.Api.ViewModels;
using Qarc.Algos.SharedKernel.InputModel;

namespace Qarc.AlgoFactory.Adapter.Api.Mapper
{
    public class CCTRBombBarProfile : Profile
    {
        public CCTRBombBarProfile()
        {
            //CreateMap<AggregatedDataInputModel, Bar>();
            CreateMap<Bar, GuerrilaBarViewModel>();
        }
    }
}
