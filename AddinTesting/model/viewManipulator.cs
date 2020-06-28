using SolidWorks.Interop.sldworks;


namespace AddinTesting.model
{
    // Manipulador de la vista usando imputs del control.
    class viewManipulator
    {

        private SldWorks swApp;
        private ModelDoc2 swModel;
        private xBoxControllerCom xboxComm;

        public viewManipulator(SldWorks SwApp, xBoxControllerCom xbc)
        {
            //Instancia actual de solidworks
            this.swApp = (SldWorks)SwApp;
            this.xboxComm = xbc;
            
        }

        public void rotateView()
        {
            swModel = (ModelDoc2)swApp.ActiveDoc;

            if (swModel!=null)
            {
                //Rotar sobre el Z
                if (int.Parse(xboxComm.LeftThumbX) > 10)
                {
                    swModel.ViewRotateplusz();
                    //swModel.ViewRotate()
                }

                if (int.Parse(xboxComm.LeftThumbX) < -10)
                {
                    swModel.ViewRotateminusz();
                }

                //Rotar sobre el X
                if (int.Parse(xboxComm.LeftThumbY) > 10)
                {
                    swModel.ViewRotateplusx();
                }
                if (int.Parse(xboxComm.LeftThumbY) <-10)
                {
                    swModel.ViewRotateminusx();
                }

                //Paneo lateral

                if (int.Parse(xboxComm.RightThumbX) > 10)
                {
                    swModel.ViewTranslateplusx();
                }
                if (int.Parse(xboxComm.RightThumbX) < -10)
                {
                    swModel.ViewTranslateminusx();
                }

                //Paneo Vertical
                if (int.Parse(xboxComm.RightThumbY) > 10)
                {
                    swModel.ViewTranslateplusy();
                }
                if (int.Parse(xboxComm.RightThumbY) < -10)
                {
                    swModel.ViewTranslateminusy();
                }

                if (int.Parse(xboxComm.LeftTrigger) > 0)
                {
                    swModel.ViewZoomin();
                }
                if (int.Parse(xboxComm.RightTrigger) > 0)
                {
                    swModel.ViewZoomout();
                }
            }


        }



    }
}
