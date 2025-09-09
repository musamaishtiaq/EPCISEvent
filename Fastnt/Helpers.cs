using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EPCISEvent.Fastnt
{
    public static class EPCISUtilities
    {
        private static readonly Regex _iso8601Regex = new Regex(
            @"^(\d{4}-[01]\d-[0-3]\dT[0-2]\d:[0-5]\d:[0-5]\d\.\d+)|" +
            @"(\d{4}-[01]\d-[0-3]\dT[0-2]\d:[0-5]\d:[0-5]\d)|" +
            @"(\d{4}-[01]\d-[0-3]\dT[0-2]\d:[0-5]\d)$",
            RegexOptions.Compiled);

        /// <summary>
        /// Returns a compiled ISO 8601 regex for use in validation of date strings
        /// </summary>
        public static Regex GetIso8601Regex()
        {
            return _iso8601Regex;
        }

        /// <summary>
        /// Generates SGTIN URNs for the list of serial numbers
        /// </summary>
        /// <param name="companyPrefix">The company prefix (GS1)</param>
        /// <param name="indicator">The GS1 indicator digit for the GTIN</param>
        /// <param name="itemReference">The item reference number for the GTIN</param>
        /// <param name="serialNumbers">A list of serial numbers</param>
        /// <returns>Enumerable of SGTIN URN strings</returns>
        public static IEnumerable<string> GenerateGtinUrns(
            string companyPrefix,
            string indicator,
            string itemReference,
            IEnumerable<int> serialNumbers)
        {
            if (indicator.Length > 1)
            {
                throw new ArgumentException("The indicator may only be one digit in length.");
            }

            if (companyPrefix.Length + indicator.Length + itemReference.Length != 13)
            {
                throw new ArgumentException(
                    "The combined length of the company prefix, indicator digit and " +
                    "item reference number must be 13.");
            }

            string prefix = $"urn:epc:id:sgtin:{companyPrefix}.{indicator}{itemReference}.";

            foreach (var serialNumber in serialNumbers)
            {
                yield return prefix + serialNumber.ToString();
            }
        }

        /// <summary>
        /// Generates SSCC URNs for the list of serial numbers
        /// </summary>
        /// <param name="companyPrefix">The company prefix (GS1)</param>
        /// <param name="extension">The extension digit</param>
        /// <param name="serialNumbers">The serial reference numbers</param>
        /// <returns>Enumerable of SSCC URN strings</returns>
        public static IEnumerable<string> GenerateSsccUrns(
            string companyPrefix,
            string extension,
            IEnumerable<object> serialNumbers)
        {
            const int ssccLength = 17;
            string prefix = $"urn:epc:id:sscc:{companyPrefix}.{extension}";

            foreach (var serialNumber in serialNumbers)
            {
                string serialStr = serialNumber.ToString();
                int actualLength = companyPrefix.Length + extension.Length + serialStr.Length;

                if (actualLength > ssccLength)
                {
                    throw new ArgumentException(
                        "The combined length of the company prefix, extension digit " +
                        "and serial number must be 17 or less.");
                }

                string padding = new string('0', ssccLength - actualLength);
                yield return prefix + padding + serialStr;
            }
        }

        /// <summary>
        /// Creates a single SGTIN URN
        /// </summary>
        /// <param name="companyPrefix">The company prefix (GS1)</param>
        /// <param name="indicator">The GS1 indicator digit</param>
        /// <param name="itemReference">The item reference number</param>
        /// <param name="serialNumber">The serial number</param>
        /// <returns>SGTIN URN string</returns>
        public static string GtinToUrn(
            string companyPrefix,
            string indicator,
            string itemReference,
            string serialNumber)
        {
            return $"urn:epc:id:sgtin:{companyPrefix}.{indicator}{itemReference}.{serialNumber}";
        }

        /// <summary>
        /// Gets the current UTC time and offset
        /// </summary>
        /// <returns>Tuple with ISO string and timezone offset</returns>
        public static (string IsoString, string TimezoneOffset) GetCurrentUtcTimeAndOffset()
        {
            var now = DateTime.UtcNow.ToString("o");
            return (now, now.Substring(now.Length - 6));
        }

        /// <summary>
        /// Converts GLN13 data to SGLN URN
        /// </summary>
        /// <param name="companyPrefix">The company prefix</param>
        /// <param name="locationReference">The location reference</param>
        /// <param name="extension">The extension (default "0")</param>
        /// <returns>SGLN URN string</returns>
        public static string Gln13ToSglnUrn(
            string companyPrefix,
            string locationReference,
            string extension = "0")
        {
            if (companyPrefix.Length + locationReference.Length != 12)
            {
                throw new ArgumentException(
                    "The company prefix and location reference variables " +
                    "must total 12 digits in length when combined.");
            }

            return $"urn:epc:id:sgln:{companyPrefix}.{locationReference}.{extension}";
        }

        //public static string GlnTypeToSdtUrn(string type)
        //{
        //    return $"urn:epcglobal:cbv:sdt:{type}";
        //}

        //public static string BusinessStepToBizStepUrn(string step)
        //{
        //    return $"urn:epcglobal:cbv:bizstep:{step}";
        //}
        //public static string DispositionToDispUrn(string disposition)
        //{
        //    return $"urn:epcglobal:cbv:disp:{disposition}";
        //}

        public static string BusinessTransactionToBtUrn(string transaction)
        {
            return $"urn:epcglobal:cbv:bt:{transaction}";
        }
        //public static string BusinessTransactionTypeToBttUrn(string transaction)
        //{
        //    return $"urn:epcglobal:cbv:btt:{transaction}";
        //}
    }
}
