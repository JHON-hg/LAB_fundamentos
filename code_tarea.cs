using System;
using System.IO;
using System.Numerics;

class Entrega
{
    public string Codigo;
    public string Archivo;
    public int Intentos;
    public int Puntaje;
    public string Observaciones;

    public Entrega(string codigo, string archivo, int intentos, int puntaje, string observaciones)
    {
        Codigo = codigo;
        Archivo = archivo;
        Intentos = intentos;
        Puntaje = puntaje;
        Observaciones = observaciones;
    }
}

class Program
{
    static Entrega[] Entregas = new Entrega[1000];
    static int totalEntregas = 0;
    static string rutaArchivo = "entrega.txt";
    // ------------------------------------------------
    // CARGAR LOS DATOS DEL ARCHIVO
    // ------------------------------------------------
    static void CargarEntregas()
    {
        if (!File.Exists(rutaArchivo))
            return;

        string[] lineas = File.ReadAllLines(rutaArchivo);

        foreach (string linea in lineas)
        {
            string[] datos = linea.Split('|');
            int intentos, puntaje;
            bool intentosOk = int.TryParse(datos[2], out intentos), puntajeOk = int.TryParse(datos[3], out puntaje);

            if (datos.Length == 5 && intentosOk && puntajeOk)
            {
                Entregas[totalEntregas] = new Entrega(datos[0], datos[1], intentos, puntaje, datos[4]);
                totalEntregas++;
            }
        }
    }

    // ------------------------------------------------
    // VALIDACIONES
    // ------------------------------------------------
    static void ValidarTexto(string texto, string nombre)
    {
        if (string.IsNullOrWhiteSpace(texto))
            throw new ArgumentException($"El campo '{nombre}' no puede estar vacio.");
    }

    static void ValidarPuntaje(int puntaje)
    {
        if (puntaje < 0)
            throw new ArgumentException($"el puntaje no puede ser negativo....");
    }

    // ------------------------------------------------
    // ENTRADA DE DATOS
    // ------------------------------------------------
    static string LeerTexto(string mensaje)
    {
        while (true)
        {
            Console.Write(mensaje);
            string texto = Console.ReadLine() ?? "";
            texto = texto.Trim();

            if (texto != "" && !texto.Contains("|"))
                return texto;

            Console.WriteLine("ingrese un texto valido... No utilice | ...");
        }
    }

    static int LeerNumero(string mensaje, int minimo)
    {
        while (true)
        {
            Console.Write(mensaje);
            string entrada = Console.ReadLine() ?? "";

            int numero;
            if (int.TryParse(entrada, out numero) && numero >= minimo)
                return numero;

            Console.WriteLine($"ingrese un numero entero mayor o igual a {minimo}...");
        }
    }

    // ------------------------------------------------
    // REGISTRAR UNA NUEVA ENTREGA
    // ------------------------------------------------
    static void RegistrarEntrega()
    {
        Console.WriteLine("\n=== REGISTRO DE ENTREGA ===");

        string codigo = LeerTexto("Código del estudiante: ").ToUpper();
        string archivo = LeerTexto("Nombre del archivo: ");

        int intentos = LeerNumero("Número de intentos: ", 1);
        int puntaje  = LeerNumero("Puntaje obtenido: ", 0);

        Console.Write("Observaciones (opcional): ");
        string observaciones = Console.ReadLine() ?? "";

        if (observaciones.Trim() == "")
            observaciones = "Sin observaciones";

        if (observaciones.Contains("|"))
        {
            Console.WriteLine("Las observaciones no pueden contener el símbolo |.");
            return;
        }

        Entrega nueva = new Entrega(codigo, archivo, intentos, puntaje, observaciones.Trim());

        Entregas[totalEntregas] = nueva;
        totalEntregas++;

        string registro = $"{codigo}|{archivo}|{intentos}|{puntaje}|{nueva.Observaciones}";
        File.AppendAllText(rutaArchivo, registro + Environment.NewLine);
        Console.WriteLine("Entrega registrada correctamente...");
    }

    // ------------------------------------------------
    // MOSTRAR LAS ENTREGAS
    // ------------------------------------------------
    static void ListarEntregas()
    {
        Console.WriteLine("\n=== ENTREGAS REGISTRADAS ===");

        if (totalEntregas == 0)
        {
            Console.WriteLine($"No existen entregas registradas.");
            return;
        }

        for (int i = 0; i < totalEntregas; i++)
        {
            Entrega e = Entregas[i];
            Console.WriteLine("--------------------------------");
            Console.WriteLine($"Código: {e.Codigo}");
            Console.WriteLine($"Archivo: {e.Archivo}");
            Console.WriteLine($"Intentos: {e.Intentos}");
            Console.WriteLine($"Puntaje: {e.Puntaje}");
            Console.WriteLine($"Observaciones: {e.Observaciones}");
        }
    }

    // ------------------------------------------------
    // 1. SUMATORIA
    // ------------------------------------------------
    static int SumatoriaPuntajes(Entrega[] lista, int total, int i = 0)
    {
        if (i == total)
            return 0;

        ValidarPuntaje(lista[i].Puntaje);
        return lista[i].Puntaje + SumatoriaPuntajes(lista, total, i + 1);
    }

    static int SumatoriaPuntajesIter(Entrega[] lista, int total)
    {
        int suma = 0;
        for (int i = 0; i < total; i++)
        {
            ValidarPuntaje(lista[i].Puntaje);
            suma += lista[i].Puntaje;
        }
        return suma;
    }

    // ------------------------------------------------
    // 2. BÚSQUEDA
    // ------------------------------------------------
    static int BuscarCodigo(Entrega[] lista, int total, string codigo, int i = 0)
    {
        ValidarTexto(codigo, "codigo a buscar");

        if (i == total)
            return -1;

        if (lista[i].Codigo.Equals(codigo.Trim(), StringComparison.OrdinalIgnoreCase))
            return i;

        return BuscarCodigo(lista, total, codigo, i + 1);
    }

    static int BuscarCodigoIter(Entrega[] lista, int total, string codigo)
    {
        ValidarTexto(codigo, "codigo a buscar");

        for (int i = 0; i < total; i++)
        {
            if (lista[i].Codigo.Equals(codigo.Trim(), StringComparison.OrdinalIgnoreCase))
                return i;
        }
        return -1;
    }

    // ------------------------------------------------
    // 3. CONTEO DE PALABRAS
    // ------------------------------------------------
    static int ContarPalabra(Entrega[] lista, int total, string palabra, int i = 0)
    {
        ValidarTexto(palabra, "palabra clave");

        if (i == total)
            return 0;

        int coincide;
        if (lista[i].Archivo.IndexOf(palabra.Trim(), StringComparison.OrdinalIgnoreCase) >= 0)
            coincide = 1;
        else
            coincide = 0;

        return coincide + ContarPalabra(lista, total, palabra, i + 1);
    }

    static int ContarPalabraIter(Entrega[] lista, int total, string palabra)
    {
        ValidarTexto(palabra, "palabra clave");

        int suma = 0;
        for (int i = 0; i < total; i++)
        {
            if (lista[i].Archivo.IndexOf(palabra.Trim(), StringComparison.OrdinalIgnoreCase) >= 0)
                suma++;
        }
        return suma;
    }

    // ------------------------------------------------
    // 4. FACTORIAL
    // ------------------------------------------------
    static BigInteger Factorial(int n)
    {
        if (n < 0)
            throw new ArgumentException("el numero no puede ser negativo.... error...");
        if (n <= 1)
            return 1;
        return n * Factorial(n - 1);
    }

    static BigInteger FactorialIter(int n)
    {
        if (n < 0)
            throw new ArgumentException("el numero no puede ser negativo.... error...");

        BigInteger resultado = 1;
        for (int i = 2; i <= n; i++)
            resultado *= i;

        return resultado;
    }

    // ------------------------------------------------
    // MENÚ PRINCIPAL
    // ------------------------------------------------
    static void Main(string[] args)
    {
        CargarEntregas();
        Console.WriteLine($"Entregas cargadas: {totalEntregas}");

        int opcion;
        do
        {
            Console.WriteLine("\n=== SISTEMA DE ANALISIS DE ENTREGAS ===");
            Console.WriteLine("1. Registrar entrega");
            Console.WriteLine("2. Mostrar entregas");
            Console.WriteLine("3. Sumar puntajes");
            Console.WriteLine("4. Buscar código de estudiante");
            Console.WriteLine("5. Contar palabra en archivos");
            Console.WriteLine("6. Calcular factorial");
            Console.WriteLine("0. Salir");

            opcion = LeerNumero("Seleccione una opción: ", 0);

            switch (opcion)
            {
                case 1:
                    RegistrarEntrega();
                    break;

                case 2:
                    ListarEntregas();
                    break;

                case 3:
                    Console.WriteLine($"Sumatoria recursiva: {SumatoriaPuntajes(Entregas, totalEntregas)}");
                    Console.WriteLine($"Sumatoria iterativa: {SumatoriaPuntajesIter(Entregas, totalEntregas)}");

                    if (totalEntregas > 0)
                    {
                        double promedio = (double)SumatoriaPuntajes(Entregas, totalEntregas) / totalEntregas;
                        Console.WriteLine($"Promedio: {promedio:F2}");
                    }
                    break;

                case 4:
                    string codigo = LeerTexto("Código a buscar: ");
                    int posicion = BuscarCodigo(Entregas, totalEntregas, codigo);

                    if (posicion == -1)
                    {
                        Console.WriteLine($"codigo no encontrado....");
                    }
                    else
                    {
                        Console.WriteLine($"Encontrado: {Entregas[posicion].Archivo}");
                    }
                    break;

                case 5:
                    string palabra = LeerTexto("Palabra a buscar: ");
                    Console.WriteLine($"Recursivo: {ContarPalabra(Entregas, totalEntregas, palabra)}");
                    Console.WriteLine($"Iterativo: {ContarPalabraIter(Entregas, totalEntregas, palabra)}");
                    break;

                case 6:
                    int numero = LeerNumero("Ingrese un número: ", 0);
                    Console.WriteLine($"Factorial recursivo: {Factorial(numero)}");
                    Console.WriteLine($"Factorial iterativo: {FactorialIter(numero)}");
                    break;

                case 0:
                    Console.WriteLine($"programa finalizado...");
                    break;

                default:
                    Console.WriteLine($"opcion no valida...");
                    break;
            }
        } while (opcion != 0);
    }
}