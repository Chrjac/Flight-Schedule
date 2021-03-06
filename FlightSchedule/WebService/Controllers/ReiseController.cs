﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using DataAcc;
using DataModel;

namespace WebService.Controllers
{
    public class ReiseController : ApiController
    {
        private DataAccess db = new DataAccess();

        // GET: api/Reise
        public IQueryable<Reise> GetReises()
        {    
            return db.Reiser; 
        }

        // GET: api/Reise/5
        [ResponseType(typeof(Reise))]
        public IHttpActionResult GetReise(int id)
        {
            Reise reise = db.Reiser.Find(id);
            if (reise == null)
            {
                return NotFound();
            }

            return Ok(reise);
        }

        // PUT: api/Reise/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutReise(int id, Reise reise)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != reise.Id)
            {
                return BadRequest();
            }

            db.Entry(reise).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReiseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Reise
        [ResponseType(typeof(Reise))]
        public IHttpActionResult PostReise(Reise reise)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
                
            }

            db.Reiser.Add(reise);
            db.SaveChanges();
            db.Entry(reise).GetDatabaseValues();
            return CreatedAtRoute("DefaultApi", new { id = reise.Id }, reise);
          
        }

        // DELETE: api/Reise/5
        [ResponseType(typeof(Reise))]
        public IHttpActionResult DeleteReise(int id)
        {
            Reise reise = db.Reiser.Find(id);
            if (reise == null)
            {
                return NotFound();
            }

            db.Reiser.Remove(reise);
            db.SaveChanges();

            return Ok(reise);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ReiseExists(int id)
        {
            return db.Reiser.Count(e => e.Id == id) > 0;
        }
    }
}