
CREATE OR ALTER PROCEDURE [dbo].[colina_usp_ipipeline_PendingPolicies] (
	@DAYSINTHEPAST  INT
)

AS

BEGIN

/*********************************************************************
Get Extended Properties
*********************************************************/
DECLARE @ING_SERVER  VARCHAR(032);
SET @ING_SERVER = (SELECT CONVERT(VARCHAR(256),VALUE) FROM SYS.EXTENDED_PROPERTIES WITH (NOLOCK) WHERE CLASS_DESC = 'DATABASE' AND NAME = 'ING_SERVER');
DECLARE @ING_DB  VARCHAR(0256);
SET @ING_DB = (SELECT CONVERT(VARCHAR(256),VALUE) FROM SYS.EXTENDED_PROPERTIES WITH (NOLOCK) WHERE CLASS_DESC = 'DATABASE' AND NAME = 'ING_DB');
DECLARE @ING_LINKED_SERVER VARCHAR(0256);
SET @ING_LINKED_SERVER = (SELECT CONVERT(VARCHAR(256),VALUE) FROM SYS.EXTENDED_PROPERTIES WITH (NOLOCK) WHERE CLASS_DESC = 'DATABASE' AND NAME = 'ING_LINKED_SERVER');


/*******************************************************************
CALCULATE DAYS IN THE PAST
Used to Filter on Policies to improve performance
********************************************************/
DECLARE @IngDaysInThePastStr VARCHAR(010);
DECLARE @iPipeLineDaysInThePastStr	VARCHAR(010);
DECLARE @MinCreatedDateStr	VARCHAR(10) = NULL;

SET @MinCreatedDateStr = (SELECT FORMAT(Min(CreatedDate), 'yyyy-MM-dd') FROM dbo.TPOL);

IF @DAYSINTHEPAST < 1
	BEGIN
		SET @DAYSINTHEPAST = 1
	END

SET @DAYSINTHEPAST = @DAYSINTHEPAST * -1;

SELECT @IngDaysInThePastStr = FORMAT(DATEADD(DAY, @DAYSINTHEPAST,GETDATE()), 'yyyy-MM-dd');

SET @iPipeLineDaysInThePastStr = @IngDaysInThePastStr;

IF (@MinCreatedDateStr IS NOT NULL AND @iPipeLineDaysInThePastStr < @MinCreatedDateStr)
	BEGIN
		SET @iPipeLineDaysInThePastStr = @MinCreatedDateStr;
	END
/*******************************************************************************************/


/****************************************************************
CREATE SQL Dynamic Query
**************************************************/
DECLARE @TSQL VARCHAR(MAX);

SET @TSQL = 'SELECT POL_ID FROM dbo.TPOL WITH (NOLOCK) ';
SET @TSQL = @TSQL + 'WHERE CreatedDate >= ' + '''' + @iPipeLineDaysInThePastStr + '''';
SET @TSQL = @TSQL + 'EXCEPT '
SET @TSQL = @TSQL + 'SELECT POL_ID FROM ';
SET @TSQL = @TSQL + 'OPENQUERY(' + @ING_LINKED_SERVER;
SET @TSQL = @TSQL + ', '' '; 
SET @TSQL = @TSQL + 'SELECT POL_ID FROM TPOL ';
SET @TSQL = @TSQL + 'WHERE POL_CREAT_DT > ' + '''''' + @IngDaysInThePastStr + ''''' ';
SET @TSQL = @TSQL + 'FOR FETCH ONLY ';
SET @TSQL = @TSQL + ''') T1';

print @TSQL

/****************************************************************
Execute Query
**************************************************/
EXEC (@TSQL);

END


