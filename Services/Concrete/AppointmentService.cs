using Cor.Apt.Entities;
using Cor.Apt.Services.Interfaces;

namespace Cor.Apt.Services.Concrete
{
    public class AppointmentService : IAppointmentService
    {
        private readonly AppointmentContext _context;

        public AppointmentService(AppointmentContext context) { _context = context; }

        public bool TestAppointment(int a) => true;

    }
}