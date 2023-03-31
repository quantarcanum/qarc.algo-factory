using MongoDB.Driver;
using Qarc.Algos.CCTRBomb.Models;
using Qarc.Algos.MarketMakerFootprint.Models;
using Qarc.Algos.SharedKernel.InputModel;

namespace Qarc.AlgoFactory.Adapter.Mongo.Indexes
{
    public static class CollectionIndex
    {
        public static void CreateIndexIfRequired<T>(IMongoCollection<T> collection)
        {
            if (typeof(T) == typeof(Bar))
            {
                SetUniqueCompositeIndexForBarCollection((IMongoCollection<Bar>)collection);
            }

            if (typeof(T) == typeof(GuerrillaBar))
            {
                SetUniqueCompositeIndexForGuerrillaBarCollection((IMongoCollection<GuerrillaBar>)collection);
            }

            if (typeof(T) == typeof(MarketMakerFootprintBar))
            {
                SetUniqueCompositeIndexForMMFootprintBarCollection((IMongoCollection<MarketMakerFootprintBar>)collection);
            }
        }

        

        private static void SetUniqueCompositeIndexForBarCollection(IMongoCollection<Bar> collection)
        {
            var indexModel = new CreateIndexModel<Bar>(
             new IndexKeysDefinitionBuilder<Bar>()
                .Ascending(x => x.Instrument.Name)
                .Ascending(x => x.Aggregation.Type)
                .Ascending(x => x.Aggregation.Value)
                .Ascending(x => x.BarInfo.Time),
             new CreateIndexOptions() { Name = "ContentUniqueIndex", Unique = true });

            collection.Indexes.CreateOne(indexModel);
        }

        private static void SetUniqueCompositeIndexForGuerrillaBarCollection(IMongoCollection<GuerrillaBar> collection)
        {
            var indexModel = new CreateIndexModel<GuerrillaBar>(
             new IndexKeysDefinitionBuilder<GuerrillaBar>()
                .Ascending(x => x.Bar.Instrument.Name)
                .Ascending(x => x.Bar.Aggregation.Type)
                .Ascending(x => x.Bar.Aggregation.Value)
                .Ascending(x => x.Bar.BarInfo.Time),
             new CreateIndexOptions() { Name = "ContentUniqueIndex", Unique = true });

            collection.Indexes.CreateOne(indexModel);
        }

        private static void SetUniqueCompositeIndexForMMFootprintBarCollection(IMongoCollection<MarketMakerFootprintBar> collection)
        {
            var indexModel = new CreateIndexModel<MarketMakerFootprintBar>(
             new IndexKeysDefinitionBuilder<MarketMakerFootprintBar>()
                .Ascending(x => x.Bar.Instrument.Name)
                .Ascending(x => x.Bar.Aggregation.Type)
                .Ascending(x => x.Bar.Aggregation.Value)
                .Ascending(x => x.Bar.BarInfo.Time),
             new CreateIndexOptions() { Name = "ContentUniqueIndex", Unique = true });

            collection.Indexes.CreateOne(indexModel);
        }
    }
}
