using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Pages
{
    public class ContactUsModel : PageModel
    {
        [BindProperty]
        public ContactDto ContactDtoProperty { get; set; } = new ContactDto();

        public List<SelectListItem> SubjectList = new List<SelectListItem>()
        {
            new SelectListItem { Value = "Order Status", Text = "Order Status" },
            new SelectListItem { Value = "Refund Request", Text = "Refund Request"},
            new SelectListItem { Value = "Job Application", Text = "Job Application"},
            new SelectListItem { Value = "Other", Text = "Other"},
        };

        private readonly IWebHostEnvironment environment;
        private readonly ApplicationDbContext context;
        private readonly string senderEmail;
        private readonly string senderName;

        public ContactUsModel(IWebHostEnvironment environment, ApplicationDbContext context, IConfiguration configuration)
        {
            this.environment = environment;
            this.context = context;
            this.senderEmail = configuration["BrevoApi:SenderEmail"]!;
            this.senderName = configuration["BrevoApi:SenderName"]!;
        }

        public void OnGet()
        {
        }

        public string errorMessage = "";
        public string successMessage = "";
        public void OnPost()
        {
            if(!ModelState.IsValid)
            {
                errorMessage = "Please make sure all the required fields are filled in";
                return;
            }

            //Create a new contact
            var contact = new Contact()
            {
                FirstName = ContactDtoProperty.FirstName,
                LastName = ContactDtoProperty.LastName,
                Email = ContactDtoProperty.Email,
                Phone = ContactDtoProperty.Phone ?? "",
                Subject = ContactDtoProperty.Subject,
                Message = ContactDtoProperty.Message,
                CreatedAt = DateTime.Now,
                Attachments = new List<Attachment>(),
            };
            
            // Save attachments if there is any
            if (ContactDtoProperty.Attachments != null)
            {
                string path = environment.ContentRootPath + "/Storage/Attachments/";

                //Storage file name format: <GUID> + "-" + <number> + <extension>
                var guid = Guid.NewGuid();
                for(int i = 0; i < ContactDtoProperty.Attachments.Count; i++)
                {
                    var file = ContactDtoProperty.Attachments[i];
                    var storageFileName = guid + "-" + i + Path.GetExtension(file.FileName);
                    var fullFilePath = path + storageFileName;
                    using(var stream = System.IO.File.Create(fullFilePath))
                    {
                        file.CopyTo(stream);
                    }

                    var attachment = new Attachment()
                    {
                        OriginalFileName = file.FileName,
                        StorageFileName = storageFileName
                    };

                    contact.Attachments.Add(attachment);
                    
                }
            }

            context.Contacts.Add(contact);
            context.SaveChanges();

            successMessage = "Message sent successfully";

            //Send confirmation email
            string receiverEmail = ContactDtoProperty.Email;
            string receiverName = ContactDtoProperty.FirstName + " " + ContactDtoProperty.LastName;
            string subject = ContactDtoProperty.Subject;
            string message = "Dear " + receiverName + ",\n" +
                "We have received your message. Thank you for contacting us.\n" +
                "Our team will contact you soon.\n" +
                "Best regards\n\n" +
                "Your Message:\n" + ContactDtoProperty.Message;

            EmailSender.SendEmail(senderEmail, senderName, receiverEmail, receiverName, subject, message);

            //Clear the form
            ContactDtoProperty.FirstName = "";
            ContactDtoProperty.LastName = "";
            ContactDtoProperty.Email = "";
            ContactDtoProperty.Phone = "";
            ContactDtoProperty.Subject = "";
            ContactDtoProperty.Message = "";

            ModelState.Clear();
        }
    }
}
