using System;
using System.IO;

namespace Inventario_Materia_Prima
{
    class Program
    {
        //========================================
        // BLOQUE 1: CONFIGURACION GLOBAL
        //========================================
        const string NOMBRE_ARCHIVO = "materias_primas.txt";
        const int MAX_REGISTROS = 100;

        // Arreglos paralelos (base de datos en memoria)
        static string[] codigos = new string[MAX_REGISTROS];
        static string[] nombres = new string[MAX_REGISTROS];
        static string[] unidades = new string[MAX_REGISTROS];
        static double[] stockActual = new double[MAX_REGISTROS];
        static double[] stockMinimo = new double[MAX_REGISTROS];
        static int totalRegistros = 0;

        //========================================
        // BLOQUE 2: MENU PRINCIPAL
        //========================================
        static void Main(string[] args)
        {
            // CONFIGURACION INICIAL OBLIGATORIA PARA UTF-8
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            CargarDatosDesdeArchivo();
            int opcion;

            do
            {
                Console.Clear();
                MostrarEncabezado("Sistema_Inventario_Materia_Prima");
                MostrarMenu();
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("  ► Seleccione una opcion: ");
                Console.ResetColor();

                if (!int.TryParse(Console.ReadLine(), out opcion))
                {
                    opcion = 0;
                }

                ProcesarOpcion(opcion);

                if (opcion != 7)
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write("  Presione una tecla para continuar...");
                    Console.ResetColor();
                    Console.ReadKey();
                }
            } while (opcion != 7);

            GuardarDatosEnArchivo();
            MostrarDespedida();
        }

        static void MostrarMenu()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("  ═══════ MENU PRINCIPAL ═══════");
            Console.ResetColor();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("  [1] ✓ Registrar nueva materia prima");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("  [2] ● Listar todas las materias primas");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  [3] ★ Actualizar stock (entrada / salida)");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("  [4] ⚠ Alertar stock bajo minimo");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("  [5] ► Buscar materia prima por codigo");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("  [6] ═ Generar reporte en archivo TXT");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("  [7] ✗ Salir del sistema");
            Console.ResetColor();
            LineaSeparadora();
        }

        //========================================
        // BLOQUE 3: LOGICA DE NEGOCIO
        //========================================
        static void ProcesarOpcion(int opcion)
        {
            Console.WriteLine();
            switch (opcion)
            {
                case 1: RegistrarMateriaPrima(); break;
                case 2: ListarMateriasPrimas(); break;
                case 3: ActualizarStock(); break;
                case 4: AlertarStockBajo(); break;
                case 5: BuscarPorCodigo(); break;
                case 6: GenerarReporteTXT(); break;
                case 7: break;
                default:
                    MensajeError("Opcion no valida. Intente nuevamente.");
                    break;
            }
        }

        static void RegistrarMateriaPrima()
        {
            if (totalRegistros >= MAX_REGISTROS)
            {
                MensajeError("Memoria llena. No se pueden registrar mas materias primas.");
                return;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("  ══ NUEVO REGISTRO ══");
            Console.ResetColor();

            string cod;
            bool codigoExiste;
            do
            {
                Console.Write("  Codigo (ej. MP001): ");
                cod = Console.ReadLine().Trim().ToUpper();
                if (string.IsNullOrWhiteSpace(cod))
                {
                    MensajeError("El codigo no puede estar vacio.");
                    codigoExiste = true;
                    continue;
                }
                codigoExiste = BuscarIndicePorCodigo(cod) != -1;
                if (codigoExiste)
                    MensajeError("El codigo ya existe en el inventario.");
            } while (codigoExiste);

            string nom = LeerTextoNoVacio("  Nombre: ");
            string uni = LeerTextoNoVacio("  Unidad de medida (ej. kg, l, und): ");

            Console.Write("  Stock actual: ");
            double stockAct = LeerNumeroPositivo();

            Console.Write("  Stock minimo: ");
            double stockMin = LeerNumeroPositivo();

            codigos[totalRegistros] = cod;
            nombres[totalRegistros] = nom;
            unidades[totalRegistros] = uni;
            stockActual[totalRegistros] = stockAct;
            stockMinimo[totalRegistros] = stockMin;
            totalRegistros++;

            MensajeExito("Materia prima registrada correctamente.");
        }

        static void ListarMateriasPrimas()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("  ══ LISTADO DE MATERIAS PRIMAS ══");
            Console.ResetColor();
            Console.WriteLine();

            if (totalRegistros == 0)
            {
                MensajeAdvertencia("No hay materias primas registradas.");
                return;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"  {"Codigo",-10}|{"Nombre",-20}|{"Unidad",-10}|{"Stock Actual",-10}|{"Stock Min.",-10}|{"Estado",-10}");
            LineaSeparadora();
            Console.ResetColor();

            for (int i = 0; i < totalRegistros; i++)
            {
                string estado = stockActual[i] < stockMinimo[i] ? "BAJO" : "OK";
                ConsoleColor colorEstado = stockActual[i] < stockMinimo[i] ? ConsoleColor.Red : ConsoleColor.Green;

                Console.Write($"  {codigos[i],-10}|{nombres[i],-20}|{unidades[i],-10}|{stockActual[i],-12:F2}|{stockMinimo[i],-10:F2}|");
                Console.ForegroundColor = colorEstado;
                Console.WriteLine($"{estado,-10}");
                Console.ResetColor();
            }

            Console.WriteLine();
            MensajeInfo($"Total de registros: {totalRegistros}");
        }

        static void ActualizarStock()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  ══ ACTUALIZAR STOCK ══");
            Console.ResetColor();
            Console.WriteLine();

            Console.Write("  Ingrese el codigo de la materia prima: ");
            string codigo = Console.ReadLine().Trim().ToUpper();
            int indice = BuscarIndicePorCodigo(codigo);

            if (indice == -1)
            {
                MensajeError("Materia prima no encontrada.");
                return;
            }

            Console.WriteLine($"  Materia: {nombres[indice]} | Stock actual: {stockActual[indice]:F2} {unidades[indice]}");
            Console.WriteLine("  [1] ► Entrada de material (+)");
            Console.WriteLine("  [2] ► Salida de material (-)");
            Console.Write("  Seleccione tipo de movimiento: ");

            int tipo;
            while (!int.TryParse(Console.ReadLine(), out tipo) || (tipo != 1 && tipo != 2))
            {
                MensajeError("Opcion invalida. Ingrese 1 o 2.");
                Console.Write("  Seleccione tipo de movimiento: ");
            }

            Console.Write("  Cantidad: ");
            double cantidad = LeerNumeroPositivo();

            if (tipo == 1)
            {
                stockActual[indice] += cantidad;
                MensajeExito($"Entrada registrada. Nuevo stock: {stockActual[indice]:F2} {unidades[indice]}");
            }
            else
            {
                if (cantidad > stockActual[indice])
                {
                    MensajeError("No hay suficiente stock para realizar la salida.");
                    return;
                }
                stockActual[indice] -= cantidad;
                MensajeExito($"Salida registrada. Nuevo stock: {stockActual[indice]:F2} {unidades[indice]}");

                if (stockActual[indice] < stockMinimo[indice])
                {
                    MensajeAdvertencia($"ALERTA: Stock por debajo del minimo ({stockMinimo[indice]:F2} {unidades[indice]})!");
                }
            }
        }

        static void AlertarStockBajo()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  ══ MATERIAS CON STOCK BAJO EL MINIMO ══");
            Console.ResetColor();
            Console.WriteLine();

            int contador = 0;
            for (int i = 0; i < totalRegistros; i++)
                if (stockActual[i] < stockMinimo[i]) contador++;

            if (contador == 0)
            {
                MensajeExito("Todas las materias primas tienen stock suficiente.");
                return;
            }

            MensajeAdvertencia($"Se encontraron {contador} materia(s) con stock bajo:");
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"  {"Codigo",-10}|{"Nombre",-20}|{"Stock Actual",-10}|{"Stock Min.",-12}");
            LineaSeparadora();
            Console.ResetColor();

            for (int i = 0; i < totalRegistros; i++)
            {
                if (stockActual[i] < stockMinimo[i])
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"  {codigos[i],-10}|{nombres[i],-20}|{stockActual[i],-10:F2}|{stockMinimo[i],12:F2}");
                    Console.ResetColor();
                }
            }
        }

        static void BuscarPorCodigo()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  ══ BUSCAR MATERIA PRIMA POR CODIGO ══");
            Console.ResetColor();
            Console.WriteLine();

            Console.Write("  Ingrese el codigo a buscar: ");
            string codigo = Console.ReadLine().Trim().ToUpper();
            int indice = BuscarIndicePorCodigo(codigo);

            if (indice == -1)
            {
                MensajeError("No se encontro ninguna materia prima con ese codigo.");
                return;
            }

            MensajeExito("Materia prima encontrada:");
            Console.WriteLine($"  Codigo:          {codigos[indice]}");
            Console.WriteLine($"  Nombre:          {nombres[indice]}");
            Console.WriteLine($"  Unidad:          {unidades[indice]}");
            Console.WriteLine($"  Stock actual:    {stockActual[indice]:F2}");
            Console.WriteLine($"  Stock minimo:    {stockMinimo[indice]:F2}");

            if (stockActual[indice] < stockMinimo[indice])
                MensajeAdvertencia("Estado: STOCK BAJO");
            else
                MensajeExito("Estado: Stock OK");
        }

        static void GenerarReporteTXT()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("  ══ GENERAR REPORTE DE INVENTARIO ══");
            Console.ResetColor();
            Console.WriteLine();

            string ruta = Path.Combine(Environment.CurrentDirectory, "Reporte_Inventario.txt");

            try
            {
                using (StreamWriter archivo = new StreamWriter(ruta))
                {
                    archivo.WriteLine("==============================================================");
                    archivo.WriteLine("     REPORTE DE CONTROL DE INVENTARIO - MATERIA PRIMA");
                    archivo.WriteLine($"     Fecha y hora: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
                    archivo.WriteLine("==============================================================");
                    archivo.WriteLine();

                    if (totalRegistros == 0)
                    {
                        archivo.WriteLine("  No hay materias primas registradas en el inventario.");
                    }
                    else
                    {
                        archivo.WriteLine($"{"Codigo",-10}|{"Nombre",-20}|{"Unidad",-10}|{"Stock Actual",-12}|{"Stock Min.",-12}|{"Estado",-10}");
                        archivo.WriteLine(new string('-', 95));

                        int bajas = 0;
                        for (int i = 0; i < totalRegistros; i++)
                        {
                            string estado = stockActual[i] < stockMinimo[i] ? "STOCK BAJO" : "OK";
                            if (stockActual[i] < stockMinimo[i]) bajas++;
                            archivo.WriteLine($"{codigos[i],-10}|{nombres[i],-20}|{unidades[i],-10}|{stockActual[i],-12:F2}|{stockMinimo[i],-12:F2}|{estado,-10}");
                        }

                        archivo.WriteLine();
                        archivo.WriteLine($"Total de materias primas registradas: {totalRegistros}");
                        archivo.WriteLine($"Materias con stock bajo minimo: {bajas}");
                    }

                    archivo.WriteLine();
                    archivo.WriteLine("==============================================================");
                    archivo.WriteLine("                    Fin del reporte");
                    archivo.WriteLine("==============================================================");
                }

                MensajeExito($"Reporte generado: {ruta}");
            }
            catch (Exception ex)
            {
                MensajeError("Error al generar reporte: " + ex.Message);
            }
        }

        //========================================
        // BLOQUE 4: PERSISTENCIA (ARCHIVOS)
        //========================================
        static void CargarDatosDesdeArchivo()
        {
            if (!File.Exists(NOMBRE_ARCHIVO))
            {
                MensajeAdvertencia("Archivo no encontrado. Iniciando base de datos vacia.");
                return;
            }

            try
            {
                string[] lineas = File.ReadAllLines(NOMBRE_ARCHIVO);
                foreach (string linea in lineas)
                {
                    if (string.IsNullOrWhiteSpace(linea)) continue;

                    string[] partes = linea.Split('|');
                    if (partes.Length == 5)
                    {
                        codigos[totalRegistros] = partes[0];
                        nombres[totalRegistros] = partes[1];
                        unidades[totalRegistros] = partes[2];
                        stockActual[totalRegistros] = double.Parse(partes[3]);
                        stockMinimo[totalRegistros] = double.Parse(partes[4]);
                        totalRegistros++;
                    }
                }

                MensajeExito($"Se cargaron {totalRegistros} registro(s) desde el archivo.");
            }
            catch (Exception ex)
            {
                MensajeError("Error al leer archivo: " + ex.Message);
            }
        }

        static void GuardarDatosEnArchivo()
        {
            try
            {
                using (StreamWriter escritor = new StreamWriter(NOMBRE_ARCHIVO))
                {
                    for (int i = 0; i < totalRegistros; i++)
                    {
                        string linea = $"{codigos[i]}|{nombres[i]}|{unidades[i]}|{stockActual[i]}|{stockMinimo[i]}";
                        escritor.WriteLine(linea);
                    }
                }
            }
            catch (Exception ex)
            {
                MensajeError("Error al guardar archivo: " + ex.Message);
            }
        }

        //========================================
        // METODOS AUXILIARES DE PRESENTACION
        //========================================
        static void MostrarEncabezado(string titulo)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine();
            Console.WriteLine("  ╔══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("  ║                                                              ║");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"  ║{titulo.PadLeft(25 + titulo.Length / 2).PadRight(62)}║");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("  ║                                                              ║");
            Console.WriteLine("  ╚══════════════════════════════════════════════════════════════╝");
            Console.ResetColor();
        }

        static void LineaSeparadora()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("  ──────────────────────────────────────────────────────────────────────────────");
            Console.ResetColor();
        }

        static void MensajeExito(string texto)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"  [OK] {texto}");
            Console.ResetColor();
        }

        static void MensajeError(string texto)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"  [ERROR] {texto}");
            Console.ResetColor();
        }

        static void MensajeAdvertencia(string texto)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"  [!] {texto}");
            Console.ResetColor();
        }

        static void MensajeInfo(string texto)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"  [i] {texto}");
            Console.ResetColor();
        }

        static void MostrarDespedida()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine();
            Console.WriteLine("  ╔══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("  ║                                                              ║");
            Console.WriteLine("  ║   Datos guardados correctamente.                             ║");
            Console.WriteLine("  ║   Gracias por usar Sistema de Inventario de Materia Prima    ║");
            Console.WriteLine("  ║   Hasta pronto!                                              ║");
            Console.WriteLine("  ║                                                              ║");
            Console.WriteLine("  ╚══════════════════════════════════════════════════════════════╝");
            Console.ResetColor();
        }

        //========================================
        // FUNCIONES AUXILIARES DE LOGICA
        //========================================
        static int BuscarIndicePorCodigo(string codigo)
        {
            for (int i = 0; i < totalRegistros; i++)
            {
                if (codigos[i] == codigo)
                    return i;
            }
            return -1;
        }

        static double LeerNumeroPositivo()
        {
            double valor;
            while (!double.TryParse(Console.ReadLine(), out valor) || valor < 0)
            {
                MensajeError("Valor invalido. Ingrese un numero positivo.");
                Console.Write("  Cantidad: ");
            }
            return valor;
        }

        static string LeerTextoNoVacio(string mensaje)
        {
            string texto;
            do
            {
                Console.Write(mensaje);
                texto = Console.ReadLine().Trim();
                if (string.IsNullOrWhiteSpace(texto))
                    MensajeError("Este campo no puede estar vacio.");
            } while (string.IsNullOrWhiteSpace(texto));
            return texto;
        }
    }
}