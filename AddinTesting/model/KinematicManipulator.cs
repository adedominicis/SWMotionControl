using SolidWorks.Interop.sldworks;
using System;
using System.Windows;
using AddinTesting.ViewModel;

namespace AddinTesting.model
{
    class KinematicManipulator
    {
        #region Private fields
        private ModelDoc2 swModel;
        private Feature swFeat;
        private Mate2 swMate;
        private AngleMateFeatureData swAngleMateData;
        private AppControl miApp;


        #endregion

        #region Constructores y publicos
        public KinematicManipulator(AppControl app)
        {
            miApp = (AppControl)app;
            initMateManipulator();
        }

        //Valores de cinemática
        public int DeltaT { get; set; }
        public decimal OmegaMaxZ { get; set; }
        public decimal VmaxZ { get; set; }
        public decimal VmaxR { get; set; }

        #endregion


        #region Metodos
        private void initMateManipulator()
        {
            //Suscribirse a evento de seleccion de mates.
            miApp.SelListener.userSelectedFeature -= mateSelectedHandler;
            miApp.SelListener.userSelectedFeature += mateSelectedHandler;
            //Suscribirse al refresh del control
            miApp.appControlUiRefresh -= App_appControlUiRefresh;
            miApp.appControlUiRefresh += App_appControlUiRefresh;

        }

        private void App_appControlUiRefresh(object sender, viewModelData e)
        {
            //Si existe un feature seleccionado:
            if (swFeat!=null)
            {
                string typeName = swFeat.GetTypeName2();
                switch (typeName)
                {
                    case "MatePlanarAngleDim":
                        angleMateManipulator(swFeat);
                        break;
                    default:
                        break;
                }
            }

        }

        private void mateSelectedHandler(object sender, Feature feat)
        {
            SelectionListener sl = (SelectionListener)sender;
            swFeat = (Feature)feat;
            swModel = (ModelDoc2)sl.SwModel;
        }
        
        private void angleMateManipulator(Feature angleMateFeat)
        {
            swMate = (Mate2)angleMateFeat.GetSpecificFeature2();
            swAngleMateData = (AngleMateFeatureData)swFeat.GetDefinition();
            
            // Controlador xbox

            if (miApp.VmData.IsPlugged)
            {

                //Calcular el incremento

                //Sumar incremento
                double delta = (double)getDelta(miApp.VmData.LeftThumbX);
                if (Math.Abs(delta)>0)
                {
                    swAngleMateData.Angle = swAngleMateData.Angle + delta;
                    //Aplicar en el modelo
                    angleMateFeat.ModifyDefinition(swAngleMateData, swModel, null);
                }

            }
            else
            {
                MessageBox.Show("No es posible controlar el mate angular sin control conectado");
            }
 
        }

        #endregion

        #region Math Helpers

        private double degToRad(decimal deg)
        {
            return (180.0 * (double)deg / Math.PI);
        }

        private double radToDeg(decimal rad)
        {
            return ((double)rad * Math.PI / 180.0);
        }

        private decimal getDelta(decimal percent)
        {
            // Velocidad angular requerida
            decimal OmegaZ = OmegaMaxZ * percent / 100;

            //Velocidad angular en radianes/sec
            decimal OmegaZradSec= (decimal)(2 * Math.PI * (double)OmegaZ) / 60M;

            //Angulo recorrido en grados en el tiempo de calculo
            return OmegaZradSec * DeltaT/1000;

        }

        #endregion



    }
}
