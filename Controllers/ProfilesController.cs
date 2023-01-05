using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NeighborGoodAPI.Models;
using Service;
using System.IO;
using System.Reflection.Emit;

namespace NeighborGoodAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly NGDbContext _context;
        private readonly IConfiguration _configuration;
        public ProfilesController(NGDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/Profiles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Profile>>> GetProfiles()
        {
            return await _context.Profiles.ToListAsync();
        }

        [HttpGet("userByAuthId/{userId}")]
        public async Task<Profile?> GetProfileByAuthId(string userId)
        {
            return await _context.Profiles.Include(p => p.Items).SingleOrDefaultAsync(p => p.Auth0Id.Equals(userId));
        }

        // GET: api/Profiles/<userId>
        [HttpGet("{id}")]
        public async Task<ActionResult<Profile?>> GetProfile(int id)
        {
            var profile = await _context.Profiles.Include(p => p.Items).SingleOrDefaultAsync(p => p.Id == id);

            if (profile == null)
            {
                return NotFound("Profile doesn't exist");
            }

            return profile;
        }

        // PUT: api/Profiles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProfile(int id, Profile profile)
        {
            if (id != profile.Id)
            {
                return BadRequest();
            }

            _context.Entry(profile).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfileExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Profiles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Profile>> PostProfile(Profile profile)
        {
            string street = profile.Address.Street;
            string city = profile.Address.City;
            string zipCode = profile.Address.ZipCode;

            Address address = new Address()
            {
                Street = street,
                City = city,
                ZipCode = zipCode
            };

            LocationService locObj = new(_configuration.GetValue<string>("GoogleMapsApiKey"));
            (double lat, double lng)? loc = await locObj.GetLatitudeLongitudeAsync(street, city, zipCode);

            if (loc == null)
            {
                return NotFound("Paikkatietojen haku epäonnistui");
            }

            Profile newProfile = new()
            {
                Auth0Id = profile.Auth0Id,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                Phone = profile.Phone,
                Address = address,
                Latitude = loc.Value.lat,
                Longitude = loc.Value.lng
            };



            _context.Profiles.Add(newProfile);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetProfile", new { id = newProfile.Id }, newProfile);
        }


        // DELETE: api/Profiles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfile(int id)
        {
            var profile = await _context.Profiles.FindAsync(id);
            if (profile == null)
            {
                return NotFound();
            }

            _context.Profiles.Remove(profile);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProfileExists(int id)
        {
            return _context.Profiles.Any(e => e.Id == id);
        }
    }
}
