using System;

namespace Semantica
{
    public class Sintaxis : Lexico
    {
        public Sintaxis()
        {
            NextToken();
        }
        public Sintaxis(string nombre) : base(nombre)
        {
            NextToken();
        }

        public void match(String espera)
        {
            if (espera == getContenido())
            {
                NextToken();
            }
            else
            {
                throw new Error("Error de sintaxis, se espera un " +espera+" en linea: "+linea, log);
            }
        }

        public void match(Tipos espera)
        {
            if (espera == getClasificacion())
            {
                NextToken();
            }
            else
            {
                throw new Error("Error de sintaxis, se espera un " +espera+" en linea: "+linea , log);
            }
        }
    }
}