using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPCISEvent.Fastnt.SBDH
{
    /// <summary>
    /// The SBDH must have partners as either senders or receivers
    /// </summary>
    public enum PartnerType
    {
        Sender,
        Receiver
    }

    /// <summary>
    /// PartnerIdentification as defined in the GS1 SBDH schema
    /// </summary>
    public class PartnerIdentification
    {
        public string Authority { get; set; }
        public string Value { get; set; }

        public PartnerIdentification(string authority, string value)
        {
            Authority = authority;
            Value = value;
        }
    }

    /// <summary>
    /// Partner represents the partner as defined in the GS1 SBDH schema
    /// </summary>
    public class Partner
    {
        public PartnerType PartnerType { get; set; }
        public PartnerIdentification PartnerId { get; set; }
        public string Contact { get; set; }
        public string EmailAddress { get; set; }
        public string FaxNumber { get; set; }
        public string TelephoneNumber { get; set; }
        public string ContactTypeIdentifier { get; set; }

        public Partner()
        {
        }

        public Partner(
            PartnerType partnerType,
            PartnerIdentification partnerId = null,
            string contact = null,
            string emailAddress = null,
            string faxNumber = null,
            string telephoneNumber = null,
            string contactTypeIdentifier = null)
        {
            PartnerType = partnerType;
            PartnerId = partnerId;
            Contact = contact;
            EmailAddress = emailAddress;
            FaxNumber = faxNumber;
            TelephoneNumber = telephoneNumber;
            ContactTypeIdentifier = contactTypeIdentifier;
        }

        public bool HasContactInfo =>
            !string.IsNullOrEmpty(Contact) ||
            !string.IsNullOrEmpty(EmailAddress) ||
            !string.IsNullOrEmpty(FaxNumber) ||
            !string.IsNullOrEmpty(TelephoneNumber) ||
            !string.IsNullOrEmpty(ContactTypeIdentifier);
    }

    /// <summary>
    /// Document types as defined in the EPCIS 1.2 standard
    /// </summary>
    public enum DocumentType
    {
        Events,
        MasterData,
        QueryControlRequest,
        QueryControlResponse,
        QueryCallBack,
        Query
    }

    /// <summary>
    /// DocumentIdentification as defined by the SBDH GS1 standard
    /// </summary>
    public class DocumentIdentification
    {
        public string Standard { get; set; }
        public string TypeVersion { get; set; }
        public string InstanceIdentifier { get; set; }
        public DocumentType DocumentType { get; set; }
        public bool? MultipleType { get; set; }
        public DateTime? CreationDateAndTime { get; set; }

        public DocumentIdentification(
            string standard = "EPCglobal",
            string typeVersion = "1.0",
            string instanceIdentifier = null,
            DocumentType documentType = DocumentType.Events,
            bool? multipleType = null,
            DateTime? creationDateAndTime = null)
        {
            Standard = standard;
            TypeVersion = typeVersion;
            InstanceIdentifier = instanceIdentifier ?? Guid.NewGuid().ToString();
            DocumentType = documentType;
            MultipleType = multipleType;
            CreationDateAndTime = creationDateAndTime;
        }

        public string GetCreationDateString()
        {
            return CreationDateAndTime?.ToString("O");
        }

        public string GetMultipleTypeString()
        {
            return MultipleType?.ToString().ToLower();
        }
    }

    /// <summary>
    /// The SBDH header as defined in the GS1 protocol
    /// </summary>
    public class StandardBusinessDocumentHeader
    {
        public string HeaderVersion { get; set; }
        public string Namespace { get; set; }
        public string SchemaLocation { get; set; }
        public DocumentIdentification DocumentIdentification { get; set; }
        public List<Partner> Partners { get; set; }

        public StandardBusinessDocumentHeader(
            string @namespace = "sbdh",
            string schemaLocation = "http://www.unece.org/cefact/namespaces/StandardBusinessDocumentHeader",
            DocumentIdentification documentIdentification = null,
            List<Partner> partners = null,
            string headerVersion = "1.0")
        {
            Namespace = @namespace;
            SchemaLocation = schemaLocation;
            DocumentIdentification = documentIdentification ?? new DocumentIdentification();
            Partners = partners ?? new List<Partner>();
            HeaderVersion = headerVersion;
        }
    }
}
