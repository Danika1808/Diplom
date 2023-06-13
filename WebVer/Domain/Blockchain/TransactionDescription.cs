﻿using WebVer.Domain.Documents;
using WebVer.Domain.Identity;

namespace WebVer.Domain.Blockchain
{
    public class EventDescription
    {
        public Guid Id { get; set; }
        public Guid DocumentId { get; set; }
        public Document Document { get; set; }
        public Guid? SubjectId { get; set; }
        public User Subject { get; set; }
        public EventAction Action { get; set; }
    }
}
