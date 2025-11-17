using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using System.Data.SqlClient;
using System.Xml.Serialization;
using System.Data;

namespace CapaDatos
{
    public class CD_DetallesNegocio
    {
        public Detalles_Negocio ObtenerDetallesNegocio()
        {
            Detalles_Negocio objDetallesNegocio = new Detalles_Negocio();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("select IdNegocio, Nombre, RUC, Direccion from Detalles_Negocio where IdNegocio = 1");
                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.CommandType = System.Data.CommandType.Text;
                    oconexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            objDetallesNegocio.IdNegocio = Convert.ToInt32(dr["IdNegocio"]);
                            objDetallesNegocio.Nombre = dr["Nombre"].ToString();
                            objDetallesNegocio.RUC = dr["RUC"].ToString();
                            objDetallesNegocio.Direccion = dr["Direccion"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                objDetallesNegocio = new Detalles_Negocio();
            }


            return objDetallesNegocio;
        }

        public bool GuardarDatos(Detalles_Negocio objDetallesNegocio)
        {
            bool respuesta = true;
            string mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
                {
                    conexion.Open();

                    StringBuilder query = new StringBuilder();
                    query.AppendLine("UPDATE Detalles_Negocio SET Nombre = @Nombre");
                    query.AppendLine(", RUC = @RUC");
                    query.AppendLine(", Direccion = @Direccion");
                    query.AppendLine("WHERE IdNegocio = 1");

                    SqlCommand cmd = new SqlCommand(query.ToString(), conexion);
                    cmd.Parameters.AddWithValue("@Nombre", objDetallesNegocio.Nombre);
                    cmd.Parameters.AddWithValue("@RUC", objDetallesNegocio.RUC);
                    cmd.Parameters.AddWithValue("@Direccion", objDetallesNegocio.Direccion);
                    cmd.CommandType = CommandType.Text;

                    if (cmd.ExecuteNonQuery() < 1)
                    {
                        respuesta = false;
                        mensaje = "No se pudo actualizar los detalles del negocio.";
                    }

                }


            }
            catch (Exception ex)
            {
                respuesta = false;
                mensaje = ex.Message;
            }



            return respuesta;
        }

        public byte[] ObtenerLogo(out bool obtenido)
        {
            obtenido = true;
            byte[] LogoByte = new byte[0];

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("select Logo from Detalles_Negocio where IdNegocio = 1");
                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.CommandType = System.Data.CommandType.Text;
                    
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while(dr.Read())
                        {
                            LogoByte = (byte[])dr["Logo"];
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                obtenido = false;
                Console.WriteLine(ex.Message);
                LogoByte = new byte[0];
            }



            return LogoByte;
        }


        public bool ActualizarLogo(byte[] imagen, out string Mensaje)
        {
            Mensaje = string.Empty;
            bool respuesta = true;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
                {
                    conexion.Open();
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("UPDATE Detalles_Negocio SET Logo = @imagen");
                    query.AppendLine("WHERE IdNegocio = 1");

                    SqlCommand cmd = new SqlCommand(query.ToString(), conexion);
                    cmd.Parameters.AddWithValue("@imagen", imagen);
                    cmd.CommandType = CommandType.Text;

                    if (cmd.ExecuteNonQuery() < 1)
                    {
                        respuesta = false;
                        Mensaje = "No se pudo actualizar el logo del negocio.";
                    }
                }
            }
            catch (Exception ex)
            {
                respuesta = false;
                Mensaje = ex.Message;
            }

            return respuesta;
        }

       

    }
}
            