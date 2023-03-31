using Qarc.AlgoFactory.Core.Application.Bars.Queries;
using System.Reflection;
using Qarc.AlgoFactory.Adapter.Mongo.Bootstrapper;
using Qarc.Algos.SharedKernel.InputModel;
using Qarc.Algos.CCTRBomb.Models;
using Qarc.Algos.MarketMakerFootprint.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddMediatR(x => x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(GetBarsHandler).Assembly));

builder.Services.AddMongoServices()
    .AddMongoRepository<Bar>("BarCollection")
    .AddMongoRepository<GuerrillaBar>("GuerrillaBarCollection")
    .AddMongoRepository<MarketMakerFootprintBar>("MarketMakerFootprintBarCollection");

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
