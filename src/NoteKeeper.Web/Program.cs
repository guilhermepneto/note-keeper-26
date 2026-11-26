using NoteKeeper.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Adiciona os serviços do MVC.
builder.Services.AddControllersWithViews();

// Registra a composição da camada de infraestrutura.
builder.Services.AdicionarInfraestrutura(builder.Configuration);

var app = builder.Build();

// Configura o pipeline de requisições HTTP.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // O valor padrão do HSTS é de 30 dias. Ajuste conforme o cenário de produção.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
