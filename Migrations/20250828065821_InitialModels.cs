using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EPCISEvent.Migrations
{
    /// <inheritdoc />
    public partial class InitialModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "MasterData");

            migrationBuilder.CreateTable(
                name: "CompanyTypes",
                schema: "MasterData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Identifier = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LocationTypes",
                schema: "MasterData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Identifier = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                schema: "MasterData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GS1CompanyPrefix = table.Column<string>(type: "text", nullable: true),
                    CompanyTypeId = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Address1 = table.Column<string>(type: "text", nullable: true),
                    Address2 = table.Column<string>(type: "text", nullable: true),
                    Address3 = table.Column<string>(type: "text", nullable: true),
                    Country = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: true),
                    StateProvince = table.Column<string>(type: "text", nullable: true),
                    PostalCode = table.Column<string>(type: "text", nullable: true),
                    Latitude = table.Column<decimal>(type: "numeric", nullable: true),
                    Longitude = table.Column<decimal>(type: "numeric", nullable: true),
                    GLN13 = table.Column<string>(type: "text", nullable: true),
                    SGLN = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Companies_CompanyTypes_CompanyTypeId",
                        column: x => x.CompanyTypeId,
                        principalSchema: "MasterData",
                        principalTable: "CompanyTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                schema: "MasterData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CompanyId = table.Column<int>(type: "integer", nullable: true),
                    SiteId = table.Column<int>(type: "integer", nullable: true),
                    LocationTypeId = table.Column<int>(type: "integer", nullable: true),
                    SST = table.Column<short>(type: "smallint", nullable: true),
                    SSA = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Address1 = table.Column<string>(type: "text", nullable: true),
                    Address2 = table.Column<string>(type: "text", nullable: true),
                    Address3 = table.Column<string>(type: "text", nullable: true),
                    Country = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: true),
                    StateProvince = table.Column<string>(type: "text", nullable: true),
                    PostalCode = table.Column<string>(type: "text", nullable: true),
                    Latitude = table.Column<decimal>(type: "numeric", nullable: true),
                    Longitude = table.Column<decimal>(type: "numeric", nullable: true),
                    GLN13 = table.Column<string>(type: "text", nullable: true),
                    SGLN = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Locations_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "MasterData",
                        principalTable: "Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Locations_LocationTypes_LocationTypeId",
                        column: x => x.LocationTypeId,
                        principalSchema: "MasterData",
                        principalTable: "LocationTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Locations_Locations_SiteId",
                        column: x => x.SiteId,
                        principalSchema: "MasterData",
                        principalTable: "Locations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TradeItems",
                schema: "MasterData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GTIN14 = table.Column<string>(type: "text", nullable: true),
                    NDC = table.Column<string>(type: "text", nullable: true),
                    NdcPattern = table.Column<string>(type: "text", nullable: true),
                    CompanyId = table.Column<int>(type: "integer", nullable: false),
                    AdditionalId = table.Column<string>(type: "text", nullable: true),
                    AdditionalIdTypeCode = table.Column<string>(type: "text", nullable: true),
                    DescriptionShort = table.Column<string>(type: "text", nullable: true),
                    DosageFormType = table.Column<string>(type: "text", nullable: true),
                    FunctionalName = table.Column<string>(type: "text", nullable: true),
                    ManufacturerName = table.Column<string>(type: "text", nullable: true),
                    NetContentDescription = table.Column<string>(type: "text", nullable: true),
                    LabelDescription = table.Column<string>(type: "text", nullable: true),
                    RegulatedProductName = table.Column<string>(type: "text", nullable: true),
                    StrengthDescription = table.Column<string>(type: "text", nullable: true),
                    TradeItemDescription = table.Column<string>(type: "text", nullable: true),
                    SerialNumberLength = table.Column<int>(type: "integer", nullable: true),
                    PackCount = table.Column<int>(type: "integer", nullable: true),
                    CountryOfOrigin = table.Column<string>(type: "text", nullable: true),
                    DrainedWeight = table.Column<float>(type: "real", nullable: true),
                    DrainedWeightUom = table.Column<string>(type: "text", nullable: true),
                    GrossWeight = table.Column<float>(type: "real", nullable: true),
                    GrossWeightUom = table.Column<string>(type: "text", nullable: true),
                    NetWeight = table.Column<float>(type: "real", nullable: true),
                    NetWeightUom = table.Column<string>(type: "text", nullable: true),
                    PackageUom = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TradeItems_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "MasterData",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LocationFields",
                schema: "MasterData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LocationId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Value = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LocationFields_Locations_LocationId",
                        column: x => x.LocationId,
                        principalSchema: "MasterData",
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LocationIdentifiers",
                schema: "MasterData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Identifier = table.Column<string>(type: "text", nullable: true),
                    IdentifierType = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    LocationId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationIdentifiers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LocationIdentifiers_Locations_LocationId",
                        column: x => x.LocationId,
                        principalSchema: "MasterData",
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OutboundMappings",
                schema: "MasterData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CompanyId = table.Column<int>(type: "integer", nullable: false),
                    FromBusinessId = table.Column<int>(type: "integer", nullable: true),
                    ShipFromId = table.Column<int>(type: "integer", nullable: true),
                    ToBusinessId = table.Column<int>(type: "integer", nullable: true),
                    ShipToId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboundMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutboundMappings_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "MasterData",
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OutboundMappings_Companies_FromBusinessId",
                        column: x => x.FromBusinessId,
                        principalSchema: "MasterData",
                        principalTable: "Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OutboundMappings_Companies_ToBusinessId",
                        column: x => x.ToBusinessId,
                        principalSchema: "MasterData",
                        principalTable: "Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OutboundMappings_Locations_ShipFromId",
                        column: x => x.ShipFromId,
                        principalSchema: "MasterData",
                        principalTable: "Locations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OutboundMappings_Locations_ShipToId",
                        column: x => x.ShipToId,
                        principalSchema: "MasterData",
                        principalTable: "Locations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TradeItemFields",
                schema: "MasterData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TradeItemId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Value = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeItemFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TradeItemFields_TradeItems_TradeItemId",
                        column: x => x.TradeItemId,
                        principalSchema: "MasterData",
                        principalTable: "TradeItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Companies_CompanyTypeId",
                schema: "MasterData",
                table: "Companies",
                column: "CompanyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_GS1CompanyPrefix",
                schema: "MasterData",
                table: "Companies",
                column: "GS1CompanyPrefix",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LocationFields_LocationId",
                schema: "MasterData",
                table: "LocationFields",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_LocationIdentifiers_LocationId",
                schema: "MasterData",
                table: "LocationIdentifiers",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_CompanyId",
                schema: "MasterData",
                table: "Locations",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_LocationTypeId",
                schema: "MasterData",
                table: "Locations",
                column: "LocationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_SiteId",
                schema: "MasterData",
                table: "Locations",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_OutboundMappings_CompanyId",
                schema: "MasterData",
                table: "OutboundMappings",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_OutboundMappings_FromBusinessId",
                schema: "MasterData",
                table: "OutboundMappings",
                column: "FromBusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_OutboundMappings_ShipFromId",
                schema: "MasterData",
                table: "OutboundMappings",
                column: "ShipFromId");

            migrationBuilder.CreateIndex(
                name: "IX_OutboundMappings_ShipToId",
                schema: "MasterData",
                table: "OutboundMappings",
                column: "ShipToId");

            migrationBuilder.CreateIndex(
                name: "IX_OutboundMappings_ToBusinessId",
                schema: "MasterData",
                table: "OutboundMappings",
                column: "ToBusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_TradeItemFields_TradeItemId",
                schema: "MasterData",
                table: "TradeItemFields",
                column: "TradeItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TradeItems_CompanyId",
                schema: "MasterData",
                table: "TradeItems",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TradeItems_GTIN14",
                schema: "MasterData",
                table: "TradeItems",
                column: "GTIN14",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LocationFields",
                schema: "MasterData");

            migrationBuilder.DropTable(
                name: "LocationIdentifiers",
                schema: "MasterData");

            migrationBuilder.DropTable(
                name: "OutboundMappings",
                schema: "MasterData");

            migrationBuilder.DropTable(
                name: "TradeItemFields",
                schema: "MasterData");

            migrationBuilder.DropTable(
                name: "Locations",
                schema: "MasterData");

            migrationBuilder.DropTable(
                name: "TradeItems",
                schema: "MasterData");

            migrationBuilder.DropTable(
                name: "LocationTypes",
                schema: "MasterData");

            migrationBuilder.DropTable(
                name: "Companies",
                schema: "MasterData");

            migrationBuilder.DropTable(
                name: "CompanyTypes",
                schema: "MasterData");
        }
    }
}
