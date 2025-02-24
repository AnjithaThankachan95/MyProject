var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Add services to the container.
builder.Services.AddControllers();
// Register Business Layer Services
builder.Services.AddScoped<CustomerManagement.Business.Interfaces.ICustomerService, CustomerManagement.Business.Services.CustomerService>();
// Register Data Layer Repository
builder.Services.AddScoped<CustomerManagement.Domain.Interfaces.ICustomerRepository, CustomerManagement.Domain.Repositories.CustomerRepository>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
