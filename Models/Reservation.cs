using System;

namespace condogenius_api_reservation.Models 
{
    public class Reservation
    {
        public int Id { get; set; }
        public int AreaId { get; set; }
        public int ResidentId { get; set; }
        public DateTime ReserveDate{ get; set; }
    }
}