using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class InitialWeaving : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Weaving_ConstructionDocuments",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ConstructionNumber = table.Column<string>(maxLength: 255, nullable: true),
                    AmountOfWarp = table.Column<int>(nullable: false),
                    AmountOfWeft = table.Column<int>(nullable: false),
                    Width = table.Column<int>(nullable: false),
                    WovenType = table.Column<string>(maxLength: 255, nullable: true),
                    WarpType = table.Column<string>(maxLength: 255, nullable: true),
                    WeftType = table.Column<string>(maxLength: 255, nullable: true),
                    TotalYarn = table.Column<double>(nullable: false),
                    MaterialTypeName = table.Column<string>(nullable: true),
                    ListOfWarp = table.Column<string>(maxLength: 20000, nullable: true),
                    ListOfWeft = table.Column<string>(maxLength: 20000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_ConstructionDocuments", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "Weaving_DailyOperationMachineDocuments",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    DateOperated = table.Column<DateTimeOffset>(nullable: false),
                    MachineId = table.Column<Guid>(nullable: true),
                    UnitId = table.Column<int>(nullable: true),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_DailyOperationMachineDocuments", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "Weaving_EstimationProductDocuments",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    EstimatedNumber = table.Column<string>(nullable: true),
                    Period = table.Column<string>(maxLength: 255, nullable: true),
                    UnitId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_EstimationProductDocuments", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "Weaving_MachineDocuments",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    MachineNumber = table.Column<string>(maxLength: 255, nullable: true),
                    WeavingUnitId = table.Column<int>(nullable: true),
                    MachineTypeId = table.Column<Guid>(nullable: true),
                    Location = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_MachineDocuments", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "Weaving_MachinesPlanningDocuments",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    Area = table.Column<string>(maxLength: 255, nullable: true),
                    Blok = table.Column<string>(maxLength: 255, nullable: true),
                    BlokKaizen = table.Column<string>(maxLength: 255, nullable: true),
                    UnitDepartementId = table.Column<int>(nullable: true),
                    MachineId = table.Column<Guid>(nullable: true),
                    UserMaintenanceId = table.Column<string>(nullable: true),
                    UserOperatorId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_MachinesPlanningDocuments", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "Weaving_MachineTypeDocuments",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    TypeName = table.Column<string>(maxLength: 255, nullable: true),
                    Speed = table.Column<int>(nullable: false),
                    MachineUnit = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_MachineTypeDocuments", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "Weaving_MaterialTypeDocument",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    Code = table.Column<string>(maxLength: 255, nullable: true),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    RingDocuments = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_MaterialTypeDocument", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "Weaving_OperatorDocuments",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    CoreAccount = table.Column<string>(nullable: true),
                    Group = table.Column<string>(maxLength: 255, nullable: true),
                    Assignment = table.Column<string>(maxLength: 255, nullable: true),
                    Type = table.Column<string>(maxLength: 255, nullable: true),
                    UnitId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_OperatorDocuments", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "Weaving_OrderDocuments",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    OrderNumber = table.Column<string>(nullable: true),
                    DateOrdered = table.Column<DateTimeOffset>(nullable: false),
                    WarpOrigin = table.Column<string>(nullable: true),
                    WeftOrigin = table.Column<string>(nullable: true),
                    WholeGrade = table.Column<int>(nullable: false),
                    YarnType = table.Column<string>(nullable: true),
                    Period = table.Column<string>(maxLength: 255, nullable: true),
                    WarpComposition = table.Column<string>(maxLength: 255, nullable: true),
                    ConstructionId = table.Column<Guid>(maxLength: 255, nullable: true),
                    UnitId = table.Column<int>(nullable: true),
                    OrderStatus = table.Column<string>(maxLength: 255, nullable: true),
                    WeftComposition = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_OrderDocuments", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "Weaving_ShiftDocuments",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    StartTime = table.Column<string>(nullable: true),
                    EndTime = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_ShiftDocuments", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "Weaving_SupplierDocuments",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    Code = table.Column<string>(maxLength: 255, nullable: true),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    CoreSupplierId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_SupplierDocuments", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "Weaving_YarnDocuments",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    Code = table.Column<string>(maxLength: 255, nullable: true),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    Tags = table.Column<string>(maxLength: 255, nullable: true),
                    MaterialTypeId = table.Column<Guid>(nullable: true),
                    YarnNumberId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_YarnDocuments", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "Weaving_YarnNumberDocuments",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    Code = table.Column<string>(maxLength: 255, nullable: true),
                    Number = table.Column<int>(maxLength: 255, nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    RingType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_YarnNumberDocuments", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "Weaving_DailyOperationMachineDetails",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    OrderDocumentId = table.Column<Guid>(nullable: true),
                    WarpsOrigin = table.Column<string>(nullable: true),
                    WeftsOrigin = table.Column<string>(nullable: true),
                    BeamDocumentId = table.Column<Guid>(nullable: true),
                    DOMTime = table.Column<string>(nullable: true),
                    ShiftDocumentId = table.Column<Guid>(nullable: true),
                    BeamOperatorDocumentId = table.Column<Guid>(nullable: true),
                    SizingOperatorDocumentId = table.Column<Guid>(nullable: true),
                    Information = table.Column<string>(maxLength: 2000, nullable: true),
                    DetailStatus = table.Column<string>(maxLength: 255, nullable: true),
                    DailyOperationMachineDocumentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_DailyOperationMachineDetails", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_Weaving_DailyOperationMachineDetails_Weaving_DailyOperationMachineDocuments_DailyOperationMachineDocumentId",
                        column: x => x.DailyOperationMachineDocumentId,
                        principalTable: "Weaving_DailyOperationMachineDocuments",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Weaving_EstimationDetails",
                columns: table => new
                {
                    Identity = table.Column<Guid>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 32, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    ModifiedBy = table.Column<string>(maxLength: 32, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Deleted = table.Column<bool>(nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 32, nullable: true),
                    OrderDocument = table.Column<string>(maxLength: 2000, nullable: true),
                    TotalGramEstimation = table.Column<double>(nullable: false),
                    ProductGrade = table.Column<string>(maxLength: 2000, nullable: true),
                    EstimatedProductionDocumentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_EstimationDetails", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_Weaving_EstimationDetails_Weaving_EstimationProductDocuments_EstimatedProductionDocumentId",
                        column: x => x.EstimatedProductionDocumentId,
                        principalTable: "Weaving_EstimationProductDocuments",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Weaving_DailyOperationMachineDetails_DailyOperationMachineDocumentId",
                table: "Weaving_DailyOperationMachineDetails",
                column: "DailyOperationMachineDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Weaving_EstimationDetails_EstimatedProductionDocumentId",
                table: "Weaving_EstimationDetails",
                column: "EstimatedProductionDocumentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Weaving_ConstructionDocuments");

            migrationBuilder.DropTable(
                name: "Weaving_DailyOperationMachineDetails");

            migrationBuilder.DropTable(
                name: "Weaving_EstimationDetails");

            migrationBuilder.DropTable(
                name: "Weaving_MachineDocuments");

            migrationBuilder.DropTable(
                name: "Weaving_MachinesPlanningDocuments");

            migrationBuilder.DropTable(
                name: "Weaving_MachineTypeDocuments");

            migrationBuilder.DropTable(
                name: "Weaving_MaterialTypeDocument");

            migrationBuilder.DropTable(
                name: "Weaving_OperatorDocuments");

            migrationBuilder.DropTable(
                name: "Weaving_OrderDocuments");

            migrationBuilder.DropTable(
                name: "Weaving_ShiftDocuments");

            migrationBuilder.DropTable(
                name: "Weaving_SupplierDocuments");

            migrationBuilder.DropTable(
                name: "Weaving_YarnDocuments");

            migrationBuilder.DropTable(
                name: "Weaving_YarnNumberDocuments");

            migrationBuilder.DropTable(
                name: "Weaving_DailyOperationMachineDocuments");

            migrationBuilder.DropTable(
                name: "Weaving_EstimationProductDocuments");
        }
    }
}
