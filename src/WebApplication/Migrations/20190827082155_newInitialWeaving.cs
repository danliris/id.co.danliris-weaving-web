using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DanLiris.Admin.Web.Migrations
{
    public partial class newInitialWeaving : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Weaving_BeamDocuments",
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
                    Number = table.Column<string>(maxLength: 255, nullable: true),
                    Type = table.Column<string>(maxLength: 255, nullable: true),
                    EmptyWeight = table.Column<double>(nullable: false),
                    YarnLength = table.Column<double>(nullable: true),
                    YarnStrands = table.Column<double>(nullable: true),
                    ContructionId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_BeamDocuments", x => x.Identity);
                });

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
                    ListOfWeft = table.Column<string>(maxLength: 20000, nullable: true),
                    ReedSpace = table.Column<int>(nullable: true),
                    TotalEnds = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_ConstructionDocuments", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "Weaving_DailyOperationLoomDocuments",
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
                    UnitId = table.Column<int>(nullable: false),
                    MachineId = table.Column<Guid>(nullable: false),
                    BeamId = table.Column<Guid>(nullable: false),
                    OrderId = table.Column<Guid>(nullable: false),
                    DailyOperationStatus = table.Column<string>(nullable: true),
                    DailyOperationMonitoringId = table.Column<Guid>(nullable: true),
                    UsedYarn = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_DailyOperationLoomDocuments", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "Weaving_DailyOperationReachingTyingDocuments",
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
                    MachineDocumentId = table.Column<Guid>(nullable: true),
                    WeavingUnitId = table.Column<int>(nullable: true),
                    OrderDocumentId = table.Column<Guid>(nullable: true),
                    SizingBeamId = table.Column<Guid>(nullable: true),
                    PISPieces = table.Column<double>(nullable: false),
                    ReachingValueObjects = table.Column<string>(nullable: true),
                    TyingValueObjects = table.Column<string>(nullable: true),
                    OperationStatus = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_DailyOperationReachingTyingDocuments", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "Weaving_DailyOperationSizingDocuments",
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
                    MachineDocumentId = table.Column<Guid>(nullable: true),
                    WeavingUnitId = table.Column<int>(nullable: true),
                    OrderDocumentId = table.Column<Guid>(nullable: true),
                    BeamsWarping = table.Column<string>(nullable: true),
                    YarnStrands = table.Column<double>(nullable: false),
                    RecipeCode = table.Column<string>(nullable: true),
                    NeReal = table.Column<double>(nullable: false),
                    MachineSpeed = table.Column<int>(nullable: true),
                    TexSQ = table.Column<string>(nullable: true),
                    Visco = table.Column<string>(nullable: true),
                    OperationStatus = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_DailyOperationSizingDocuments", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "Weaving_DailyOperationWarpingDocuments",
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
                    ConstructionId = table.Column<Guid>(nullable: false),
                    MaterialTypeId = table.Column<Guid>(nullable: false),
                    AmountOfCones = table.Column<int>(nullable: false),
                    ColourOfCone = table.Column<string>(nullable: true),
                    DateTimeOperation = table.Column<DateTimeOffset>(nullable: false),
                    OperatorId = table.Column<Guid>(nullable: false),
                    DailyOperationStatus = table.Column<string>(nullable: true),
                    OrderId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_DailyOperationWarpingDocuments", x => x.Identity);
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
                name: "Weaving_MaterialTypeDocuments",
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
                    table.PrimaryKey("PK_Weaving_MaterialTypeDocuments", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "Weaving_MovementDocuments",
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
                    DailyOperationId = table.Column<Guid>(nullable: false),
                    MovementType = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_MovementDocuments", x => x.Identity);
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
                    StartTime = table.Column<TimeSpan>(nullable: false),
                    EndTime = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_ShiftDocuments", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "Weaving_StockCardDocuments",
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
                    StockNumber = table.Column<string>(nullable: true),
                    DailyOperationId = table.Column<Guid>(nullable: false),
                    DateTimeOperation = table.Column<DateTimeOffset>(nullable: false),
                    BeamDocument = table.Column<string>(nullable: true),
                    IsAvailable = table.Column<bool>(nullable: false),
                    IsReaching = table.Column<bool>(nullable: false),
                    IsTying = table.Column<bool>(nullable: false),
                    StockType = table.Column<string>(nullable: true),
                    StockStatus = table.Column<string>(nullable: true),
                    Expired = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_StockCardDocuments", x => x.Identity);
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
                    AdditionalNumber = table.Column<int>(nullable: true),
                    RingType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_YarnNumberDocuments", x => x.Identity);
                });

            migrationBuilder.CreateTable(
                name: "Weaving_DailyOperationLoomDetails",
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
                    ShiftId = table.Column<Guid>(nullable: false),
                    BeamOperatorId = table.Column<Guid>(nullable: false),
                    WarpOrigin = table.Column<string>(nullable: true),
                    WeftOrigin = table.Column<string>(nullable: true),
                    DateTimeOperation = table.Column<DateTimeOffset>(nullable: false),
                    OperationStatus = table.Column<string>(nullable: true),
                    IsUp = table.Column<bool>(nullable: false),
                    IsDown = table.Column<bool>(nullable: false),
                    DailyOperationLoomDocumentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_DailyOperationLoomDetails", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_Weaving_DailyOperationLoomDetails_Weaving_DailyOperationLoomDocuments_DailyOperationLoomDocumentId",
                        column: x => x.DailyOperationLoomDocumentId,
                        principalTable: "Weaving_DailyOperationLoomDocuments",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Weaving_DailyOperationReachingTyingDetails",
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
                    OperatorDocumentId = table.Column<Guid>(nullable: false),
                    DateTimeMachine = table.Column<DateTimeOffset>(nullable: false),
                    ShiftDocumentId = table.Column<Guid>(nullable: false),
                    MachineStatus = table.Column<string>(nullable: true),
                    DailyOperationReachingTyingDocumentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_DailyOperationReachingTyingDetails", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_Weaving_DailyOperationReachingTyingDetails_Weaving_DailyOperationReachingTyingDocuments_DailyOperationReachingTyingDocumentId",
                        column: x => x.DailyOperationReachingTyingDocumentId,
                        principalTable: "Weaving_DailyOperationReachingTyingDocuments",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Weaving_DailyOperationSizingBeamDocuments",
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
                    SizingBeamId = table.Column<Guid>(nullable: false),
                    DateTimeBeamDocument = table.Column<DateTimeOffset>(nullable: false),
                    Counter = table.Column<string>(nullable: true),
                    Weight = table.Column<string>(nullable: true),
                    PISMeter = table.Column<double>(nullable: false),
                    SPU = table.Column<double>(nullable: false),
                    SizingBeamStatus = table.Column<string>(nullable: true),
                    DailyOperationSizingDocumentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_DailyOperationSizingBeamDocuments", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_Weaving_DailyOperationSizingBeamDocuments_Weaving_DailyOperationSizingDocuments_DailyOperationSizingDocumentId",
                        column: x => x.DailyOperationSizingDocumentId,
                        principalTable: "Weaving_DailyOperationSizingDocuments",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Weaving_DailyOperationSizingDetails",
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
                    ShiftDocumentId = table.Column<Guid>(nullable: false),
                    OperatorDocumentId = table.Column<Guid>(nullable: false),
                    DateTimeMachine = table.Column<DateTimeOffset>(nullable: false),
                    MachineStatus = table.Column<string>(nullable: true),
                    Information = table.Column<string>(nullable: true),
                    Causes = table.Column<string>(nullable: true),
                    SizingBeamNumber = table.Column<string>(nullable: true),
                    DailyOperationSizingDocumentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_DailyOperationSizingDetails", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_Weaving_DailyOperationSizingDetails_Weaving_DailyOperationSizingDocuments_DailyOperationSizingDocumentId",
                        column: x => x.DailyOperationSizingDocumentId,
                        principalTable: "Weaving_DailyOperationSizingDocuments",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Weaving_DailyOperationWarpingBeamProduct",
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
                    BeamId = table.Column<Guid>(nullable: false),
                    Length = table.Column<double>(nullable: true),
                    Tention = table.Column<int>(nullable: true),
                    Speed = table.Column<int>(nullable: true),
                    PressRoll = table.Column<double>(nullable: true),
                    BeamStatus = table.Column<string>(nullable: true),
                    DailyOperationWarpingDocumentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_DailyOperationWarpingBeamProduct", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_Weaving_DailyOperationWarpingBeamProduct_Weaving_DailyOperationWarpingDocuments_DailyOperationWarpingDocumentId",
                        column: x => x.DailyOperationWarpingDocumentId,
                        principalTable: "Weaving_DailyOperationWarpingDocuments",
                        principalColumn: "Identity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Weaving_DailyOperationWarpingHistory",
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
                    ShiftId = table.Column<Guid>(nullable: false),
                    BeamNumber = table.Column<string>(nullable: true),
                    BeamOperatorId = table.Column<Guid>(nullable: false),
                    DateTimeOperation = table.Column<DateTimeOffset>(nullable: false),
                    Information = table.Column<string>(nullable: true),
                    OperationStatus = table.Column<string>(nullable: true),
                    DailyOperationWarpingDocumentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weaving_DailyOperationWarpingHistory", x => x.Identity);
                    table.ForeignKey(
                        name: "FK_Weaving_DailyOperationWarpingHistory_Weaving_DailyOperationWarpingDocuments_DailyOperationWarpingDocumentId",
                        column: x => x.DailyOperationWarpingDocumentId,
                        principalTable: "Weaving_DailyOperationWarpingDocuments",
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
                name: "IX_Weaving_DailyOperationLoomDetails_DailyOperationLoomDocumentId",
                table: "Weaving_DailyOperationLoomDetails",
                column: "DailyOperationLoomDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Weaving_DailyOperationReachingTyingDetails_DailyOperationReachingTyingDocumentId",
                table: "Weaving_DailyOperationReachingTyingDetails",
                column: "DailyOperationReachingTyingDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Weaving_DailyOperationSizingBeamDocuments_DailyOperationSizingDocumentId",
                table: "Weaving_DailyOperationSizingBeamDocuments",
                column: "DailyOperationSizingDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Weaving_DailyOperationSizingDetails_DailyOperationSizingDocumentId",
                table: "Weaving_DailyOperationSizingDetails",
                column: "DailyOperationSizingDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Weaving_DailyOperationWarpingBeamProduct_DailyOperationWarpingDocumentId",
                table: "Weaving_DailyOperationWarpingBeamProduct",
                column: "DailyOperationWarpingDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Weaving_DailyOperationWarpingHistory_DailyOperationWarpingDocumentId",
                table: "Weaving_DailyOperationWarpingHistory",
                column: "DailyOperationWarpingDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Weaving_EstimationDetails_EstimatedProductionDocumentId",
                table: "Weaving_EstimationDetails",
                column: "EstimatedProductionDocumentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Weaving_BeamDocuments");

            migrationBuilder.DropTable(
                name: "Weaving_ConstructionDocuments");

            migrationBuilder.DropTable(
                name: "Weaving_DailyOperationLoomDetails");

            migrationBuilder.DropTable(
                name: "Weaving_DailyOperationReachingTyingDetails");

            migrationBuilder.DropTable(
                name: "Weaving_DailyOperationSizingBeamDocuments");

            migrationBuilder.DropTable(
                name: "Weaving_DailyOperationSizingDetails");

            migrationBuilder.DropTable(
                name: "Weaving_DailyOperationWarpingBeamProduct");

            migrationBuilder.DropTable(
                name: "Weaving_DailyOperationWarpingHistory");

            migrationBuilder.DropTable(
                name: "Weaving_EstimationDetails");

            migrationBuilder.DropTable(
                name: "Weaving_MachineDocuments");

            migrationBuilder.DropTable(
                name: "Weaving_MachinesPlanningDocuments");

            migrationBuilder.DropTable(
                name: "Weaving_MachineTypeDocuments");

            migrationBuilder.DropTable(
                name: "Weaving_MaterialTypeDocuments");

            migrationBuilder.DropTable(
                name: "Weaving_MovementDocuments");

            migrationBuilder.DropTable(
                name: "Weaving_OperatorDocuments");

            migrationBuilder.DropTable(
                name: "Weaving_OrderDocuments");

            migrationBuilder.DropTable(
                name: "Weaving_ShiftDocuments");

            migrationBuilder.DropTable(
                name: "Weaving_StockCardDocuments");

            migrationBuilder.DropTable(
                name: "Weaving_SupplierDocuments");

            migrationBuilder.DropTable(
                name: "Weaving_YarnDocuments");

            migrationBuilder.DropTable(
                name: "Weaving_YarnNumberDocuments");

            migrationBuilder.DropTable(
                name: "Weaving_DailyOperationLoomDocuments");

            migrationBuilder.DropTable(
                name: "Weaving_DailyOperationReachingTyingDocuments");

            migrationBuilder.DropTable(
                name: "Weaving_DailyOperationSizingDocuments");

            migrationBuilder.DropTable(
                name: "Weaving_DailyOperationWarpingDocuments");

            migrationBuilder.DropTable(
                name: "Weaving_EstimationProductDocuments");
        }
    }
}
