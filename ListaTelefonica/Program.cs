using ListaTelefonica.Models;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
app.UseHttpsRedirection();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var client = new MongoClient("mongodb://localhost:27017");
var db = client.GetDatabase("lista-telefonica");
var collection = db.GetCollection<Contato>("contatos");

app.MapGet("contato", async () =>
{
    var filter = Builders<Contato>.Filter.Empty;
    var cursor = await collection.FindAsync(filter);
    var contatos = await cursor.ToListAsync();
    return contatos.Any() ? Results.Ok(contatos) : Results.NoContent();
});

app.MapGet("contato/{cpf}", async (string cpf) =>
{
    var filter = Builders<Contato>.Filter.Eq(c => c.Cpf, cpf);
    var cursor = await collection.FindAsync(filter);
    var contato = await cursor.FirstOrDefaultAsync();
    return contato == null ? Results.NoContent() : Results.Ok(contato);

});

app.MapPost("contato", async (Contato contato) =>
{
    if (contato == null)
        return Results.BadRequest();
    await collection.InsertOneAsync(contato);
    return Results.Created($"https://localhost:7019/contato/{contato.Cpf}", contato);
});

app.MapPut("contato/{cpf}", async (string cpf, Contato contato) =>
{
    if (contato == null)
        return Results.BadRequest();
    var filter = Builders<Contato>.Filter.Eq(c => c.Cpf, cpf);
    var update = Builders<Contato>.Update
        .Set(c => c.Cpf, contato.Cpf)
        .Set(c => c.Nome, contato.Nome)
        .Set(c => c.Sobrenome, contato.Sobrenome)
        .Set(c => c.Telefone, contato.Telefone);
    var result = await collection.UpdateOneAsync(filter, update);
    return Results.Ok();
});

app.MapDelete("contato/{cpf}", async (string cpf) =>
{
    var filter = Builders<Contato>.Filter.Eq(c => c.Cpf, cpf);
    await collection.DeleteOneAsync(filter);
    return Results.Ok();
});

app.Run();
