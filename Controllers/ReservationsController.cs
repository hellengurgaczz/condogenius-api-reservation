using System;
using Microsoft.AspNetCore.Mvc;
using condogenius_api_reservation.Models;
using condogenius_api_reservation.Database;

namespace condogenius_api_reservation.Controllers 
{

    [ApiController]
    [Route("api/reservations")]

    public class ReservationsController : ControllerBase 
    {
        private readonly DataContext _context;
        public ReservationsController(DataContext context) => _context = context;

        [HttpPost]
        public IActionResult Create([FromBody] Reservation reservation)
        {
            _context.Reservations.Add(reservation);
            _context.SaveChanges();
            return Created("Reserva cadastrada!", reservation);
        }
    }

}