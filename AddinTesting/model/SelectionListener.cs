using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Windows;

namespace AddinTesting.model
{
    class SelectionListener
    {
        /// <summary>
        /// Esta clase sirve de base para crear listeners de eventos en solidworks.
        /// 
        /// En esta primera definicion, esta clase detectará si el usuario ha seleccionado algo
        /// Ese elemento seleccionado, en caso de ser no nulo, será colgado en alguno de los eventos expuestos en la region Eventos
        /// </summary>


        #region Privados
        private PartDoc swPart;
        private AssemblyDoc swAssy;
        private DrawingDoc swDraw;
        private ModelDoc2 swModel;
        private SelectionMgr selMgr;
        private Feature swFeat;
        private SldWorks swApp;

        #endregion

        #region Constructores y publicos
        // Evento que indica si el usuario seleccionó un feature. El feature se adjunta como mensaje.


        // A traves de este setter puede actualizarse el swModel.
        public ModelDoc2 SwModel
        {
            get { return swModel; }
            internal set
            {
                swModel = value;
                updateSwDoc();
            }
        }

        public SelectionListener(SldWorks swapp)
        {
            // Setear instancia de solidworks
            swApp = (SldWorks)swapp;
            // Suscribirse a los eventos SolidWorks de cambio de documento
            subscribeToSWChangeDocEvents();
        }
        #endregion

        #region Eventos

        /// <summary>
        /// Todos los eventos listados acá se disparan cuando el usuario seleccione el feature correspondiente
        /// Se incluye el objeto seleccionado en el argumento del evento.
        /// </summary>
        public event EventHandler<Feature> userSelectedFeature;
        #endregion

        #region SolidWorks document change handling
        /// <summary>
        /// Este bloque se encarga de actualizar la referencia a swModel cuando el usuario cambia de documento.
        /// Adicionalmente, crea el swDoc correspondiente, bien sea PartDoc, AssemblyDoc o DrawingDoc
        /// </summary>
        private void subscribeToSWChangeDocEvents()
        {
            swApp.ActiveDocChangeNotify += SwApp_ActiveDocChangeNotify;
            swApp.FileOpenNotify += SwApp_FileOpenNotify;
            swApp.FileNewNotify2 += SwApp_FileNewNotify2;

        }
        // Handlers
        private int SwApp_FileNewNotify2(object NewDoc, int DocType, string TemplateName)
        {
            //Actualizar swModel
            SwModel = (ModelDoc2)NewDoc;
            return 0;
        }
        private int SwApp_FileOpenNotify(string FileName)
        {
            //Actualizar swModel
            SwModel = (ModelDoc2)swApp.ActiveDoc;
            return 0;
        }
        private int SwApp_ActiveDocChangeNotify()
        {
            //Actualizar swModel
            SwModel = (ModelDoc2)swApp.ActiveDoc;
            return 0;
        }
        //Actualizar el tipo de documento y castear el tipo al swDoc correcto
        public void updateSwDoc()
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
        }
        #endregion

        #region Suscripción a Listeners y publicación de selecciones
        /// <summary>
        /// Suscribirse a los listeners de seleccion de varios tipos de features.
        /// </summary>
        public void subToSelectionListeners()
        {
            if (swPart != null)
            {
                //Eventos de seleccion en partes
                swPart.UserSelectionPostNotify += SwPart_UserSelectionPostNotify;
            }
            if (swAssy != null)
            {
                //Eventos de seleccion en ensamblajes
                swAssy.UserSelectionPostNotify += SwAssy_UserSelectionPostNotify;
            }
            if (swDraw != null)
            {
                //Eventos de seleccion en dibujos
                swDraw.UserSelectionPostNotify += SwDraw_UserSelectionPostNotify;
            }
        }


        //Event Handlers

        /// <summary>
        /// Este metodo detecta la seleccion de un feature y lo expone en un evento
        /// </summary>

        private int SwAssy_UserSelectionPostNotify()
        {
            try
            {
                //Tomar selectionManager.
                selMgr = (SelectionMgr)swModel.SelectionManager;
                //Obtener el feature de lo que fue seleccionado
                swFeat = (Feature)selMgr.GetSelectedObject6(1, -1);
                if (swFeat!=null)
                {
                    //Disparar evento.
                    userSelectedFeature?.Invoke(this, swFeat);
                    MessageBox.Show("User selected a Feature!");
                }
                return 0;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error disparando userSelectedFeature en  SwAssy_UserSelectionPostNotify: " + e.Message);
            }
            return 0;
        }

        // Por implementar
        private int SwPart_UserSelectionPostNotify()
        {
            SwAssy_UserSelectionPostNotify();
            return 0;
        }

        //Por implementar
        private int SwDraw_UserSelectionPostNotify()
        {
            SwAssy_UserSelectionPostNotify();
            return 0;
        }
        #endregion
    }
}
