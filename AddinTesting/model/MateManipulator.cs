using SolidWorks.Interop.sldworks;
using System.Windows;

namespace AddinTesting.model
{
    class MateManipulator
    {
        #region Private fields
        private ModelDoc2 swModel;
        private Feature swFeat;
        private Mate2 swMate;
        private MateFeatureData swMateData;
        private AngleMateFeatureData swAngleMateData;
        #endregion

        #region Constructores y publicos
        public MateManipulator(AppControl app)
        {
            //Suscribirse a evento de seleccion de mates.
            app.SelListener.userSelectedFeature += mateSelectedHandler;
        }

        /// <summary>
        /// Este event handler se activa cuando el usuario selecciona un FEATURE segun los permisibles en SelectionListener.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="feat"></param>
        private void mateSelectedHandler(object sender, Feature feat)
        {
            SelectionListener sl = (SelectionListener)sender;
            swFeat = (Feature)feat;
            swModel = (ModelDoc2)sl.SwModel;

            //Verificar si esto es un mate.
            MessageBox.Show("Esto es un :"+swFeat.GetTypeName2());

        }
        #endregion

    }
}
