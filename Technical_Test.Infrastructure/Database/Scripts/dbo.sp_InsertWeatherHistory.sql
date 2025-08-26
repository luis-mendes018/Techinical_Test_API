USE [WeatherDatabase]
GO

/****** Object:  StoredProcedure [dbo].[sp_InsertWeatherHistory]    Script Date: 26/08/2025 17:15:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_InsertWeatherHistory]
    @Lon FLOAT,
    @Lat FLOAT,
    @TempMin FLOAT,
    @TempMax FLOAT,
    @Visibility INT,
    @Sunrise BIGINT,
    @Sunset BIGINT,
    @Description NVARCHAR(255),
    @Main NVARCHAR(255),
    @Speed FLOAT,
	@City NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO WeatherHistory (
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
    )
    VALUES (
        @Lon,
        @Lat,
        @TempMin,
        @TempMax,
        @Visibility,
        @Sunrise,
        @Sunset,
        @Description,
        @Main,
        @Speed,
		@City
    );
END
GO

