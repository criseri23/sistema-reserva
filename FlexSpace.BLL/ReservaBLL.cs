using FlexSpace.DAL;

namespace FlexSpace.BLL
{
    public class ReservaBLL
    {
        private ClienteDAL clienteDAL = new ClienteDAL();
        private PuestoDAL puestoDAL = new PuestoDAL();
        private ReservaDAL reservaDAL = new ReservaDAL();

        public decimal CalcularPrecio(Cliente cliente, Puesto puesto, DateTime inicio, DateTime fin)
        {
            if (fin <= inicio)
                throw new Exception("La fecha final debe ser mayor a la fecha inicial");

            double horas = (fin - inicio).TotalHours;

            decimal subtotal = (decimal)horas * puesto.TarifaBasePorHora;
            decimal total = subtotal;

            if (IncluyeFinDeSemana(inicio, fin))
                total = total * 1.15m;

       
            if (horas >= 5)
                total = total * 0.90m;

           
            if (cliente.TipoCliente == "VIP")
                total = total * 0.95m;

            if (cliente.SancionesActivas > 0)
                total = subtotal * 1.20m;

            return Math.Round(total, 2);
        }

        private bool IncluyeFinDeSemana(DateTime inicio, DateTime fin)
        {
            DateTime fecha = inicio.Date;

            while (fecha <= fin.Date)
            {
                if (fecha.DayOfWeek == DayOfWeek.Saturday ||
                    fecha.DayOfWeek == DayOfWeek.Sunday)
                {
                    return true;
                }

                fecha = fecha.AddDays(1);
            }

            return false;
        }

        public decimal RegistrarReserva(
            int clienteId,
            int puestoId,
            DateTime inicio,
            DateTime fin,
            bool confirmar)
        {
            var datosCliente = clienteDAL.BuscarPorId(clienteId);

            if (datosCliente == null)
                throw new Exception("No existe el cliente");

            Cliente cliente = new Cliente();

            cliente.Id = datosCliente.Value.Id;
            cliente.Nombre = datosCliente.Value.Nombre;
            cliente.Email = datosCliente.Value.Email;
            cliente.TipoCliente = datosCliente.Value.TipoCliente;
            cliente.SancionesActivas = datosCliente.Value.SancionesActivas;

            var datosPuesto = puestoDAL.BuscarPorId(puestoId);

            if (datosPuesto == null)
                throw new Exception("No existe el puesto");

            Puesto puesto = new Puesto();

            puesto.Id = datosPuesto.Value.Id;
            puesto.Codigo = datosPuesto.Value.Codigo;
            puesto.TipoPuesto = datosPuesto.Value.TipoPuesto;
            puesto.TarifaBasePorHora = datosPuesto.Value.TarifaBase;

            if (cliente.SancionesActivas >= 3)
                throw new ClienteSancionadoException(
                    "El cliente tiene 3 o más sanciones y no puede reservar.");

            if (fin <= inicio)
                throw new Exception(
                    "La fecha final debe ser mayor a la fecha inicial");

            bool disponible = reservaDAL.EstaDisponible(
                puestoId,
                inicio,
                fin);

            if (!disponible)
                throw new Exception(
                    "El puesto ya tiene una reserva en ese horario.");

  
            decimal costo = CalcularPrecio(
                cliente,
                puesto,
                inicio,
                fin);

            if (confirmar)
            {
                reservaDAL.Agregar(
                    clienteId,
                    puestoId,
                    inicio,
                    fin,
                    "Confirmada",
                    costo);
            }

            return costo;
        }

        public void CancelarReserva(int reservaId)
        {
            var datos = reservaDAL.BuscarPorId(reservaId);

            if (datos == null)
                throw new Exception("La reserva no existe");

            if (datos.Value.Estado != "Confirmada")
                throw new Exception("La reserva no está confirmada");

            TimeSpan diferencia =
                datos.Value.FechaInicio - DateTime.Now;

            if (diferencia.TotalHours < 2)
            {
                clienteDAL.AgregarSancion(
                    datos.Value.ClienteId);
            }

            reservaDAL.Cancelar(reservaId);
        }

        public List<Reserva> ConsultarReservasPorPuesto(string codigo)
        {
            var puesto = puestoDAL.BuscarPorCodigo(codigo);

            if (puesto == null)
                throw new Exception(
                    "No existe un puesto con ese código");

            var datos =
                reservaDAL.BuscarActivasPorPuesto(
                    puesto.Value.Id);

            List<Reserva> reservas =
                new List<Reserva>();

            foreach (var dato in datos)
            {
                Reserva reserva = new Reserva();

                reserva.Id = dato.Id;
                reserva.ClienteId = dato.ClienteId;
                reserva.PuestoId = dato.PuestoId;
                reserva.FechaInicio = dato.FechaInicio;
                reserva.FechaFin = dato.FechaFin;
                reserva.Estado = dato.Estado;
                reserva.CostoTotal = dato.CostoTotal;

                reservas.Add(reserva);
            }

            return reservas;
        }

        public List<(int Id, string Nombre, string Email,
            string TipoCliente, int SancionesActivas)>
            ListarClientesSancionados()
        {
            return clienteDAL.ListarSancionados();
        }
    }
}