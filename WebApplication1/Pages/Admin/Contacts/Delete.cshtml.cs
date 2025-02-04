using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Services;

namespace WebApplication1.Pages.Admin.Contacts
{
    [Authorize(Roles = "admin")]
    public class DeleteModel : PageModel
    {
        private readonly IWebHostEnvironment environment;
        private readonly ApplicationDbContext context;

        public DeleteModel(IWebHostEnvironment environment, ApplicationDbContext context)
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
            if (contact == null)
            {
                Response.Redirect("/Admin/Contacts/Index");
                return;
            }

            //Delete attachment files
            var path = environment.ContentRootPath + "/Storage/Attachments/";
            foreach (var attachment in contact.Attachments)
            {
                var fullFilePath = path + attachment.StorageFileName;
                System.IO.File.Delete(fullFilePath);
                context.Attachments.Remove(attachment);
            }
            context.Contacts.Remove(contact);
            context.SaveChanges();

            Response.Redirect("/Admin/Contacts/Index");
        }
    }
}
