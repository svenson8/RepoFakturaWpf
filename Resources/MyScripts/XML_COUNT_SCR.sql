

CREATE OR ALTER PROCEDURE [dbo].[READXML_COUNTRIES]
	@filepath varchar(100)

AS
BEGIN
DECLARE @SQL NVARCHAR(MAX)

SET @SQL = N'
Declare @xml XML
Select @xml =
CONVERT(XML,bulkcolumn,2) FROM OPENROWSET(BULK ''' +@filepath+ ''',SINGLE_BLOB) AS X

SET ARITHABORT ON

Insert into [TSlownik]
(
SLKOMUN1, SLKOMUN2, SLRODZ, ACTIVE, IUSERID
)

Select
P.value(''symbol[1]'',''VARCHAR(50)'') AS symbol,
P.value(''name[1]'',''VARCHAR(50)'') AS name,
''KRAJ'' as Kraj,
''T'' as Act,
1 as us

From @xml.nodes(''/Countries/Country'') PropertyFeed(P)
'

EXEC(@SQL)
END
