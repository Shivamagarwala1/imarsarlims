using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

namespace iMARSARLIMS.Controllers
{
    //[Authorize]

    public abstract class BaseController<TDBEntity>: ControllerBase where TDBEntity : class
    {
        protected readonly ContextClass db;
        protected readonly ILogger<BaseController<TDBEntity>> logger;
        protected abstract IQueryable<TDBEntity> DbSet { get; }

        protected BaseController(ContextClass context, ILogger<BaseController<TDBEntity>> logger)
        {
            db = context;
            this.logger = logger;
        }
        [EnableQuery]
        [HttpGet]
        public virtual IQueryable<TDBEntity> GetData()
        {
            return DbSet;
        }
        [HttpPost, EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All, MaxExpansionDepth = 5)]
        public virtual async Task<IActionResult> create([FromBody] TDBEntity entity)
        {
            if (entity == null)
            {
                return BadRequest("entity Can't be null");
            }
            db.Set<TDBEntity>().Add(entity);
            await db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetData), new { id = GetEntityId(entity) }, entity);
        }
        [HttpPut, EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All, MaxExpansionDepth = 5)]
        public virtual async Task<IActionResult> Update([FromBody] TDBEntity updatedEntity)
        {
            if (updatedEntity == null)
            {
                return BadRequest("Entity cannot be null");
            }

            // Get the entity's id (assuming it has a property named "id" and it's an int)
            var entityId = GetEntityId(updatedEntity);

            // Convert entityId to the correct type (int in this case)
            if (entityId == null || !EntityExists((int)entityId))
            {
                return BadRequest("Entity not found or invalid");
            }

            try
            {
                var existingEntity = await db.Set<TDBEntity>().FindAsync((int)entityId);

                if (existingEntity != null)
                {
                    db.Entry(existingEntity).State = EntityState.Detached; // Detach to avoid tracking issues
                }
                foreach (var property in db.Entry(updatedEntity).Properties)
                {
                    if (property.Metadata.Name != "id" && property.Metadata.Name != "createdById" && property.Metadata.Name != "createdDateTime" && property.CurrentValue != null)
                    {
                        property.IsModified = true;  // Mark the property as modified
                    }
                }
                // Attach and mark the updated entity as modified
                db.Entry(updatedEntity).State = EntityState.Modified;
                await db.SaveChangesAsync();
                var updatedDbEntity = await db.Set<TDBEntity>().FindAsync((int)entityId);

                if (updatedDbEntity == null)
                {
                    return NotFound();
                }

                // Return the updated entity
                return Ok(updatedDbEntity);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntityExists((int)entityId))
                {
                    return NotFound();
                }
                throw;
            }
        }




        // DELETE: api/[controller]/id - Delete an entity
        [HttpDelete("{id}"), EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All, MaxExpansionDepth = 5)]
        public virtual async Task<IActionResult> Delete(int id)
        {
            var entity = await db.Set<TDBEntity>().FindAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            db.Set<TDBEntity>().Remove(entity);
            await db.SaveChangesAsync();

            return NoContent();
        }

        // Helper method to check if an entity exists
        protected virtual bool EntityExists(int id)
        {
            return db.Set<TDBEntity>().Find(id) != null;
        }
        protected virtual object GetEntityId(TDBEntity entity)
        {
            return entity.GetType().GetProperty("id")?.GetValue(entity, null);
        }
    }

}
