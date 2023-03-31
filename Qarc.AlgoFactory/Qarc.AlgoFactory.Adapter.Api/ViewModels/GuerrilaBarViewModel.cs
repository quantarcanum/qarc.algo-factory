

using Qarc.Algos.SharedKernel.InputModel;

namespace Qarc.AlgoFactory.Adapter.Api.ViewModels
{
    public class GuerrilaBarViewModel
    {
        public string Id { get; set; }
        public BarInfo BarInfo { get; set; }
        public Instrument Instrument { get; set; }
        public Aggregation Aggregation { get; set; }
    }
}
