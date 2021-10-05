﻿using JardinesEf.Entidades.Entidades;
using JardinesEF.Web.Models.Categoria;
using JardinesEF.Web.Models.Ciudad;
using JardinesEF.Web.Models.Pais;
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
    }
}