using MySql.Data.MySqlClient;

namespace FlexSpace.DAL
{
    public class ClienteDAL
    {
        private Conexion conexion = new Conexion();

        public (int Id, string Nombre, string Email, string TipoCliente, int SancionesActivas)?
            BuscarPorId(int id)
        {
            using (MySqlConnection con = conexion.CrearConexion())
            {
                string sql = @"SELECT Id, Nombre, Email, TipoCliente, SancionesActivas
                               FROM Cliente
                               WHERE Id = @id";

                MySqlCommand cmd = new MySqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@id", id);

                con.Open();

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return (
                        Convert.ToInt32(reader["Id"]),
                        reader["Nombre"].ToString(),
                        reader["Email"].ToString(),
                        reader["TipoCliente"].ToString(),
                        Convert.ToInt32(reader["SancionesActivas"])
                    );
                }
            }

            return null;
        }

        public void AgregarSancion(int clienteId)
        {
            using (MySqlConnection con = conexion.CrearConexion())
            {
                string sql = @"UPDATE Cliente
                               SET SancionesActivas = SancionesActivas + 1
                               WHERE Id = @id";

                MySqlCommand cmd = new MySqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@id", clienteId);

                con.Open();

                cmd.ExecuteNonQuery();
            }
        }
    }
}