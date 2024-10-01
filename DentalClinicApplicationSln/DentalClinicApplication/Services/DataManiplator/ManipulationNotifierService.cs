using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.Services.DataManiplator
{
    /// <summary>
    /// raise an event when a data manipulation happens
    /// </summary>
    //can be extended by add eventArgs to each event and deals with it
    public class ManipulationNotifierService
    {
        IDataManipulator Manipulator { get;}

        public ManipulationNotifierService(IDataManipulator dataManipulator)
        {
            Manipulator = dataManipulator;
            Manipulator.DataManipulated += OnDataManipulated;
        }

        public event Action? DataManipulated;
        public void OnDataManipulated()
        {
            DataManipulated?.Invoke();
        }
    }
}
