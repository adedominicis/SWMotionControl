using SolidWorks.Interop.sldworks;
using System;
using System.Windows;

namespace AddinTesting.model
{
    class MateManipulator
    {
        #region Private fields
        private ModelDoc2 swModel;
        private Feature swFeat;
        private Mate2 swMate;
        private AngleMateFeatureData swAngleMateData;
        private AppControl miApp;
        private const int maxRpm = 30;
        private decimal tickDeltaMilliSec;
        #endregion

        #region Constructores y publicos
        public MateManipulator(AppControl app)
        {
            //Suscribirse a evento de seleccion de mates.
            app.SelListener.userSelectedFeature -= mateSelectedHandler;
            app.SelListener.userSelectedFeature += mateSelectedHandler;
            //Suscribirse al refresh del control
            app.appControlUiRefresh -= App_appControlUiRefresh;
            app.appControlUiRefresh += App_appControlUiRefresh;
            tickDeltaMilliSec = app.XBoxConInstance.tickDeltaMilliSec;

            miApp=(AppControl)app;

            
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
        #endregion

        #region Metodos

        private void angleMateManipulator(Feature angleMateFeat)
        {
            swMate = (Mate2)angleMateFeat.GetSpecificFeature2();
            swAngleMateData = (AngleMateFeatureData)swFeat.GetDefinition();
            
            // Controlador xbox

            if (miApp.XBoxConInstance.PlugStatus)
            {

                //Calcular el incremento

                //Sumar incremento
                double delta = (double)getDelta(miApp.XBoxConInstance.LeftThumbX);
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



        // Math helpers.
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
            decimal rpm = maxRpm * percent / 100;

            //Velocidad angular en radianes/sec
            decimal radSec= (decimal)(2 * Math.PI * (double)rpm) / 60M;

            //Angulo recorrido en grados en el tiempo de calculo
            return radSec* tickDeltaMilliSec/1000;

        }

        #endregion



    }
}
