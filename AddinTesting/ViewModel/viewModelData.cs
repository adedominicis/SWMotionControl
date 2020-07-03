using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AddinTesting.model;

namespace AddinTesting.ViewModel
{
    /// <summary>
    /// Data para alimentar el viewmodel.
    /// Properties con null safety y "commands", metodos.
    /// </summary>
    public class viewModelData
    {


        #region Properties

        //Posiciones analogo izquierdo

        public decimal LeftThumbX { get; set; }
        public decimal LeftThumbY { get; set; }

        //Posiciones analogo derecho

        public decimal RightThumbX { get; set; }
        public decimal RightThumbY { get; set; }

        //Posiciones triggers
        public decimal RightTrigger { get; set; }
        public decimal LeftTrigger { get; set; }

        // Intervalo de solucion.
        public decimal VmaxZ { get; set; }

        //Velocidad máxima en el plano radial del eje  1
        public decimal VmaxR { get; set; }

        //Intervalo de tiempo para la resolución de constraints y rendering
        public int DeltaT { get; set; }

        //Velocidad angular máxima 
        public decimal OmegaMaxZ { get; set; }

        //Deadzone de analogos
        public int DeadZone { get; set; }

        //Está el control conectado?

        public bool IsPlugged { get; set; }

        #endregion


    }
}
