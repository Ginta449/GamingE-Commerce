using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GamingE_Commerce.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string GameName { get; set; }
        public string GameDescription { get; set; }
        public decimal GamePrice { get; set; }
        public DateTime GameReleased { get; set; }
        public string GameImageName { get; set; }
        public Platform Platform { get; set; }
        public int PlatformId { get; set; }
        public Publisher Publisher { get; set; }
        public int PublisherId { get; set; }
        public UserReviews UserReview { get; set; }
        public int UserReviewId { get; set; }
        public string VideoLinkName { get; set; }
        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile ImageFile { get; set; }
        public string Search { get; set; }
    }
}
