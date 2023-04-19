using System;

namespace condogenius_api_reservation.Models 
{
    public class Reservation
    {
        public Reservation() {
            created_at = DateTime.Now;
        }

        public int id { get; set; }
        public int area_id { get; set; }
        public int resident_id { get; set; }
        public DateTime? reserve_date { get; set; }
        public DateTime created_at { get; set; }
    }
}