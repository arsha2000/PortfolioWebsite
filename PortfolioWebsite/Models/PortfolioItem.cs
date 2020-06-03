using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace PortfolioWebsite.Models
{
    public struct PortfolioItem : IEquatable<PortfolioItem>
    { 
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public string RepositoryUrl { get; set; }

        public bool Equals([AllowNull] PortfolioItem other)
        {
            return ID == other.ID;
        }

    }   
}

