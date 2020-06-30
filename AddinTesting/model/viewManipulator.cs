using SolidWorks.Interop.sldworks;


namespace AddinTesting.model
{
    // Manipulador de la vista usando imputs del control.
    class viewManipulator
    {

        private SldWorks swApp;
        private ModelDoc2 swModel;
        private xBoxControllerCom xBoxConInstance;

        public viewManipulator(SldWorks SwApp, xBoxControllerCom xbc)
        {
            //Instancia actual de solidworks
            this.swApp = (SldWorks)SwApp;
            this.xBoxConInstance = xbc;
            
        }

        public void rotateView()
        {
            swModel = (ModelDoc2)swApp.ActiveDoc;

            if (swModel!=null)
            {
                //Rotar sobre el Z
                if (xBoxConInstance.LeftThumbX > 10)
                {
                    swModel.ViewRotateplusz();
                    //swModel.ViewRotate()
                }

                if (xBoxConInstance.LeftThumbX < -10)
                {
                    swModel.ViewRotateminusz();
                }

                //Rotar sobre el X
                if (xBoxConInstance.LeftThumbY > 10)
                {
                    swModel.ViewRotateplusx();
                }
                if (xBoxConInstance.LeftThumbY <-10)
                {
                    swModel.ViewRotateminusx();
                }

                //Paneo lateral

                if (xBoxConInstance.RightThumbX > 10)
                {
                    swModel.ViewTranslateplusx();
                }
                if (xBoxConInstance.RightThumbX < -10)
                {
                    swModel.ViewTranslateminusx();
                }

                //Paneo Vertical
                if (xBoxConInstance.RightThumbY > 10)
                {
                    swModel.ViewTranslateplusy();
                }
                if (xBoxConInstance.RightThumbY < -10)
                {
                    swModel.ViewTranslateminusy();
                }

                if (xBoxConInstance.LeftTrigger > 0)
                {
                    swModel.ViewZoomin();
                }
                if (xBoxConInstance.RightTrigger > 0)
                {
                    swModel.ViewZoomout();
                }
            }


        }



    }
}
