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
        IEnumerable<IDataManipulator> DataManipulators { get; }

        public ManipulationNotifierService(IEnumerable<IDataManipulator> dataManipulators)
        {
            DataManipulators = dataManipulators;
            foreach (IDataManipulator manipulator in DataManipulators)
            {
                manipulator.DataManipulated += OnDataManipulated;
            }
        }

        public event Action? DataManipulated;
        public void OnDataManipulated()
        {
            DataManipulated?.Invoke();
        }
    }
}
