User-Agent: Fiddler
Host: smartcity-webapp.azurewebsites.net
Content-Type: application/json
Content-Length: 97
{
"EMail":"admin@homesnailhome.be",
"Password":"Smart_2016",
"ConfirmPassword":"Smart_2016"
}


USE [ProjetSmart1]
GO

INSERT INTO [dbo].[Roles]
           ([Title],[UserRight],[HousingRight])
     VALUES
           ('UserAsmin', 1, 1),
		   ('HousingAdmin', 0, 1),
		   ('User', 0, 0)
GO

USE [ProjetSmart1]
GO

INSERT INTO [dbo].[Beds]
           ([Name])
     VALUES
           ('Lit Simple'),
           ('Lit Double'),
           ('Lits Superposes'),
           ('Lits Separes')
GO

USE [ProjetSmart1]
GO

INSERT INTO [dbo].[Localities]
           ([Zip],[Name])
     VALUES
			(5000, 'Beez'),
			(5000, 'Namur'),
			(5001, 'Belgrade'),
			(5002, 'Saint-Servais'),
			(5003, 'Saint-Marc'),
			(5004, 'Bouge'),
			(5020, 'Champion'),
			(5020, 'Daussoulx'),
			(5020, 'Flawinne'),
			(5020, 'Malonne'),
			(5020, 'Suarlée'),
			(5020, 'Temploux'),
			(5020, 'Vedrin'),
			(5021, 'Boninne'),
			(5022, 'Cognelée'),
			(5024, 'Gelbressée'),
			(5024, 'Marche-les-Dames'),
			(5100, 'Dave'),
			(5101, 'Erpent'),
			(5100, 'Jambes'),
			(5101, 'Lives'),
			(5101, 'Loyer'),
			(5100, 'Naninne'),
			(5100, 'Wépion'),
			(5100, 'Wierde')
GO