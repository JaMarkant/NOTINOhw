using NOTINOhw.Components.FileConvertor;
using StorageSolution;
using MailSender;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<FileToJsonConvertor>();
builder.Services.AddSingleton<FileToXmlConvertor>();
builder.Services.AddSingleton<IStorage, LocalStorage>();
builder.Services.AddSingleton<ConvertedFilesMailSender>();
builder.Services.AddSingleton<FileConvertorFacade>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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
