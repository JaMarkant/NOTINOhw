using NOTINOhw.Components.FileConverter;
using StorageSolution;
using MailSender;
using NOTINOhw;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<FileToJsonConverter>();
builder.Services.AddSingleton<FileToXmlConverter>();
builder.Services.AddSingleton<IStorage, LocalStorage>();
builder.Services.AddSingleton<ConvertedFilesMailSender>();
builder.Services.AddSingleton<FileConverterFacade>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
    {
        options.SchemaFilter<EnumSchemaFilter>();
    });
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
