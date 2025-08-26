USE [WeatherDatabase]
GO

/****** Object:  StoredProcedure [dbo].[sp_GetWeatherHistoryById]    Script Date: 26/08/2025 17:13:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_GetWeatherHistoryById]
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
	    Id,
        Lon,
        Lat,
        TempMin,
        TempMax,
        Visibility,
        Sunrise,
        Sunset,
        Description,
        Main,
        Speed,
        City
    FROM
        WeatherHistory
    WHERE
        Id = @Id;
END
GO

