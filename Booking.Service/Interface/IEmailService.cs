using Booking.Domain.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Service.Interface
{
    public interface IEmailService
    {
        Boolean SendEmailAsync(EmailMessage allMails);
    }
}
