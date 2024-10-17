﻿using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DTOs.Post
{
    public class PostCreateDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int UserId { get; set; }
        public IFormFile? ImageFile { get; set; }
        public int ImageId { get; set; }
        [Required]
        public List<string> Tags { get; set; }
    }
}
