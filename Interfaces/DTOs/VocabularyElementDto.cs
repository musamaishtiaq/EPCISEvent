using System.Collections.Generic;

namespace EPCISEvent.Interfaces.DTOs
{
    public class VocabularyElementDto
    {
        public string Id { get; set; }
        public List<VocabularyAttributeDto> Attributes { get; set; }
    }
}
