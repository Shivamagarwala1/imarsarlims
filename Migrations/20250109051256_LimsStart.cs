using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace iMARSARLIMS.Migrations
{
    /// <inheritdoc />
    public partial class LimsStart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Allergy_SubType_Master",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    typeId = table.Column<int>(type: "int", nullable: true),
                    sub_TypeName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    firstRange = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    secondRange = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    thirdRange = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    imagePath = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    descrition = table.Column<string>(type: "longtext", nullable: true),
                    defultReading = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    unit = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Allergy_SubType_Master", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Allergy_TypeMaster",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    allergyType = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Allergy_TypeMaster", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "area_master",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    areaName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    cityId = table.Column<int>(type: "int", nullable: false),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_area_master", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "bank_master",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    bankName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    type = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bank_master", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "barcode_series",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    barcodeNo = table.Column<int>(type: "int", nullable: true),
                    suffix = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_barcode_series", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "centerAccess",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    empId = table.Column<int>(type: "int", nullable: false),
                    centreId = table.Column<int>(type: "int", nullable: false),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_centerAccess", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "centerTypeMaster",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    centerTypeName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_centerTypeMaster", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "centreInvoice",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    invoiceNo = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    centreid = table.Column<int>(type: "int", nullable: false),
                    rate = table.Column<double>(type: "double", nullable: false),
                    fromDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    toDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    createdBy = table.Column<int>(type: "int", nullable: false),
                    InvoiceDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    createDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    cancelReason = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true),
                    cancelByID = table.Column<int>(type: "int", nullable: true),
                    isCancel = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    cancelDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_centreInvoice", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "centreLedgerRemarks",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    centreId = table.Column<int>(type: "int", nullable: false),
                    remarks = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_centreLedgerRemarks", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "centreMaster",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    centretype = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    centretypeid = table.Column<int>(type: "int", nullable: false),
                    centrecode = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    companyName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    mobileNo = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    address = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    pinCode = table.Column<int>(type: "int", nullable: false),
                    email = table.Column<string>(type: "longtext", nullable: true),
                    ownerName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    proId = table.Column<int>(type: "int", nullable: false),
                    reportEmail = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    parentCentreID = table.Column<int>(type: "int", nullable: false),
                    processingLab = table.Column<int>(type: "int", nullable: false),
                    creditLimt = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    allowDueReport = table.Column<int>(type: "int", nullable: false),
                    reportLock = table.Column<int>(type: "int", nullable: false),
                    bookingLock = table.Column<int>(type: "int", nullable: false),
                    unlockTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    paymentMode = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    paymentModeId = table.Column<int>(type: "int", nullable: false),
                    reportHeader = table.Column<string>(type: "longtext", nullable: true),
                    reciptHeader = table.Column<string>(type: "longtext", nullable: true),
                    reciptFooter = table.Column<string>(type: "longtext", nullable: true),
                    showISO = table.Column<int>(type: "int", nullable: false),
                    showBackcover = table.Column<int>(type: "int", nullable: false),
                    reportBackImage = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    reporrtHeaderHeightY = table.Column<int>(type: "int", nullable: true),
                    patientYHeader = table.Column<int>(type: "int", nullable: true),
                    barcodeXPosition = table.Column<int>(type: "int", nullable: true),
                    barcodeYPosition = table.Column<int>(type: "int", nullable: true),
                    QRCodeXPosition = table.Column<int>(type: "int", nullable: true),
                    QRCodeYPosition = table.Column<int>(type: "int", nullable: true),
                    isQRheader = table.Column<int>(type: "int", nullable: true),
                    isBarcodeHeader = table.Column<int>(type: "int", nullable: true),
                    footerHeight = table.Column<int>(type: "int", nullable: true),
                    NABLxPosition = table.Column<int>(type: "int", nullable: true),
                    NABLyPosition = table.Column<int>(type: "int", nullable: true),
                    docSignYPosition = table.Column<int>(type: "int", nullable: true),
                    receiptHeaderY = table.Column<int>(type: "int", nullable: true),
                    PAN = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    adharNo = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: true),
                    bankAccount = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    IFSCCode = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    bankID = table.Column<int>(type: "int", nullable: false),
                    salesExecutiveID = table.Column<int>(type: "int", nullable: false),
                    isDefault = table.Column<int>(type: "int", nullable: true),
                    isLab = table.Column<int>(type: "int", nullable: true),
                    minBookingAmt = table.Column<int>(type: "int", nullable: false),
                    lockedBy = table.Column<int>(type: "int", nullable: true),
                    LockDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    unlockBy = table.Column<string>(type: "longtext", nullable: true),
                    unlockDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    state = table.Column<int>(type: "int", nullable: false),
                    chequeNo = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    bankName = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    chequeAmount = table.Column<int>(type: "int", nullable: false),
                    creditPeridos = table.Column<int>(type: "int", nullable: false),
                    showClientCode = table.Column<int>(type: "int", nullable: false),
                    patientRate = table.Column<int>(type: "int", nullable: false),
                    clientRate = table.Column<int>(type: "int", nullable: false),
                    isLock = table.Column<int>(type: "int", nullable: false),
                    isPrePrintedBarcode = table.Column<int>(type: "int", nullable: false),
                    ac = table.Column<int>(type: "int", nullable: false),
                    clientmrp = table.Column<int>(type: "int", nullable: false),
                    documentType = table.Column<int>(type: "int", nullable: false),
                    Document = table.Column<string>(type: "longtext", nullable: false),
                    receptionarea = table.Column<int>(type: "int", nullable: false),
                    waitingarea = table.Column<int>(type: "int", nullable: false),
                    watercooler = table.Column<int>(type: "int", nullable: false),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_centreMaster", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CentrePayment",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    centreId = table.Column<int>(type: "int", nullable: false),
                    paymentDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    paymentMode = table.Column<string>(type: "longtext", nullable: true),
                    advancePaymentAmt = table.Column<float>(type: "float", nullable: true),
                    bank = table.Column<string>(type: "longtext", nullable: true),
                    tnxNo = table.Column<string>(type: "longtext", nullable: true),
                    tnxDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    remarks = table.Column<string>(type: "longtext", nullable: true),
                    rejectRemarks = table.Column<string>(type: "longtext", nullable: true),
                    approved = table.Column<short>(type: "smallint", nullable: false),
                    createdBy = table.Column<int>(type: "int", nullable: true),
                    createdDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    updateByID = table.Column<int>(type: "int", nullable: true),
                    apprvoedByID = table.Column<int>(type: "int", nullable: true),
                    paymentType = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    documentName = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CentrePayment", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "centreWelcomeEmail",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    centreId = table.Column<int>(type: "int", nullable: false),
                    centreCode = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    emailId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    emailBody = table.Column<string>(type: "longtext", nullable: false),
                    subject = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    isSent = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    sendDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    createdByID = table.Column<int>(type: "int", nullable: false),
                    errorMsg = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_centreWelcomeEmail", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "changeCentreLog",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    workOrderId = table.Column<string>(type: "longtext", nullable: false),
                    oldBookingCentre = table.Column<int>(type: "int", maxLength: 20, nullable: false),
                    newBookingCentre = table.Column<int>(type: "int", nullable: false),
                    oldCompanyId = table.Column<int>(type: "int", nullable: false),
                    newCompanyId = table.Column<int>(type: "int", nullable: false),
                    oldRateTypeId = table.Column<int>(type: "int", nullable: false),
                    newRateTypeId = table.Column<int>(type: "int", nullable: false),
                    dtEntry = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    enteredBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_changeCentreLog", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "cityMaster",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    cityName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    districtid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cityMaster", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "containerColorMaster",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    colorName = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_containerColorMaster", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "degreeMaster",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    degreeName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_degreeMaster", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "designationMaster",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    designationName = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_designationMaster", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "discountReasonMaster",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    discountReasonName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_discountReasonMaster", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "discountTypeMaster",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    type = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_discountTypeMaster", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "districtMaster",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    district = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    stateId = table.Column<int>(type: "int", nullable: false),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_districtMaster", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "doctorApprovalDepartments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    doctorId = table.Column<int>(type: "int", nullable: false),
                    centreId = table.Column<int>(type: "int", nullable: false),
                    departmentId = table.Column<int>(type: "int", nullable: false),
                    empId = table.Column<int>(type: "int", nullable: false),
                    fromTime = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    toTime = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_doctorApprovalDepartments", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "doctorApprovalMaster",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    centreId = table.Column<int>(type: "int", nullable: false),
                    empId = table.Column<int>(type: "int", nullable: false),
                    doctorName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    signature = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    approve = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    notApprove = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    hold = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    unHold = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    doctorId = table.Column<int>(type: "int", nullable: false),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_doctorApprovalMaster", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "doctorReferalMaster",
                columns: table => new
                {
                    doctorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    doctorCode = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    title = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    doctorName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    imaRegistartionNo = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    email = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    reportEmail = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    mobileNo = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true),
                    mobileno2 = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true),
                    address1 = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    address2 = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    pinCode = table.Column<int>(type: "int", nullable: true),
                    degreeId = table.Column<int>(type: "int", nullable: true),
                    degreeName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    specializationID = table.Column<int>(type: "int", nullable: true),
                    specialization = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    dob = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    anniversary = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    allowsharing = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    referMasterShare = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    proId = table.Column<int>(type: "int", nullable: true),
                    areaId = table.Column<int>(type: "int", nullable: true),
                    city = table.Column<int>(type: "int", nullable: true),
                    state = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_doctorReferalMaster", x => x.doctorId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "documentMaster",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    transactionId = table.Column<int>(type: "int", nullable: false),
                    fileName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    documentTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_documentMaster", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "documentTypeMaster",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    documentType = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_documentTypeMaster", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "empDepartmentAccess",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    empId = table.Column<int>(type: "int", nullable: false),
                    departmentId = table.Column<int>(type: "int", nullable: false),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_empDepartmentAccess", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "empLoginDetails",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    empId = table.Column<int>(type: "int", nullable: false),
                    centreId = table.Column<int>(type: "int", nullable: false),
                    ipAddress = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    macAddress = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    browserName = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    hostName = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    userName = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    empName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    roleId = table.Column<int>(type: "int", nullable: false),
                    logindatetime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    logoutdatetime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    creadteddate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_empLoginDetails", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "empMaster",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    empCode = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    title = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    fName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    lName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    address = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    pinCode = table.Column<int>(type: "int", nullable: true),
                    email = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    mobileNo = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    deptAccess = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    dob = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    qualification = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    bloodGroup = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: true),
                    designationId = table.Column<int>(type: "int", nullable: false),
                    userName = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    password = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    zone = table.Column<int>(type: "int", nullable: false),
                    state = table.Column<int>(type: "int", nullable: false),
                    city = table.Column<int>(type: "int", nullable: false),
                    area = table.Column<int>(type: "int", nullable: false),
                    defaultcentre = table.Column<int>(type: "int", nullable: false),
                    pro = table.Column<int>(type: "int", nullable: true),
                    defaultrole = table.Column<int>(type: "int", nullable: false),
                    rate = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    fileName = table.Column<string>(type: "longtext", nullable: false),
                    autoCreated = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    centreId = table.Column<int>(type: "int", nullable: false),
                    allowDueReport = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    employeeType = table.Column<int>(type: "int", nullable: false),
                    isSalesTeamMember = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    isDiscountAppRights = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    isPwdchange = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    isemailotp = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    fromIP = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    toIP = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    isdeviceAuthentication = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    adminPassword = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    tempPassword = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_empMaster", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "formulaMaster",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    itemId = table.Column<int>(type: "int", nullable: false),
                    labTestId = table.Column<int>(type: "int", nullable: false),
                    formula = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_formulaMaster", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "helpMenuMapping",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    helpId = table.Column<int>(type: "int", nullable: true),
                    labTestId = table.Column<int>(type: "int", nullable: true),
                    itemId = table.Column<int>(type: "int", nullable: true),
                    mappedName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    helpName = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_helpMenuMapping", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "helpMenuMaster",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    helpName = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_helpMenuMaster", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "idMaster",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    centreId = table.Column<int>(type: "int", nullable: true),
                    maxID = table.Column<int>(type: "int", nullable: true),
                    fYear = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: true),
                    typeId = table.Column<int>(type: "int", nullable: true),
                    type = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_idMaster", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "item_outsourcemaster",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    bookingCentreId = table.Column<int>(type: "int", nullable: true),
                    processingCentreId = table.Column<int>(type: "int", nullable: true),
                    itemId = table.Column<int>(type: "int", nullable: true),
                    departmentId = table.Column<int>(type: "int", nullable: true),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_item_outsourcemaster", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "itemCommentMaster",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    type = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    itemId = table.Column<int>(type: "int", nullable: false),
                    observationId = table.Column<int>(type: "int", nullable: false),
                    templateName = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    template = table.Column<string>(type: "longtext", nullable: false),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_itemCommentMaster", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "itemDocumentMapping",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    itemId = table.Column<int>(type: "int", nullable: false),
                    documentId = table.Column<int>(type: "int", nullable: false),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_itemDocumentMapping", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "itemMaster",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    itemName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    dispalyName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    testMethod = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    deptId = table.Column<int>(type: "int", nullable: true),
                    code = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    sortName = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    allowDiscont = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    allowShare = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    allowReporting = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    itemType = table.Column<int>(type: "int", nullable: false),
                    isOutsource = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    lmpRequire = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    reportType = table.Column<int>(type: "int", nullable: true),
                    gender = table.Column<string>(type: "longtext", nullable: true),
                    sampleVolume = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    containerColor = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    testRemarks = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    defaultsampletype = table.Column<int>(type: "int", nullable: false),
                    agegroup = table.Column<int>(type: "int", nullable: true),
                    samplelogisticstemp = table.Column<string>(type: "longtext", nullable: true),
                    printsamplename = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    showinpatientreport = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    showinonlinereport = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    autosaveautoapprove = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    printseperate = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    isorganism = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    culturereport = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    ismic = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    showOnWebsite = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    isSpecialItem = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    isAllergyTest = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    displaySequence = table.Column<int>(type: "int", nullable: true),
                    consentForm = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_itemMaster", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ItemObservationMapping",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    itemId = table.Column<int>(type: "int", nullable: true),
                    itemObservationId = table.Column<int>(type: "int", nullable: true),
                    isTest = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    isProfile = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    isPackage = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    itemType = table.Column<short>(type: "smallint", nullable: false),
                    formula = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    dlcCheck = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    showInReport = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    isHeader = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    isBold = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    isCritical = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    printSeparate = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    mappedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemObservationMapping", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "itemObservationMaster",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    labObservationName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    dlcCheck = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    gender = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: true),
                    printSeparate = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    shortName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    roundUp = table.Column<double>(type: "double", nullable: true),
                    method = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    suffix = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: true),
                    formula = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    observationWiseInterpretation = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true),
                    resultRequired = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    collectionRequire = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    displaySequence = table.Column<int>(type: "int", nullable: true),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_itemObservationMaster", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "itemRateMaster",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    itemId = table.Column<int>(type: "int", nullable: false),
                    rateTypeId = table.Column<int>(type: "int", nullable: true),
                    itemCode = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_itemRateMaster", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "itemSampleTypeMapping",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    sampleTypeId = table.Column<int>(type: "int", nullable: false),
                    sampleTypeName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    isDefault = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_itemSampleTypeMapping", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "labDepartment",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    deptCode = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    deptName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    subDeptName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    abbreviation = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: true),
                    printSequence = table.Column<byte>(type: "tinyint unsigned", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_labDepartment", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "labReportHeader",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    headerCSS = table.Column<string>(type: "longtext", nullable: true),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_labReportHeader", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "logoDetails",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    imageName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    logoType = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    remarks = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    status = table.Column<int>(type: "int", nullable: false),
                    logoDescription = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_logoDetails", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "machine_result",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    observationId = table.Column<int>(type: "int", nullable: true),
                    testId = table.Column<int>(type: "int", nullable: true),
                    workOrderId = table.Column<string>(type: "longtext", nullable: true),
                    barcodeNo = table.Column<string>(type: "longtext", nullable: true),
                    labCentreId = table.Column<int>(type: "int", nullable: true),
                    investigationName = table.Column<string>(type: "longtext", nullable: true),
                    labObservationName = table.Column<string>(type: "longtext", nullable: true),
                    macReading1 = table.Column<string>(type: "longtext", nullable: true),
                    macId1 = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    machineName1 = table.Column<string>(type: "longtext", nullable: true),
                    MacReading2 = table.Column<string>(type: "longtext", nullable: true),
                    macId2 = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    machineName2 = table.Column<string>(type: "longtext", nullable: true),
                    MacReading3 = table.Column<string>(type: "longtext", nullable: true),
                    macId3 = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    machineName3 = table.Column<string>(type: "longtext", nullable: true),
                    patientName = table.Column<string>(type: "longtext", nullable: true),
                    dob = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    gender = table.Column<string>(type: "longtext", nullable: true),
                    machineComments = table.Column<string>(type: "longtext", nullable: true),
                    status = table.Column<string>(type: "longtext", nullable: true),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_machine_result", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "machineMaster",
                columns: table => new
                {
                    id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    machineName = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    machineType = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_machineMaster", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "machineObservationMapping",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    machineId = table.Column<short>(type: "smallint", nullable: false),
                    assay = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    labTestID = table.Column<int>(type: "int", nullable: true),
                    isOrderable = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    formula = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    roundUp = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    multiplication = table.Column<double>(type: "double", nullable: true),
                    suffix = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: true),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_machineObservationMapping", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "menuIconMaster",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    icon = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_menuIconMaster", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "menuMaster",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    menuName = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true),
                    navigationUrl = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    displaySequence = table.Column<int>(type: "int", nullable: true),
                    parentId = table.Column<int>(type: "int", nullable: true),
                    isHide = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_menuMaster", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "observationComment",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    testId = table.Column<int>(type: "int", nullable: false),
                    observationId = table.Column<int>(type: "int", nullable: false),
                    comment = table.Column<string>(type: "longtext", nullable: false),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_observationComment", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "observationReferenceRanges",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    observationId = table.Column<int>(type: "int", nullable: true),
                    gender = table.Column<string>(type: "longtext", nullable: true),
                    genderTextValue = table.Column<string>(type: "longtext", nullable: true),
                    fromAge = table.Column<int>(type: "int", nullable: true),
                    toAge = table.Column<int>(type: "int", nullable: true),
                    minValue = table.Column<double>(type: "double", nullable: true),
                    maxValue = table.Column<double>(type: "double", nullable: true),
                    method = table.Column<string>(type: "longtext", nullable: true),
                    unit = table.Column<string>(type: "longtext", nullable: true),
                    reportReading = table.Column<string>(type: "longtext", nullable: true),
                    defaultValue = table.Column<string>(type: "longtext", nullable: true),
                    autoHold = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    minCritical = table.Column<double>(type: "double", nullable: true),
                    maxCritical = table.Column<double>(type: "double", nullable: true),
                    minAutoApprovalValue = table.Column<double>(type: "double", nullable: true),
                    maxAutoApprovalValue = table.Column<double>(type: "double", nullable: true),
                    centreId = table.Column<int>(type: "int", nullable: true),
                    machineID = table.Column<int>(type: "int", nullable: true),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_observationReferenceRanges", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "organismAntibioticMaster",
                columns: table => new
                {
                    id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    organismAntibiotic = table.Column<string>(type: "longtext", nullable: false),
                    machineCode = table.Column<string>(type: "longtext", nullable: true),
                    microType = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_organismAntibioticMaster", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "organismAntibioticTagMaster",
                columns: table => new
                {
                    id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    organismId = table.Column<int>(type: "int", nullable: false),
                    antibiticId = table.Column<int>(type: "int", nullable: false),
                    centreId = table.Column<int>(type: "int", nullable: false),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_organismAntibioticTagMaster", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "outSourcelabmaster",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    labName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_outSourcelabmaster", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "patientDemographicLog",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    patientId = table.Column<int>(type: "int", nullable: false),
                    title = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    patientName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    gender = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    ageTotal = table.Column<int>(type: "int", nullable: false),
                    ageDays = table.Column<int>(type: "int", nullable: false),
                    ageMonth = table.Column<int>(type: "int", nullable: false),
                    ageYear = table.Column<int>(type: "int", nullable: false),
                    dob = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    mobileNo = table.Column<string>(type: "longtext", nullable: false),
                    address = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    transactionId = table.Column<int>(type: "int", nullable: false),
                    refID1 = table.Column<int>(type: "int", nullable: false),
                    RefDoctor1 = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    refID2 = table.Column<int>(type: "int", nullable: false),
                    refDoctor2 = table.Column<string>(type: "longtext", nullable: false),
                    centreId = table.Column<int>(type: "int", nullable: false),
                    updateDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    updateById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_patientDemographicLog", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "patientReportEmail",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    transactionId = table.Column<int>(type: "int", nullable: false),
                    workOrderId = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    deliveryDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    emailSentTo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    emailCC = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    isSend = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    isAutoSend = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    sentBy = table.Column<int>(type: "int", nullable: true),
                    sendDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    remarks = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    type = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    testId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_patientReportEmail", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "rateTypeMaster",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    rateName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    rateTypeId = table.Column<int>(type: "int", nullable: false),
                    rateType = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rateTypeMaster", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "rateTypeTagging",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    rateTypeId = table.Column<int>(type: "int", nullable: false),
                    centreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rateTypeTagging", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "rateTypeWiseRateList",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    deptId = table.Column<int>(type: "int", nullable: false),
                    panelId = table.Column<int>(type: "int", nullable: false),
                    rateTypeId = table.Column<int>(type: "int", nullable: false),
                    mrp = table.Column<double>(type: "double", nullable: false),
                    discount = table.Column<double>(type: "double", nullable: false),
                    rate = table.Column<double>(type: "double", nullable: false),
                    itemid = table.Column<int>(type: "int", nullable: false),
                    itemCode = table.Column<string>(type: "longtext", nullable: true),
                    panelItemName = table.Column<string>(type: "longtext", nullable: true),
                    createdBy = table.Column<string>(type: "longtext", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: false),
                    createdOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    transferRemarks = table.Column<string>(type: "longtext", nullable: true),
                    transferDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rateTypeWiseRateList", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ResultEntryResponseModle",
                columns: table => new
                {
                    workOrderId = table.Column<string>(type: "longtext", nullable: true),
                    transactionId = table.Column<int>(type: "int", nullable: false),
                    patientId = table.Column<int>(type: "int", nullable: false),
                    title = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    Pname = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    gender = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: false),
                    investigationName = table.Column<string>(type: "longtext", nullable: false),
                    testId = table.Column<int>(type: "int", nullable: true),
                    labObservationId = table.Column<int>(type: "int", nullable: true),
                    observationName = table.Column<string>(type: "longtext", nullable: true),
                    value = table.Column<string>(type: "longtext", nullable: true),
                    flag = table.Column<string>(type: "longtext", nullable: true),
                    minVal = table.Column<string>(type: "longtext", nullable: true),
                    maxVal = table.Column<string>(type: "longtext", nullable: true),
                    minCritical = table.Column<string>(type: "longtext", nullable: true),
                    maxCritical = table.Column<string>(type: "longtext", nullable: true),
                    isCritical = table.Column<int>(type: "int", nullable: true),
                    readingFormat = table.Column<string>(type: "longtext", nullable: true),
                    unit = table.Column<string>(type: "longtext", nullable: true),
                    displayReading = table.Column<string>(type: "longtext", nullable: true),
                    machineReading = table.Column<string>(type: "longtext", nullable: true),
                    machineID = table.Column<int>(type: "int", nullable: true),
                    printSeperate = table.Column<int>(type: "int", nullable: true),
                    isBold = table.Column<int>(type: "int", nullable: true),
                    machineName = table.Column<string>(type: "longtext", nullable: true),
                    showInReport = table.Column<int>(type: "int", nullable: true),
                    method = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "roleMaster",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    roleName = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    isSalesRole = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roleMaster", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "roleMenuAccess",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    roleId = table.Column<int>(type: "int", nullable: true),
                    menuId = table.Column<int>(type: "int", nullable: true),
                    subMenuId = table.Column<int>(type: "int", nullable: true),
                    employeeId = table.Column<int>(type: "int", nullable: true),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roleMenuAccess", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "sampleRejectionReason",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    rejectionReason = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sampleRejectionReason", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "sampletype_master",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    sampleTypeName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sampletype_master", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SingleStringResponseModel",
                columns: table => new
                {
                    Value = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "sms",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    transactionId = table.Column<int>(type: "int", nullable: false),
                    workOrderId = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    smsText = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    deliveryDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    mobileNo = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    isSend = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    isAutoSend = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    sentBy = table.Column<int>(type: "int", nullable: true),
                    sendDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    remarks = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    type = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    createdDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    templateId = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sms", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "stateMaster",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    state = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    zoneId = table.Column<int>(type: "int", nullable: false),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stateMaster", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Testing",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    titleId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    DOB = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Department = table.Column<string>(type: "longtext", nullable: false),
                    DesignationId = table.Column<int>(type: "int", nullable: false),
                    isactive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    EmpTypeId = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Testing", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "testInterpretation",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    itemId = table.Column<int>(type: "int", nullable: false),
                    interpretation = table.Column<string>(type: "longtext", nullable: false),
                    centreId = table.Column<int>(type: "int", nullable: false),
                    showInReport = table.Column<int>(type: "int", nullable: false),
                    showinPackages = table.Column<int>(type: "int", nullable: false),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_testInterpretation", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "testInterpretationLog",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    itemId = table.Column<int>(type: "int", nullable: false),
                    centreId = table.Column<int>(type: "int", nullable: false),
                    interpretation = table.Column<string>(type: "longtext", nullable: false),
                    showInReport = table.Column<int>(type: "int", nullable: false),
                    showinPackages = table.Column<int>(type: "int", nullable: false),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_testInterpretationLog", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ThemeColour",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    headerColor = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    menuColor = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    subMenuColor = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    textColor = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    blockColor = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    color = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    iconColor = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    order = table.Column<int>(type: "int", nullable: false),
                    isdefault = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    isActive = table.Column<byte>(type: "tinyint unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThemeColour", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "titleMaster",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    title = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_titleMaster", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tnx_Allergy_ResultEntry",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    test_id = table.Column<int>(type: "int", nullable: false),
                    bookingItemId = table.Column<int>(type: "int", nullable: false),
                    transactionId = table.Column<int>(type: "int", nullable: false),
                    allergyType = table.Column<int>(type: "int", nullable: false),
                    allergyTypeName = table.Column<string>(type: "longtext", nullable: false),
                    allergySubType = table.Column<int>(type: "int", nullable: false),
                    allergySubTypeName = table.Column<string>(type: "longtext", nullable: false),
                    reading = table.Column<string>(type: "longtext", nullable: false),
                    displayReading = table.Column<string>(type: "longtext", nullable: false),
                    firstRange = table.Column<string>(type: "longtext", nullable: false),
                    secondRange = table.Column<string>(type: "longtext", nullable: false),
                    thirdRange = table.Column<string>(type: "longtext", nullable: false),
                    min = table.Column<double>(type: "double", nullable: false),
                    max = table.Column<double>(type: "double", nullable: false),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tnx_Allergy_ResultEntry", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tnx_BookingPatient",
                columns: table => new
                {
                    patientId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    title_id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    gender = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: false),
                    ageTotal = table.Column<int>(type: "int", nullable: false),
                    ageDays = table.Column<int>(type: "int", nullable: false),
                    ageMonth = table.Column<int>(type: "int", nullable: false),
                    ageYear = table.Column<short>(type: "smallint", nullable: false),
                    dob = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    isActualDOB = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    emailId = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    mobileNo = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true),
                    address = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    pinCode = table.Column<int>(type: "int", nullable: false),
                    cityId = table.Column<int>(type: "int", nullable: false),
                    centreId = table.Column<int>(type: "int", nullable: false),
                    areaId = table.Column<int>(type: "int", nullable: false),
                    stateId = table.Column<int>(type: "int", nullable: false),
                    districtId = table.Column<int>(type: "int", nullable: false),
                    countryId = table.Column<int>(type: "int", nullable: false),
                    visitCount = table.Column<int>(type: "int", nullable: false),
                    remarks = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    documentId = table.Column<int>(type: "int", nullable: true),
                    documnetnumber = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    password = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tnx_BookingPatient", x => x.patientId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tnx_InvestigationRemarks",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    invRemarks = table.Column<string>(type: "longtext", nullable: true),
                    transactionId = table.Column<int>(type: "int", nullable: true),
                    customerID = table.Column<int>(type: "int", nullable: true),
                    itemId = table.Column<int>(type: "int", nullable: true),
                    itemName = table.Column<string>(type: "longtext", nullable: true),
                    isActive = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    isInternal = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tnx_InvestigationRemarks", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tnx_Invoice_Payment",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    invoiceNo = table.Column<string>(type: "longtext", nullable: true),
                    centreId = table.Column<int>(type: "int", nullable: true),
                    fromDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    toDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    createdBy = table.Column<int>(type: "int", nullable: true),
                    createdOn = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    createdDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    cancelReason = table.Column<string>(type: "longtext", nullable: true),
                    cancelById = table.Column<int>(type: "int", nullable: true),
                    cancelByName = table.Column<string>(type: "longtext", nullable: true),
                    isCancel = table.Column<byte>(type: "tinyint unsigned", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tnx_Invoice_Payment", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tnx_Observations",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    testId = table.Column<int>(type: "int", nullable: true),
                    labObservationId = table.Column<int>(type: "int", nullable: true),
                    observationName = table.Column<string>(type: "longtext", nullable: true),
                    value = table.Column<string>(type: "longtext", nullable: true),
                    flag = table.Column<string>(type: "longtext", nullable: true),
                    min = table.Column<double>(type: "double", nullable: true),
                    max = table.Column<double>(type: "double", nullable: true),
                    minCritical = table.Column<double>(type: "double", nullable: true),
                    maxCritical = table.Column<double>(type: "double", nullable: true),
                    isCritical = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    readingformat = table.Column<string>(type: "longtext", nullable: true),
                    unit = table.Column<string>(type: "longtext", nullable: true),
                    testMethod = table.Column<string>(type: "longtext", nullable: true),
                    displayReading = table.Column<string>(type: "longtext", nullable: true),
                    machineReading = table.Column<string>(type: "longtext", nullable: true),
                    machineID = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    printseperate = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    isBold = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    machineName = table.Column<string>(type: "longtext", nullable: true),
                    showInReport = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tnx_Observations", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tnx_Observations_Histo",
                columns: table => new
                {
                    histoObservationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    testId = table.Column<int>(type: "int", nullable: true),
                    clinicalHistory = table.Column<string>(type: "longtext", nullable: true),
                    specimen = table.Column<string>(type: "longtext", nullable: true),
                    gross = table.Column<string>(type: "longtext", nullable: true),
                    typesFixativeUsed = table.Column<string>(type: "longtext", nullable: true),
                    blockKeys = table.Column<string>(type: "longtext", nullable: true),
                    stainsPerformed = table.Column<string>(type: "longtext", nullable: true),
                    biospyNumber = table.Column<string>(type: "longtext", nullable: true),
                    microscopy = table.Column<string>(type: "longtext", nullable: true),
                    finalImpression = table.Column<string>(type: "longtext", nullable: true),
                    comment = table.Column<string>(type: "longtext", nullable: true),
                    dtEntry = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tnx_Observations_Histo", x => x.histoObservationId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tnx_Observations_Histo_Log",
                columns: table => new
                {
                    histoObservationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    testId = table.Column<int>(type: "int", nullable: true),
                    clinicalHistory = table.Column<string>(type: "longtext", nullable: true),
                    specimen = table.Column<string>(type: "longtext", nullable: true),
                    gross = table.Column<string>(type: "longtext", nullable: true),
                    typesFixativeUsed = table.Column<string>(type: "longtext", nullable: true),
                    blockKeys = table.Column<string>(type: "longtext", nullable: true),
                    stainsPerformed = table.Column<string>(type: "longtext", nullable: true),
                    biospyNumber = table.Column<string>(type: "longtext", nullable: true),
                    microscopy = table.Column<string>(type: "longtext", nullable: true),
                    finalImpression = table.Column<string>(type: "longtext", nullable: true),
                    comment = table.Column<string>(type: "longtext", nullable: true),
                    dtEntry = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tnx_Observations_Histo_Log", x => x.histoObservationId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tnx_Observations_Log",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    testId = table.Column<int>(type: "int", nullable: true),
                    labObservationId = table.Column<int>(type: "int", nullable: true),
                    observationName = table.Column<string>(type: "longtext", nullable: true),
                    value = table.Column<string>(type: "longtext", nullable: true),
                    flag = table.Column<string>(type: "longtext", nullable: true),
                    min = table.Column<double>(type: "double", nullable: true),
                    max = table.Column<double>(type: "double", nullable: true),
                    minCritical = table.Column<double>(type: "double", nullable: true),
                    maxCritical = table.Column<double>(type: "double", nullable: true),
                    isCritical = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    readingformat = table.Column<string>(type: "longtext", nullable: true),
                    unit = table.Column<string>(type: "longtext", nullable: true),
                    testMethod = table.Column<string>(type: "longtext", nullable: true),
                    displayReading = table.Column<string>(type: "longtext", nullable: true),
                    machineReading = table.Column<string>(type: "longtext", nullable: true),
                    machineID = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    printseperate = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    isBold = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    machineName = table.Column<string>(type: "longtext", nullable: true),
                    dtEntry = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tnx_Observations_Log", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tnx_Observations_Micro_Flowcyto",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    testId = table.Column<int>(type: "int", nullable: false),
                    labTestId = table.Column<int>(type: "int", nullable: true),
                    transactionId = table.Column<int>(type: "int", nullable: true),
                    observationName = table.Column<string>(type: "longtext", nullable: true),
                    result = table.Column<string>(type: "longtext", nullable: true),
                    machineID = table.Column<short>(type: "smallint", nullable: false),
                    flag = table.Column<string>(type: "longtext", nullable: true),
                    isBold = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    reportType = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    organismId = table.Column<short>(type: "smallint", nullable: false),
                    organismName = table.Column<string>(type: "longtext", nullable: true),
                    antibiticId = table.Column<short>(type: "smallint", nullable: false),
                    antibitiName = table.Column<string>(type: "longtext", nullable: true),
                    colonyCount = table.Column<string>(type: "longtext", nullable: true),
                    interpretation = table.Column<string>(type: "longtext", nullable: true),
                    mic = table.Column<string>(type: "longtext", nullable: true),
                    positivity = table.Column<string>(type: "longtext", nullable: true),
                    intensity = table.Column<string>(type: "longtext", nullable: true),
                    reportStatus = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    approvedBy = table.Column<int>(type: "int", nullable: true),
                    approvedName = table.Column<string>(type: "longtext", nullable: true),
                    comments = table.Column<string>(type: "longtext", nullable: true),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tnx_Observations_Micro_Flowcyto", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tnx_Observations_Micro_flowcyto_Log",
                columns: table => new
                {
                    testId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    labTestId = table.Column<int>(type: "int", nullable: true),
                    transactionId = table.Column<int>(type: "int", nullable: true),
                    observationName = table.Column<string>(type: "longtext", nullable: true),
                    result = table.Column<string>(type: "longtext", nullable: true),
                    machineID = table.Column<short>(type: "smallint", nullable: false),
                    flag = table.Column<string>(type: "longtext", nullable: true),
                    isBold = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    reportType = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    organismId = table.Column<short>(type: "smallint", nullable: false),
                    organismName = table.Column<string>(type: "longtext", nullable: true),
                    antibiticId = table.Column<short>(type: "smallint", nullable: false),
                    antibitiName = table.Column<string>(type: "longtext", nullable: true),
                    colonyCount = table.Column<string>(type: "longtext", nullable: true),
                    interpretation = table.Column<string>(type: "longtext", nullable: true),
                    mic = table.Column<string>(type: "longtext", nullable: true),
                    positivity = table.Column<string>(type: "longtext", nullable: true),
                    intensity = table.Column<string>(type: "longtext", nullable: true),
                    reportStatus = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    approvedName = table.Column<string>(type: "longtext", nullable: true),
                    approvedBy = table.Column<int>(type: "int", nullable: true),
                    comments = table.Column<string>(type: "longtext", nullable: true),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tnx_Observations_Micro_flowcyto_Log", x => x.testId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tnx_outhousedeatils",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    itemId = table.Column<int>(type: "int", nullable: true),
                    itemName = table.Column<string>(type: "longtext", nullable: true),
                    centreId = table.Column<int>(type: "int", nullable: true),
                    transactionId = table.Column<int>(type: "int", nullable: true),
                    Companyid = table.Column<int>(type: "int", nullable: true),
                    workOrderId = table.Column<string>(type: "longtext", nullable: true),
                    bookingRate = table.Column<double>(type: "double", nullable: true),
                    outHoseRate = table.Column<double>(type: "double", nullable: true),
                    outHosueLabID = table.Column<int>(type: "int", nullable: true),
                    outhouseLabName = table.Column<string>(type: "longtext", nullable: true),
                    OutsouceDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    sentByid = table.Column<int>(type: "int", nullable: true),
                    SentName = table.Column<string>(type: "longtext", nullable: true),
                    remarks = table.Column<string>(type: "longtext", nullable: true),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tnx_outhousedeatils", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tnx_ReceiptDetails",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    transactionId = table.Column<int>(type: "int", nullable: true),
                    transactionType = table.Column<string>(type: "longtext", nullable: true),
                    workOrderId = table.Column<string>(type: "longtext", nullable: true),
                    receiptNo = table.Column<int>(type: "int", nullable: true),
                    receivedAmt = table.Column<double>(type: "double", nullable: true),
                    cashAmt = table.Column<double>(type: "double", nullable: true),
                    creditCardAmt = table.Column<double>(type: "double", nullable: true),
                    creditCardNo = table.Column<string>(type: "longtext", nullable: true),
                    chequeAmt = table.Column<double>(type: "double", nullable: true),
                    chequeNo = table.Column<string>(type: "longtext", nullable: true),
                    onlinewalletAmt = table.Column<double>(type: "double", nullable: true),
                    walletno = table.Column<string>(type: "longtext", nullable: true),
                    NEFTamt = table.Column<double>(type: "double", nullable: true),
                    BankName = table.Column<string>(type: "longtext", nullable: true),
                    paymentModeId = table.Column<short>(type: "smallint", nullable: false),
                    isCancel = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    cancelDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    canceledBy = table.Column<string>(type: "longtext", nullable: true),
                    cancelReason = table.Column<string>(type: "longtext", nullable: true),
                    bookingCentreId = table.Column<int>(type: "int", nullable: true),
                    settlementCentreID = table.Column<int>(type: "int", nullable: true),
                    receivedBy = table.Column<string>(type: "longtext", nullable: true),
                    receivedID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tnx_ReceiptDetails", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tnx_Sra",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    testId = table.Column<int>(type: "int", nullable: false),
                    transactionId = table.Column<int>(type: "int", nullable: false),
                    investigationId = table.Column<int>(type: "int", nullable: false),
                    fromCentreId = table.Column<int>(type: "int", nullable: false),
                    toCentreId = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "longtext", nullable: false),
                    entryById = table.Column<int>(type: "int", nullable: false),
                    entryDatetime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    batchNumber = table.Column<string>(type: "longtext", nullable: false),
                    transferredOn = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    barcodeNo = table.Column<string>(type: "longtext", nullable: false),
                    transferByID = table.Column<int>(type: "int", nullable: false),
                    remarks = table.Column<string>(type: "longtext", nullable: true),
                    receivedBYId = table.Column<int>(type: "int", nullable: true),
                    receivedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    outsourceRate = table.Column<double>(type: "double", nullable: true),
                    invoiceNo = table.Column<string>(type: "longtext", nullable: true),
                    invoiceDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    invoicecreatedID = table.Column<int>(type: "int", nullable: true),
                    invoiceAmount = table.Column<double>(type: "double", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tnx_Sra", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "whatsapp",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    transactionId = table.Column<int>(type: "int", nullable: false),
                    workOrderId = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    smsText = table.Column<string>(type: "longtext", nullable: false),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    reportPath = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    deliveryDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    mobileNo = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    isSend = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    isAutoSend = table.Column<byte>(type: "tinyint unsigned", nullable: true),
                    sentBy = table.Column<int>(type: "int", nullable: true),
                    sendDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    remarks = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    type = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    createdDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    templateId = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_whatsapp", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "zoneMaster",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    zone = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_zoneMaster", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "empCenterAccess",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    empId = table.Column<int>(type: "int", nullable: false),
                    centreId = table.Column<int>(type: "int", nullable: false),
                    empMasterid = table.Column<int>(type: "int", nullable: true),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_empCenterAccess", x => x.id);
                    table.ForeignKey(
                        name: "FK_empCenterAccess_empMaster_empMasterid",
                        column: x => x.empMasterid,
                        principalTable: "empMaster",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "empRoleAccess",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    empId = table.Column<int>(type: "int", nullable: false),
                    roleId = table.Column<int>(type: "int", nullable: false),
                    empMasterid = table.Column<int>(type: "int", nullable: true),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_empRoleAccess", x => x.id);
                    table.ForeignKey(
                        name: "FK_empRoleAccess_empMaster_empMasterid",
                        column: x => x.empMasterid,
                        principalTable: "empMaster",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tnx_Booking",
                columns: table => new
                {
                    transactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    workOrderId = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    billNo = table.Column<int>(type: "int", nullable: false),
                    bookingDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    clientCode = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    patientId = table.Column<int>(type: "int", nullable: false),
                    title_id = table.Column<int>(type: "int", maxLength: 10, nullable: false),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    gender = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: false),
                    dob = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ageYear = table.Column<int>(type: "int", nullable: false),
                    ageMonth = table.Column<int>(type: "int", nullable: false),
                    ageDay = table.Column<int>(type: "int", nullable: false),
                    totalAge = table.Column<int>(type: "int", nullable: false),
                    mobileNo = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    mrp = table.Column<double>(type: "double", nullable: false),
                    grossAmount = table.Column<double>(type: "double", nullable: false),
                    discount = table.Column<double>(type: "double", nullable: false),
                    netAmount = table.Column<double>(type: "double", nullable: false),
                    paidAmount = table.Column<double>(type: "double", nullable: false),
                    sessionCentreid = table.Column<int>(type: "int", nullable: false),
                    centreId = table.Column<int>(type: "int", nullable: false),
                    rateId = table.Column<int>(type: "int", nullable: false),
                    isCredit = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    paymentMode = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    source = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    discountType = table.Column<int>(type: "int", nullable: false),
                    discountid = table.Column<int>(type: "int", nullable: true),
                    discountReason = table.Column<string>(type: "longtext", nullable: true),
                    discountApproved = table.Column<int>(type: "int", nullable: false),
                    isDisCountApproved = table.Column<int>(type: "int", nullable: false),
                    patientRemarks = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    labRemarks = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    otherLabRefer = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    OtherLabReferID = table.Column<int>(type: "int", nullable: true),
                    RefDoctor1 = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    refID1 = table.Column<int>(type: "int", nullable: true),
                    refDoctor2 = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    refID2 = table.Column<int>(type: "int", nullable: true),
                    tempDOCID = table.Column<int>(type: "int", nullable: true),
                    tempDoctroName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    uploadDocument = table.Column<string>(type: "longtext", nullable: true),
                    invoiceNo = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    salesExecutiveID = table.Column<int>(type: "int", nullable: true),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tnx_Booking", x => x.transactionId);
                    table.ForeignKey(
                        name: "FK_tnx_Booking_tnx_BookingPatient_patientId",
                        column: x => x.patientId,
                        principalTable: "tnx_BookingPatient",
                        principalColumn: "patientId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tnx_BookingItem",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    workOrderId = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    transactionId = table.Column<int>(type: "int", nullable: false),
                    testcode = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    itemId = table.Column<int>(type: "int", nullable: false),
                    packageID = table.Column<int>(type: "int", nullable: false),
                    deptId = table.Column<int>(type: "int", nullable: false),
                    barcodeNo = table.Column<string>(type: "varchar(12)", maxLength: 12, nullable: true),
                    departmentName = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    investigationName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    isPackage = table.Column<int>(type: "int", nullable: false),
                    packageName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    itemType = table.Column<int>(type: "int", nullable: false),
                    mrp = table.Column<double>(type: "double", nullable: false),
                    rate = table.Column<double>(type: "double", nullable: false),
                    discount = table.Column<double>(type: "double", nullable: false),
                    netAmount = table.Column<double>(type: "double", nullable: false),
                    packMrp = table.Column<double>(type: "double", nullable: false),
                    packItemRate = table.Column<double>(type: "double", nullable: false),
                    packItemDiscount = table.Column<double>(type: "double", nullable: false),
                    packItemNet = table.Column<double>(type: "double", nullable: false),
                    reportType = table.Column<int>(type: "int", nullable: false),
                    centreId = table.Column<int>(type: "int", nullable: false),
                    sessionCentreid = table.Column<int>(type: "int", nullable: false),
                    isSra = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    isMachineOrder = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    isEmailsent = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    sampleTypeId = table.Column<int>(type: "int", nullable: false),
                    sampleTypeName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    sampleCollectionDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    sampleCollectedby = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    sampleCollectedID = table.Column<int>(type: "int", nullable: false),
                    sampleReceiveDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    sampleReceivedBY = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    resultDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    resultDoneByID = table.Column<int>(type: "int", nullable: false),
                    resutDoneBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    isResultDone = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    isApproved = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    approvedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    approvedByID = table.Column<int>(type: "int", nullable: false),
                    approvedbyName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    notApprovedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    notApprovedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    isReporting = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    isCritical = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    deliveryDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    isInvoiceCreated = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    invoiceNumber = table.Column<int>(type: "int", nullable: false),
                    isUrgent = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    isSampleCollected = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: false),
                    samplecollectionremarks = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    departmentReceiveRemarks = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    departmentReceiveDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    departmentReceiveBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    departmentReceiveByID = table.Column<int>(type: "int", nullable: false),
                    isRemoveItem = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    sampleRejectionBy = table.Column<int>(type: "int", nullable: false),
                    sampleRejectionByName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    sampleRejectionOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    interpretationId = table.Column<int>(type: "int", nullable: false),
                    approvalDoctor = table.Column<int>(type: "int", nullable: false),
                    isOuthouse = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    outhouseLab = table.Column<int>(type: "int", nullable: false),
                    labName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    outhouseDoneBy = table.Column<int>(type: "int", nullable: false),
                    outhouseDoneOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    sampleRecollectedby = table.Column<int>(type: "int", nullable: false),
                    sampleRecollectedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    isrerun = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    invoiceNo = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    invoiceDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    invoiceCycle = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    invoiceAmount = table.Column<double>(type: "double", nullable: false),
                    invoiceCreatedBy = table.Column<int>(type: "int", nullable: false),
                    invoiceNoOld = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    remarks = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    showonReportdate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tnx_BookingItem", x => x.id);
                    table.ForeignKey(
                        name: "FK_tnx_BookingItem_tnx_Booking_transactionId",
                        column: x => x.transactionId,
                        principalTable: "tnx_Booking",
                        principalColumn: "transactionId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tnx_BookingStatus",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    transactionId = table.Column<int>(type: "int", nullable: false),
                    patientId = table.Column<int>(type: "int", nullable: false),
                    barcodeNo = table.Column<string>(type: "varchar(12)", maxLength: 12, nullable: true),
                    status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    centreId = table.Column<int>(type: "int", nullable: false),
                    roleId = table.Column<int>(type: "int", nullable: false),
                    remarks = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    testId = table.Column<int>(type: "int", nullable: false),
                    isActive = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    createdById = table.Column<int>(type: "int", nullable: true),
                    createdDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updateById = table.Column<int>(type: "int", nullable: true),
                    updateDateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tnx_BookingStatus", x => x.id);
                    table.ForeignKey(
                        name: "FK_tnx_BookingStatus_tnx_Booking_transactionId",
                        column: x => x.transactionId,
                        principalTable: "tnx_Booking",
                        principalColumn: "transactionId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_empCenterAccess_empMasterid",
                table: "empCenterAccess",
                column: "empMasterid");

            migrationBuilder.CreateIndex(
                name: "IX_empRoleAccess_empMasterid",
                table: "empRoleAccess",
                column: "empMasterid");

            migrationBuilder.CreateIndex(
                name: "IX_tnx_Booking_patientId",
                table: "tnx_Booking",
                column: "patientId");

            migrationBuilder.CreateIndex(
                name: "IX_tnx_BookingItem_transactionId",
                table: "tnx_BookingItem",
                column: "transactionId");

            migrationBuilder.CreateIndex(
                name: "IX_tnx_BookingStatus_transactionId",
                table: "tnx_BookingStatus",
                column: "transactionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Allergy_SubType_Master");

            migrationBuilder.DropTable(
                name: "Allergy_TypeMaster");

            migrationBuilder.DropTable(
                name: "area_master");

            migrationBuilder.DropTable(
                name: "bank_master");

            migrationBuilder.DropTable(
                name: "barcode_series");

            migrationBuilder.DropTable(
                name: "centerAccess");

            migrationBuilder.DropTable(
                name: "centerTypeMaster");

            migrationBuilder.DropTable(
                name: "centreInvoice");

            migrationBuilder.DropTable(
                name: "centreLedgerRemarks");

            migrationBuilder.DropTable(
                name: "centreMaster");

            migrationBuilder.DropTable(
                name: "CentrePayment");

            migrationBuilder.DropTable(
                name: "centreWelcomeEmail");

            migrationBuilder.DropTable(
                name: "changeCentreLog");

            migrationBuilder.DropTable(
                name: "cityMaster");

            migrationBuilder.DropTable(
                name: "containerColorMaster");

            migrationBuilder.DropTable(
                name: "degreeMaster");

            migrationBuilder.DropTable(
                name: "designationMaster");

            migrationBuilder.DropTable(
                name: "discountReasonMaster");

            migrationBuilder.DropTable(
                name: "discountTypeMaster");

            migrationBuilder.DropTable(
                name: "districtMaster");

            migrationBuilder.DropTable(
                name: "doctorApprovalDepartments");

            migrationBuilder.DropTable(
                name: "doctorApprovalMaster");

            migrationBuilder.DropTable(
                name: "doctorReferalMaster");

            migrationBuilder.DropTable(
                name: "documentMaster");

            migrationBuilder.DropTable(
                name: "documentTypeMaster");

            migrationBuilder.DropTable(
                name: "empCenterAccess");

            migrationBuilder.DropTable(
                name: "empDepartmentAccess");

            migrationBuilder.DropTable(
                name: "empLoginDetails");

            migrationBuilder.DropTable(
                name: "empRoleAccess");

            migrationBuilder.DropTable(
                name: "formulaMaster");

            migrationBuilder.DropTable(
                name: "helpMenuMapping");

            migrationBuilder.DropTable(
                name: "helpMenuMaster");

            migrationBuilder.DropTable(
                name: "idMaster");

            migrationBuilder.DropTable(
                name: "item_outsourcemaster");

            migrationBuilder.DropTable(
                name: "itemCommentMaster");

            migrationBuilder.DropTable(
                name: "itemDocumentMapping");

            migrationBuilder.DropTable(
                name: "itemMaster");

            migrationBuilder.DropTable(
                name: "ItemObservationMapping");

            migrationBuilder.DropTable(
                name: "itemObservationMaster");

            migrationBuilder.DropTable(
                name: "itemRateMaster");

            migrationBuilder.DropTable(
                name: "itemSampleTypeMapping");

            migrationBuilder.DropTable(
                name: "labDepartment");

            migrationBuilder.DropTable(
                name: "labReportHeader");

            migrationBuilder.DropTable(
                name: "logoDetails");

            migrationBuilder.DropTable(
                name: "machine_result");

            migrationBuilder.DropTable(
                name: "machineMaster");

            migrationBuilder.DropTable(
                name: "machineObservationMapping");

            migrationBuilder.DropTable(
                name: "menuIconMaster");

            migrationBuilder.DropTable(
                name: "menuMaster");

            migrationBuilder.DropTable(
                name: "observationComment");

            migrationBuilder.DropTable(
                name: "observationReferenceRanges");

            migrationBuilder.DropTable(
                name: "organismAntibioticMaster");

            migrationBuilder.DropTable(
                name: "organismAntibioticTagMaster");

            migrationBuilder.DropTable(
                name: "outSourcelabmaster");

            migrationBuilder.DropTable(
                name: "patientDemographicLog");

            migrationBuilder.DropTable(
                name: "patientReportEmail");

            migrationBuilder.DropTable(
                name: "rateTypeMaster");

            migrationBuilder.DropTable(
                name: "rateTypeTagging");

            migrationBuilder.DropTable(
                name: "rateTypeWiseRateList");

            migrationBuilder.DropTable(
                name: "ResultEntryResponseModle");

            migrationBuilder.DropTable(
                name: "roleMaster");

            migrationBuilder.DropTable(
                name: "roleMenuAccess");

            migrationBuilder.DropTable(
                name: "sampleRejectionReason");

            migrationBuilder.DropTable(
                name: "sampletype_master");

            migrationBuilder.DropTable(
                name: "SingleStringResponseModel");

            migrationBuilder.DropTable(
                name: "sms");

            migrationBuilder.DropTable(
                name: "stateMaster");

            migrationBuilder.DropTable(
                name: "Testing");

            migrationBuilder.DropTable(
                name: "testInterpretation");

            migrationBuilder.DropTable(
                name: "testInterpretationLog");

            migrationBuilder.DropTable(
                name: "ThemeColour");

            migrationBuilder.DropTable(
                name: "titleMaster");

            migrationBuilder.DropTable(
                name: "tnx_Allergy_ResultEntry");

            migrationBuilder.DropTable(
                name: "tnx_BookingItem");

            migrationBuilder.DropTable(
                name: "tnx_BookingStatus");

            migrationBuilder.DropTable(
                name: "tnx_InvestigationRemarks");

            migrationBuilder.DropTable(
                name: "tnx_Invoice_Payment");

            migrationBuilder.DropTable(
                name: "tnx_Observations");

            migrationBuilder.DropTable(
                name: "tnx_Observations_Histo");

            migrationBuilder.DropTable(
                name: "tnx_Observations_Histo_Log");

            migrationBuilder.DropTable(
                name: "tnx_Observations_Log");

            migrationBuilder.DropTable(
                name: "tnx_Observations_Micro_Flowcyto");

            migrationBuilder.DropTable(
                name: "tnx_Observations_Micro_flowcyto_Log");

            migrationBuilder.DropTable(
                name: "tnx_outhousedeatils");

            migrationBuilder.DropTable(
                name: "tnx_ReceiptDetails");

            migrationBuilder.DropTable(
                name: "tnx_Sra");

            migrationBuilder.DropTable(
                name: "whatsapp");

            migrationBuilder.DropTable(
                name: "zoneMaster");

            migrationBuilder.DropTable(
                name: "empMaster");

            migrationBuilder.DropTable(
                name: "tnx_Booking");

            migrationBuilder.DropTable(
                name: "tnx_BookingPatient");
        }
    }
}
