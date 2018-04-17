﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using RTMS_API;

namespace RTMS_API.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using RTMS_API;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<SensorCode>("SensorCodes");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class SensorCodesController : ODataController
    {
        private OData_RTMSEntities db = new OData_RTMSEntities();

        // GET: odata/SensorCodes
        [EnableQuery]
        public IQueryable<SensorCode> GetSensorCodes()
        {
            List<SensorCode> codes = db.SensorCodes.ToList();
            return db.SensorCodes;
        }

        // GET: odata/SensorCodes(5)
        [EnableQuery]
        public SingleResult<SensorCode> GetSensorCode([FromODataUri] double key)
        {
            return SingleResult.Create(db.SensorCodes.Where(sensorCode => sensorCode.id == key));
        }

        // PUT: odata/SensorCodes(5)
        public async Task<IHttpActionResult> Put([FromODataUri] double key, Delta<SensorCode> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SensorCode sensorCode = await db.SensorCodes.FindAsync(key);
            if (sensorCode == null)
            {
                return NotFound();
            }

            patch.Put(sensorCode);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SensorCodeExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(sensorCode);
        }

        // POST: odata/SensorCodes
        public async Task<IHttpActionResult> Post(SensorCode sensorCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SensorCodes.Add(sensorCode);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SensorCodeExists(sensorCode.id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(sensorCode);
        }

        // PATCH: odata/SensorCodes(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] double key, Delta<SensorCode> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SensorCode sensorCode = await db.SensorCodes.FindAsync(key);
            if (sensorCode == null)
            {
                return NotFound();
            }

            patch.Patch(sensorCode);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SensorCodeExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(sensorCode);
        }

        // DELETE: odata/SensorCodes(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] double key)
        {
            SensorCode sensorCode = await db.SensorCodes.FindAsync(key);
            if (sensorCode == null)
            {
                return NotFound();
            }

            db.SensorCodes.Remove(sensorCode);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SensorCodeExists(double key)
        {
            return db.SensorCodes.Count(e => e.id == key) > 0;
        }
    }
}