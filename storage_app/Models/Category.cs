﻿namespace storage_app.Models
{
    internal class Category
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;

        public override string ToString()
        {
            return this.Description;
        }
    }
}
