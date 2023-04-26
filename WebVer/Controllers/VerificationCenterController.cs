using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VerificationCenter.Model;
using VerificationCenter.VerificationCenterServices;
using WebVer.Domain.VerificationCenter;
using WebVer.Models;

namespace WebVer.Controllers
{
    [Authorize]
    public class VerificationCenterController : Controller
    {
        private readonly IVerificationCenterService _verificationCenterService;
        private readonly ApplicationDbContext _applicationDbContext;
        public VerificationCenterController(IVerificationCenterService verificationCenterService, ApplicationDbContext applicationDbContext)
        {
            _verificationCenterService = verificationCenterService;
            _applicationDbContext = applicationDbContext;
        }

        public IActionResult Index()
        {
            return Redirect(nameof(CreateSelfSignedCertificate));
        }


        public IActionResult CreateIssueCertificate()
        {
            var selfSignedCertificateInfo = _applicationDbContext.CertificateInfo.FirstOrDefault(x => x.IsSelfSignedCertificate == true);

            if (selfSignedCertificateInfo == null)
            {
                return Redirect(nameof(CreateSelfSignedCertificate));
            }

            var user = _applicationDbContext.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);

            var fullname = CreateFullName(user.FirstName, user.LastName, user.Patronymic);

            var searchString =
                _verificationCenterService.GetSearchString(fullname, user.Inn);

            var certificate = _applicationDbContext.CertificateInfo.FirstOrDefault(x => x.SubjectName == searchString);

            return View(certificate);
        }

        public IActionResult CreateIssueCertificateForm()
        {
            return PartialView();
        }

        public IActionResult CreateSelfSignedCertificate()
        {
            var certificate = _applicationDbContext.CertificateInfo.FirstOrDefault(x => x.IsSelfSignedCertificate == true);

            return View(certificate);
        }

        public IActionResult CreateSelfSignedCertificateForm()
        {
            return PartialView();
        }

        [Authorize(Roles = "Админ")]
        [HttpPost]
        public IActionResult CreateSelfSignedCertificate(CreateSelfSignedCertificateViewModel model)
        {
            if (!ModelState.IsValid) return RedirectToAction(nameof(Index));

            var command = new CreateSelfSignedCertificateCommand()
            {
                OrganizationName = model.OrganizationName,
                OrganizationInn = model.OrganizationInn,
                Password = model.Password,
                ValidityPeriod = model.ValidityPeriod
            };

            var searchString =
                _verificationCenterService.GetSearchString(command.OrganizationName, command.OrganizationInn);

            var isExist = _applicationDbContext.CertificateInfo.Any(x => x.SubjectName == searchString);

            if (isExist)
            {
                return RedirectToAction(nameof(Index));
            }

            var certificate = _verificationCenterService.GenerateSelfSignedCertificate(command);

            var certificateInfo = new CertificateInfo()
            {
                Id = Guid.NewGuid(),
                SubjectName = certificate.SubjectName.Name,
                NotAfter = certificate.NotAfter,
                NotBefore = certificate.NotBefore,
                PublicKey = certificate.GetPublicKeyString(),
                SignatureAlgorithmName = certificate.SignatureAlgorithm.FriendlyName,
                IsSelfSignedCertificate = true
            };

            _applicationDbContext.CertificateInfo.Add(certificateInfo);

            _applicationDbContext.SaveChanges();

            return RedirectToAction(nameof(CreateSelfSignedCertificate));
        }

        [HttpPost]
        public IActionResult CreateIssueCertificate(CreateIssuerCertificateViewModel model)
        {
            if (!ModelState.IsValid) return RedirectToAction(nameof(Index));

            var user = _applicationDbContext.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);

            var command = new CreateIssuerCertificateCommand()
            {
                FullName = CreateFullName(user.FirstName, user.LastName, user.Patronymic),
                Inn = user.Inn,
                Password = model.Password
            };

            var searchString =
                _verificationCenterService.GetSearchString(command.FullName, command.Inn);

            var isExist = _applicationDbContext.CertificateInfo.Any(x => x.SubjectName == searchString);

            if (isExist)
            {
                return RedirectToAction(nameof(CreateIssueCertificate));
            }

            var selfSignedCertificateInfo = _applicationDbContext.CertificateInfo.FirstOrDefault(x => x.IsSelfSignedCertificate == true);

            var certificate = _verificationCenterService.GenerateIssueCertificate(selfSignedCertificateInfo.SubjectName, command);

            var certificateInfo = new CertificateInfo()
            {
                Id = Guid.NewGuid(),
                SubjectName = certificate.SubjectName.Name,
                NotAfter = certificate.NotAfter,
                NotBefore = certificate.NotBefore,
                PublicKey = certificate.GetPublicKeyString(),
                SignatureAlgorithmName = certificate.SignatureAlgorithm.FriendlyName,
                IsSelfSignedCertificate = true
            };

            _applicationDbContext.CertificateInfo.Add(certificateInfo);

            _applicationDbContext.SaveChanges();

            return RedirectToAction(nameof(CreateIssueCertificate));
        }

        private string CreateFullName(string firstName, string lastName, string patronymic)
        {
            return firstName + " " + lastName + " " + patronymic;
        }
    }
}
