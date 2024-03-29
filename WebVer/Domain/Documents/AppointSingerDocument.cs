﻿using WebVer.Domain.Identity;

namespace WebVer.Domain.Documents
{
    public class AppointSingerDocument
    {
        public Guid Id { get; set; }
        public Guid DocumentId { get; set; }
        public Document Document { get; set; }
        public Guid SignerId { get; set; }
        public User Signer { get; set; }
    }
}
