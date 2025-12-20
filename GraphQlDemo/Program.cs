using GraphQlDemo.Services;
using Microsoft.EntityFrameworkCore;








var builder = WebApplication.CreateBuilder(args);

// Add services to the container

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.  AddDbContext<GraphQlDemo.Models.EmployeesDbContext>(options => 
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services
    .AddGraphQLServer()
    .AddQueryType<GraphQlDemo.GraphQL.Query.Query>()
    .AddMutationType<GraphQlDemo.GraphQL.Mutation.Mutation>();

builder.Services.AddGrpc();
builder.Services.AddSwaggerGen();
// Add CORS before building the app
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

app.MapGraphQL("/graphQl");
app.MapGrpcService<EmployeeGrpcService>();
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
