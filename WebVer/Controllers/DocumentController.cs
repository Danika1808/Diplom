using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VerificationCenter.VerificationCenterServices;
using WebVer.Domain.Blockchain;
using WebVer.Domain.Documents;
using WebVer.Models;

namespace WebVer.Controllers
{
    [Authorize(Roles = "Админ")]
    public class DocumentController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IVerificationCenterService _verificationCenterService;
        public DocumentController(ApplicationDbContext applicationDbContext, IVerificationCenterService verificationCenterService)
        {
            _applicationDbContext = applicationDbContext;
            _verificationCenterService = verificationCenterService;
        }

        public IActionResult AddDocument()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddDocument(AddDocumentViewModel model)
        {
            var user = _applicationDbContext.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);

            byte[] bytes;

            using (var ms = new MemoryStream())
            {
                model.File.CopyTo(ms);
                bytes = ms.ToArray();
            }

            var document = new Document()
            {
                Id = Guid.NewGuid(),
                Data = bytes,
                Description = model.Description,
                Name = model.Name,
                UserId = user.Id
            };

            _applicationDbContext.Documents.Add(document);

            _applicationDbContext.SaveChanges();

            return Redirect(nameof(Documents));
        }

        public IActionResult Documents()
        {
            var documents = _applicationDbContext.Documents
                .Include(x => x.User)
                .Select(x => new DocumentViewModel()
            {
                Id = x.Id,
                Description = x.Description,
                Name = x.Name,
                Author = CreateFullName(x.User.FirstName, x.User.LastName, x.User.Patronymic)
                }).ToList();

            return View(documents);
        }

        public IActionResult AppointSigner()
        {
            var users = _applicationDbContext.Users.ToList();
            var documents = _applicationDbContext.Documents.ToList();

            var result = new AppointSignerViewModel()
            {
                Users = new SelectList(users, nameof(Domain.Identity.User.Id), nameof(Domain.Identity.User.FirstName)),
                Documents = new SelectList(documents, nameof(Document.Id), nameof(Document.Name))
            };

            return View(result);
        }

        [HttpPost]
        public IActionResult AppointSigner(CreateAppointSignerViewModel model)
        {
            var user = _applicationDbContext.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);

            var appointSingerDocument = new AppointSingerDocument()
            {
                DocumentId = model.DocumentId,
                SignerId = model.UserId
            };

            _applicationDbContext.AppointSingerDocuments.Add(appointSingerDocument);

            var transactionDescription = new TransactionDescription()
            {
                Action = TransactionAction.AppointSigner,
                DocumentId = model.DocumentId,
                SubjectId = model.UserId
            };

            var transaction = new Transaction()
            {
                CreatedDate = DateTime.Now,
                Description = transactionDescription,
                IssuerId = user.Id
            };

            _applicationDbContext.Transactions.Add(transaction);

            _applicationDbContext.SaveChanges();

            return Redirect(nameof(Documents));
        }

        public IActionResult SignDocument()
        {
            var user = _applicationDbContext.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);

            var documents = _applicationDbContext
                .AppointSingerDocuments.Where(x => x.SignerId == user.Id).Select(x => x.Document)
                .ToList();

            var view = new SignDocumentViewModel()
            {
                AppointSingerDocuments = new SelectList(documents, nameof(Document.Id), nameof(Document.Name))
            };

            return View(view);
        }

        [HttpPost]
        public IActionResult SignDocument(CreateSignDocumentViewModel model)
        {
            var user = _applicationDbContext.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);

            var document = _applicationDbContext.Documents.FirstOrDefault(x => x.Id == model.DocumentId);

            var fullname = CreateFullName(user.FirstName, user.LastName, user.Patronymic);

            var searchString =
                _verificationCenterService.GetSearchString(fullname, user.Inn);

            var signedDocumentResult = _verificationCenterService.SignPdfDocument(model.Password, document.Data, searchString);

            var signedDocument = new Document()
            {
                Data = signedDocumentResult,
                Description = $"Подписанный документ пользователем - {fullname}",
                Name = $"signed-document-{document.Name}",
                UserId = user.Id
            };

            _applicationDbContext.Documents.Add(signedDocument);

            var transactionDescription = new TransactionDescription()
            {
                Action = TransactionAction.SignDocument,
                DocumentId = model.DocumentId,
                SubjectId = user.Id
            };

            var transaction = new Transaction()
            {
                CreatedDate = DateTime.Now,
                Description = transactionDescription,
                IssuerId = user.Id
            };

            _applicationDbContext.Transactions.Add(transaction);

            _applicationDbContext.SaveChanges();

            return Redirect(nameof(Documents));
        }

        private static string CreateFullName(string firstName, string lastName, string patronymic)
        {
            return firstName + " " + lastName + " " + patronymic;
        }

        public FileResult Download([FromRoute] Guid id)
        {
            var document = _applicationDbContext.Documents.FirstOrDefault(x => x.Id == id);

            return File(document.Data, "application/pdf", $"{document.Name}.pdf");
        }
    }
}
