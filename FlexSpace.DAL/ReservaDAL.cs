using MySql.Data.MySqlClient;

namespace FlexSpace.DAL
{
    public class ReservaDAL
    {
        private Conexion conexion = new Conexion();

        public bool EstaDisponible(
            int puestoId,
            DateTime inicio,
            DateTime fin)
        {
            using (MySqlConnection con =
                   conexion.CrearConexion())
            {
                string sql = @"SELECT COUNT(*)
                               FROM Reserva
                               WHERE PuestoId = @puestoId
                               AND Estado = 'Confirmada'
                               AND FechaInicio < @fin
                               AND FechaFin > @inicio";

                MySqlCommand cmd =
                    new MySqlCommand(sql, con);

                cmd.Parameters.AddWithValue(
                    "@puestoId",
                    puestoId);

                cmd.Parameters.AddWithValue(
                    "@inicio",
                    inicio);

                cmd.Parameters.AddWithValue(
                    "@fin",
                    fin);

                con.Open();

                int cantidad =
                    Convert.ToInt32(
                        cmd.ExecuteScalar());

                return cantidad == 0;
            }
        }

        public void Agregar(
            int clienteId,
            int puestoId,
            DateTime inicio,
            DateTime fin,
            string estado,
            decimal costo)
        {
            using (MySqlConnection con =
                   conexion.CrearConexion())
            {
                string sql = @"INSERT INTO Reserva
                               (ClienteId, PuestoId,
                                FechaInicio, FechaFin,
                                Estado, CostoTotal)
                               VALUES
                               (@clienteId, @puestoId,
                                @inicio, @fin,
                                @estado, @costo)";

                MySqlCommand cmd =
                    new MySqlCommand(sql, con);

                cmd.Parameters.AddWithValue(
                    "@clienteId",
                    clienteId);

                cmd.Parameters.AddWithValue(
                    "@puestoId",
                    puestoId);

                cmd.Parameters.AddWithValue(
                    "@inicio",
                    inicio);

                cmd.Parameters.AddWithValue(
                    "@fin",
                    fin);

                cmd.Parameters.AddWithValue(
                    "@estado",
                    estado);

                cmd.Parameters.AddWithValue(
                    "@costo",
                    costo);

                con.Open();

                cmd.ExecuteNonQuery();
            }
        }

        public (int Id, int ClienteId, int PuestoId,
            DateTime FechaInicio, DateTime FechaFin,
            string Estado, decimal CostoTotal)?
            BuscarPorId(int id)
        {
            using (MySqlConnection con =
                   conexion.CrearConexion())
            {
                string sql = @"SELECT Id, ClienteId,
                                      PuestoId,
                                      FechaInicio,
                                      FechaFin,
                                      Estado,
                                      CostoTotal
                               FROM Reserva
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
                        Convert.ToInt32(reader["ClienteId"]),
                        Convert.ToInt32(reader["PuestoId"]),
                        Convert.ToDateTime(
                            reader["FechaInicio"]),
                        Convert.ToDateTime(
                            reader["FechaFin"]),
                        reader["Estado"].ToString(),
                        Convert.ToDecimal(
                            reader["CostoTotal"])
                    );
                }
            }

            return null;
        }

        public void Cancelar(int id)
        {
            using (MySqlConnection con =
                   conexion.CrearConexion())
            {
                string sql = @"UPDATE Reserva
                               SET Estado = 'Cancelada'
                               WHERE Id = @id";

                MySqlCommand cmd =
                    new MySqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@id", id);

                con.Open();

                cmd.ExecuteNonQuery();
            }
        }

        public List<(int Id, int ClienteId, int PuestoId,
            DateTime FechaInicio, DateTime FechaFin,
            string Estado, decimal CostoTotal)>
            BuscarActivasPorPuesto(int puestoId)
        {
            List<(int, int, int, DateTime, DateTime,
                string, decimal)> lista =
                new List<(int, int, int, DateTime,
                    DateTime, string, decimal)>();

            using (MySqlConnection con =
                   conexion.CrearConexion())
            {
                string sql = @"SELECT Id, ClienteId,
                                      PuestoId,
                                      FechaInicio,
                                      FechaFin,
                                      Estado,
                                      CostoTotal
                               FROM Reserva
                               WHERE PuestoId = @puestoId
                               AND Estado = 'Confirmada'
                               AND FechaInicio > @fecha
                               ORDER BY FechaInicio";

                MySqlCommand cmd =
                    new MySqlCommand(sql, con);

                cmd.Parameters.AddWithValue(
                    "@puestoId",
                    puestoId);

                cmd.Parameters.AddWithValue(
                    "@fecha",
                    DateTime.Now);

                con.Open();

                MySqlDataReader reader =
                    cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(
                        (
                            Convert.ToInt32(reader["Id"]),
                            Convert.ToInt32(
                                reader["ClienteId"]),
                            Convert.ToInt32(
                                reader["PuestoId"]),
                            Convert.ToDateTime(
                                reader["FechaInicio"]),
                            Convert.ToDateTime(
                                reader["FechaFin"]),
                            reader["Estado"].ToString(),
                            Convert.ToDecimal(
                                reader["CostoTotal"])
                        )
                    );
                }
            }

            return lista;
        }
    }
}