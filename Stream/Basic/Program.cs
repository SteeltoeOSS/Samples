using Basic;
using Microsoft.AspNetCore.Builder;
using Steeltoe.Stream.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Host.AddStreamServices<MyStreamProcessor>();

var app = builder.Build();

await app.RunAsync();
