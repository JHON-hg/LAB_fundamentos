using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
class Entrega
{
    public string Codigo { get; set; }
    public string Archivo { get; set; }
    public int Puntaje { get; set; }

    public Entrega(string codigo, string archivo, int puntaje)
    {
        Codigo = codigo;
        Archivo = archivo;
        Puntaje = puntaje;
    }
}