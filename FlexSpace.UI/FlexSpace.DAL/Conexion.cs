using MySql.Data.MySqlClient;

namespace FlexSpace.DAL
{
    public class Conexion
    {
        private string cadena =
            "Server=127.0.0.1;Database=FlexSpace;Uid=root;Pwd=;";

        public MySqlConnection CrearConexion()
        {
            return new MySqlConnection(cadena);
        }
    }
}