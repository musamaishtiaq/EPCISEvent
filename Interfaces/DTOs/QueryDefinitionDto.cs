using System;

namespace EPCISEvent.Interfaces.DTOs
{
    public class QueryDefinitionDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public QueryParameters Parameters { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
