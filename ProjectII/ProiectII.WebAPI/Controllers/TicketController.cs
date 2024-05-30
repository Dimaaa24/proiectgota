﻿using Microsoft.AspNetCore.Mvc;
using ProiectII.BusinessModels.Models;
using ProjectII.DataAccess.Sqlite;

namespace ProiectII.WebAPI.Controllers
{
    [Route("Tickets")]
    [ApiController]
    public class TicketController : Controller
    {
        private readonly CFRContext cfrContext;

        public TicketController(CFRContext cfrContext)
        {
            this.cfrContext = cfrContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Ticket>>> GetAll()
        {
            return Ok(cfrContext.Tickets.ToList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> GetRoute(int id)
        {
            Ticket ticket = cfrContext.Tickets.Find(id);
            if (ticket == null)
                return NotFound();
            return Ok(ticket);
        }

        [HttpPost]
        public async Task<ActionResult<List<Ticket>>> AddTicket(Ticket ticket)
        {
            cfrContext.Tickets.Add(ticket);
            await cfrContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{request.id}")]
        public async Task<ActionResult<List<Ticket>>> UpdateTicket(Ticket request)
        {
            Ticket ticket = cfrContext.Tickets.Find(request.Id);

            if (ticket == null)
            {
                return NotFound();
            }

            ticket.UserId = request.UserId;
            ticket.SeatNumber = request.SeatNumber;
            ticket.TrainId = request.TrainId;

            await cfrContext.SaveChangesAsync();

            return Ok(ticket);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Ticket>> Delete(int id)
        {
            foreach (var ticket in cfrContext.Tickets)
            {
                if (ticket.Id == id)
                    cfrContext.Tickets.Remove(ticket);
            }

            await cfrContext.SaveChangesAsync();
            return Ok();
        }

    }
}