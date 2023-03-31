

using Qarc.Algos.SharedKernel.InputModel;

namespace Qarc.AlgoFactory.Adapter.Api.InputModels
{
    public class RunAlgoInputModel
    {
        public string InstrumentName { get; set; }
        public string InstrumentExchange { get; set; }
        public Aggregation Aggregation { get; set; }
    }
}
