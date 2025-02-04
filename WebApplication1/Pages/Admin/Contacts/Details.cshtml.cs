using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Pages.Admin.Contacts
{
    [Authorize(Roles = "admin")]
    public class DetailsModel : PageModel
    {
        private readonly IWebHostEnvironment environment;
        private readonly ApplicationDbContext context;

        public Contact ContactProperty { get; set; } = new Contact();

        public DetailsModel(IWebHostEnvironment environment, ApplicationDbContext context)
        {
            this.environment = environment;
            this.context = context;
        }
        public void OnGet(int? id)
        {
            if (id == null)
            {
                Response.Redirect("/Admin/Contacts/Index");
                return;
            }

            var contact = context.Contacts.Include(c => c.Attachments).FirstOrDefault(c => c.Id == id);
            if(contact == null)
            {
                Response.Redirect("/Admin/Contacts/Index");
                return;
            }
            ContactProperty = contact;
        }

        //For attachment handler
        public ActionResult OnGetAttachment(int? attachmentId)
        {
            if(attachmentId == null)
            {
                return NotFound();
            }

            var attachment = context.Attachments.Find(attachmentId);
            if(attachment == null)
            {
                return NotFound();
            }

            var path = environment.ContentRootPath + "/Storage/Attachments/";
            var fullPath = path + attachment.StorageFileName;
            if(System.IO.File.Exists(fullPath))
            {
                //Read file data into byte array
                byte[] bytes = System.IO.File.ReadAllBytes(fullPath);

                //Send file to download
                return File(bytes, "application/octet-stream", attachment.OriginalFileName);
            }
            return NotFound();
        }

    }
}
