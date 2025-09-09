using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPCISEvent.Fastnt.SBDH
{
    public interface ITemplateMixin
    {
        string Template { get; set; }
        List<string> Namespaces { get; }
        string Render();
        string RenderJson();
        string RenderPrettyJson(int indent = 4, bool sortKeys = false);
        Dictionary<string, object> RenderDict();
    }

    /// <summary>
    /// The SBDH header as defined in the GS1 protocol with template rendering support
    /// </summary>
    public class RenderStandardBusinessDocumentHeader : ITemplateMixin
    {
        private readonly ITemplateRenderer _templateRenderer;
        private Dictionary<string, object> _context;
        private bool _renderXmlDeclaration;

        private readonly StandardBusinessDocumentHeader _sbdHeader;
        public StandardBusinessDocumentHeader _header { get; } = new StandardBusinessDocumentHeader();

        //public StandardBusinessDocumentHeaderEncoder _encoder { get; } = new StandardBusinessDocumentHeaderEncoder();

        public RenderStandardBusinessDocumentHeader(
            string @namespace = "sbdh",
            string schemaLocation = "http://www.unece.org/cefact/namespaces/StandardBusinessDocumentHeader",
            DocumentIdentification documentIdentification = null,
            List<Partner> partners = null,
            string headerVersion = "1.0",
            ITemplateRenderer templateRenderer = null,
            bool renderXmlDeclaration = false)
        {
            _sbdHeader = new StandardBusinessDocumentHeader(@namespace, schemaLocation, documentIdentification, partners, headerVersion);
            _templateRenderer = templateRenderer ?? new DefaultTemplateRenderer();
            _renderXmlDeclaration = renderXmlDeclaration;
            _context = new Dictionary<string, object>
            {
                { "header", this },
                { "render_xml_declaration", _renderXmlDeclaration }
            };
        }

        public string Template { get; set; } = "epcis/sbdh.xml";

        public List<string> Namespaces => new List<string>();

        public string Render()
        {
            // Reset context with current values
            _context = new Dictionary<string, object>
            {
                { "header", this },
                { "render_xml_declaration", _renderXmlDeclaration }
            };

            // Use template renderer to generate output
            return _templateRenderer.Render(Template, _context);
        }

        public string RenderJson()
        {
            return JsonSerializer.Serialize(this);
        }

        public string RenderPrettyJson(int indent = 4, bool sortKeys = false)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                IgnoreNullValues = true
            };
            return JsonSerializer.Serialize(this, options);
        }

        public Dictionary<string, object> RenderDict()
        {
            var json = RenderJson();
            return JsonSerializer.Deserialize<Dictionary<string, object>>(json);
        }
    }

    public interface ITemplateRenderer
    {
        string Render(string templateName, Dictionary<string, object> context);
    }

    public class DefaultTemplateRenderer : ITemplateRenderer
    {
        public string Render(string templateName, Dictionary<string, object> context)
        {
            // In a real implementation, this would use a templating engine
            // like Razor, Scriban, or Handlebars.NET
            // For now, we'll just return JSON representation

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            return JsonSerializer.Serialize(context, options);
        }
    }
}
