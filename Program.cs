using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Conexión a TU base de datos existente
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configuración HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

// ──────────────────────────────────────────────
// Todo lo demás en el mismo archivo (simple y rápido)

// Contexto EF
public class AppDbContext : DbContext
{
    public DbSet<Producto> Productos { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}

// Modelo (copia exacta del screening)
public class Producto
{
    public long ProductoId { get; set; }
    public string Nombre { get; set; } = "";
    public decimal Precio { get; set; }
    public int Stock { get; set; }
    public bool Activo { get; set; } = true;
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
}

// Respuesta estándar (exacto como piden)
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = "";
    public List<string> Errors { get; set; } = new();
    public T? Data { get; set; }
}