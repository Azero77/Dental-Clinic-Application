using DentalClinicApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinicApplication.Exceptions
{
    public class AppointmentAlreadyTakenException
        : Exception
    {
        public Appointment ConflictedAppointment { get; }
        public AppointmentAlreadyTakenException(Appointment conflictedAppointment)
        {
            this.ConflictedAppointment = conflictedAppointment;
        }

        public AppointmentAlreadyTakenException(Appointment conflictedAppointment, string? message) : base(message)
        {
            this.ConflictedAppointment = conflictedAppointment;
        }

        public AppointmentAlreadyTakenException(Appointment conflictedAppointment, string? message, Exception? innerException) : base(message, innerException)
        {
            this.ConflictedAppointment = conflictedAppointment;
        }

    }
}
