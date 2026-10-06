using MySql.Data.MySqlClient;

namespace FlexSpace.DAL
{
    public class PuestoDAL
    {
        private Conexion conexion = new Conexion();

        public (int Id, string Codigo, string TipoPuesto,
            decimal TarifaBase)?
            BuscarPorId(int id)
        {
            using (MySqlConnection con =
                   conexion.CrearConexion())
            {
                string sql = @"SELECT Id, Codigo,
                                      TipoPuesto,
                                      TarifaBasePorHora
                               FROM Puesto
                               WHERE Id = @id";

                MySqlCommand cmd =
                    new MySqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@id", id);

                con.Open();

                MySqlDataReader reader =
                    cmd.ExecuteReader();

                if (reader.Read())
                {
                    return (
                        Convert.ToInt32(reader["Id"]),
                        reader["Codigo"].ToString(),
                        reader["TipoPuesto"].ToString(),
                        Convert.ToDecimal(
                            reader["TarifaBasePorHora"])
                    );
                }
            }

            return null;
        }

        public (int Id, string Codigo, string TipoPuesto,
            decimal TarifaBase)?
            BuscarPorCodigo(string codigo)
        {
            using (MySqlConnection con =
                   conexion.CrearConexion())
            {
                string sql = @"SELECT Id, Codigo,
                                      TipoPuesto,
                                      TarifaBasePorHora
                               FROM Puesto
                               WHERE Codigo = @codigo";

                MySqlCommand cmd =
                    new MySqlCommand(sql, con);

                cmd.Parameters.AddWithValue(
                    "@codigo",
                    codigo);

                con.Open();

                MySqlDataReader reader =
                    cmd.ExecuteReader();

                if (reader.Read())
                {
                    return (
                        Convert.ToInt32(reader["Id"]),
                        reader["Codigo"].ToString(),
                        reader["TipoPuesto"].ToString(),
                        Convert.ToDecimal(
                            reader["TarifaBasePorHora"])
                    );
                }
            }

            return null;
        }
    }
}