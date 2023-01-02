﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NeighborGoodAPI.Models;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Azure;

namespace NeighborGoodAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly NGDbContext _context;
        private readonly IConfiguration _configuration;

        public ItemsController(NGDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/Items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> GetItems()
        {
            return await _context.Items.ToListAsync();
        }

        // GET: api/Items/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> GetItem(int id)
        {
            var item = await _context.Items.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        // PUT: api/Items/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem(int id, Item item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(id))
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

        // testausta varten, swaggerilla voi kokeilla että kuva menee Azureen
        [HttpPost("dummy")]
        public async Task<ActionResult<Item>> PostDummyItem(IFormFile file)
        {
            string fileName = $"{Guid.NewGuid()}_{file.FileName}";
            //Item item = new()
            //{
            //    Name = "Dummy name",
            //    Description = "Dummy description",
            //    Category = "Vasara"
            //};
            if (!await UploadImageToAzureAsync(file, fileName))
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                        new { message = "Failed to upload image to Azure :(" });
            }
            return NoContent();
            //_context.Items.Add(item);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetItem", new { id = item.Id }, item);
        }

        // POST: api/Items
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Item>> PostItem(IFormCollection formData)
        {
            var file = formData.Files.FirstOrDefault();
            string? fileName = null;

            if (file != null && IsImage(file))
            {
                fileName = $"{Guid.NewGuid()}_{file.FileName}";
                if (!(await UploadImageToAzureAsync(file, fileName)))
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new { message = "Failed to upload image to Azure :(" });
                }

            }

            var itemName = formData["itemName"].FirstOrDefault();
            var description = formData["description"].FirstOrDefault();
            var auth0Id = formData["userId"].FirstOrDefault();
            if (itemName == null)
            {
                return BadRequest("Item name missing");
            }
            if(auth0Id == null)
            {
                return BadRequest("User_id missing");

            }
            var owner = await _context.Profiles.SingleOrDefaultAsync(p => p.Auth0Id == auth0Id);
            if (owner == null)
            {
                return NotFound($"Cannot find user with user_id: {auth0Id}");
            }

            Item item = new()
            {
                Name = itemName,
                Owner = owner,
                Description = description,
                ImageName = fileName
            };

            _context.Items.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItem", new { id = item.Id }, item);
        }

        // DELETE: api/Items/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IsImage(IFormFile file)
        {
            // image/png, image/gif, image/jpeg
            if (file.ContentType != "image/png" && file.ContentType != "image/gif" && file.ContentType != "image/jpeg")
            {
                return false;
            }
            string fileExtension = Path.GetExtension(file.FileName);
            string[] allowedExtensions = new[] { ".png", ".jfif", ".pjpeg", ".jpeg", ".pjp", ".jpg" };
            if (!allowedExtensions.Contains(fileExtension))
            {
                return false;
            }
            return true;
        }

        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.Id == id);
        }

        private async Task<bool> UploadImageToAzureAsync(IFormFile file, string imageName)
        {
            try
            {
                BlobServiceClient serviceClient = new BlobServiceClient(_configuration.GetValue<string>("BlobConnectionString"));
                BlobContainerClient containerClient = serviceClient.GetBlobContainerClient(_configuration.GetValue<string>("BlobContainerName"));

                BlobClient blobClient = containerClient.GetBlobClient(imageName);

                await using (var data = file.OpenReadStream())
                {
                    var response = await blobClient.UploadAsync(data, new BlobUploadOptions { HttpHeaders = new BlobHttpHeaders { ContentType = file.ContentType } });
                    if (response.GetRawResponse().IsError)
                    {
                        return false;
                    }
                }
                BlobHttpHeaders headers = new()
                {
                    ContentType = file.ContentType
                };
                blobClient.SetHttpHeaders(headers);
            }
            catch (RequestFailedException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
            return true;
        }
    }
}