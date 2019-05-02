using System;

namespace RutrackerXmlToDatabase.Core.Models
{
    public class BaseModel<T> where T : IEquatable<T>
    {
        public T Id { get; set; }
    }
}