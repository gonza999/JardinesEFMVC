using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JardinesEF.Datos.Comun.Facades;
using JardinesEf.Entidades.Entidades;

namespace JardinesEF.Datos.Sql.Repositorios
{
    public class ProductosRepositorio : IProductosRepositorio
    {
        private readonly ViveroSqlDbContext _context;

        public ProductosRepositorio(ViveroSqlDbContext context)
        {
            _context = context;
        }
        public List<Producto> GetLista(int cantidad, int pagina)
        {
            try
            {
                return _context.Productos
                    .OrderBy(c => c.NombreProducto)
                    .Skip(cantidad * (pagina - 1))
                    .Take(cantidad)
                    .AsNoTracking().ToList();
            }
            catch (Exception e)
            {
                throw new Exception("Error al leer : " + e.Message);
            }

        }

        public Producto GetTEntityPorId(int id)
        {
            try
            {
                return _context.Productos
                    .Include(c => c.Categoria)
                    .Include(c => c.Proveedor)
                    .SingleOrDefault(c => c.ProductoId == id);
            }
            catch (Exception e)
            {
                throw new Exception("Error al leer");
            }
        }

        public void Guardar(Producto producto)
        {
            try
            {
                if (producto.Categoria != null)
                {
                    _context.Categorias.Attach(producto.Categoria);
                }
                if (producto.Proveedor != null)
                {
                    _context.Proveedores.Attach(producto.Proveedor);
                }
                if (producto.ProductoId == 0)
                {
                    _context.Productos.Add(producto);
                }
                else
                {
                    var productoInDb =
                        _context.Productos.SingleOrDefault(c => c.ProductoId == producto.ProductoId);
                    if (productoInDb == null)
                    {
                        throw new Exception("Producto inexistente");
                    }
                    productoInDb.NombreProducto = producto.NombreProducto;
                    productoInDb.ProveedorId = producto.ProveedorId;
                    productoInDb.CategoriaId = producto.CategoriaId;
                    productoInDb.UnidadesEnStock = producto.UnidadesEnStock;
                    productoInDb.PrecioUnitario = producto.PrecioUnitario;
                    productoInDb.NivelDeReposicion = producto.NivelDeReposicion;
                    productoInDb.NombreLatin = producto.NombreLatin;
                    productoInDb.Imagen = producto.Imagen;
                    _context.Entry(productoInDb).State = EntityState.Modified;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error al intentar guardar un registro");
            }
        }

        public bool Existe(Producto producto)
        {
            try
            {
                if (producto.ProductoId == 0)
                {
                    return _context.Productos.Any(c => c.NombreProducto == producto.NombreProducto);
                }

                return _context.Productos.Any(c => c.NombreProducto == producto.NombreProducto
                                                                             && c.ProductoId != producto.ProductoId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public bool EstaRelacionado(Producto producto)
        {
            try
            {
                return _context.DetallesOrdenes.Any(d => d.ProductoId == producto.ProductoId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int GetCantidad()
        {
            try
            {
                return _context.Productos.Count();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Borrar(int id)
        {
            Producto productoInDb = null;
            try
            {
                productoInDb = _context.Productos
                    .SingleOrDefault(c => c.ProductoId == id);
                if (productoInDb == null) return;
                _context.Entry(productoInDb).State = EntityState.Deleted;
                //_context.SaveChanges();
            }
            catch (Exception e)
            {
                _context.Entry(productoInDb).State = EntityState.Unchanged;
                throw new Exception(e.Message);
            }
        }

        public List<Producto> GetLista()
        {
            try
            {
                return _context.Productos.
                    OrderBy(c => c.NombreProducto).
                    ToList();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int GetCantidad(Func<Producto, bool> predicate)
        {
            try
            {
                return _context.Productos.Count(predicate);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<Producto> Find(Func<Producto, bool> predicate, int? cantidad, int? pagina)
        {
            try
            {
                IEnumerable<Producto> query = _context.Productos
                   .Include(c => c.Categoria).AsEnumerable()
                   .Where(predicate)
                   .OrderBy(p => p.NombreProducto);
                if (cantidad.HasValue && pagina.HasValue)
                {
                    query = query.Skip(cantidad.Value * (pagina.Value - 1))
                        .Take(cantidad.Value);
                }
                return query.ToList();

            }
            catch (Exception e)
            {
                throw new Exception("Error al leer");
            }
        }

        public List<Producto> GetLista(int categoriaId)
        {
            try
            {
                return _context.Productos
                    .Where(p => p.CategoriaId == categoriaId)
                    .OrderBy(c => c.NombreProducto)
                    .ToList();
            }
            catch (Exception e)
            {
                throw new Exception("Error al leer");
            }
        }

        public void SetearReservarProducto(int productoId, decimal cantidad)
        {
            try
            {
                var productoInDb = _context.Productos.SingleOrDefault(p => p.ProductoId == productoId);
                productoInDb.UnidadesEnPedido += Convert.ToInt32(cantidad);
                _context.Entry(productoInDb).State = EntityState.Modified;
            }
            catch (Exception e)
            {
                throw new Exception("Error al guardar");
            }
        }

        public void ActualizarStock(int productoId, decimal cantidad)
        {
            try
            {
                var productoInDb = _context.Productos.SingleOrDefault(p => p.ProductoId == productoId);
                productoInDb.UnidadesEnPedido -= Convert.ToInt32(cantidad);
                productoInDb.UnidadesEnStock -= Convert.ToInt32(cantidad);
                _context.Entry(productoInDb).State = EntityState.Modified;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<IGrouping<int, Ciudad>> GetGrupos()
        {
            throw new NotImplementedException();
        }

    }
}
