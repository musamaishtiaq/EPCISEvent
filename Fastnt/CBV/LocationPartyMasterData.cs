using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EPCISEvent.Fastnt.CBV
{
    public interface ILocationAttributeName
    {
        string Value { get; }
    }

    public enum LocationPartyMasterDataAttributes
    {
        [EnumMember(Value = "urn:epcglobal:cbv:mda:site")]
        Site,

        [EnumMember(Value = "urn:epcglobal:cbv:mda:sst")]
        Sst,

        [EnumMember(Value = "urn:epcglobal:cbv:mda:ssa")]
        Ssa,

        [EnumMember(Value = "urn:epcglobal:cbv:mda:ssd")]
        Ssd,

        [EnumMember(Value = "urn:epcglobal:cbv:mda#name")]
        Name,

        [EnumMember(Value = "urn:epcglobal:cbv:mda#streetAddressOne")]
        StreetAddressOne,

        [EnumMember(Value = "urn:epcglobal:cbv:mda#streetAddressTwo")]
        StreetAddressTwo,

        [EnumMember(Value = "urn:epcglobal:cbv:mda#streetAddressThree")]
        StreetAddressThree,

        [EnumMember(Value = "urn:epcglobal:cbv:mda#city")]
        City,

        [EnumMember(Value = "urn:epcglobal:cbv:mda#state")]
        State,

        [EnumMember(Value = "urn:epcglobal:cbv:mda#postalCode")]
        PostalCode,

        [EnumMember(Value = "urn:epcglobal:cbv:mda#countryCode")]
        CountryCode,

        [EnumMember(Value = "urn:epcglobal:cbv:mda#latitude")]
        Latitude,

        [EnumMember(Value = "urn:epcglobal:cbv:mda#longitude")]
        Longitude
    }

    public enum SubSiteTypeMasterDataAttribute
    {
        [EnumMember(Value = "201")]
        Backroom,

        [EnumMember(Value = "202")]
        StorageArea,

        [EnumMember(Value = "203")]
        SalesFloor,

        [EnumMember(Value = "207")]
        ReturnsArea,

        [EnumMember(Value = "208")]
        ProductionArea,

        [EnumMember(Value = "209")]
        ReceivingArea,

        [EnumMember(Value = "210")]
        ShippingArea,

        [EnumMember(Value = "211")]
        SalesFloorTransitionArea,

        [EnumMember(Value = "212")]
        CustomerPickUpArea,

        [EnumMember(Value = "213")]
        Yard,

        [EnumMember(Value = "214")]
        ContainerDeck,

        [EnumMember(Value = "215")]
        CargoTerminal,

        [EnumMember(Value = "251")]
        PackagingArea,

        [EnumMember(Value = "252")]
        PickingArea,

        [EnumMember(Value = "253")]
        PharmacyArea,

        [EnumMember(Value = "299")]
        Undefined
    }

    public enum SubSiteAttributesMasterDataAttribute
    {
        [EnumMember(Value = "401")]
        Electronics,

        [EnumMember(Value = "402")]
        ColdStorage,

        [EnumMember(Value = "403")]
        Shelf,

        [EnumMember(Value = "404")]
        Frozen,

        [EnumMember(Value = "405")]
        Fresh,

        [EnumMember(Value = "406")]
        Promotion,

        [EnumMember(Value = "407")]
        EndCap,

        [EnumMember(Value = "408")]
        PointOfSale,

        [EnumMember(Value = "409")]
        Security,

        [EnumMember(Value = "411")]
        GeneralMdse,

        [EnumMember(Value = "412")]
        Grocery,

        [EnumMember(Value = "413")]
        BoxCrusher,

        [EnumMember(Value = "414")]
        DockDoor,

        [EnumMember(Value = "415")]
        ConveyorBelt,

        [EnumMember(Value = "416")]
        PalletWrapper,

        [EnumMember(Value = "417")]
        FixedReader,

        [EnumMember(Value = "418")]
        MobileReader,

        [EnumMember(Value = "419")]
        ShelfStorage,

        [EnumMember(Value = "420")]
        Returns,

        [EnumMember(Value = "421")]
        Staging,

        [EnumMember(Value = "422")]
        Assembly,

        [EnumMember(Value = "423")]
        LayAway,

        [EnumMember(Value = "424")]
        Dispenser,

        [EnumMember(Value = "425")]
        Quarantine,

        [EnumMember(Value = "426")]
        ControlledSubstance,

        [EnumMember(Value = "427")]
        RecalledProduct,

        [EnumMember(Value = "428")]
        QualityControl,

        [EnumMember(Value = "429")]
        PrintingRoom,

        [EnumMember(Value = "430")]
        LoadingDock,

        [EnumMember(Value = "431")]
        EntranceGate,

        [EnumMember(Value = "432")]
        ExitGate,

        [EnumMember(Value = "433")]
        Gate,

        [EnumMember(Value = "434")]
        ReadPointVerificationSpot
    }

    public static class LocationExtensions
    {
        public static string ToValueString(this ILocationAttributeName attribute)
        {
            var enumType = attribute.GetType();
            var memberInfo = enumType.GetMember(attribute.ToString());
            var attributeValue = memberInfo[0]
                .GetCustomAttributes(typeof(EnumMemberAttribute), false)
                .Cast<EnumMemberAttribute>()
                .FirstOrDefault()?
                .Value;

            return attributeValue ?? attribute.ToString();
        }
    }
}
