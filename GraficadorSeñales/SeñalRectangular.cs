using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraficadorSeñales
{
    class SeñalRectangular : Señal 
    {
        public SeñalRectangular()
        {
            Tiempo = 0;
            Muestras = new List<Muestra>();
            AmplitudMaxima = 0.0;
        }

        public SeñalRectangular(double tiempo) // Los constructores se llaman como la clase
        {                       // Éstas de arriba son variables internas
            Tiempo = tiempo;
            Muestras = new List<Muestra>();
            AmplitudMaxima = 0.0;
        }

        override public double evaluar(double tiempo)
        {
            double resultado = 0;
            if ( Math.Abs(tiempo) > 0.5  )
            {
                resultado = 0;
            }else if ( Math.Abs(tiempo) == 0.5)
            {
                resultado = 0.5;
            }else if ( Math.Abs(tiempo) < 0.5)
            {
                resultado = 1;
            }

            return resultado;
        }
    }
}
