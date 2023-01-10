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
using Service.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Routing;

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
            return await _context.Profiles.Include(p => p.Items.OrderByDescending(i => i.ItemAdded))
                .Include(p => p.Address).SingleOrDefaultAsync(p => p.Auth0Id.Equals(userId));
        }

        // GET: api/Profiles/<userId>
        [HttpGet("{id}")]
        public async Task<ActionResult<Profile?>> GetProfile(int id)
        {
            var profile = await _context.Profiles.Include(p => p.Items.OrderByDescending(i => i.ItemAdded))
                .Include(p => p.Address)
                .SingleOrDefaultAsync(p => p.Id == id);

            if (profile == null)
            {
                return NotFound("Profile doesn't exist");
            }

            return profile;
        }

        // PUT: api/Profiles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<Profile>> UpdateProfile(int id, Profile profile)
        {
          
            var dbProfile = _context.Profiles.Include(p => p.Address)
            .SingleOrDefault(p => p.Id == id);
            if (dbProfile == null) return BadRequest("Profile not found");

            dbProfile.Auth0Id = profile.Auth0Id;
            dbProfile.Id = id;
            dbProfile.Email = profile.Email;
            dbProfile.Phone = profile.Phone;
            dbProfile.Address = new Address()
            {
                Street = profile.Address.Street,
                City = profile.Address.City,
                ZipCode = profile.Address.ZipCode,
            };

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
                Console.WriteLine("profiili päivitetty");
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

            LocationService locService = new(_configuration.GetValue<string>("GoogleMapsApiKey"));
            LocationObject? locObj = await locService.GetLocationObject(street, city, zipCode);

            if (locObj == null)
            {
                return NotFound("Paikkatietojen haku epäonnistui");
            }
            System.Diagnostics.Debug.WriteLine(JsonSerializer.Serialize(locObj));

            if (locObj.status != "OK")
            {
                return NotFound("Osoitetta ei löytynyt");
            }

            Profile newProfile = new()
            {
                Auth0Id = profile.Auth0Id,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                Phone = profile.Phone,
                Address = address,
                Email = profile.Email,
                ImageUrl = profile.ImageUrl,
                Latitude = locObj.results[0].geometry.location.lat,
                Longitude = locObj.results[0].geometry.location.lng
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
                return NotFound("Profile not found");
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