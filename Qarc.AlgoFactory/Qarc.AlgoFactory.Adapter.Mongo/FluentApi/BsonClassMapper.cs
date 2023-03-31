using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using Qarc.Algos.CCTRBomb.Models;
using Qarc.Algos.MarketMakerFootprint.Models;
using Qarc.Algos.SharedKernel.InputModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qarc.AlgoFactory.Adapter.Mongo.FluentApi
{
    public static class BsonClassMapper
    {
        public static void MapAll()
        {
            MapBar();
        }

        public static void MapBar()
        {
            BsonClassMap.RegisterClassMap<Bar>(cm =>
            {
                cm.AutoMap(); //use this first, then override specific props

                cm.MapIdMember(c => c.Id)
                    .SetIdGenerator(new StringObjectIdGenerator())
                    .SetSerializer(new StringSerializer(BsonType.ObjectId))
                    .SetElementName("id");

                cm.SetIgnoreExtraElements(true); // no exception if more props come from json then the model has

                //cm.MapProperty(p => p.Instrument).SetElementName("instrument");
                //cm.UnmapMember(c => c.SomeProperty);
                //cm.MapMember(c => c.FavoriteColor).SetSerializer(new EnumSerializer<Color>(BsonType.String));
            });

            BsonClassMap.RegisterClassMap<GuerrillaBar>(cm =>
            {
                cm.AutoMap(); //use this first, then override specific props

                cm.MapIdMember(c => c.Id)
                    .SetIdGenerator(new StringObjectIdGenerator())
                    .SetSerializer(new StringSerializer(BsonType.ObjectId))
                    .SetElementName("id");

                cm.SetIgnoreExtraElements(true); // no exception if more props come from json then the model has

                cm.UnmapMember(c => c.PivotBias);
                cm.UnmapMember(c => c.PivotIndex);

            });

            BsonClassMap.RegisterClassMap<MarketMakerFootprintBar>(cm =>
            {
                cm.AutoMap();

                cm.MapIdMember(c => c.Id)
                    .SetIdGenerator(new StringObjectIdGenerator())
                    .SetSerializer(new StringSerializer(BsonType.ObjectId))
                    .SetElementName("id");

                cm.SetIgnoreExtraElements(true);

                cm.UnmapMember(c => c.BarIndex);
            });
        }

    }
}
