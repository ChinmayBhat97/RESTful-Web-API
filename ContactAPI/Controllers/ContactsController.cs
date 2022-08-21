using ContactAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ContactAPI.Controllers
{
    [ApiController]
    [Route("api/Contacts")]
    public class ContactsController : Controller
    {
        private readonly ContactDbContext dbContext;

        public ContactsController(ContactDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
       
        [HttpGet]
        public async Task<IActionResult> GetContacts()
        {
          return Ok(dbContext.Contacts.ToList());
            
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetContact([FromRoute] Guid id)
        {
            var contact = await dbContext.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }

            return Ok (contact);

        }

        [HttpPost]
        public async Task<IActionResult> AddContact(AddContactRequest addContactRequest)
        {
            var contact = new Contact()
            {
                Id = Guid.NewGuid(),
                Address = addContactRequest.Address,
                Email = addContactRequest.Email,
                FullName = addContactRequest.FullName,
                PhoneNumber = addContactRequest.PhoneNumber,

            };
            await dbContext.Contacts.AddAsync(contact);
            await dbContext.SaveChangesAsync();
            return Ok(contact);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateContact([FromRoute] Guid id, UpdateContactRequest updateContactRequest)
        {
            var contact = await dbContext.Contacts.FindAsync(id);
            if (contact!=null)
            {
                contact.FullName = updateContactRequest.FullName;
                contact.Email = updateContactRequest.Email;
                contact.PhoneNumber=updateContactRequest.PhoneNumber;
                contact.Address = updateContactRequest.Address;

                await dbContext.SaveChangesAsync();
                return Ok(contact);
            }
            return NotFound();

        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteContact([FromRoute] Guid id)
        {
            var contact = await dbContext.Contacts.FindAsync(id);

            if(contact!= null)
            {
                dbContext.Remove(contact);
               await dbContext.SaveChangesAsync();
                return Ok(contact);
            }
            return NotFound();
        }

    }
}
