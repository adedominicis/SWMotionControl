using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddinTesting.model
{
    /// <summary>
    /// Data para alimentar el viewmodel.
    /// Properties con null safety y "commands", metodos.
    /// </summary>
    class viewModelData
    {

        #region Private Fields

        // Analog positions
        private decimal leftThumbX;
        private decimal leftThumbY;
        private decimal rightThumbX;
        private decimal rightThumbY;
        private decimal rightTrigger;
        private decimal leftTrigger;

        #endregion

        #region Properties

        //Analog positions left thumbstick

        public decimal LeftThumbX
        {
            set { leftThumbX = value; }
            get { return leftThumbX; }
        }
        public decimal LeftThumbY
        {
            set { leftThumbY = value; }
            get { return leftThumbY; }
        }

        //Analog positions right thumbstick

        public decimal RightThumbX
        {
            set { rightThumbX = value; }
            get { return rightThumbX; }
        }
        public decimal RightThumbY
        {
            set { rightThumbY = value; }
            get { return rightThumbY; }
        }

        //Analog positions triggers
        public decimal RightTrigger
        {
            set { rightTrigger = value; }
            get { return rightTrigger; }
        }
        public decimal LeftTrigger
        {
            set { leftTrigger = value; }
            get { return leftTrigger; }
        }

        // Commands



        #endregion


    }
}
