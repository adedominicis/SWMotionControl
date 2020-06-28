using SolidWorks.Interop.sldworks;



namespace AddinTesting.model
{
    class MateManipulator
    {

        private SldWorks swApp;
        private ModelDoc2 swModel;
        private xBoxControllerCom xboxComm;

        public MateManipulator(SldWorks SwApp, xBoxControllerCom xbc)
        {
            //Instancia actual de solidworks
            this.swApp = (SldWorks)SwApp;
            this.xboxComm = xbc;

        }

        public void angleMateDriver(MateEntity2 mate)
        {

        }
    }
}
