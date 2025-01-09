using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iMARSARLIMS.Migrations
{
    /// <inheritdoc />
    public partial class CreateProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE GetTestObservationTestIdWise(
                    IN _TestId INT,
                    IN _Gender VARCHAR(10),
                    IN _Age INT,
                    IN _ToAge INT,
                    IN _CentreId INT
                )
                BEGIN
                    SELECT * FROM (
                        -- First part of the UNION query
                        SELECT bo.Gender, CONCAT(bo.Name, ', ', CONCAT(bo.AgeYear, 'Y ', bo.AgeMonth, 'M ', bo.AgeDay, 'D')) Pname,
                               IFNULL(bo.RefDoctor1, 'SELF') ReferDoc, DATEDIFF(NOW(), bo.DOB) AgeInDays, bo.transactionId,
                               bi.barcodeno, bo.WorkOrderId, bi.isapproved, bi.itemid, bi.investigationName, bi.ReportType, 1 AS IsHead,
                               'Header' AS TYPE, im.ItemName, '' AS ObservationName, '' AS VALUE, '' AS COMMENT, '' AS Unit,
                               '' AS readingFormat, '' AS displayreading, '' AS minVal, '' AS maxVal, '' AS MinCritical, 
                               '' AS MaxCritical, '' AS Flag, bi.Id AS testId, '' AS MachineReading, '' AS method,
                               0 AS isCritical, im.printseperate, 0 AS isBold, 0 AS machineID, '' AS MachineName,
                               '' AS TestIdmac, 0 AS ResultRequired, 0 AS DLCCheck, 1 AS ShowinReport, '' AS HelpName,
                               '' AS Formula, 0 AS labObservationId, '' AS Calculation
                        FROM tnx_bookingitem bi
                        INNER JOIN tnx_booking bo ON bi.TransactionId = bo.transactionId
                        INNER JOIN itemmaster im ON bi.itemid = im.Id
                        WHERE bi.IsSampleCollected = 'Y' AND bi.Id = _TestId
                        
                        UNION ALL
                        
                        -- Second part of the UNION query
                        SELECT bo.Gender, CONCAT(bo.Name, ', ', CONCAT(bo.AgeYear, 'Y ', bo.AgeMonth, 'M ', bo.AgeDay, 'D')) Pname,
                               IFNULL(bo.RefDoctor1, 'SELF') ReferDoc, DATEDIFF(NOW(), bo.DOB) AgeInDays, bo.transactionId,
                               bi.barcodeno, bo.WorkOrderId, bi.isapproved, bi.itemid, bi.investigationName, bi.ReportType, 1 AS IsHead,
                               'Header' AS TYPE, im.ItemName, io.labObservationName AS ObservationName,
                               (CASE WHEN iom.isHeader = 1 THEN 'Header' WHEN IFNULL(tob.value, '') != '' THEN IFNULL(tob.value, '')
                               WHEN IFNULL(tob.value, '') = '' AND IFNULL(mac.MacReading1, '') != '' THEN mac.MacReading1 
                               WHEN IFNULL(tob.value, '') = '' AND IFNULL(mac.MacReading1, '') = '' AND IFNULL(lr.defaultvalue, '') != '' 
                               THEN IFNULL(lr.defaultvalue, '') ELSE '' END) AS VALUE,
                               IFNULL((SELECT COMMENT FROM observationcomment WHERE TestID = bi.Id AND ObservationId = 0), '') AS `comment`,
                               IF(IFNULL(tob.Unit, '') = '', IFNULL(lr.Unit, ''), IFNULL(tob.Unit, '')) AS Unit,
                               CONCAT(IFNULL(lr.MinValue, ''), '-', IFNULL(lr.MaxValue, '')) AS readingFormat,
                               IF(IFNULL(tob.value, '') = '', IFNULL(lr.ReportReading, ''), IFNULL(tob.displayreading, '')) AS displayreading,
                               IF(IFNULL(tob.value, '') = '', IFNULL(lr.MinValue, ''), IFNULL(tob.Min, '')) AS minVal,
                               IF(IFNULL(tob.value, '') = '', IFNULL(lr.MaxValue, ''), IFNULL(tob.Max, '')) AS maxVal,
                               IF(IFNULL(tob.value, '') = '', lr.MinCritical, IFNULL(tob.MinCritical, '')) AS MinCritical,
                               IF(IFNULL(tob.value, '') = '', lr.MaxCritical, IFNULL(tob.MaxCritical, '')) AS MaxCritical,
                               IFNULL(tob.Flag, '') AS Flag, bi.Id AS testId,
                               IF(IFNULL(tob.value, '') = '', mac.MacReading1, IFNULL(tob.MachineReading, '')) AS MachineReading,
                               IF(IFNULL(tob.testMethod, '') = '', io.method, tob.testMethod) AS testMethodio,
                               0 AS isCritical, 0 AS printseperate, 0 AS isBold, IFNULL(mac.MacID1, 0) AS machineID,
                               IF(IFNULL(tob.value, '') = '', CONCAT(IFNULL(mac.MachineName1, ''), '#', IFNULL(mac.MacID1, '0')),
                               CONCAT(IFNULL(tob.MachineName, ''), '#', IFNULL(tob.MachineID, '0'))) AS MachineName,
                               iom.itemid AS TestIdmac, io.ResultRequired, io.DLCCheck, iom.ShowinReport,
                               IFNULL((SELECT GROUP_CONCAT(HelpName) FROM helpmenumapping WHERE itemid = iom.itemid), '') AS HelpName,
                               IFNULL(io.Formula, '') AS Formula, io.id AS labObservationId, '' AS Calculation
                        FROM tnx_bookingitem bi
                        INNER JOIN tnx_booking bo ON bi.TransactionId = bo.transactionId
                        INNER JOIN itemmaster im ON bi.itemid = im.Id
                        INNER JOIN itemobservationmapping iom ON im.id = iom.itemid
                        INNER JOIN itemobservationmaster io ON iom.itemobservationid = io.id
                        LEFT JOIN tnx_Observations tob ON tob.TestId = bi.Id AND tob.LabObservationId = io.id
                        LEFT JOIN machine_result mac ON mac.observationId = io.id AND mac.testid = bi.id
                        LEFT JOIN observationReferenceRanges lr ON io.id = lr.observationId AND lr.machineid = 1
                        WHERE bi.IsSampleCollected = 'Y' AND bi.Id = _TestId
                    ) t
                    ORDER BY t.testId, t.labObservationId;
                END;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS GetTestObservationTestIdWise;");
        }
    }
}
