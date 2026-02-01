using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class ProductosController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductosController(AppDbContext context)
    {
        _context = context;
    }

    // 1. GET /api/productos → todos activos
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await _context.Productos
            .Where(p => p.Activo)
            .ToListAsync();

        return Ok(new ApiResponse<List<Producto>>
        {
            Success = true,
            Message = "",
            Errors = new(),
            Data = items
        });
    }

    // 2. GET /api/productos/{id}
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id)
    {
        var item = await _context.Productos.FindAsync(id);

        if (item == null || !item.Activo)
        {
            return NotFound(new ApiResponse<Producto?>
            {
                Success = false,
                Message = "Producto no encontrado",
                Errors = new() { "No existe un producto con el ID especificado" },
                Data = null
            });
        }

        return Ok(new ApiResponse<Producto>
        {
            Success = true,
            Message = "",
            Errors = new(),
            Data = item
        });
    }

    // 3. POST /api/productos
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductoCreateRequest request)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(request.Nombre) || request.Nombre.Length < 3 || request.Nombre.Length > 100)
            errors.Add("El nombre debe tener al menos 3 caracteres y máximo 100");

        if (request.Precio <= 0)
            errors.Add("El precio debe ser mayor a 0");

        if (request.Stock < 0)
            errors.Add("El stock debe ser mayor o igual a 0");

        if (errors.Count > 0)
        {
            return BadRequest(new ApiResponse<Producto?>
            {
                Success = false,
                Message = "Error de validación",
                Errors = errors,
                Data = null
            });
        }

        var producto = new Producto
        {
            Nombre = request.Nombre,
            Precio = request.Precio,
            Stock = request.Stock
            // Activo y FechaCreacion ya tienen default
        };

        _context.Productos.Add(producto);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = producto.ProductoId },
            new ApiResponse<Producto>
            {
                Success = true,
                Message = "Producto creado exitosamente",
                Errors = new(),
                Data = producto
            });
    }

    // BONUS: PUT /api/productos/{id}/stock
    [HttpPut("{id:long}/stock")]
    public async Task<IActionResult> UpdateStock(long id, [FromBody] StockUpdateRequest request)
    {
        var producto = await _context.Productos.FindAsync(id);

        if (producto == null || !producto.Activo)
        {
            return NotFound(new ApiResponse<Producto?>
            {
                Success = false,
                Message = "Producto no encontrado",
                Errors = new() { "No existe un producto con el ID especificado" },
                Data = null
            });
        }

        if (request.Cantidad < 0)
        {
            return BadRequest(new ApiResponse<Producto?>
            {
                Success = false,
                Message = "Error de validación",
                Errors = new() { "La cantidad debe ser mayor o igual a 0" },
                Data = null
            });
        }

        producto.Stock = request.Cantidad;
        await _context.SaveChangesAsync();

        return Ok(new ApiResponse<Producto>
        {
            Success = true,
            Message = "Stock actualizado exitosamente",
            Errors = new(),
            Data = producto
        });
    }
}

public class ProductoCreateRequest
{
    public string Nombre { get; set; } = "";
    public decimal Precio { get; set; }
    public int Stock { get; set; }
}

public class StockUpdateRequest
{
    public int Cantidad { get; set; }
}