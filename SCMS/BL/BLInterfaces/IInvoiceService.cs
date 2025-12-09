using System.Collections.Generic;
using SCMS.Models;

namespace SCMS.BL.BLInterfaces
{
    public interface IInvoiceService
    {
        Invoice? CreateForBooking(int bookingId);
        Invoice? GetById(int invoiceId);
        Invoice? GetByBooking(int bookingId);
        List<Invoice> GetByPatient(int patientId);
        bool MarkAsPaid(int invoiceId);
    }
}
