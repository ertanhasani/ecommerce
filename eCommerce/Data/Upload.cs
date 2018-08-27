using System;
using System.Collections.Generic;

namespace eCommerce.Data
{
    public partial class Upload
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
        public int? ProductId { get; set; }
        public bool? IsThumbnail { get; set; }
        public bool? IsCarousel { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime? CreatedOnDate { get; set; }
        public string LasUpdatedByUserId { get; set; }
        public DateTime? LastUpdatedOnDate { get; set; }
        public bool? IsDeleted { get; set; }

        public Product Product { get; set; }
    }
}
