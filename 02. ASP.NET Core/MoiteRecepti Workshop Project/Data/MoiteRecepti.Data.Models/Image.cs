﻿namespace MoiteRecepti.Data.Models
{
    using System;

    using MoiteRecepti.Data.Common.Models;

    public class Image : BaseModel<string>
    {
        public Image()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public int RecipeId { get; set; }

        public virtual Recipe Recipe { get; set; }

        public string AddedByUserId { get; set; }

        public ApplicationUser AddedByUser { get; set; }

        // The content of the image is in the file system
        public string Extension { get; set; }

        public string Url { get; set; }
    }
}
