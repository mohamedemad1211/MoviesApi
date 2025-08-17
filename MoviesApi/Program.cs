using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MoviesApi.Data;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionstring = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options
    => options.UseSqlServer(connectionstring));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors();
builder.Services.AddSwaggerGen(options =>
{// ثابت كفكره لكن بيتغير من مشروع للتانى SwaggerDoc
    options.SwaggerDoc("v1", new OpenApiInfo
    { 
        Version = "v1",
        Title = "TestApi",
        Description ="My First Api",
        TermsOfService = new Uri("https://facebook.com"),
        Contact = new OpenApiContact
        {
            Name = "Muhammed",
            Email = "moabdeen1911@gmail.com",
            Url =  new Uri("https://facebook.com")
        },
       License = new OpenApiLicense
       {
           Name = "My License",
           Url = new Uri("https://facebook.com")

       },
       
       
    });
    //ثابت لو المشروع بيستخدم JWT، لكن بيتغير لو فيه نوع Authentication مختلف
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT Key"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Name = "Bearer",
                In = ParameterLocation.Header,


            },
            new List<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseAuthorization();

app.MapControllers();

app.Run();
