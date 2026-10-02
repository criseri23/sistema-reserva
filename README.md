# FlexSpace - Sistema de Gestión de Reservas

Este trabajo práctico consiste en realizar un sistema en C# para gestionar reservas de puestos de trabajo en un espacio de co-working.

El proyecto está dividido en 3 capas:
- FlexSpace.UI: contiene el menú y permite que el usuario ingrese los datos.
- FlexSpace.BLL: contiene las clases y la lógica del sistema.
- FlexSpace.DAL: se encarga de la conexión y consultas a la base de datos.

Para la base de datos se utilizó MySQL con XAMPP y la librería MySql.Data.

Hasta el momento realice las opciones 1, 2 y 3 del menú:

1. Registrar una nueva reserva.
2. Cancelar una reserva.
3. Consultar reservas activas por puesto.

También se realizaron las validaciones principales, como evitar reservas en horarios ocupados, calcular el costo de la reserva, aplicar descuentos, recargos y sanciones.

Hasta ahora se llegó a realizar las opciones 1, 2 y 3 del menú y la conexión con la base de datos.
