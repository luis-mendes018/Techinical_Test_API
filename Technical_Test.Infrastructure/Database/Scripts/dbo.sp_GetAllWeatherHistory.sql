USE [WeatherDatabase]
GO

/****** Object:  StoredProcedure [dbo].[sp_GetAllWeatherHistory]    Script Date: 26/08/2025 17:12:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_GetAllWeatherHistory]
    @PageNumber INT = 1,
    @PageSize INT = 10
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
    ORDER BY
        Id
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;

    SELECT COUNT(Id) FROM WeatherHistory;
END
GO

