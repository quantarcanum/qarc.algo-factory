using Qarc.Algos.MarketMakerFootprint.Models;

namespace Qarc.AlgoFactory.Adapter.Api.InputModels
{
    public class RunMMFAlgoInputModel
    {
        public RunAlgoInputModel RunAlgoInputModel { get; set; }
        public FootprintPatternsFilter FootprintPatternsFilter { get; set; }
    }
}
