using JardinesEf.Entidades.Entidades;
using JardinesEF.Web.Models.Categoria;
using JardinesEF.Web.Models.Ciudad;
using JardinesEF.Web.Models.Cliente;
using JardinesEF.Web.Models.Pais;
using JardinesEF.Web.Models.Producto;
using JardinesEF.Web.Models.Proveedores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JardinesEF.Web.Clases
{
    public class Mapeador
    {
        #region Pais
        public static List<PaisListVm> ConstruirListaPaisesVm(List<Pais> listaPaises)
        {
            var lista = new List<PaisListVm>();
            foreach (var pais in listaPaises)
            {
                var paisVm = ConstruirPaisVm(pais);
                lista.Add(paisVm);
            }

            return lista;
        }

        public static PaisListVm ConstruirPaisVm(Pais pais)
        {
            return new PaisListVm()
            {
                PaisId = pais.PaisId,
                NombrePais = pais.NombrePais
            };
        }

        public static Pais ConstruirPais(PaisEditVm paisVm)
        {
            return new Pais()
            {
                PaisId = paisVm.PaisId,
                NombrePais = paisVm.NombrePais
            };
        }

        public static PaisEditVm ConstruirPaisEditVm(Pais pais)
        {
            return new PaisEditVm()
            {
                PaisId = pais.PaisId,
                NombrePais = pais.NombrePais
            };
        }
        public static PaisDetailsVm ConstruirPaisDetailsVm(Pais pais)
        {
            return new PaisDetailsVm()
            {
                PaisId = pais.PaisId,
                NombrePais=pais.NombrePais
                
            };
        }
        #endregion
        #region Categoria
        public static List<CategoriaListVm> ConstruirListaCategoriaVm(List<Categoria> lista)
        {
            var listaVm = new List<CategoriaListVm>();
            foreach (var c in lista)
            {
                var categoriaVm = ConstruirCategoriaVm(c);
                listaVm.Add(categoriaVm);
            }
            return listaVm;
        }

        public static CategoriaListVm ConstruirCategoriaVm(Categoria c)
        {
            return new CategoriaListVm()
            {
                CategoriaId = c.CategoriaId,
                NombreCategoria = c.NombreCategoria
            };
        }

        public static Categoria ContruirCategoria(CategoriaEditVm categoriaEditVm)
        {
            return new Categoria()
            {
                CategoriaId = categoriaEditVm.CategoriaId,
                NombreCategoria = categoriaEditVm.NombreCategoria,
                Descripcion = categoriaEditVm.Descripcion
            };
        }
        public static CategoriaEditVm ConstruirCategoriaEditVm(Categoria categoria)
        {
            return new CategoriaEditVm()
            {
                CategoriaId = categoria.CategoriaId,
                NombreCategoria = categoria.NombreCategoria,
                Descripcion = categoria.Descripcion
            };
        }
        public static CategoriaDetailsVm ConstruirCategoriaDetailsVm(Categoria categoria)
        {
            return new CategoriaDetailsVm()
            {
                CategoriaId = categoria.CategoriaId,
                NombreCategoria=categoria.NombreCategoria
            };
        }

        #endregion
        #region Ciudad
        public static List<CiudadListVm> ConstruirListaCiudadVm(List<Ciudad> listaCiudades)
        {
            var lista = new List<CiudadListVm>();
            foreach (var c in listaCiudades)
            {
                var ciudadVm = ConstruirCiudadVm(c);
                lista.Add(ciudadVm);
            }
            return lista;
        }

        public static CiudadListVm ConstruirCiudadVm(Ciudad c)
        {
            return new CiudadListVm()
            {
                CiudadId = c.CiudadId,
                NombreCiudad=c.NombreCiudad,
                NombrePais=c.Pais.NombrePais
            };
        }

        public static Ciudad ConstruirCiudad(CiudadEditVm ciudadEditVm)
        {
            return new Ciudad()
            {
                CiudadId = ciudadEditVm.CiudadId,
                NombreCiudad=ciudadEditVm.NombreCiudad,
                PaisId=ciudadEditVm.PaisId
            };
        }
        public static CiudadEditVm ConstruirCiudadEditVm(Ciudad ciudad)
        {
            return new CiudadEditVm()
            {
                CiudadId = ciudad.CiudadId,
                NombreCiudad = ciudad.NombreCiudad,
                PaisId = ciudad.PaisId
            };
        }
        #endregion
        #region Productos

        public static List<ProductoListVm> ConstruirListaProductosVm(List<Producto> listaProductos)
        {
            var lista = new List<ProductoListVm>();
            foreach (var p in listaProductos)
            {
                var productoVm = ConstruirProductoVm(p);
                lista.Add(productoVm);
            }
            return lista;
        }

        public static ProductoListVm ConstruirProductoVm(Producto p)
        {
            return new ProductoListVm()
            {
                ProductoId = p.ProductoId,
                NombreProducto=p.NombreProducto,
                NombreLatin=p.NombreLatin,
                NombreCategoria=p.Categoria.NombreCategoria,
                NombreProveedor=p.Proveedor.NombreProveedor,
                PrecioUnitario=p.PrecioUnitario,
                UnidadesEnStock=p.UnidadesEnStock
            };
        }

        internal static Producto ConstruirProducto(ProductoEditVm productoEditVm)
        {
            return new Producto()
            {
                ProductoId = productoEditVm.ProductoId,
                NombreProducto=productoEditVm.NombreProducto,
                NombreLatin=productoEditVm.NombreLatin,
                CategoriaId=productoEditVm.CategoriaId,
                PrecioUnitario=productoEditVm.PrecioUnitario,
                UnidadesEnStock=productoEditVm.UnidadesEnStock,
                ProveedorId=productoEditVm.ProveedorId
            };
        }

        public static ProductoEditVm ConstruirProductoEditVm(Producto producto)
        {
            return new ProductoEditVm()
            {
                ProductoId = producto.ProductoId,
                NombreProducto=producto.NombreProducto,
                NombreLatin=producto.NombreLatin,
                ProveedorId=producto.ProveedorId,
                CategoriaId=producto.CategoriaId,
                PrecioUnitario=producto.PrecioUnitario,
                UnidadesEnStock=producto.UnidadesEnStock
            };
        }
        #endregion

        #region Proveedor
        public static List<ProveedorListVm> ConstruirListaProveedoresVm(List<Proveedor> listaProveedores)
        {
            var lista = new List<ProveedorListVm>();
            foreach (var p in listaProveedores)
            {
                var proveedorVm = ConstruirProveedorVm(p);
                lista.Add(proveedorVm);
            }
            return lista;
        }

        public static ProveedorListVm ConstruirProveedorVm(Proveedor p)
        {
            return new ProveedorListVm()
            {
                ProveedorId = p.ProveedorId,
                NombreProveedor=p.NombreProveedor,
                CodigoPostal=p.CodigoPostal,
                Direccion=p.Direccion,
                NombreCiudad=p.Ciudad.NombreCiudad,
                CantidadProductos=p.Productos.Count()
            };
        }


        #endregion
        #region Cliente
        public static List<ClienteListVm> ConstruirListaClienteVm(List<Cliente> listaClientes)
        {
            var lista = new List<ClienteListVm>();
            foreach (var c in listaClientes)
            {
                var clienteVm = ConstruirClienteVm(c);
                lista.Add(clienteVm);
            }
            return lista;
        }

        private static ClienteListVm ConstruirClienteVm(Cliente c)
        {
            return new ClienteListVm()
            {
                ClienteId=c.ClienteId,
                Apellido=c.Apellido,
                Nombres=c.Nombres,
                Ciudad=c.Ciudad.NombreCiudad,
                Pais=c.Pais.NombrePais
            };
        }
        #endregion
    }
}