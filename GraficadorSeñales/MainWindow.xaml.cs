﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraficadorSeñales
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double amplitudMaxima = 1;
        Señal señal;
        Señal señalResultado;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BotonGraficar_Click(object sender, RoutedEventArgs e)
        {
            double tiempoInicial = double.Parse(txt_TiempoInicial.Text);
            double tiempoFinal = double.Parse(txt_TiempoFinal.Text);
            double frecuenciaMuestreo = double.Parse(txt_FrecuenciaDeMuestreo.Text);

            

            switch (cb_TipoSeñal.SelectedIndex)
            {
                // Señal Senoidal
                case 0:
                    double amplitud = double.Parse(((ConfiguracionSeñalSenoidal)(panelConfiguracion.Children[0])).txt_Amplitud.Text);
                    double fase = double.Parse(((ConfiguracionSeñalSenoidal)(panelConfiguracion.Children[0])).txt_Fase.Text);
                    double frecuencia = double.Parse(((ConfiguracionSeñalSenoidal)(panelConfiguracion.Children[0])).txt_Frecuencia.Text);

                    señal = new SeñalSenoidal(amplitud, fase, frecuencia);
                    break;

                // Señal Rampa
                case 1:
                    señal = new SeñalRampa();
                    break;

                // Señal Exponencial
                case 2:
                    double alpha = double.Parse(((ConfiguracionSeñalExponencial)(panelConfiguracion.Children[0])).txt_Alpha.Text);
                    señal = new SeñalExponencial(alpha);
                    break;

                // Señal Rectangular
                case 3:
                    señal = new SeñalRectangular();
                    break;

                default:
                    señal = null;
                    break;
            }

           

            // Primer Señal
            señal.TiempoInicial = tiempoInicial;
            señal.TiempoFinal = tiempoFinal;
            señal.FrecuenciaMuestreo = frecuenciaMuestreo;

           
            
            señal.construirSeñalDigital();
      

            // Truncar
            if ((bool)ckb_Truncado.IsChecked)
            {
                double n = double.Parse(txt_Truncado.Text);
                señal.truncar(n);
            }

        

            // Escalar
            if ((bool)ckb_Escala.IsChecked)
            {
                double factorEscala = double.Parse(txt_EscalaAmplitud.Text);
                señal.escalar(factorEscala);
            }

        

            // Desplazar
            if ((bool)ckb_Desplazamiento.IsChecked)
            {
                double factorDesplazamiento = double.Parse(txt_Desplazamiento.Text);
                señal.desplazar(factorDesplazamiento);
            }

         

            // Actualizar
            señal.actualizarAmplitudMaxima();
           

            

            // Limpieza de polylines
            plnGrafica.Points.Clear();
            
        

            // Impresión de la amplitud máxima en los labels de la ventana.
            lbl_AmplitudMaxima.Text = amplitudMaxima.ToString("F");
            lbl_AmplitudMinima.Text = "-" + amplitudMaxima.ToString("F");

            if (señal != null)
            {
                // Sirve para recorrer una coleccion o arreglo
                foreach (Muestra muestra in señal.Muestras)
                {
                    plnGrafica.Points.Add(new Point((muestra.X - tiempoInicial) * scrContenedor.Width, (muestra.Y / amplitudMaxima * ((scrContenedor.Height / 2) - 30) * -1 + (scrContenedor.Height / 2))));
                }
            }

            

            // Línea del Eje X
            plnEjeX.Points.Clear();
            plnEjeX.Points.Add(new Point(0, scrContenedor.Height / 2));
            plnEjeX.Points.Add(new Point((tiempoFinal - tiempoInicial) * scrContenedor.Width, scrContenedor.Height / 2));

            // Línea del Eje Y
            plnEjeY.Points.Clear();
            plnEjeY.Points.Add(new Point((-tiempoInicial) * scrContenedor.Width, 0));
            plnEjeY.Points.Add(new Point((-tiempoInicial) * scrContenedor.Width, scrContenedor.Height));
        }


        // Combo Box de la Primera Señal
        private void cb_TipoSeñal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            panelConfiguracion.Children.Clear();
            switch (cb_TipoSeñal.SelectedIndex)
            {
                // Señal Senoidal
                case 0:
                    panelConfiguracion.Children.Add(new ConfiguracionSeñalSenoidal());
                    break;
                    
                // Señal Rampa
                case 1:
                    break;

                // Señal Senoidal
                case 2:
                    panelConfiguracion.Children.Add(new ConfiguracionSeñalExponencial());
                    break;

                // Señal Rectangular
                case 3:
                    break;

                default:
                    break;
            }
        }

        private void btn_TransformadaDeFourier_Click(object sender, RoutedEventArgs e)
        {
            Señal transformada = Señal.transformar(señal);

            plnGrafica_Resultado.Points.Clear();

            lbl_AmplitudMaxima_Resultado.Text = transformada.AmplitudMaxima.ToString("F");
            lbl_AmplitudMinima_Resultado.Text = "-" + transformada.AmplitudMaxima.ToString("F");

            if (señal != null)
            {
                // Sirve para recorrer una coleccion o arreglo
                foreach (Muestra muestra in señal.Muestras)
                {
                    plnGrafica_Resultado.Points.Add(new Point((muestra.X - transformada.TiempoInicial) * scrContenedor.Width,
                        (muestra.Y / transformada.AmplitudMaxima * ((scrContenedor_Resultado.Height / 2) - 30) * -1 + (scrContenedor_Resultado.Height / 2))));
                }
            }



            // Línea del Eje X
            plnEjeX_Resultado.Points.Clear();
            plnEjeX_Resultado.Points.Add(new Point(0, scrContenedor_Resultado.Height / 2));
            plnEjeX_Resultado.Points.Add(new Point((transformada.TiempoFinal - transformada.TiempoInicial) *
                scrContenedor_Resultado.Width, scrContenedor_Resultado.Height / 2));

            // Línea del Eje Y
            plnEjeY_Resultado.Points.Clear();
            plnEjeY_Resultado.Points.Add(new Point((-transformada.TiempoInicial) * scrContenedor_Resultado.Width, 0));
            plnEjeY_Resultado.Points.Add(new Point((-transformada.TiempoInicial) * scrContenedor_Resultado.Width, scrContenedor_Resultado.Height));
        }
           
        
    }
}
