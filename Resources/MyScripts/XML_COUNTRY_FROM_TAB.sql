--select XML.value('(//Countries/Country/name)[1]', 'varchar(100)') as name
-- from XML_STORE 

Insert into [TSlownik]
(
SLKOMUN1, SLKOMUN2, SLRODZ, ACTIVE, IUSERID
)

SELECT
   x.m.value('(name)[1]', 'varchar(50)') AS 'name',
   x.m.value('(symbol)[1]', 'Varchar(50)') AS 'symbol',
   'KRAJ' as Kraj,
   'T' as Act,
    1 as us
FROM
   XML_STORE xt 
   cross apply xt.XML.nodes( '/Countries/Country' ) x(m)
   where xt.NAME = 'Countries'
