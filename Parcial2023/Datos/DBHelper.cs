using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using Produccion.Domino;
using Parcial2023.Dominio;
using System.Windows.Forms;

namespace Parcial2023.Datos
{
    public class DBHelper
    {
        private static DBHelper instancia = null;
        private SqlConnection conexion;
        private DBHelper()
        {
            conexion = new SqlConnection(@"Su cadena de conexion");
        }
        
        public static DBHelper GetInstancia()
        {
            if (instancia == null)
                instancia = new DBHelper();
            return instancia;
        }


        public List<Componente> Consultar()
        {
            List<Componente> lista = new List<Componente>();
            conexion.Open();
            SqlCommand comando = new SqlCommand();
            comando.Connection = conexion;
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandText = "SP_CONSULTAR_...";
            DataTable tabla = new DataTable();
            tabla.Load(comando.ExecuteReader());
            conexion.Close();

            foreach(DataRow dr in tabla.Rows)
            {
                Componente comp = new Componente();
                comp.Codigo = Convert.ToInt32(dr["codigo"].ToString());
                comp.Nombre = dr["nombre"].ToString();
                lista.Add(comp);
            }

            return lista;   
        }

        public bool Confirmar(OrdenProduccion orden)
        {
            bool aux = true;
            SqlTransaction t = null;

            try
            {
                conexion.Open();
                t = conexion.BeginTransaction();

                SqlCommand comando = new SqlCommand("SP_INSERTAR_MAESTRO", conexion, t);
                comando.CommandType=CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@fecha", orden.Fecha);
                comando.Parameters.AddWithValue("@modelo", orden.Modelo);
                comando.Parameters.AddWithValue("@estado", orden.Estado);
                comando.Parameters.AddWithValue("@cantidad", orden.Cantidad);

                SqlParameter param = new SqlParameter();
                param.ParameterName = "@nro_orden";
                param.Direction = ParameterDirection.Output;
                param.SqlDbType = SqlDbType.Int;
                comando.Parameters.Add(param);

                comando.ExecuteNonQuery();

                int id = (int)param.Value;
                int idDetalle = id;

                foreach(DetalleOrden detalle in orden.Detalle)
                {
                    SqlCommand cmdDetalle = new SqlCommand("SP_INSERTAR_DETALLE", conexion, t);
                    cmdDetalle.CommandType = CommandType.StoredProcedure;
                    cmdDetalle.Parameters.AddWithValue("@id", idDetalle);
                    cmdDetalle.Parameters.AddWithValue("@nro_orden", id);
                    cmdDetalle.Parameters.AddWithValue("@componente", detalle.Componente.Codigo);
                    cmdDetalle.Parameters.AddWithValue("@cantidad", detalle.Cantidad);
                    cmdDetalle.ExecuteNonQuery();
                    idDetalle++;
                }
                
                t.Commit();
            }
            catch (Exception ex) 
            {
                if(t != null)
                {
                    t.Rollback();
                    aux = false;
                }
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if(conexion != null && conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
            return aux;
        }
    }
}
