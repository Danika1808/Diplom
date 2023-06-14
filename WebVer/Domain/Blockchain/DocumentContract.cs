using WebVer.Domain.Documents;
using WebVer.Domain.Identity;

namespace WebVer.Domain.Blockchain
{
    public class DocumentSignContract : SmartContract
    {
        public Guid IssuerId { get; private set; }

        public User Issuer { get; private set; }

        public Guid SignerId { get; private set; }

        public User Signer { get; private set; }

        public DocumentSignContract(Guid issuerId)
        {
            IssuerId = issuerId;
        }

        public bool SetSigner(Guid signerId, Event @event)
        {
            if (!IsCompleted) return false;

            if (SignerId != Guid.Empty) return false;

            SignerId = signerId;
            Events.Add(@event);

            return true;
        }

        public bool ApproveToSign(Guid signerId, Guid issuerId)
        {
            if (!IsCompleted) return false;

            if (SignerId != signerId) return false;

            if (SignerId != issuerId) return false;

            return Events.Any(x => x.Description.Action == EventAction.AppointSigner);
        }

        public Document? SignDocument(byte[] signedDocumentResult, Document document, Event @event)
        {
            var signedDocument = new Document()
            {
                Data = signedDocumentResult,
                Description = $"Подписанный документ пользователем",
                Name = $"signed-document-{document.Name}",
                IssuerId = IssuerId,
                SignerId = SignerId,
                IsSigned = true
            };

            Events.Add(@event);

            IsCompleted = true;

            return signedDocument;
        }
    }
}
