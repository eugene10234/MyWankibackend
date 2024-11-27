using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace prjWankibackend.Migrations
{
    /// <inheritdoc />
    public partial class AddImageUrlToTProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tBlock",
                columns: table => new
                {
                    fBlockSId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fMemberId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    fBlockUserId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    fBlockType = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tBlock", x => x.fBlockSId);
                });

            migrationBuilder.CreateTable(
                name: "tBroker",
                columns: table => new
                {
                    fBrokerId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    fBrokerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tBroker__41F14AB6745A9C6D", x => x.fBrokerId);
                });

            migrationBuilder.CreateTable(
                name: "tComment",
                columns: table => new
                {
                    fCommentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fPostId = table.Column<int>(type: "int", nullable: true),
                    fMemberId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    fContent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    fCratedAT = table.Column<DateOnly>(type: "date", nullable: true),
                    fUpdateAt = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tComment", x => x.fCommentId);
                });

            migrationBuilder.CreateTable(
                name: "tCustomerInvestAccount",
                columns: table => new
                {
                    fInvestAccountId = table.Column<int>(type: "int", nullable: false),
                    fMemberId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    fBrokerId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    fInvestAccount = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    fInvestPass = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tCustome__B185030D21627549", x => x.fInvestAccountId);
                });

            migrationBuilder.CreateTable(
                name: "tCustomerPreference",
                columns: table => new
                {
                    fPreferId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fMemberId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    fStockId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tCustome__04C2DDC6E0778F80", x => x.fPreferId);
                });

            migrationBuilder.CreateTable(
                name: "tCustomerStra",
                columns: table => new
                {
                    fStraId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fMemberId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    fStockId = table.Column<int>(type: "int", nullable: false),
                    fPriceSet = table.Column<decimal>(type: "money", nullable: true),
                    fTranType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    fBuySell = table.Column<bool>(type: "bit", nullable: true),
                    f5MA = table.Column<decimal>(type: "money", nullable: true),
                    f10MA = table.Column<decimal>(type: "money", nullable: true),
                    f20MA = table.Column<decimal>(type: "money", nullable: true),
                    fKValue = table.Column<int>(type: "int", nullable: true),
                    fDValue = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tCustome__3FE09AD1FA411830", x => x.fStraId);
                });

            migrationBuilder.CreateTable(
                name: "tDistrict",
                columns: table => new
                {
                    fDistrictId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fDistrict = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tDistrict", x => x.fDistrictId);
                });

            migrationBuilder.CreateTable(
                name: "tEmployeeMember",
                columns: table => new
                {
                    fEmployeeSId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fMemberId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    fAccount = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    fPassword = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    fUserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    fFirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    fLastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    fEmail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    fIdentification = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    fBirthDate = table.Column<DateOnly>(type: "date", nullable: true),
                    fSex = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: true),
                    fRegDate = table.Column<DateOnly>(type: "date", nullable: true),
                    fStatus = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: true),
                    fPermissions = table.Column<int>(type: "int", nullable: true),
                    fIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    fMemberImage = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    fMemberImagePath = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tEmployeeMember", x => x.fEmployeeSId);
                });

            migrationBuilder.CreateTable(
                name: "tFollower",
                columns: table => new
                {
                    fFollowerSId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fFollowerMemberId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    fFollowingMemberId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tFollower", x => x.fFollowerSId);
                });

            migrationBuilder.CreateTable(
                name: "tGroupMember",
                columns: table => new
                {
                    fGroupSId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fMemberId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    fAccount = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    fPassword = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    fCorporation = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    fEmail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    fRepresentName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    fCoLocation = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    fUniBusinessNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    fTotalCapital = table.Column<int>(type: "int", nullable: true),
                    fRegDate = table.Column<DateOnly>(type: "date", nullable: true),
                    fStatus = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: true),
                    fPermissions = table.Column<int>(type: "int", nullable: true),
                    fIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    fMemberImage = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    fMemberImagePath = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tGroupMember", x => x.fGroupSId);
                });

            migrationBuilder.CreateTable(
                name: "tHashtag",
                columns: table => new
                {
                    fHashTagSId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fHashTag = table.Column<string>(type: "char(10)", unicode: false, fixedLength: true, maxLength: 10, nullable: true),
                    fPostId = table.Column<int>(type: "int", nullable: true),
                    fMemberId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    fMemberType = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tHashtag", x => x.fHashTagSId);
                });

            migrationBuilder.CreateTable(
                name: "tHelp",
                columns: table => new
                {
                    fHelpId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fMemberId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    fMemberType = table.Column<int>(type: "int", nullable: true),
                    fName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    fPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    fNId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    fTaxID = table.Column<int>(type: "int", nullable: true),
                    fDistrictId = table.Column<int>(type: "int", nullable: true),
                    fHelpDescribe = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    fHelpClassId = table.Column<int>(type: "int", nullable: true),
                    fHelpSkillId = table.Column<int>(type: "int", nullable: true),
                    fHelpStatus = table.Column<int>(type: "int", nullable: true),
                    fMfdDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    fExpDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tHelp", x => x.fHelpId);
                });

            migrationBuilder.CreateTable(
                name: "tHelpClass",
                columns: table => new
                {
                    fHelpClassId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fHelpClass = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tHelpClass", x => x.fHelpClassId);
                });

            migrationBuilder.CreateTable(
                name: "tHelpMessageRecord",
                columns: table => new
                {
                    fMessageRecord = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fHelpId = table.Column<int>(type: "int", nullable: true),
                    fContent = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    fSendDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    fPublicOrNot = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "tHelpSkill",
                columns: table => new
                {
                    fHelpSkillId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fHelpSkill = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "tInvestRecord",
                columns: table => new
                {
                    fRepoId = table.Column<int>(type: "int", nullable: false),
                    fMemberId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    fStartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    fEndDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    fStockId = table.Column<int>(type: "int", nullable: false),
                    fSoldTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    fCost = table.Column<decimal>(type: "money", nullable: false),
                    fDealPrice = table.Column<decimal>(type: "money", nullable: false),
                    fStockQty = table.Column<int>(type: "int", nullable: false),
                    fProSum = table.Column<double>(type: "float", nullable: false),
                    fProTSum = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tInvestR__0ECC57CCA8608E4C", x => x.fRepoId);
                });

            migrationBuilder.CreateTable(
                name: "tLikes",
                columns: table => new
                {
                    fLikesSId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fUserId = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true),
                    fPostId = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true),
                    fTimestamp = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tLikes", x => x.fLikesSId);
                });

            migrationBuilder.CreateTable(
                name: "tLoginRecord",
                columns: table => new
                {
                    fLogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fMemberId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    fTimestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    fIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tLoginRecord", x => x.fLogId);
                });

            migrationBuilder.CreateTable(
                name: "tMatch",
                columns: table => new
                {
                    fMatchId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fHelpId = table.Column<int>(type: "int", nullable: true),
                    fMemberId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    fMatchDateTime = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    fPoint = table.Column<int>(type: "int", nullable: true),
                    fMatchStatus = table.Column<int>(type: "int", nullable: true),
                    fGrade = table.Column<int>(type: "int", nullable: true),
                    fGradeDateTime = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    fMessage = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tMatch", x => x.fMatchId);
                });

            migrationBuilder.CreateTable(
                name: "tMemberSkill",
                columns: table => new
                {
                    fMemberId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    fHelpSkillId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "tMessage",
                columns: table => new
                {
                    fMessid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fSId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    fRId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    fMessContent = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    fTimestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tMessage", x => x.fMessid);
                });

            migrationBuilder.CreateTable(
                name: "tMgerScActivity",
                columns: table => new
                {
                    fActivityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fMemberId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    fActivityType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    fOccurreAt = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "tMgrScFile",
                columns: table => new
                {
                    fFileId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fMemberId = table.Column<string>(type: "nchar(20)", fixedLength: true, maxLength: 20, nullable: true),
                    fFileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    fFilePath = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    fFileType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    fUploadateAt = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "tOrder",
                columns: table => new
                {
                    fOrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fMemberId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    fTotalHelpPoint = table.Column<int>(type: "int", nullable: true),
                    fStatus = table.Column<int>(type: "int", nullable: true),
                    fOrderTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    fExecStatus = table.Column<int>(type: "int", nullable: true),
                    fBeginTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    fFinishTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    fProof = table.Column<byte[]>(type: "image", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tOrder", x => x.fOrderId);
                });

            migrationBuilder.CreateTable(
                name: "tOrderDetail",
                columns: table => new
                {
                    fOrderDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fOrderId = table.Column<int>(type: "int", nullable: false),
                    fProductId = table.Column<int>(type: "int", nullable: true),
                    fAmount = table.Column<int>(type: "int", nullable: true),
                    fHelpPoint = table.Column<int>(type: "int", nullable: true),
                    FPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "tPersonMember",
                columns: table => new
                {
                    fPersonSId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fMemberId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    fAccount = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    fPassword = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    fUserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    fFirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    fLastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    fEmail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    fIdentification = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    fBirthDate = table.Column<DateOnly>(type: "date", nullable: true),
                    fSex = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: true),
                    fRegDate = table.Column<DateOnly>(type: "date", nullable: true),
                    fTotalHelpPoint = table.Column<int>(type: "int", nullable: true),
                    fTotalAsset = table.Column<int>(type: "int", nullable: true),
                    fDistrictId = table.Column<int>(type: "int", nullable: true),
                    fStatus = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: true),
                    fPermissions = table.Column<int>(type: "int", nullable: true),
                    fIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    fMemberImage = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    fMemberImagePath = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tPersonMember", x => x.fPersonSId);
                });

            migrationBuilder.CreateTable(
                name: "tPointList",
                columns: table => new
                {
                    fPointListId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fMemberId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    fMatchId = table.Column<int>(type: "int", nullable: true),
                    fOrderId = table.Column<int>(type: "int", nullable: true),
                    fSourse = table.Column<int>(type: "int", nullable: true),
                    fRecordTime = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tPointList", x => x.fPointListId);
                });

            migrationBuilder.CreateTable(
                name: "tPost",
                columns: table => new
                {
                    fPostId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fMemberId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    fUserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    fPostContent = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    fLikes = table.Column<int>(type: "int", nullable: true),
                    fTimestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    fMemberType = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: true),
                    fParentCommentId = table.Column<int>(type: "int", nullable: true),
                    fFinStatement = table.Column<byte[]>(type: "image", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tPost", x => x.fPostId);
                });

            migrationBuilder.CreateTable(
                name: "tProduct",
                columns: table => new
                {
                    fProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fProductCategoryId = table.Column<int>(type: "int", nullable: false),
                    fSponsorId = table.Column<int>(type: "int", nullable: true),
                    fProductName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    fDescription = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    fSales = table.Column<int>(type: "int", nullable: true),
                    fStock = table.Column<int>(type: "int", nullable: true),
                    fUnitlHelpPoint = table.Column<int>(type: "int", nullable: true),
                    fStatus = table.Column<int>(type: "int", nullable: true),
                    FImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tProduct", x => x.fProductId);
                });

            migrationBuilder.CreateTable(
                name: "tProductCategory",
                columns: table => new
                {
                    fProductCategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fProductCategoryName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    fDescription = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tProductCategory", x => x.fProductCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "tSponsor",
                columns: table => new
                {
                    fSponsorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fSponsorName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    fSponsorCategoryId = table.Column<int>(type: "int", nullable: true),
                    fAddress = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: true),
                    fPhone = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    fCapital = table.Column<decimal>(type: "money", nullable: true),
                    fIntroduction = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    fStatus = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tSponsor", x => x.fSponsorId);
                });

            migrationBuilder.CreateTable(
                name: "tSponsorCategory",
                columns: table => new
                {
                    fSponsorCategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fSponsorCategoryrName = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    fDescriptionfDescription = table.Column<string>(name: "fDescription\r\nfDescription", type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tSponsorCategory", x => x.fSponsorCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "tStock",
                columns: table => new
                {
                    fStockId = table.Column<int>(type: "int", nullable: false),
                    fStockName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    fStockPriceN = table.Column<decimal>(type: "money", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tStock__966DF63F3E161DE1", x => x.fStockId);
                });

            migrationBuilder.CreateTable(
                name: "tStockInStock",
                columns: table => new
                {
                    fInStockId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fMemberId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    fBrokerId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    fStockId = table.Column<int>(type: "int", nullable: false),
                    fStockName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    fTranType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    fLeftStock = table.Column<int>(type: "int", nullable: true),
                    fStockAdj = table.Column<int>(type: "int", nullable: true),
                    fStockNow = table.Column<int>(type: "int", nullable: true),
                    fStockTran = table.Column<int>(type: "int", nullable: true),
                    fStockPriceN = table.Column<decimal>(type: "money", nullable: true),
                    fStockPriceT = table.Column<decimal>(type: "money", nullable: true),
                    fStockPriceTS = table.Column<decimal>(type: "money", nullable: true),
                    fStockCost = table.Column<decimal>(type: "money", nullable: true),
                    fEstPro = table.Column<decimal>(type: "money", nullable: true),
                    fEstProP = table.Column<double>(type: "float", nullable: true),
                    fBalancePrice = table.Column<decimal>(type: "money", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tStockInStock", x => x.fInStockId);
                });

            migrationBuilder.CreateTable(
                name: "tTranRecord",
                columns: table => new
                {
                    fTranId = table.Column<int>(type: "int", nullable: false),
                    fMemberId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    fStockId = table.Column<int>(type: "int", nullable: false),
                    fBrokerId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    fTranType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    fBuySell = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    fStockQty = table.Column<int>(type: "int", nullable: false),
                    fStockPrice = table.Column<decimal>(type: "money", nullable: false),
                    fTranTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tTranRec__56FABBBF8D192D15", x => x.fTranId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tBlock");

            migrationBuilder.DropTable(
                name: "tBroker");

            migrationBuilder.DropTable(
                name: "tComment");

            migrationBuilder.DropTable(
                name: "tCustomerInvestAccount");

            migrationBuilder.DropTable(
                name: "tCustomerPreference");

            migrationBuilder.DropTable(
                name: "tCustomerStra");

            migrationBuilder.DropTable(
                name: "tDistrict");

            migrationBuilder.DropTable(
                name: "tEmployeeMember");

            migrationBuilder.DropTable(
                name: "tFollower");

            migrationBuilder.DropTable(
                name: "tGroupMember");

            migrationBuilder.DropTable(
                name: "tHashtag");

            migrationBuilder.DropTable(
                name: "tHelp");

            migrationBuilder.DropTable(
                name: "tHelpClass");

            migrationBuilder.DropTable(
                name: "tHelpMessageRecord");

            migrationBuilder.DropTable(
                name: "tHelpSkill");

            migrationBuilder.DropTable(
                name: "tInvestRecord");

            migrationBuilder.DropTable(
                name: "tLikes");

            migrationBuilder.DropTable(
                name: "tLoginRecord");

            migrationBuilder.DropTable(
                name: "tMatch");

            migrationBuilder.DropTable(
                name: "tMemberSkill");

            migrationBuilder.DropTable(
                name: "tMessage");

            migrationBuilder.DropTable(
                name: "tMgerScActivity");

            migrationBuilder.DropTable(
                name: "tMgrScFile");

            migrationBuilder.DropTable(
                name: "tOrder");

            migrationBuilder.DropTable(
                name: "tOrderDetail");

            migrationBuilder.DropTable(
                name: "tPersonMember");

            migrationBuilder.DropTable(
                name: "tPointList");

            migrationBuilder.DropTable(
                name: "tPost");

            migrationBuilder.DropTable(
                name: "tProduct");

            migrationBuilder.DropTable(
                name: "tProductCategory");

            migrationBuilder.DropTable(
                name: "tSponsor");

            migrationBuilder.DropTable(
                name: "tSponsorCategory");

            migrationBuilder.DropTable(
                name: "tStock");

            migrationBuilder.DropTable(
                name: "tStockInStock");

            migrationBuilder.DropTable(
                name: "tTranRecord");
        }
    }
}
