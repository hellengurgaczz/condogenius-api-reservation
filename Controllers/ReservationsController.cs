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

        public ReservationsController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Create([FromBody] Reservation reservation)
        {
            _context.Reservations.Add(reservation);
            _context.SaveChanges();
            return Created("Reserva cadastrada!", reservation);
        }


        [HttpGet]
        public List<Reservation> List() => _context.Reservations.ToList();

        private Reservation getById(Guid id)
        {
            Reservation? reservation = _context.Reservations.Find(id);
            return reservation;
            
        }
        
        [HttpDelete]
        public IActionResult Delete( [FromRoute] Guid id)
        {
            Reservation reservation = getById(id);

            if(reservation == null) {
                return NotFound();
            }


            _context.Reservations.Remove(reservation);
            
            _context.SaveChanges();
            
            return Ok();
        }
    }

}