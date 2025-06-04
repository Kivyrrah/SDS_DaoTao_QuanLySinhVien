// SchoolGRPC.Server/Program.cs
using ProtoBuf.Grpc.Server; 
using SchoolGRPC.Server.Data;
using SchoolGRPC.Server.Repositories;
using SchoolGRPC.Server.IRepositories;
using SchoolGRPC.Server.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCodeFirstGrpc(config => {
    
});

builder.Services.AddSingleton(sp => NHibernateHelper.SessionFactory);

builder.Services.AddScoped<ITeacherRepository, TeacherRepository>();
builder.Services.AddScoped<IClassRoomRepository, ClassRoomRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IStatisticsRepository, StatisticsRepository>();

var app = builder.Build();

app.UseRouting(); 

app.MapGrpcService<TeacherServiceImpl>();
app.MapGrpcService<ClassRoomServiceImpl>();
app.MapGrpcService<StudentServiceImpl>();
app.MapGrpcService<StatisticsServiceImpl>();

app.MapGet("/", () => "Code-First gRPC Server is running. Communication must be made through a gRPC client configured for Code-First.");

app.Run();