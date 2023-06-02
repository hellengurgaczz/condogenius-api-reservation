using System;

namespace condogenius_api_reservation.Models 
{
    public class Message
    {
            public int Contact { get; set; }
            public string ResidentName { get; set; }
            public DateTime ReserveDate { get; set; }
            public Reservation Reservation { get; set; }
    }
}