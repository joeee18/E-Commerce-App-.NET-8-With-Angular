﻿using ECom.Core.Entites.Product;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECom.Core.DTO
{
    public record ProductDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public ICollection<PhotoDTO>? Photos { get; set; }
        public string? CategoryName { get; set; }

    }

    public record PhotoDTO
    {
        public string? ImageName { get; set; }

        public int? ProductId { get; set; }

    }
    public record AddProductDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? NewPrice { get; set; }
        public decimal? OldPrice { get; set; }
        public int? CategoryId { get; set; }
        public IFormFileCollection? photo {  get; set; }
    }
    public record UpdateProductDTO : AddProductDTO
    {
        public int Id { get; set; }
    }
}
