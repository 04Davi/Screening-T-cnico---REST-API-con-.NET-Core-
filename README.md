# Screening Técnico - API REST de Productos (.NET 8)

API REST sencilla desarrollada para el ejercicio de screening técnico (1 hora), cumpliendo los requisitos principales del documento proporcionado.

**Objetivo cumplido:**
- Evaluar conocimientos básicos de .NET Core, Entity Framework Core y APIs REST.

## Tecnologías utilizadas

- **.NET 8**
- ASP.NET Core Web API
- Entity Framework Core (con SQL Server)
- Swagger UI (para pruebas rápidas)

## Requisitos implementados

- GET /api/productos → Todos los productos activos
- GET /api/productos/{id} → Producto por ID (con manejo de no encontrado)
- POST /api/productos → Crear producto con validaciones (nombre, precio, stock)
- BONUS: PUT /api/productos/{id}/stock → Actualizar solo el stock

Formato de respuesta consistente:
```json
{
  "success": true/false,
  "message": "...",
  "errors": ["..."],
  "data": { ... } o null
}
