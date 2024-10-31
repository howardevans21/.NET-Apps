
/****** Object:  StoredProcedure [dbo].[[colina_usp_api_ipipeline_Get_TTTAB_Results]]    Script Date: 8/29/2024 11:13:51 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF OBJECT_ID('colina_usp_api_ipipeline_Get_TTTAB_Results', 'P') IS NOT NULL
    DROP PROCEDURE [colina_usp_api_ipipeline_Get_TTTAB_Results]
GO


CREATE  PROCEDURE  [dbo].[colina_usp_api_ipipeline_Get_TTTAB_Results]
	@TranslationType varchar(30)
	,@TranslationFrom varchar(30)
	
AS
BEGIN
/*********************************************************************
Get Extended Properties
*********************************************************/
DECLARE @CDRDB  VARCHAR(0256);
SET @CDRDB = (SELECT CONVERT(VARCHAR(256),VALUE) FROM SYS.EXTENDED_PROPERTIES WITH (NOLOCK) WHERE CLASS_DESC = 'DATABASE' AND NAME = 'CDRDB');

/*********************************************
Create SQL Dynamic Query
***************************************************/
create table #IPTable (
		CompanyUD varchar(2) 
		,TranslationType varchar(30)
		,TranslationFrom varchar(30) 
		,TranslationTo varchar(30))

DECLARE @SQL VARCHAR(MAX);


DECLARE @INSERTSQL VARCHAR(MAX);
DECLARE @TableSQL varchar(MAX);

SET @SQL = 'SELECT CompanyUD, TranslationType, TranslationFrom, TranslationTo';
SET @SQL = @SQL + ' FROM ';
SET @SQL = @SQL + @CDRDB + '.dbo.' + 'TTTAB ';
SET @SQL = @SQL + ' WHERE ';
SET @SQL = @SQL + 'TranslationType = ''' + @TranslationType + ''' ';
SET @SQL = @SQL + 'AND ' ;
SET @SQL = @SQL + ' TranslationFrom = ''' + @TranslationFrom + '''';

SET @INSERTSQL = 'INSERT INTO #IPTable ' + @SQL
SET @TableSQL = @INSERTSQL + @SQL 

EXEC(@TableSQL)

select * from #IPTable

 -- Clean up
drop table #IPTable

END

GO