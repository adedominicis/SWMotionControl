using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Windows;

namespace AddinTesting.model
{
    class SelectionListener
    {
        #region Privados
        PartDoc swPart;
        AssemblyDoc swAssy;
        DrawingDoc swDraw;
        ModelDoc2 swModel;
        #endregion

        public SelectionListener(ModelDoc2 model)
        {
            swModel =(ModelDoc2) model;
            updateDocumentModel();
            
        }

        public void updateDocumentModel()
        {
            //Crear objetos swDoc
            if (swModel.GetType() == (int)swDocumentTypes_e.swDocPART)
            {
                swPart = (PartDoc)swModel;
            }
            else if (swModel.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY)
            {
                swAssy = (AssemblyDoc)swModel;
            }
            else if (swModel.GetType() == (int)swDocumentTypes_e.swDocDRAWING)
            {
                swDraw = (DrawingDoc)swModel;
            }

            // Suscribirse a eventos de solidworks
            if ((swPart != null))
            {
                swPart.UserSelectionPostNotify += swPart_UserSelectionPostNotify;
                swPart.ActiveConfigChangeNotify += ActiveConfigChangeNotify;
            }
            if ((swAssy != null))
            {
                swAssy.ActiveConfigChangeNotify += ActiveConfigChangeNotify;
                //swAssy.NewSelectionNotify += SwAssy_NewSelectionNotify;
                swAssy.UserSelectionPostNotify += swAssy_UserSelectionPostNotify;
            }
            if ((swDraw != null))
            {
                swDraw.UserSelectionPostNotify += swDraw_UserSelectionPostNotify;
            }

        }

        private int ActiveConfigChangeNotify()
        {
            int functionReturnValue = 0;
            MessageBox.Show("Cambio de configuración!");
            return functionReturnValue;
        }

        private int SwAssy_NewSelectionNotify()
        {
            int functionReturnValue = 0;
            MessageBox.Show("Nueva Seleccion!");
            return functionReturnValue;
        }

        private int swPart_UserSelectionPostNotify()
        {
            int functionReturnValue = 0;
            MessageBox.Show("An entity was selected in a part document.");
            return functionReturnValue;
        }

        private int swAssy_UserSelectionPostNotify()
        {
            int functionReturnValue = 0;
            MessageBox.Show("An entity was selected in a assembly document.");
            return functionReturnValue;
        }

        private int swDraw_UserSelectionPostNotify()
        {
            int functionReturnValue = 0;
            MessageBox.Show("An entity was selected in a drawing document.");
            return functionReturnValue;
        }
    }
}
