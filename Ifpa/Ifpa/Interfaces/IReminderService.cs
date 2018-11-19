using Ifpa.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ifpa.Interfaces
{
    public interface IReminderService
    {
        Task<bool> CreateReminder(CalendarDetailViewModel calendarDetail);
    }
}
