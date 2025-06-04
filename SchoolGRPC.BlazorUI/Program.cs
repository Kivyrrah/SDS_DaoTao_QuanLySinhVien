using SchoolGRPC.BlazorUI.Components;
using SchoolGRPC.Shared.Contracts.Services;
using ProtoBuf.Grpc.ClientFactory; 

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddAntDesign(); 

var grpcServerUrl = builder.Configuration["GrpcServerUrl"] ?? "https://localhost:7001"; 

builder.Services.AddCodeFirstGrpcClient<ITeacherService>(options => 
{
    options.Address = new Uri(grpcServerUrl);
});
builder.Services.AddCodeFirstGrpcClient<IClassRoomService>(options => 
{
    options.Address = new Uri(grpcServerUrl);
});
builder.Services.AddCodeFirstGrpcClient<IStudentService>(options => 
{
    options.Address = new Uri(grpcServerUrl);
});
builder.Services.AddCodeFirstGrpcClient<IStatisticsService>(options => 
{
    options.Address = new Uri(grpcServerUrl);
});


var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();