using System;
using System.Collections.Generic;

namespace EPCISEvent.Interfaces.DTOs
{
    public class VocabularyDto
    {
        public string Type { get; set; }
        public List<VocabularyElementDto> VocabularyElementList { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}
