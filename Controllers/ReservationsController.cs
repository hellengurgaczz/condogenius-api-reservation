using System;
using Microsoft.AspNetCore.Mvc;
using condogenius_api_reservation.Models;
using condogenius_api_reservation.Database;
using Swashbuckle.AspNetCore.Annotations;


namespace condogenius_api_reservation.Controllers 
{

    [ApiController]
    [Route("api/reservations")]

    public class ReservationsController : ControllerBase 
    {
        private readonly DataContext _context;
        public ReservationsController(DataContext context) => _context = context;

        [HttpPost]
        [SwaggerOperation("Retorna uma lista de objetos")]
        [SwaggerResponse(StatusCodes.Status200OK, "Retorna uma lista de objetos", typeof(IEnumerable<Reservation>))]
        public IActionResult Create([FromBody] Reservation reservation)
        {
            _context.Reservations.Add(reservation);
            _context.SaveChanges();
            return Created("Reserva cadastrada!", reservation);
        }
    }

}