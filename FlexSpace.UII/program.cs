using FlexSpace.BLL;

ReservaBLL reservaBLL = new ReservaBLL();

int opcion = 0;

while (opcion != 5)
{
    Console.Clear();

    Console.WriteLine("=================================");
    Console.WriteLine("          FLEXSPACE");
    Console.WriteLine("=================================");
    Console.WriteLine("1. Registrar nueva reserva");
    Console.WriteLine("2. Cancelar reserva");
    Console.WriteLine("3. Consultar reservas activas");
    Console.WriteLine("4. Listar clientes sancionados");
    Console.WriteLine("5. Salir");
    Console.WriteLine("=================================");

    Console.Write("\nSeleccione una opcion: ");
    int.TryParse(Console.ReadLine(), out opcion);

    try
    {
        switch (opcion)
        {
            case 1:
                RegistrarReserva();
                break;

            case 2:
                CancelarReserva();
                break;

            case 3:
                ConsultarReservas();
                break;

            case 4:
                ListarClientesSancionados();
                break;

            case 5:
                Console.WriteLine("\nSaliendo...");
                break;

            default:
                Console.WriteLine("\nOpcion incorrecta.");
                break;
        }
    }
    catch (ClienteSancionadoException ex)
    {
        Console.WriteLine("\nNo se pudo realizar la reserva.");
        Console.WriteLine(ex.Message);
    }
    catch (Exception ex)
    {
        Console.WriteLine("\nError: " + ex.Message);
    }

    if (opcion != 5)
    {
        Console.WriteLine("\nPresione una tecla para continuar...");
        Console.ReadKey();
    }
}

void RegistrarReserva()
{
    Console.WriteLine("\n--- NUEVA RESERVA ---");

    Console.Write("ID del cliente: ");
    int clienteId = int.Parse(Console.ReadLine());

    Console.Write("ID del puesto: ");
    int puestoId = int.Parse(Console.ReadLine());

    Console.Write("Fecha inicio (dd/MM/yyyy HH:mm): ");
    DateTime inicio = DateTime.Parse(Console.ReadLine());

    Console.Write("Fecha fin (dd/MM/yyyy HH:mm): ");
    DateTime fin = DateTime.Parse(Console.ReadLine());

    decimal costo = reservaBLL.RegistrarReserva(
        clienteId, puestoId, inicio, fin, false);

    Console.WriteLine("\n==========================");
    Console.WriteLine("       RESUMEN");
    Console.WriteLine("==========================");
    Console.WriteLine("Inicio: " + inicio);
    Console.WriteLine("Fin: " + fin);
    Console.WriteLine("Costo total: $" + costo);

    Console.Write("\n¿Confirmar reserva? (S/N): ");
    string respuesta = Console.ReadLine().ToUpper();

    if (respuesta == "S")
    {
        reservaBLL.RegistrarReserva(
            clienteId, puestoId, inicio, fin, true);

        Console.WriteLine("\nReserva registrada correctamente.");
    }
    else
    {
        Console.WriteLine("\nLa reserva no fue guardada.");
    }
}

void CancelarReserva()
{
    Console.WriteLine("\n--- CANCELAR RESERVA ---");

    Console.Write("Ingrese el ID de la reserva: ");
    int reservaId = int.Parse(Console.ReadLine());

    reservaBLL.CancelarReserva(reservaId);

    Console.WriteLine("\nReserva cancelada correctamente.");
}

void ConsultarReservas()
{
    Console.WriteLine("\n--- RESERVAS ACTIVAS ---");

    Console.Write("Ingrese el codigo del puesto: ");
    string codigo = Console.ReadLine();

    List<Reserva> reservas =
        reservaBLL.ConsultarReservasPorPuesto(codigo);

    if (reservas.Count == 0)
    {
        Console.WriteLine("\nNo hay reservas futuras para este puesto.");
        return;
    }

    foreach (Reserva reserva in reservas)
    {
        Console.WriteLine("\n------------------------");
        Console.WriteLine("Reserva ID: " + reserva.Id);
        Console.WriteLine("Cliente ID: " + reserva.ClienteId);
        Console.WriteLine("Inicio: " + reserva.FechaInicio);
        Console.WriteLine("Fin: " + reserva.FechaFin);
        Console.WriteLine("Estado: " + reserva.Estado);
        Console.WriteLine("Costo: $" + reserva.CostoTotal);
    }
}

void ListarClientesSancionados()
{
    Console.WriteLine("\n--- CLIENTES SANCIONADOS ---");

    var clientes = reservaBLL.ListarClientesSancionados();

    if (clientes.Count == 0)
    {
        Console.WriteLine("\nNo hay clientes sancionados.");
        return;
    }

    foreach (var cliente in clientes)
    {
        Console.WriteLine("\n------------------------");
        Console.WriteLine("ID: " + cliente.Id);
        Console.WriteLine("Nombre: " + cliente.Nombre);
        Console.WriteLine("Email: " + cliente.Email);
        Console.WriteLine("Tipo de cliente: " + cliente.TipoCliente);
        Console.WriteLine("Sanciones activas: " + cliente.SancionesActivas);
    }
}