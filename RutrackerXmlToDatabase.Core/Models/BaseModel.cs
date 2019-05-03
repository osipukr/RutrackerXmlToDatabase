using System;

namespace RutrackerXmlToDatabase.Core.Models
{
    public class BaseModel<TPrimaryKey> where TPrimaryKey : IEquatable<TPrimaryKey>
    {
        public TPrimaryKey Id { get; set; }
    }
}