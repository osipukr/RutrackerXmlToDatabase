using System;

namespace RutrackerXmlToDatabase.Core.Models
{
    public abstract class BaseModel<TPrimaryKey> 
        where TPrimaryKey : IEquatable<TPrimaryKey>
    {
        public TPrimaryKey Id { get; set; }
    }
}