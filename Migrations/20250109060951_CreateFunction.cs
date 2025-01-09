using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iMARSARLIMS.Migrations
{
    /// <inheritdoc />
    public partial class CreateFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE FUNCTION get_workorderid(_CentreID INT(4),_type CHAR(1)) RETURNS VARCHAR(50) CHARSET utf8
BEGIN
DECLARE id INT DEFAULT 0;
DECLARE workOrderId VARCHAR(25) DEFAULT '';
SELECT IFNULL(max(MaxID),0)+1 FROM idmaster 
WHERE  centreId =1 AND type=_type INTO id;
IF (id = 1) THEN
	INSERT INTO idmaster(centreId,MaxID,type)VALUES(1,1,_type);
	SET workOrderId = CONCAT('iMarsar','1');
ELSE
	SET workOrderId = CONCAT('iMarsar',id);
	UPDATE idmaster SET MaxID=id WHERE centreId =1 AND type=_type;
END IF;
RETURN workOrderId;
     END;");


            migrationBuilder.Sql(@"CREATE  FUNCTION Get_barcodeSeries() RETURNS VARCHAR(10) CHARSET utf8
BEGIN
    
    DECLARE barcode INT DEFAULT 0;
    SELECT IFNULL(max(barcodeNo),0)+1 FROM  barcode_series INTO barcode;
    
    IF(barcode=1) THEN
     SET  barcode=100001;
    INSERT INTO  barcode_series (barcodeNo) VALUE(barcode);
    ELSE
    UPDATE barcode_series SET barcodeNo=barcode;
    END IF;
    RETURN barcode;
    END;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@" DROP FUNCTION Get_barcodeSeries()");
            migrationBuilder.Sql(@" DROP FUNCTION get_workorderid; ");
        }
    }
}
